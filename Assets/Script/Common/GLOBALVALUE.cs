using UnityEngine;
using System.Collections;

public class GLOBALVALUE
{
    public static bool IS_USING_OFFLINE_DATA = false;
    public static bool ALL_UNIT_PAUSE = false;   //全操作暫停
    public static bool CAMERA_PAUSE = false;    //設相機暫停
    public static float SOUNDVOLUME = 1.0f;
    public static bool IS_EXTRACT = false;
    public static bool IS_VEERSION_CHECK_COMPLETE = false;
    public static int EXTRACT_PROGRESS = 0;
    public static bool IS_CHATTING = false;
    public static int SELECT_OPTION_TIME = 5;
    public static bool IS_USED_ASSETBUNDLE = true;
    public static PlayerObject CURRENT_ENEMY;
    public static E_MatchMode CURRENT_MATCH_MODE;
    public static string CURRENT_MAP_NAME = "Map_B003";
    public static bool IS_LOGINABLE = true;

    private static E_SystemLanguage m_System_Language = E_SystemLanguage.None;

    public static E_SystemLanguage SYSTEM_LANGUAGE
    {
        get
        {
            if (m_System_Language != E_SystemLanguage.None)
                return m_System_Language;
            try
            {
                m_System_Language = (E_SystemLanguage)PlayerPrefs.GetInt(GLOBALCONST.SAVE_KEY_LANGUAGE, 0);
            }
            catch
            {
                m_System_Language = E_SystemLanguage.None;
            }

            //if never set language , according systemlanguage to set application language
            if (m_System_Language == E_SystemLanguage.None)
            {
                SystemLanguage SystemLanguge = Application.systemLanguage;
                switch (SystemLanguge)
                {
                    case SystemLanguage.ChineseSimplified:
                        m_System_Language = E_SystemLanguage.Simplified_Chinese;
                        PlayerPrefs.SetInt(GLOBALCONST.SAVE_KEY_LANGUAGE, (int)m_System_Language);
                        break;
                    case SystemLanguage.ChineseTraditional:
                        m_System_Language = E_SystemLanguage.Traditional_Chinese;
                        PlayerPrefs.SetInt(GLOBALCONST.SAVE_KEY_LANGUAGE, (int)m_System_Language);
                        break;
                    case SystemLanguage.Japanese:
                        m_System_Language = E_SystemLanguage.Japanese;
                        PlayerPrefs.SetInt(GLOBALCONST.SAVE_KEY_LANGUAGE, (int)m_System_Language);
                        break;
                    default:
                        m_System_Language = E_SystemLanguage.English;
                        PlayerPrefs.SetInt(GLOBALCONST.SAVE_KEY_LANGUAGE, (int)m_System_Language);
                        break;
                }
            }
            return m_System_Language;

        }
        set
        {
            m_System_Language = value;
            PlayerPrefs.SetInt(GLOBALCONST.SAVE_KEY_LANGUAGE, (int)m_System_Language);

        }
    }


    private static ulong m_NOW_SERIALNUMBER = 0;
    public static ulong NEW_SERIALNUMBER
    {
        get
        {
            m_NOW_SERIALNUMBER++;
            return m_NOW_SERIALNUMBER;
        }
    }

    public static bool DEBUG_BOUNDING_BOX_ISOPEN = false;

    public static bool NeedUpdate = true;
    private static string mURL = "http://sjcdn.jingrui-game.com/sjonline/";   //資源更新路徑
    //private static string mURL = "http://120.24.48.184/sjonline/";
    private static string mPlantform = "Official/";
    public static string ProductName = CONSTSTRING.APPLICATION_NAME;


    private static string m_DefaultLoginServerUrl = "52.221.16.117";
    //private static string m_DefaultLoginServerUrl = "127.0.0.1";

    public static string LOGIN_SERVER_URL
    {
        get
        {
            return PlayerPrefs.GetString(GLOBALCONST.SAVE_SERVER_ADDRESS, m_DefaultLoginServerUrl);
        }
        set
        {
            PlayerPrefs.SetString(GLOBALCONST.SAVE_SERVER_ADDRESS, value);
        }
    }

