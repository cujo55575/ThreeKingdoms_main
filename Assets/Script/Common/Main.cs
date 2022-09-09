using UnityEngine;
using XEResources;
public class Main : MonoBehaviour
{

    private static Main m_Instance = null;

    private E_GameStatus m_GameStatus = E_GameStatus.None;
    private IMainPhase m_CurrentPhase = null;
    private bool m_PhaseLoadResource = false;

    public static Main Instance
    {
        get
        {
            return m_Instance;
        }
    }
    public E_GameStatus GameStatus
    {
        get
        {
            return m_GameStatus;
        }
    }


    private void Awake()
    {
        //Initialize
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
#if DEBUG_TOOL
        gameObject.AddComponent<DebugManager>();
#endif
    }

    private void Start()
    {
        ChangeGameStatus(E_GameStatus.Init);
    }

    private void Update()
    {
        UpdateKey();
        ResourceManager.Instance.OnUpdate();
        SoundManager.Instance.OnUpdate();
        CNetManager.Instance.OnUpdate();

        if (m_CurrentPhase == null)
            return;
        if (m_PhaseLoadResource)
        {
            if (ResourceManager.Instance.NowLoading)
                return;
            if (UIManager.Instance.NowLoading)
                return;
            m_PhaseLoadResource = false;
            m_CurrentPhase.OnStatus();
            //   UIManager.Instance.CloseUI(GLOBALCONST.RESOURCE_UI_NAME_LOADING);
            return;
        }

        m_CurrentPhase.OnUpdate();
    }

	void OnApplicationQuit()
	{
		PlayerDataManager.Instance.SavePlayerData();
		Debug.Log("Application ending after " + Time.time + " seconds");
		GLOBALVALUE.IS_LOGINABLE = false;
	}

	public void ChangeGameStatus(E_GameStatus GameStatus)
    {
        m_GameStatus = GameStatus;
        if (m_CurrentPhase != null)
            m_CurrentPhase.OnClear();
        switch (GameStatus)
        {
            case E_GameStatus.Init:
                m_CurrentPhase = InitPhase.Instance;
                break;
            case E_GameStatus.Version:
                m_CurrentPhase = VersionPhase.Instance;
                break;
            case E_GameStatus.Login:
                m_CurrentPhase = LoginPhase.Instance;
                break;
            case E_GameStatus.Play:
                m_CurrentPhase = PlayPhase.Instance;
                break;
            default:
                m_CurrentPhase = null;
                break;
        }

        if (m_CurrentPhase != null)
        {
            m_CurrentPhase.OnLoad();
            m_PhaseLoadResource = true;
            // UIManager.Instance.ShowUI(GLOBALCONST.RESOURCE_UI_NAME_LOADING);
        }
    }

    private void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            //PlayerDataManager.Instance.SavePlayerSave();
        }
    }

    private void UpdateKey()
    {
	
    }
}