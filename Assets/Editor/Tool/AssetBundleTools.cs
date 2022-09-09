using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;


public static class AssetBundleTools
{

    /// <summary>
    /// 打包 assetbundle 
    /// </summary>
    /// <param name="mainAsset"></param>
    /// <param name="assets"></param>
    /// <param name="pathName"></param>
    /// <returns></returns>
    static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName)
    {
#if AB_NO_ENCRYPTED        
        BuildAssetBundleOptions BABO = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.CollectDependencies;
#else
        BuildAssetBundleOptions BABO = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.CollectDependencies;
#endif

#if UNITY_STANDALONE_OSX
		return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, BABO, BuildTarget.StandaloneOSXUniversal);
#elif UNITY_STANDALONE_WIN
        return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, BABO, BuildTarget.StandaloneWindows);
#elif UNITY_IPHONE
        return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, BABO, BuildTarget.iOS);
#elif UNITY_ANDROID
        return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, BABO, BuildTarget.Android);		
#endif
        System.Threading.Thread.Sleep(100);		//當build 較多時, 可避免當機
    }

	/// <summary>
	/// 打包 assetbundle 
	/// </summary>
	/// <returns>
	/// The asset bundle.
	/// </returns>
	/// <param name='mainAsset'>
	/// If set to <c>true</c> main asset.
	/// </param>
	/// <param name='assets'>
	/// If set to <c>true</c> assets.
	/// </param>
	/// <param name='outputDir'>
	/// If set to <c>true</c> output dir.
	/// </param>
	/// <param name='outputFileName'>
	/// If set to <c>true</c> output file ext.
	/// </param>
	public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string outputDir, string outputFileName)	
	{
        //檢查輸出目錄是否存在, 不存在則自動建立
        if (!Directory.Exists(outputDir))
        {
            Debug.Log(string.Format("Create Dir = {0}", outputDir));
            Directory.CreateDirectory(outputDir);
        }

        //若是沒輸出檔名, 表示輸出的方式為各自打包
        if (outputFileName == string.Empty)
        {
			foreach(UnityEngine.Object o in assets)
			{
				Debug.Log(string.Format("Start BuildAssetBundle form {0}", o.name));
				if (BuildAssetBundle(o, null, outputDir + o.name + ".unity3d"))
				{
				    Debug.Log(string.Format("BuildAssetBundle Success ,  OutPath={0}", outputDir + outputFileName));
				}
				else
				{
					Debug.Log(string.Format("BuildAssetBundle Fail ,  SrcName={0}", o.name));
				}
			}
            
        }
        else
        {
            Debug.Log(string.Format("Start BuildAssetBundle form {0}", "Select File"));
            if (BuildAssetBundle(mainAsset, assets, outputDir + outputFileName))
			{
            	Debug.Log(string.Format("BuildAssetBundle Success ,  OutPath={0}", outputDir + outputFileName));
			}
			else
			{
				Debug.Log(string.Format("BuildAssetBundle Fail ,  OutPath={0}", outputDir + outputFileName));
			}
        }

        EncryptedAssetbundleToBytes(outputDir, outputFileName , GLOBALCONST.EXTENSION_UNITY3D);

		return true;
		
	}

    public static bool BuildAssetBundle(string searchDir, string searchFileExtPer, string outputDir, List<string> resources = null)
    {
        return BuildAssetBundle(searchDir, searchFileExtPer, outputDir, SearchOption.AllDirectories, resources);
    }

    /// <summary>
    /// 打包 assetbundle 
    /// </summary>
    /// <param name="searchDir"></param>
    /// <param name="searchFileExtPer"></param>
    /// <param name="outputDir"></param>
    /// <returns></returns>
    public static bool BuildAssetBundle(string searchDir, string searchFileExtPer, string outputDir, SearchOption SOption, List<string> resources = null)
    {
		//去掉目錄最後的"/"		
		int lastindex = searchDir.LastIndexOf('/');
		if (lastindex == searchDir.Length-1)
			searchDir=searchDir.Remove(lastindex);
		
		//來源目錄        
        if (!Directory.Exists(searchDir))
        {
            Debug.Log(searchDir + "not exists");
        }
		
        //取得目錄下相關的檔案,並讀取成assetdata 
        string[] fullPathNames = searchFileExtPer.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(searchDir, filter, SOption)).ToArray();
        if (fullPathNames.Length == 0)
        {
            Debug.Log("No Build File");
            return false;
        }

        //檢查輸出目錄是否存在, 不存在則自動建立
        if (!Directory.Exists(outputDir))
        {
            Debug.Log(string.Format("Create Dir = {0}", outputDir));
            Directory.CreateDirectory(outputDir);
        }

        for (int i = 0; i < fullPathNames.Length; i++ )
        {
            string fileName = Path.GetFileNameWithoutExtension(fullPathNames[i]);
            if (resources != null && !resources.Contains(fileName)) continue;

            string filePath = fullPathNames[i];
            filePath = filePath.Replace("\\", "/");

            string path = outputDir + fileName + GLOBALCONST.EXTENSION_UNITY3D;
            Debug.Log(string.Format("Start BuildAssetBundle form {0}", searchDir));
            Object selection = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object)); //xe130925 
			if (selection != null)
			{
            	if (BuildAssetBundle(selection, null, path))
				{
					Debug.Log(string.Format("BuildAssetBundle Success ,  OutPath={0}", path));
				}
				else
				{
					Debug.Log(string.Format("BuildAssetBundle Fail ,  SrcName={0}", fileName));
				}
			}
			else
			{
				Debug.LogError(string.Format("BuildAssetBundle Load File Fail ,  SrcName={0}", fileName));
			}
            
        }

        EncryptedAssetbundleToBytes(outputDir, string.Empty, GLOBALCONST.EXTENSION_UNITY3D);

        return true;
    }

    public static bool BuildSceneBundle(string[] levels , string Localpath)    
    {
#if AB_NO_ENCRYPTED
        BuildOptions BO = BuildOptions.UncompressedAssetBundle;
#else
        BuildOptions BO = BuildOptions.None;
#endif        



#if UNITY_STANDALONE_OSX
		BuildPipeline.BuildStreamedSceneAssetBundle(levels,Localpath,BuildTarget.StandaloneOSXUniversal, BO);
        //BuildPipeline.BuildAssetBundles()
#elif UNITY_STANDALONE_WIN
        BuildPipeline.BuildStreamedSceneAssetBundle(levels, Localpath, BuildTarget.StandaloneWindows, BO);
#elif UNITY_ANDROID
        BuildPipeline.BuildStreamedSceneAssetBundle(levels,Localpath,BuildTarget.Android , BO);
#elif UNITY_IOS
        BuildPipeline.BuildStreamedSceneAssetBundle(levels,Localpath,BuildTarget.iOS , BO);
#endif               

        return true;
    }

    public static bool EncryptedAssetbundleToBytes(string outputDir, string outputFileName, string searchFileExtPer)
    {
        if (!Directory.Exists(outputDir))
            return false;

        string[] fullPathNames;
        if (outputFileName == string.Empty)
            fullPathNames = Directory.GetFiles(outputDir, string.Format("*{0}", searchFileExtPer), SearchOption.AllDirectories);
        else
            fullPathNames = Directory.GetFiles(outputDir, outputFileName, SearchOption.AllDirectories);

        if (fullPathNames.Length <= 0)
        {
            Debug.LogWarning(string.Format("No Files Have Found with {0}{1}{2}", outputDir, outputFileName, searchFileExtPer));
            return false;
        }

        for (int i = 0; i < fullPathNames.Length; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(fullPathNames[i]);

            FileStream FStream = System.IO.File.Open(fullPathNames[i], FileMode.Open);
            byte[] Buff = new byte[FStream.Length];
            using (BinaryReader br = new BinaryReader(FStream))
            {
                Buff = br.ReadBytes(Buff.Length);
            }

            //Buff = DllTool.EncryptedAssetbundle(Buff);

            FileStream NFStream = System.IO.File.Create(outputDir + fileName + GLOBALCONST.EXTENSION_RELEASEDATA);
            NFStream.Write(Buff, 0, Buff.Length);
            NFStream.Close();
            FStream.Close();

            //刪除Assetbundle檔案
            System.IO.File.Delete(fullPathNames[i]);


            Debug.Log("Success Creat File:" + outputDir + fileName + GLOBALCONST.EXTENSION_RELEASEDATA);
        }

        AssetDatabase.Refresh();


        return true;
    }


	/// <summary>
	/// 打包 assetbundle , 來自選擇到的物件
	/// </summary>
    [MenuItem("Assets/Build AssetBundle One by one(Selected)")]
    static void ExportAssetBundle()
    {
        string outputDir = GLOBALVALUE.EDITOR_RESOURCE_ROOT;
        outputDir = EditorUtility.OpenFolderPanel("Output: " + outputDir, outputDir, "");// User choose Output

        if (outputDir == string.Empty)
        {
            Debug.Log("Cancel bundle assets");
            return;
        }
            
        outputDir = outputDir + "/";        

        string outputFileExt = string.Empty;

        BuildAssetBundle(Selection.activeObject, Selection.objects, outputDir, outputFileExt);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Build AssetBundle All in one(Selected)")]
    static void ExprotAssetBundle_All()
    {
        string outputDir = GLOBALVALUE.EDITOR_RESOURCE_ROOT;
        string path = EditorUtility.SaveFilePanel("Output", outputDir, Selection.activeObject.name, "unity3d");
        if (path.Length == 0)
            return;
        DirectoryInfo DirInfo = new DirectoryInfo(path);

        outputDir = DirInfo.Parent.FullName + "/";
        string outputFileExt = DirInfo.Name;

        BuildAssetBundle(Selection.activeObject, Selection.objects, outputDir, outputFileExt);
        AssetDatabase.Refresh();
        
    }    	
}