    // TODO delete in real version
    public static int CharacterId = 1;

#if UNITY_ANDROID
    private static string mDevice = GLOBALCONST.FOLDER_PLATFORM_ANDROID;
    //private static string mfolder_Version = /storage/emulated/0/JingRui/;
    //private static string mfolder_Version = Application.streamingAssetsPath;
    //private static string mfolder_Resource = string.Empty;
    /*public static string mfolder_Resource = System.IO.Path.Combine(Application.persistentDataPath, "assets");
    public static string mfolder_Persistent_Path = Application.persistentDataPath;
    public static string mfolder_Data_Path = Application.dataPath;
    public static string android_Current_Version = PlayerPrefs.GetString("version", "0");*/
    private static string mfolder_Resource = Application.streamingAssetsPath;
#elif UNITY_IPHONE    
    private static string mDevice = GLOBALCONST.FOLDER_PLATFORM_IOS;
    //private static string mfolder_Version = string.Empty;
    private static string mfolder_Resource = Application.persistentDataPath;
#else    
    private static string mDevice = GLOBALCONST.FOLDER_PLATFORM_STANDALONE;
    //private static string mfolder_Version = string.Empty;
    private static string mfolder_Resource = Application.streamingAssetsPath;
#endif

#if UNITY_EDITOR
    private static string m_EDITOR_RESOURCE_ROOT = string.Format("{0}/{1}{2}", Application.dataPath, GLOBALCONST.FOLDER_ASSETBUNDLE, mDevice);
    //private static string m_EDITOR_RESOURCE_PUBLISH_ROOT = string.Format("{0}/{1}{2}{3}{4}", Application.dataPath, GLOBALCONST.FOLDER_PUBLISH, mDevice, GLOBALCONST.FOLDER_ASSETBUNDLE);
    //private static string m_EDITOR_VERSION_PUBLISH_ROOT = string.Format("{0}/{1}{2}{3}{4}", Application.dataPath, GLOBALCONST.FOLDER_PUBLISH, mDevice, GLOBALCONST.FOLDER_VERSION);
#endif

    //private static string m_URL_LOGINSERVER_REGISTER = string.Format("{0}{1}", mLoginServerURL, GLOBALCONST.LOGINSERVER_FOLDER_REGISTER);  //登錄服務器域名
    //private static string m_URL_LOGINSERVER_LOGIN = string.Format("{0}{1}", mLoginServerURL, GLOBALCONST.LOGINSERVER_FOLDER_LOGIN);  //登錄服務器域名    

    //private static string m_URL_RESOURCE_ROOT = string.Format("{0}{1}{2}{3}", mURL, mDevice, mPlantform, GLOBALCONST.FOLDER_ASSETBUNDLE);//资源下载服务器目录    
    //private static string m_URL_VERSION_ROOT = string.Format("{0}{1}{2}{3}", mURL, mDevice, mPlantform, GLOBALCONST.FOLDER_VERSION);//资源下载服务器目录    
#if UNITY_ANDROID && !UNITY_EDITOR
    private static string m_FOLDER_RESOURCE_ROOT = System.IO.Path.Combine(mfolder_Resource, GLOBALCONST.FOLDER_CONTENT);
#else
    private static string m_FOLDER_RESOURCE_ROOT = string.Format("{0}/{1}", mfolder_Resource, GLOBALCONST.FOLDER_CONTENT);
#endif
    //private static string m_FOLDER_VERSION_ROOT = string.Format("{0}{1}/", mfolder_Version, BundleIdentifier);

    //    public static string URL_LOGINSERVER_REGISTER
    //    {
    //        get
    //        {
    //            return m_URL_LOGINSERVER_REGISTER;
    //        }
    //    }

    //    public static string URL_LOGINSERVER_LOGIN
    //    {
    //        get
    //        {
    //            return m_URL_LOGINSERVER_LOGIN;
    //        }
    //    }


    //    public static string URL_DYNAMIC_TIME
    //    {
    //        get
    //        {
    //            return string.Format("?time={0}", System.DateTime.Now.Ticks);
    //        }
    //    }

    //    //遠端下載資源的根路徑
    //    public static string URL_RESOURCE_ROOT
    //    {
    //        get
    //        {
    //            return m_URL_RESOURCE_ROOT;
    //        }
    //    }

    //    //遠端下載版本的根路徑
    //    public static string URL_VERSION_ROOT
    //    {
    //        get
    //        {
    //            return m_URL_VERSION_ROOT;
    //        }
    //    }

    //本地儲存資源的根路徑
    public static string FOLDER_RESOURCE_ROOT
    {
        get
        {
#if UNITY_EDITOR
            return m_EDITOR_RESOURCE_ROOT;
#else
            return m_FOLDER_RESOURCE_ROOT;   
#endif
        }
    }

    //    //本地儲存版本的根路徑
    //    public static string FOLDER_VERSION_ROOT
    //    {
    //        get
    //        {
    //            if (string.IsNullOrEmpty(mfolder_Version))
    //                return string.Empty;
    //            else
    //                return m_FOLDER_VERSION_ROOT;
    //        }
    //    }

#if UNITY_EDITOR
    //編輯器工具儲存資源的路徑 未壓縮路徑
    public static string EDITOR_RESOURCE_ROOT
    {
        get
        {
            return m_EDITOR_RESOURCE_ROOT;
        }
    }


    //編輯器工具發布資源的路徑 壓縮路徑
    //public static string EDITOR_RESOURCE_PUBLISH_ROOT
    //{
    //    get
    //    {
    //        return m_EDITOR_RESOURCE_PUBLISH_ROOT;
    //    }
    //}

    //編輯器工具發布資源的路徑 壓縮路徑
    //public static string EDITOR_VERSION_PUBLISH_ROOT
    //{
    //    get
    //    {
    //        return m_EDITOR_VERSION_PUBLISH_ROOT;
    //    }
    //}

#endif


    // Daily Rewards
    public static bool DAILY_REWARDS_CAN_GET = false;
    public static int DAILY_REWARDS_GET_COUNT = 0;
}
