using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XEResources
{
    public class ResourceLoader<T> where T : Object
    {
        private bool m_IsReady = false;
        protected Object m_Asset = null;
        protected string m_ResourceName = string.Empty;
        private System.Action<T> result = null;

        public string ResourceName
        {
            get { return m_ResourceName; }
        }

        public void SetResource(string name)
        {
            m_ResourceName = name;
        }

        public void SetCallback(System.Action<T> callback)
        {
            result = callback;
        }

        public bool IsReady
        {
            get { return m_IsReady; }
        }

        public T GetAsset()
        {
            return m_Asset as T;
        }

        public void Notice()
        {
           
            m_IsReady = true;
            m_Asset = ResourceManager.Instance.Load<T>(m_ResourceName);

            if (m_Asset == null)
                Debug.LogError("Resource Loading failed - " + m_ResourceName);

            if (result != null)
                result(GetAsset());
        }

        public void ResNotice(string path, string name,E_ResourceKind resKind=E_ResourceKind.Texture)
        {
            m_IsReady = true;
            m_Asset = ResourceManager.Instance.FindResource<T>(path, name,resKind );

            if (m_Asset == null)
                Debug.LogError("Resource Loading failed - " + m_ResourceName);

            if (result != null)
                result(GetAsset());
        }


        public static void Destroy(ResourceLoader<T> loader)
        {
            loader.m_Asset = null;
            loader.m_ResourceName = null;
            loader = null;
        }
    }

    public class ResourceManager : Singleton<ResourceManager>
    {
        List<AssetBundle> unloadAssets = new List<AssetBundle>();

        private class AssetBundleLoader
        {
            private string m_Name;
            private AssetBundle m_AB;
            private AssetBundleRequest m_ABR;
            private System.Action m_Notices;

            public AssetBundleLoader(string name, AssetBundle ab, AssetBundleRequest abr)
            {
                m_Name = name;
                m_AB = ab;
                m_ABR = abr;
            }

            public string Name
            {
                get { return m_Name; }
            }
            public bool IsDone
            {
                get { return m_ABR.isDone; }
            }

            public Object[] Assets
            {
                get { return m_ABR.allAssets; }
            }

            public void Loaded()
            {
                if (m_Notices != null)
                    m_Notices();
                m_ABR = null;
                Instance.UnloadAssetBundle(m_AB);
            }

            public void AddNotice(System.Action userNotice)
            {
                m_Notices += userNotice;
            }
            public void RemoveNotice(System.Action userNotice)
            {
                m_Notices -= userNotice;
            }
        }

        private bool m_IsLoadScene = false;
        private AsyncOperation m_AsyncOperation;
        private AssetBundle m_AsyncSceneAB;
        private List<string> m_LoadedAssetbundles = new List<string>();
        private Dictionary<string, Object> m_FileMap = new Dictionary<string, Object>();//檔名查詢列表
        private List<AssetBundleLoader> m_AssetBundleLoaders = new List<AssetBundleLoader>();

        private string GetPathFromKind(byte kind)
        {
            switch (kind)
            {
                case (byte)E_ResourceKind.Model:
                    return GLOBALCONST.FOLDER_MODEL;
                case (byte)E_ResourceKind.Action:
                    return GLOBALCONST.FOLDER_ACTION;
                case (byte)E_ResourceKind.Material:
                    return GLOBALCONST.FOLDER_MATERIAL;
                case (byte)E_ResourceKind.Texture:
                    return GLOBALCONST.FOLDER_TEXTURE;
                case (byte)E_ResourceKind.Sound:
                    return GLOBALCONST.FOLDER_SOUND;
                case (byte)E_ResourceKind.UI:
                    return GLOBALCONST.FOLDER_UI;
                case (byte)E_ResourceKind.Effect:
                    return GLOBALCONST.FOLDER_EFFECT;
                case (byte)E_ResourceKind.Scene:
                    return GLOBALCONST.FOLDER_SCENE;
                case (byte)E_ResourceKind.Map:
                    return GLOBALCONST.FOLDER_MAP;
                case (byte)E_ResourceKind.Table:
                    return GLOBALCONST.FOLDER_TABLE;
            }
            return string.Empty;
        }

		private void LoadAssetbundleAsync<T>(string resourceName,System.Action noticeMathod,System.Action<string,string,E_ResourceKind> resNoticMethod,E_ResourceKind resKind = E_ResourceKind.Texture) where T : Object
		{
			if (string.IsNullOrEmpty(resourceName))
				return;
			ResourceListData Data = TableManager.Instance.ResourceListDataTable.GetData(resourceName);
			//Debug.Log("Load asset bundle - " + resourceName + " : " + Data);
			if (Data == null)
			{
				noticeMathod();
				return;
			}
			if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
			{
				if (m_LoadedAssetbundles.Contains(Data.Assetbundle))
				{
					for (int i = 0; i < m_AssetBundleLoaders.Count; i++)
					{
						if (m_AssetBundleLoaders[i].Name.Equals(Data.Assetbundle))
						{
							m_AssetBundleLoaders[i].AddNotice(noticeMathod);
							return;
						}
					}
				}
				else
					m_LoadedAssetbundles.Add(Data.Assetbundle);

				//Debug.Log("Loading... - " + Data.Assetbundle);
				AssetBundle AssetBundle = null;
				string AssetbundlePath = GetPathFromKind(Data.AssetbundlePathKind);

				AssetBundle = AssetBundle.LoadFromFile(GLOBALVALUE.FOLDER_RESOURCE_ROOT + AssetbundlePath + Data.Assetbundle + GLOBALCONST.EXTENSION_RELEASEDATA);

				// AssetBundle = BetterStreamingAssets.LoadAssetBundle(GLOBALVALUE.FOLDER_RESOURCE_ROOT + AssetbundlePath + Data.Assetbundle + GLOBALCONST.EXTENSION_RELEASEDATA);
				if (AssetBundle == null)
				{
					Debug.Log(string.Format("Load assetbundle {0} failed",resourceName) + E_DebugLogType.Error);
					noticeMathod();
					return;
				}

				AssetBundleRequest ABR = AssetBundle.LoadAllAssetsAsync<T>();

				AssetBundleLoader ABLoader = new AssetBundleLoader(Data.Assetbundle,AssetBundle,ABR);
				//GLOBALFUNCTION.Log("Assets - " + ABR.asset);

				ABLoader.AddNotice(noticeMathod);
				m_AssetBundleLoaders.Add(ABLoader);
			}
			else
			{
				string path = "Art/";
				resNoticMethod(path,resourceName,resKind);
			}

		}

		public bool NowLoading
        {
            get
            {
                if (m_IsLoadScene)
                    return true;
                return false;
            }
        }

        public void OnUpdate()
        {

            while (unloadAssets.Count > 0)
            {
                unloadAssets[0].Unload(false);
                unloadAssets.RemoveAt(0);
            }

            for (int i = m_AssetBundleLoaders.Count - 1; i >= 0; i--)
            {
                if (!m_AssetBundleLoaders[i].IsDone || m_AssetBundleLoaders[i].Assets.Length == 0)
                    continue;

                for (int j = 0; j < m_AssetBundleLoaders[i].Assets.Length; j++)
                {
                    if (!m_FileMap.ContainsKey(m_AssetBundleLoaders[i].Assets[j].name))
                        m_FileMap.Add(m_AssetBundleLoaders[i].Assets[j].name, m_AssetBundleLoaders[i].Assets[j]);
                    else
                        Debug.Log(string.Format("Duplicate file name {0}", m_AssetBundleLoaders[i].Assets[j].name) + E_DebugLogType.Error);
                }

                m_AssetBundleLoaders[i].Loaded();

                m_AssetBundleLoaders.RemoveAt(i);
            }

            if (!m_IsLoadScene)
                return;
            if (m_AsyncOperation == null)
                return;
            if (!m_AsyncOperation.isDone)
                return;
            if (m_AsyncSceneAB != null)
                m_AsyncSceneAB.Unload(false);

            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            m_IsLoadScene = false;
            m_AsyncOperation = null;
            m_AsyncSceneAB = null;
        }

        private string prePath = "";
        private string[] subdirs;

        public T FindResource<T>(string path, string name,E_ResourceKind  resKind=E_ResourceKind.Texture) where T : Object
        {
            SearchOption sop = SearchOption.AllDirectories;
            string ApplicationPath = Application.dataPath;

            if (prePath != path)
                subdirs = Directory.GetDirectories(Path.Combine(ApplicationPath, path), "*", sop);

            T resource = null;
            for (int i = 0; i < subdirs.Length; i++)
            {
                string normalPath = subdirs[i];
                string tmpPath = subdirs[i].Replace(ApplicationPath, "");
                tmpPath = Path.Combine("Assets/"+tmpPath, name);
                tmpPath = tmpPath.Replace("\\", "/");
                string extension = "";
                if (resKind == E_ResourceKind.Texture)
                {
                    if (File.Exists(Path.Combine(normalPath, name + ".png")))
                        extension = ".png";
                    else
                        extension = ".jpg";
                }
                else if (resKind == E_ResourceKind.Material)
                    extension = ".mat";
                else if (resKind == E_ResourceKind.Sound)
                {
                    if (File.Exists(Path.Combine(normalPath, name + ".mp3")))
                        extension = ".mp3";
                    else
                        extension = ".wav";
                }
                else if (resKind == E_ResourceKind.Material || resKind == E_ResourceKind.Model || resKind == E_ResourceKind.Effect || resKind == E_ResourceKind.UI)
                    extension = ".prefab";

                tmpPath = tmpPath + extension;
                tmpPath = tmpPath.Replace("//","/");

                if (!File.Exists(Path.Combine (normalPath,name + extension)))
                    continue;
#if UNITY_EDITOR
                resource = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(tmpPath);
#endif              

                if (resource != null)
                    break;
            }
            prePath = path;
            return resource;
        }
        //Don't use this method to load resource immediately , only use for ResourceLoader     
        public T Load<T>(string resourceName) where T : Object
        {
            if (m_FileMap.ContainsKey(resourceName))
                return m_FileMap[resourceName] as T;
            if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
                return Resources.Load<T>(resourceName);
            else
                return FindResource<T>("Resources/", resourceName);
        }

        ////public ResourceLoader<T> LoadAsync<T>(string resourceName, System.Action<T> callback = null, E_ResourceKind resKind = E_ResourceKind.Texture) where T : Object
        ////{
        ////    ResourceLoader<T> Loader = new ResourceLoader<T>();
        ////    Loader.SetResource(resourceName);

        ////    if (callback != null)
        ////        Loader.SetCallback(callback);


        ////    if (m_FileMap.ContainsKey(resourceName))
        ////    {
        ////        if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
        ////            Loader.Notice();
        ////        else
        ////            Loader.ResNotice("Art/", resourceName);
        ////    }
        ////    else
        ////        LoadAssetbundleAsync<T>(resourceName, Loader.Notice, Loader.ResNotice,resKind );


        ////    return Loader;
        ////}

        public TextAsset LoadTableSync(E_ResourceKind tableKind, string name)
        {
            //Debug.Log ("LoadTableSync("+name+")");
            if (tableKind != E_ResourceKind.Map
                && tableKind != E_ResourceKind.Table)
                return null;
			if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
			{
				string ResourcePath = string.Format("{0}{1}{2}{3}"
												,GLOBALVALUE.FOLDER_RESOURCE_ROOT
												,GetPathFromKind((byte)tableKind)
												,name,GLOBALCONST.EXTENSION_RELEASEDATA);
				AssetBundle AssetBundle = AssetBundle.LoadFromFile(ResourcePath);
				//Debug.Log("Resourcepath = " + ResourcePath);
				if (AssetBundle == null)
				{
					Debug.Log("AssetBundleNull");
					return null;
				}
			
                TextAsset Result = null;
                Result =AssetBundle.LoadAsset<TextAsset>(name);
				//Debug.Log("TextAsset = ...." + Result.name);
                UnloadAssetBundle(AssetBundle);
                return Result;
            }
            else
            {
                 string path = Path.Combine("Assets/Art/"+GetPathFromKind ((byte)tableKind ), name+".txt");
                TextAsset Result = null;
#if UNITY_EDITOR
                 Result = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
#endif 
                return Result;
            }
        }

        public GameObject LoadEffectSync(string name)
        {
            string ResourcePath = string.Format("{0}{1}{2}{3}"
                , GLOBALVALUE.FOLDER_RESOURCE_ROOT
                , GetPathFromKind((byte)E_ResourceKind.Effect)
                , name, GLOBALCONST.EXTENSION_RELEASEDATA);
            //Debug.Log ("LoadEffectSync("+name+")");
            AssetBundle AssetBundle = null;
            GameObject Result = null;
            try
            {
                if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
                {
                    AssetBundle = AssetBundle.LoadFromFile(ResourcePath);
                }
                else
                {
                    string path = Path.Combine("Art/", GetPathFromKind((byte)E_ResourceKind.Effect));
                    Result = FindResource<GameObject>(path, name,E_ResourceKind.Effect);

                }
            }
            catch (System.Exception e)
            {

                Debug.LogError(ResourcePath + e.Message);
            }
            finally
            {
                if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
                {
                    if (AssetBundle == null)
                    {
                        name = string.Format("{0}{1}", "Art/" + GLOBALCONST.FOLDER_EFFECT, name);
                        Result = Resources.Load<GameObject>(name);
                    }
                    else
                    {
                        Result = AssetBundle.LoadAsset<GameObject>(name);
                        //AssetBundle.Unload(false);
                        Instance.UnloadAssetBundle(AssetBundle);
                    }
                }
                else
                {
                    if (Result == null)
                    {
                        string path = Path.Combine("Art/", GetPathFromKind((byte)E_ResourceKind.Effect) + "Prefab/");
                        Result = FindResource<GameObject>(path, name,E_ResourceKind.Effect);
                    }
                }
            }
            return Result;
        }

        public UIBase LoadUISync(string name)
        {
            string ResourcePath = string.Format("{0}{1}{2}{3}"
                                                , GLOBALVALUE.FOLDER_RESOURCE_ROOT
                                                , GetPathFromKind((byte)E_ResourceKind.UI)
                                                , name, GLOBALCONST.EXTENSION_RELEASEDATA);
            //Debug.Log ("LoadUISync("+name+")");
            AssetBundle AssetBundle = null;
            UIBase Result = null;
            try
            {
                //if (System.IO.File.Exists(ResourcePath))
                //{
                if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
                {
                    AssetBundle = AssetBundle.LoadFromFile(ResourcePath);
                }
                else
                {
                    string path = Path.Combine("Art/", GetPathFromKind((byte)E_ResourceKind.UI));
                    Result = FindResource<UIBase>(path, name,E_ResourceKind.UI);
                }

                // AssetBundle = BetterStreamingAssets.LoadAssetBundle(ResourcePath);
                //}
            }
            catch (System.Exception e)
            {

                Debug.LogError(e.Message);
            }
            finally
            {
                if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
                {
                    if (AssetBundle == null)
                    {
                        name = string.Format("{0}{1}", GLOBALCONST.FOLDER_UI, name);
                        Result = Resources.Load<UIBase>(name);
                    }
                    else
                    {
                        GameObject Obj = AssetBundle.LoadAsset<GameObject>(name);
                        if (Obj != null)
                            Result = Obj.GetComponent<UIBase>();
                        //AssetBundle.Unload(false);
                        Instance.UnloadAssetBundle(AssetBundle);
                    }
                }
                else
                {
                    if (Result == null)
                    {
                        name = string.Format("{0}{1}", GLOBALCONST.FOLDER_UI, name);
                        Result = Resources.Load<UIBase>(name);
                    }
                    
                }
            }

            return Result;

        }


		public void RemoveNotice<T>(string resourceName,object loader) where T : Object
		{
			if (loader == null)
				return;
			ResourceLoader<T> Loader = loader as ResourceLoader<T>;
			if (string.IsNullOrEmpty(resourceName))
				return;
			ResourceListData Data = TableManager.Instance.ResourceListDataTable.GetData(resourceName);
			if (Data == null)
				return;
			for (int i = 0; i < m_AssetBundleLoaders.Count; i++)
			{
				if (m_AssetBundleLoaders[i].Name.Equals(resourceName))
				{
					m_AssetBundleLoaders[i].RemoveNotice(Loader.Notice);
				}
			}
		}

		public void UnloadAssetBundle(AssetBundle ab)
        {
            unloadAssets.Add(ab);
        }
    }

}
