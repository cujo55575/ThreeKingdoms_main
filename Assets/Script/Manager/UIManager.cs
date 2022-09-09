using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEResources;

public class UIManager : Singleton<UIManager>
{
    private const string NAME_UIROOT = "UIRoot";
    private const string NAME_UIROOT_STATIC = "UIRoot_Static";
    private const string NAME_3DUIROOT = "3DUIRoot";
    private const string NAME_UICAMERA = "UICamera";


    private const string FORMAT_UIPATH = "NUI/{0}";

    private static Dictionary<string, UIBase> m_UIMap = new Dictionary<string, UIBase>();
    private static List<UIBase> m_OpeningUI = new List<UIBase>();
    private static UIBase m_CatchUI;
    private static List<GameObject> m_UIEffect = new List<GameObject>();
    private Transform m_UIRoot;
    private Transform m_UIRootStatic;
    private Transform m_3DUIRoot;
    private Camera m_UICamera;

    public Camera UICamera
    {
        get
        {
            return m_UICamera;
        }
    }

    private void SortUI()
    {
        if (m_OpeningUI == null)
            return;

        int Count = -1;
        for (int i = m_OpeningUI.Count - 1; i >= 0; i--)
        {
            m_OpeningUI[i].SortOrder = Count;
            m_OpeningUI[i].Depth = Count * -10;
            Count--;
        }
        if (m_OpeningUI.Count > 0)
            m_OpeningUI[m_OpeningUI.Count - 1].Focus();
    }

    public override void Initialize()
    {
        base.Initialize();
        GameObject UIRooTObj = GameObject.Find(NAME_UIROOT);
        Object.DontDestroyOnLoad(UIRooTObj);
        m_UIRoot = UIRooTObj.transform;
        CanvasScaler m_UIRootScaler = UIRooTObj.GetComponent<CanvasScaler>();
        m_UIRootScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        m_UIRootScaler.referenceResolution = new Vector2(GLOBALCONST.REFERENCE_RESOLUTION_WIDTH, GLOBALCONST.REFERENCE_RESOLUTION_HEIGHT);

        GameObject UIRootStaticObj = GameObject.Find(NAME_UIROOT_STATIC);
        Object.DontDestroyOnLoad(UIRootStaticObj);
        m_UIRootStatic = UIRootStaticObj.transform;
        CanvasScaler m_UIRootStaticScaler = UIRootStaticObj.GetComponent<CanvasScaler>();
        m_UIRootStaticScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        m_UIRootStaticScaler.referenceResolution = new Vector2(GLOBALCONST.REFERENCE_RESOLUTION_WIDTH, GLOBALCONST.REFERENCE_RESOLUTION_HEIGHT);

        GameObject UI3DRootObj = GameObject.Find(NAME_3DUIROOT);
        Object.DontDestroyOnLoad(UI3DRootObj);
        m_3DUIRoot = UI3DRootObj.transform;

        GameObject UICameraObj = GameObject.Find(NAME_UICAMERA);
        //Object.DontDestroyOnLoad(UICameraObj);
        m_UICamera = UICameraObj.GetComponent<Camera>();
    }

    public bool NowLoading
    {
        get
        {
            if (!IsOpened(GLOBALCONST.RESOURCE_UI_NAME_LOADING))
                return false;
            //UILoading UI = UIInstance<UILoading>();
            //if (UI == null)
            //    return false;
            //if (UI.ProgressValue < 100)
            //    return true;
            return false;
        }
    }

    public void ShowUI(string UIName, params object[] Objects)
    {
        if (!m_UIMap.ContainsKey(UIName))
        {
            UIBase UIAsset = ResourceManager.Instance.Load<UIBase>(string.Format(FORMAT_UIPATH, UIName));
            if (UIAsset == null)
            {
                Debug.LogError(string.Format("load UI failed :{0}", UIName));
                return;
            }
            UIBase UI = Object.Instantiate(UIAsset) as UIBase;
            UI.gameObject.name = UIName;
            if (UI.Is3DUI)
                UI.transform.SetParent(m_3DUIRoot);
            else if (UI.IsStatic)
                UI.transform.SetParent(m_UIRootStatic);
            else
                UI.transform.SetParent(m_UIRoot);
            UI.Init();
            UI.gameObject.SetActive(false);
            m_UIMap.Add(UIName, UI);
        }

        if (m_UIMap[UIName] == null)
        {
            return;
        }
        if (m_OpeningUI.Count > 0)
            m_OpeningUI[m_OpeningUI.Count - 1].Defocus();
        if (m_OpeningUI.Contains(m_UIMap[UIName]))
            m_OpeningUI.Remove(m_UIMap[UIName]);
        m_OpeningUI.Add(m_UIMap[UIName]);
        m_UIMap[UIName].Show(Objects);
        SortUI();
    }

    public void CloseUI(string UIName)
    {
        if (!m_UIMap.ContainsKey(UIName))
            return;
        if (m_UIMap[UIName] == null)
            return;
        if (m_OpeningUI.Contains(m_UIMap[UIName]))
            m_OpeningUI.Remove(m_UIMap[UIName]);
        m_UIMap[UIName].Close();
        SortUI();
    }

    public bool IsOpened(string UIName)
    {
        if (!m_UIMap.ContainsKey(UIName))
            return false;
        return m_UIMap[UIName].Opened;
    }

    public void LoadSceneClear()
    {
        for (int i = m_UIRoot.transform.childCount - 1; i >= 0; i--)
        {
            Transform UI = m_UIRoot.GetChild(i);
            if (UI == null)
                continue;
            if (m_UIMap.ContainsKey(UI.name))
                m_UIMap.Remove(UI.name);
            Object.DestroyObject(UI.gameObject);
        }
        m_OpeningUI.Clear();

        for (int i = 0; i < m_UIEffect.Count; i++)
        {
            GameObject.Destroy(m_UIEffect[i]);
        }
        m_UIEffect.Clear();

    }

    public static T UIInstance<T>() where T : UIBase
    {
        if (m_CatchUI != null && m_CatchUI is T)
            return m_CatchUI as T;

        for (int i = 0; i < m_OpeningUI.Count; i++)
        {
            if (m_OpeningUI[i] is T)
            {
                m_CatchUI = m_OpeningUI[i];
                return m_OpeningUI[i] as T;
            }
        }
        return null;
    }

    public UIBase GetUIBase(string uiName)
    {
        if (!m_UIMap.ContainsKey(uiName))
            return null;
        return m_UIMap[uiName];
    }

    public GameObject PlayUIEffect(int EffectID, GameObject Position = null)
    {
        string EffectPath = string.Format(GLOBALCONST.FORMAT_EFFECTPATH, string.Format(GLOBALCONST.FORMAT_EFFECTNAME, EffectID));
        GameObject EffectAsset = ResourceManager.Instance.Load<GameObject>(EffectPath);
        if (EffectAsset != null)
        {
            GameObject Result = GameObject.Instantiate<GameObject>(EffectAsset);
            if (Position != null)
                Result.transform.SetParent(Position.transform);
            else
                Result.transform.SetParent(null);
            Result.transform.localPosition = Vector3.zero;
            return Result;
        }
        else
            return null;

    }
    public void CloseAllUI()
    {
        for (int i = 0; i < m_OpeningUI.Count; i++)
        {
            m_OpeningUI[i].Close();
        }
    }
}
