using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;
using System.Text;
using System;


namespace XEResources
{
    public class TableTool
    {
        static string SourcePath = Application.dataPath + "/Art/Table/";
		static string TempFolderPath = GLOBALVALUE.EDITOR_RESOURCE_ROOT + GLOBALCONST.FOLDER_TABLE + "temp";
		static string OutputTableDir = GLOBALVALUE.EDITOR_RESOURCE_ROOT + GLOBALCONST.FOLDER_TABLE;
		static char[] TableSeparator = new char[] { '\t' };
        const string STAR_FORMAT = "*{0}";

        public static T ReadTable<T, F>() where T : TableBase<F>, new() where F : TableDataBase, new()
        {
            Stream stream = null;
            FileStream FS = null;
            StreamReader SR = null;
            Type type = typeof(F);  //取得表格資料類型
            string FileName = type.ToString();

            // for debug
            int row = 0;
            int column = 0;
            try
            {
                FS = new FileStream(string.Format("{0}/{1}.txt", SourcePath, FileName), FileMode.Open);
                if (FS == null)
                {
                    Debug.LogError(string.Format("No Found File:{0}", FileName));
                    return null;
                }
                SR = new StreamReader(FS, Encoding.Unicode);

                string Info = SR.ReadLine();    //第一行中文說明        
                string parameter = SR.ReadLine();       //取第二行變數名
                string[] parameters = parameter.Split(TableSeparator);   //表格上的變數名
                T TObj = new T();       //實例化表格組
                while (!SR.EndOfStream) //讀檔
                {
                    string dataLine = SR.ReadLine();
                    if (string.IsNullOrEmpty(dataLine)) //空行
                        continue;

                    row++;

                    F FObj = new F();   //新增一個實例物件
                    string[] datas = dataLine.Split(TableSeparator);   //表格上的變數名
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (string.IsNullOrEmpty(parameters[i]))
                            continue;

                        column = i + 1;

                        FieldInfo fieldInfo = type.GetField(parameters[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fieldInfo == null)
                            continue;
                        switch (fieldInfo.FieldType.Name.ToLower())
                        {
                            case "byte":
                                fieldInfo.SetValue(FObj, byte.Parse(datas[i]));
                                break;
                            case "int16":
                                fieldInfo.SetValue(FObj, short.Parse(datas[i]));
                                break;
                            case "int32":
                                fieldInfo.SetValue(FObj, int.Parse(datas[i]));
                                break;
                            case "int64":
                                fieldInfo.SetValue(FObj, long.Parse(datas[i]));
                                break;
                            case "uint16":
                                fieldInfo.SetValue(FObj, ushort.Parse(datas[i]));
                                break;
                            case "uint32":
                                fieldInfo.SetValue(FObj, uint.Parse(datas[i]));
                                break;
                            case "uint64":
                                fieldInfo.SetValue(FObj, ulong.Parse(datas[i]));
                                break;
                            case "single":
                                fieldInfo.SetValue(FObj, float.Parse(datas[i]));
                                break;
                            case "double":
                                fieldInfo.SetValue(FObj, double.Parse(datas[i]));
                                break;
                            case "boolean":
                                fieldInfo.SetValue(FObj, bool.Parse(datas[i]));
                                break;
                            case "string":
                                if (datas.Length > i)
                                    fieldInfo.SetValue(FObj, datas[i]);
                                else
                                    fieldInfo.SetValue(FObj, string.Empty);
                                break;
                            default:
                                Debug.LogError(string.Format("type error {0}", fieldInfo.FieldType.Name));
                                return null;
                        }
                    }
                    TObj.AddData(FObj);
                }

                return TObj;

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + " at row - " + row + ", column - " + column);
                Debug.LogError(e.StackTrace);
                Debug.LogError(string.Format("SaveTable Error !! {0}/{1}.txt", SourcePath, FileName));
                EditorUtility.ClearProgressBar();
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (FS != null)
                    FS.Close();
                if (SR != null)
                    SR.Close();
            }

            return null;
        }

        static Stream GetTable<T, F>(Stream stream) where T : TableBase<F>, new() where F : TableDataBase, new()
        {
            Type type = typeof(F);  //取得表格資料類型
            string FileName = type.ToString();

            T TObj = ReadTable<T, F>();
            if (TObj != null)
            {
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, TObj);

            }
            else
            {
                Debug.LogError("Load Table Failed - " + FileName);
            }

