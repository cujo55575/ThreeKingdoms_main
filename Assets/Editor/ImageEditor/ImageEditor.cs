using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ImageEditor : EditorWindow
{
    //Scroll View
    public Vector2 ScrollPos;
    public List<string> ScrollObj;
    public bool IsImport=true;
    public bool IsRename;
    public bool IsAddToResourceManager;
    public bool IsAddToGlobalConstant;
    public bool IsLocalizeImage;
    public int SpacingX;
    public int SpacingXS;

    public ImageType ImageTypeOption;
    public SaveLocation SaveLocationOption;
    public NameRule NameRuleOption;
    public int CustomNameSelected;

    public string NewCustomName;
    public string GlobalConstantName;
    public string GLobalConstantStringFormat;

    public string TargetPath;
    public static string ImageType = "Default";

    public int CopyProgress=-1;
    char TableSeparator ='\t' ;

    private string TABLE_PATH;
    private string EDITOR_DATA_PATH;
    private string GLOBALCONSTANT_DATA_PATH;

    private const string RESOURCE_LIST_DATA_TABLE = "ResourceListData.txt";
    private const string LOCAL_IMAGE_TABLE = "LocaleImageData.txt";
    private const string LOCAL_STRING_TABLE = "LocalStringData.txt";
    private const string LOCAL_TEST_TABLE = "Test.txt";
    private List<String> CustomNameList;
    private int LastLine;
    private int LastIndex;

    private bool IsAnyError = false;
    private string Message;

    [MenuItem("GameFactory/ImageImport")]
    public static void CreateMenuEditor()
    {
        ImageEditor ImageImport = (ImageEditor)EditorWindow.CreateInstance<ImageEditor>() as ImageEditor;
        ImageImport.titleContent = new GUIContent("Image Import Tool");
        ImageImport.minSize = new Vector2(Screen.width / 2, Screen.height);

        ImageImport.OnInit();
        ImageImport.Focus();
        ImageImport.Show();
    }

    private void OnInit()
    {
        ScrollObj = new List<string>();
        SpacingX = 15;
        SpacingXS = 5;
        TABLE_PATH = Application.dataPath + "/Table/";
        EDITOR_DATA_PATH = Application.dataPath + "/Editor/ImageEditor/data.z";
        GLOBALCONSTANT_DATA_PATH = Application.dataPath + "/Script/Common/GLOBALCONST.cs";
        CustomNameList = (List<string>)ReadDataObject();

        TargetPath = Application.dataPath + "\\Resources\\Image\\";
    }

    private void OnGUI()
    {
        /***** Image List Group ***************/
        GUILayout.BeginArea(new Rect(10, 10, 600, Screen.height));
        GUILayout.BeginVertical();

        /////   Import  ////
        IsImport = GUILayout.Toggle(IsImport, "Import Image");
        if (IsImport)
        {
            GUILayout.Space(SpacingXS);
            ImageTypeOption = (ImageType)EditorGUILayout.EnumPopup("Select Image Type",  ImageTypeOption,GUILayout.Width(300));
            SaveLocationOption = (SaveLocation)EditorGUILayout.EnumPopup("Select Location", SaveLocationOption, GUILayout.Width(300));
            ImageType = ImageTypeOption.ToString();
        }
        GUILayout.Space(SpacingX);

        /////   Rename    /////
        IsRename = GUILayout.Toggle(IsRename, "Rename");
        if (IsRename)
        {
            GUILayout.Space(SpacingXS);
            NameRuleOption = (NameRule)EditorGUILayout.EnumPopup("Select Name Rule",NameRuleOption,GUILayout.Width(300));
            CustomNameSelected = EditorGUILayout.Popup("Select Name", CustomNameSelected, CustomNameList.ToArray(), GUILayout.Width(300));
            if (CustomNameSelected == 0)
            {
                NewCustomName = EditorGUILayout.TextField("Enter Name", NewCustomName, GUILayout.Width(300));
                if (GUILayout.Button("Update",GUILayout.Width(100), GUILayout.Height(20)))
                {
                    if (NewCustomName != null || NewCustomName.Trim() != "")
                    {
                        int lastindex = CustomNameList.Count;
                        CustomNameList.Add(NewCustomName);
                        WriteDataObject(CustomNameList);
                        NewCustomName = "";
                        CustomNameSelected = lastindex;
                        Repaint();
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                if (GUILayout.Button("Delete", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    CustomNameList.RemoveAt(CustomNameSelected);
                    CustomNameSelected--;
                    WriteDataObject(CustomNameList);
                    Repaint();
                }
            }
          
        }
        GUILayout.Space(SpacingX);

        /////   Add To Resource Data   /////
        //IsAddToResourceManager = GUILayout.Toggle(IsAddToResourceManager, "Add List to ResourceManager");
        //if (IsAddToResourceManager)
        //{
        //    GUILayout.Box("Resource");
        //    GUILayout.Label("Resource ");
        //}
        //GUILayout.Space(SpacingX);

        /////   Add To GlobalConstant  ///
        IsAddToGlobalConstant = GUILayout.Toggle(IsAddToGlobalConstant, "Add To GlobalConstant");
        if (IsAddToGlobalConstant)
        {
            if (GlobalConstantName != null)
                GlobalConstantName = GlobalConstantName.ToUpper();

            GlobalConstantName = EditorGUILayout.TextField("GlobalConstant Name", GlobalConstantName, GUILayout.Width(300));
           
            if (ScrollObj.Count > 1 && CustomNameSelected!=0)
            {
                if(NameRuleOption == NameRule.Localized)
                    GLobalConstantStringFormat = NameStringFormat.LOCALIZE_NAME_FORMAT.Replace("{0}", CustomNameList[CustomNameSelected]);
                else
                    GLobalConstantStringFormat = NameStringFormat.CUSTOM_NAME_FORMAT.Replace("{0}", CustomNameList[CustomNameSelected]);

                GLobalConstantStringFormat = EditorGUILayout.TextField("GlobalConstant Format", GLobalConstantStringFormat, GUILayout.Width(300));
            }
        }
        else
        {
            GLobalConstantStringFormat = "";
        }
        GUILayout.Space(SpacingX);

        /////   Save /////
        if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (!IsFileUsed())
            {
                CopyImages(ScrollObj, Path.Combine(TargetPath, SaveLocationOption.ToString()));
                if (IsAddToGlobalConstant && !string.IsNullOrEmpty(GlobalConstantName) && !string.IsNullOrEmpty(GLobalConstantStringFormat))
                {
                    try
                    {
                        var txtLines = File.ReadAllLines(GLOBALCONSTANT_DATA_PATH).ToList();   //Fill a list with the lines from the txt file.

                        string dataString = string.Format(NameStringFormat.GLOBALCONSTANT_SAVE_FORMAT, GlobalConstantName, GLobalConstantStringFormat);
                        bool isDuplicate = txtLines.Contains(dataString);
                        if (!isDuplicate)
                        {
                            int lastLine = txtLines.FindLastIndex(x => x.Contains('}'));
                            txtLines.Insert(lastLine, dataString);
                            WriteDataString(GLOBALCONSTANT_DATA_PATH, txtLines, false);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "File are opened", "OK");
            }
        }

        ////    Test    ///
        if (GUILayout.Button("Test", GUILayout.Width(100), GUILayout.Height(30)))
        {
            //  Debug.Log( GetMaximumUnit(LOCAL_TEST_TABLE, 0).ToString() );
            //List<string> str = new List<string>();
            //str.Add(string.Format(NameStringFormat.RESOURCE_LIST_SAVE_FORMAT, 11111, "bbbb", 0, "aaaa"));
            //str.Add(string.Format(NameStringFormat.RESOURCE_LIST_SAVE_FORMAT, 11112, "cccc", 0, "ddd"));
            //str.Add(string.Format(NameStringFormat.RESOURCE_LIST_SAVE_FORMAT, 11113, "fff", 0, "ee"));
            //WriteDataString(RESOURCE_LIST_DATA_TABLE, str);
            //string[] str = NameStringFormat.LOCALIZE_NAME_FORMAT.Split('}');
            //str = NameStringFormat.LOCALIZE_NAME_FORMAT.Split('{');
            // int i = 0;
            // string str = "Hero_00001";
            // Debug.Log(Regex.IsMatch(str,@"\AHero_\d"));
            // int lastline;
            // int lastIndex;
            // GetMaximumUnit(RESOURCE_LIST_DATA_TABLE, 0,1,out lastline,out lastIndex);
            //List<string> tmp = GetRename();
            //foreach(string str in tmp)
            //    Debug.Log(str);
            //string str = NameStringFormat.LOCALIZE_NAME_FORMAT.Replace("{0}", "H");
            //Debug.Log(str);
           // List<string> str = new List<string>();
           // str.Add(string.Format(NameStringFormat.GLOBALCONSTANT_SAVE_FORMAT, "Test","Hero"));
           // var txtLines = File.ReadAllLines(GLOBALCONSTANT_DATA_PATH).ToList();   //Fill a list with the lines from the txt file.
           // int tmp2 = txtLines.FindLastIndex(x=>x.Contains('}'));
           // int tmp = txtLines.LastIndexOf("}\t");
           // txtLines.Insert(tmp, string.Format(NameStringFormat.GLOBALCONSTANT_SAVE_FORMAT, "Test", "Hero"));  //Insert the line you want to add last under the tag 'item1'.
           //// File.WriteAllLines(GLOBALCONSTANT_DATA_PATH, txtLines.ToArray());
           // WriteDataString(GLOBALCONSTANT_DATA_PATH, txtLines,false);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();


        /***** Image List Group ***************/
        
        GUI.Label(new Rect((Screen.width - 400), 10, 300, 30), ScrollObj.Count>0?"Total Image -"+ScrollObj.Count:"");
        if (GUI.Button(new Rect((Screen.width - 105), 5, 100, 25), "Clear"))
        {
            ScrollObj = new List<string>();
            Repaint();
        }

        GUI.BeginGroup(new Rect((Screen.width - 400), 40, 400, Screen.height));

        GUI.Box(new Rect(0, 0, 400, Screen.height), ScrollObj.Count==0?"Drag and Drop Image Here!":"");

        /***** Scroll View *********/
        EditorGUILayout.BeginVertical();
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Width(400), GUILayout.Height(Screen.height-50));

        for(int i = 0; i < ScrollObj.Count; i++)
        {
            GUILayout.Label(ScrollObj[i]);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        /***** Drag and Drop ********/
        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        if (Event.current.type == EventType.DragExited)
        {
            for (int i = 0; i < DragAndDrop.paths.Length; i++)
            {
                ScrollObj.Add(DragAndDrop.paths[i]);//sprites.Add(DragAndDrop.objectReferences[i].name);
            }
            Repaint();
        }
       
        GUI.EndGroup();

        
    }

    private float CalculatePercentage(int current)
    {
        return current / ScrollObj.Count;
    }

    private string GetFormatString()
    {
        string m_formatStr="";

        return m_formatStr;
    }

    private void CopyImages(List<string> m_sourcePathList,string m_targetPath)
    {
        string tmpTargetFileName;
        string tmpTargetFilePath;
        List<string> fileNameList;
        List<string> tmpDataLineList = new List<string>();

        try
        {
            if (!Directory.Exists(m_targetPath))
            {
                Directory.CreateDirectory(m_targetPath);
            }

            fileNameList = GetRename();
            for (int i = 0; i < m_sourcePathList.Count; i++)
            {
                tmpTargetFileName = IsRename?fileNameList[i]:Path.GetFileName(m_sourcePathList[i]);
                tmpTargetFilePath = Path.Combine(m_targetPath, tmpTargetFileName);
                File.Copy(m_sourcePathList[i], tmpTargetFilePath);
                if (IsRename)
                {
                    string tmpDataLine = string.Format(NameStringFormat.RESOURCE_LIST_SAVE_FORMAT, ++LastLine, Path.GetFileNameWithoutExtension(fileNameList[i]), 0, Path.GetFileNameWithoutExtension(fileNameList[i]));
                    tmpDataLineList.Add(tmpDataLine);
                }
                else
                {
                    string tmpDataLine = string.Format(NameStringFormat.RESOURCE_LIST_SAVE_FORMAT, ++LastLine, Path.GetFileNameWithoutExtension(m_sourcePathList[i]), 0, Path.GetFileNameWithoutExtension(m_sourcePathList[i]));
                    tmpDataLineList.Add(tmpDataLine);
                }
                EditorUtility.DisplayProgressBar("Copying File,Please wait a second", "Copy Progress", CalculatePercentage(i+1));
            }

            /* Add To ResourceListData Table */           
            WriteDataString(RESOURCE_LIST_DATA_TABLE, tmpDataLineList);

        }catch(Exception e)
        {
            Debug.Log(e.Message);
           
        }
        EditorUtility.ClearProgressBar();
    }

    private void GetMaximumUnit(string m_Resource,int m_LineIndex,int m_ValueIndex,out int lastLine,out int lastIndex)
    {
        lastLine    = 0;
        lastIndex   = 0;
        string pattern  = @"\A" + CustomNameList[CustomNameSelected] + @"_\d"; ;

        FileStream FS   = null;
        StreamReader SR = null;
        m_Resource  = Path.Combine(TABLE_PATH, m_Resource);

        if (File.Exists(m_Resource))
        {
            FS = new FileStream(m_Resource, FileMode.Open);
            SR = new StreamReader(FS, Encoding.Unicode);
            
            SR.ReadLine();
            while (!SR.EndOfStream)
            {
                string dataLine = SR.ReadLine();
                string[] dataList = dataLine.Split(TableSeparator);
                int tmp;
                if (int.TryParse(dataList[m_LineIndex], out tmp))
                {
                    if (tmp > lastLine)
                        lastLine = tmp;
                }
                tmp = 0;
                if (Regex.IsMatch(dataList[m_ValueIndex], pattern))
                {
                    string str = dataList[m_ValueIndex].Split('_')[1];

                    if (int.TryParse(str, out tmp))
                    {
                        if (tmp > lastIndex)
                            lastIndex = tmp;
                    }
                }

            }
            SR.Close();
            FS.Close();
        }

    }

    private void WriteDataString(string m_Resource,List<string> data,bool m_isUnicode=true)
    {
        m_Resource = Path.Combine(TABLE_PATH, m_Resource);
        StreamWriter sw;
        if(!m_isUnicode)
            sw = File.CreateText(m_Resource);
        else 
            sw = new StreamWriter(m_Resource, true, Encoding.Unicode);

        for(int i = 0; i < data.Count; i++)
        {
            sw.WriteLine(data[i]);
        }
        sw.Flush();
        sw.Close();
    }
    private void WriteDataObject(System.Object obj)
    {
        using (Stream stream = File.Open(EDITOR_DATA_PATH, FileMode.Create))
        {
            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            bformatter.Serialize(stream, obj);
            stream.Flush();
            stream.Close();
        }
    }
    private System.Object ReadDataObject()
    {
        if (!File.Exists(EDITOR_DATA_PATH))
        {
            CustomNameList = new List<string>();
            CustomNameList.Add("Create New");
            WriteDataObject(CustomNameList);
        }

        using (Stream stream = File.Open(EDITOR_DATA_PATH, FileMode.Open))
        {
            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var obj = bformatter.Deserialize(stream);
            stream.Close();
            return obj;
        }
    }

    private List<string> GetRename()
    {
        LastLine = 1;
        LastIndex = 0;
        List<string> fileList = new List<string>();

        string m_NameRule;

        GetMaximumUnit(RESOURCE_LIST_DATA_TABLE, 0, 1, out LastLine, out LastIndex);
        if (NameRuleOption == NameRule.Localized)
        {

        }
        else
        {
            foreach(string str in ScrollObj)
            {
                string fileFormat = Path.GetExtension(str);
                string fileName = String.Format(NameStringFormat.CUSTOM_NAME_FORMAT, CustomNameList[CustomNameSelected],++LastIndex);
                fileList.Add(fileName + fileFormat);
            }
        }
        return fileList;
    }

    protected bool IsFileUsed()
    {
        FileInfo fileResourceTable = new FileInfo(Path.Combine(TABLE_PATH, RESOURCE_LIST_DATA_TABLE));
        FileInfo fileGlobalConstant = new FileInfo(Path.Combine(TABLE_PATH, GLOBALCONSTANT_DATA_PATH));
        FileInfo fileLocalImageTable = new FileInfo(Path.Combine(TABLE_PATH, LOCAL_IMAGE_TABLE));

        FileStream stream = null;
        try
        {
            stream = fileResourceTable.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
            stream = fileGlobalConstant.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
            stream = fileLocalImageTable.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            return true;
        }
      
        return false;
    }
}


public enum ImageType : byte
{
    Sprite_2D,
    None,
}
public enum SaveLocation : byte
{
   Sprite,
   Icon,
   Frame,
}
public enum NameRule : byte
{
    Localized,
    Custom,
}

class NameStringFormat
{
    public static string LOCALIZE_NAME_FORMAT = "{0}_{1:00}{2:0000}";
    public static string CUSTOM_NAME_FORMAT = "{0}_{1:0000}";

    public static string RESOURCE_LIST_SAVE_FORMAT = "{0}\t{1}\t{2}\t{3}";
    public static string LOCALIZE_IMAGE_SAVE_FORMAT = "{0}\t{1}\t{2}\t{2}";
    public static string GLOBALCONSTANT_SAVE_FORMAT = "\tpublic const string FORMAT_{0} = \"{1}\";";
}


