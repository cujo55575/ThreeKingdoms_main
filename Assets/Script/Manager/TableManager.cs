using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using XEResources;
using System;
using System.Reflection;

public class TableManager : Singleton<TableManager>
{
    public ResourceListDataTable ResourceListDataTable;
    public HeroDataTable HeroDataTable;
    public PlayerHeroDataTable PlayerHeroDataTable;
    public AttributeLinkDataTable AttributeLinkDataTable;
    public SkillDataTable SkillDataTable;
    public SkillEffectDataTable SkillEffectDataTable;
    public ArmyDataTable ArmyDataTable;
    public ArmyUnlockDataTable ArmyUnlockDataTable;
    public HeroArmyDataTable HeroArmyDataTable;
    public AttributeDataTable AttributeDataTable;
    public LocaleStringDataTable LocaleStringDataTable;
    public LocaleImageDataTable LocaleImageDataTable;
    public CardCountDataTable CardCountDataTable;
    public HeroArmyListDataTable HeroArmyListDataTable;
    public GachaGroupDataTable GachaGroupDataTable;
    public GachaPoolDataTable GachaPoolDataTable;
    public NpcDataTable NpcDataTable;
    public LevelDataTable LevelDataTable;
    public DialogueDataTable DialogueDataTable;
    public TowerModeLevelDataTable TowerModeLevelDataTable;
    public DialogueMessageDataTable DialogueMessageDataTable;
    public DialogueMessageGroupDataTable DialogueMessageGroupDataTable;

    public override void Initialize()
    {
        base.Initialize();
        LoadTable();
    }

    private static T LoadTable<T, F>() where T : TableBase<F>, new() where F : TableDataBase, new()
    {
        string FileName = typeof(F).ToString();
        TextAsset Data = ResourceManager.Instance.LoadTableSync(E_ResourceKind.Table, FileName);
        GLOBALVALUE.IS_USED_ASSETBUNDLE = true;

        if (GLOBALVALUE.IS_USED_ASSETBUNDLE)
        {
            Stream stream = new MemoryStream(Data.bytes);
            IFormatter formatter = new BinaryFormatter();
            T TableObj = (T)formatter.Deserialize(stream);
            TableObj.Init();
            stream.Close();
            Data = null;

            return TableObj;
        }
        else
        {
            T TObj = TableTool.ReadTable<T, F>();
            TObj.Init();
            return TObj;
        }

    }

    //版本、檔案檢查完後 讀檔
    public void LoadTable()
    {
        ResourceListDataTable = LoadTable<ResourceListDataTable, ResourceListData>();
        HeroDataTable = LoadTable<HeroDataTable, HeroData>();
        PlayerHeroDataTable = LoadTable<PlayerHeroDataTable, PlayerHeroData>();
        AttributeLinkDataTable = LoadTable<AttributeLinkDataTable, AttributeLinkData>();
        SkillDataTable = LoadTable<SkillDataTable, SkillData>();
        SkillEffectDataTable = LoadTable<SkillEffectDataTable, SkillEffectData>();
        ArmyDataTable = LoadTable<ArmyDataTable, ArmyData>();
        ArmyUnlockDataTable = LoadTable<ArmyUnlockDataTable, ArmyUnlockData>();
        HeroArmyDataTable = LoadTable<HeroArmyDataTable, HeroArmyData>();
        AttributeDataTable = LoadTable<AttributeDataTable, AttributeData>();
        LocaleStringDataTable = LoadTable<LocaleStringDataTable, LocaleStringData>();
        CardCountDataTable = LoadTable<CardCountDataTable, CardCountData>();
        LocaleImageDataTable = LoadTable<LocaleImageDataTable, LocaleImageData>();
        HeroArmyListDataTable = LoadTable<HeroArmyListDataTable, HeroArmyListData>();
        GachaGroupDataTable = LoadTable<GachaGroupDataTable, GachaGroupData>();
        GachaPoolDataTable = LoadTable<GachaPoolDataTable, GachaPoolData>();
        NpcDataTable = LoadTable<NpcDataTable, NpcData>();
        LevelDataTable = LoadTable<LevelDataTable, LevelData>();
        DialogueDataTable = LoadTable<DialogueDataTable, DialogueData>();
        TowerModeLevelDataTable = LoadTable<TowerModeLevelDataTable, TowerModeLevelData>();
        DialogueMessageDataTable = LoadTable<DialogueMessageDataTable, DialogueMessageData>();
        DialogueMessageGroupDataTable = LoadTable<DialogueMessageGroupDataTable, DialogueMessageGroupData>();

    }
    public class TableTool
    {
        static string SourcePath = Application.dataPath + "/Art/Table/";
        // static string TempFolderPath = GLOBALVALUE.EDITOR_RESOURCE_ROOT + GLOBALCONST.FOLDER_TABLE + "temp";
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
                // EditorUtility.ClearProgressBar();
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

    }
}