            return stream;
        }

        static void SaveTable<T, F>() where T : TableBase<F>, new() where F : TableDataBase, new()
        {
            Type type = typeof(F);  //取得表格資料類型
            string FileName = type.ToString();

            T TObj = ReadTable<T, F>();
            if (TObj != null)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(string.Format("{0}/{1}.bytes", OutputTableDir, FileName), FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, TObj);

                if (stream != null)
                    stream.Close();
            }
            else
            {
                Debug.LogError("Load Table Failed - " + FileName);
            }
        }

        //[MenuItem("Tools/Build/0 Build All Table")]
        public static void SaveAllTable()
        {
            // Delete All files first.
            
            if (Directory.Exists(OutputTableDir)) { Directory.Delete(OutputTableDir, true); }
            Directory.CreateDirectory(TempFolderPath);

			EditorUtility.DisplayProgressBar("Build All Table", "ResourceListData", 1);
            SaveTable<ResourceListDataTable, ResourceListData>();

			EditorUtility.DisplayProgressBar("Build All Table","HeroData",1);
			SaveTable<HeroDataTable,HeroData>();

			EditorUtility.DisplayProgressBar("Build All Table","PlayerHeroData",1);
			SaveTable<PlayerHeroDataTable,PlayerHeroData>();

			EditorUtility.DisplayProgressBar("Build All Table","AttributeLinkData",1);
			SaveTable<AttributeLinkDataTable,AttributeLinkData>();

			EditorUtility.DisplayProgressBar("Build All Table","SkillSData",1);
			SaveTable<SkillDataTable,SkillData>();

			EditorUtility.DisplayProgressBar("Build All Table","SkillEffectData",1);
			SaveTable<SkillEffectDataTable,SkillEffectData>();

			EditorUtility.DisplayProgressBar("Build All Table","ArmyData",1);
			SaveTable<ArmyDataTable,ArmyData>();

			EditorUtility.DisplayProgressBar("Build All Table","ArmyUnlockData",1);
			SaveTable<ArmyUnlockDataTable,ArmyUnlockData>();

			EditorUtility.DisplayProgressBar("Build All Table","HeroArmyData",1);
			SaveTable<HeroArmyDataTable,HeroArmyData>();

			EditorUtility.DisplayProgressBar("Build All Table","AttributeData",1);
			SaveTable<AttributeDataTable,AttributeData>();

			EditorUtility.DisplayProgressBar("Build All Table","LocalStringData",1);
			SaveTable<LocaleStringDataTable,LocaleStringData>();

			EditorUtility.DisplayProgressBar("Build All Table","CardCountData",1);
			SaveTable<CardCountDataTable,CardCountData>();

			EditorUtility.DisplayProgressBar("Build All Table","LocaleImageData",1);
			SaveTable<LocaleImageDataTable,LocaleImageData>();

			EditorUtility.DisplayProgressBar("Build All Table","KingdomArmyListData",1);
			SaveTable<HeroArmyListDataTable,HeroArmyListData>();

			EditorUtility.DisplayProgressBar("Build All Table","GachaGroupData",1);
			SaveTable<GachaGroupDataTable,GachaGroupData>();

			EditorUtility.DisplayProgressBar("Build All Table","GachaPoolData",1);
			SaveTable<GachaPoolDataTable,GachaPoolData>();

			EditorUtility.DisplayProgressBar("Build All Table","NpcData",1);
			SaveTable<NpcDataTable,NpcData>();

			EditorUtility.DisplayProgressBar("Build All Table","LevelData",1);
			SaveTable<LevelDataTable,LevelData>();

			EditorUtility.DisplayProgressBar("Build All Table","DialogueData",1);
			SaveTable<DialogueDataTable,DialogueData>();

			EditorUtility.DisplayProgressBar("Build All Table","TowerModeLevelData",1);
			SaveTable<TowerModeLevelDataTable,TowerModeLevelData>();

			EditorUtility.DisplayProgressBar("Build All Table","DialogueMessageData",1);
			SaveTable<DialogueMessageDataTable,DialogueMessageData>();

			EditorUtility.DisplayProgressBar("Build All Table","DialogueMessageGroupData",1);
			SaveTable<DialogueMessageGroupDataTable,DialogueMessageGroupData>();

			AssetDatabase.Refresh();
            EditorUtility.DisplayProgressBar("Build All Table", " Build All Table To Assetbundle", 0.5f);

            string SearchDir = OutputTableDir.Replace("\\", "/");
            SearchDir = SearchDir.Replace(Application.dataPath + "/", GLOBALCONST.FOLDER_ASSETS);
            string SearchFileExt = string.Format(STAR_FORMAT, GLOBALCONST.EXTENSION_RELEASEDATA);

            AssetBundleTools.BuildAssetBundle(SearchDir + "/", SearchFileExt, OutputTableDir);    //輸出為空代表檔案各自打包

            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
			if (Directory.Exists(TempFolderPath))
			{
				Directory.Delete(TempFolderPath,true);
			}
            Debug.Log("Build All Table Success");

        }

       // [MenuItem("Assets/Open in TableEditor")]
        static void OpenInTextEditor()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(Selection.activeObject));
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //process.StartInfo = new System.Diagnostics.ProcessStartInfo("/Applications/TextEdit.app", "--args " + filePath);
            ////process.StartInfo.CreateNoWindow = true;
            //process.Start();
            System.Diagnostics.Process.Start("/Applications/TableEditor.app", "--args " + filePath);
        }

        [MenuItem("Assets/Open in TableEditor", true)]
        static bool ValidateOpenInTextEditor()
        {
            return AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith(".txt", System.StringComparison.OrdinalIgnoreCase);
        }
    }

}

