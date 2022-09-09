using XEResources;

public class PlayPhase : Singleton<PlayPhase>, IMainPhase
{

	private IPlayLevel m_CurrentLevel;
	private bool m_LevelLoadResource = false;
	private E_PlayLevel m_PlayLevel;
	public E_PlayLevel NowPlayLevel
	{
		get
		{
			return m_PlayLevel;
		}
	}


	public void OnClear()
	{
	}

	public void OnStatus()
	{
	}

	public void OnUpdate()
	{
		if (m_CurrentLevel == null)
			return;
		if (m_LevelLoadResource)
		{
			if (ResourceManager.Instance.NowLoading)
				return;
			if (UIManager.Instance.NowLoading)
				return;
			m_LevelLoadResource = false;
			m_CurrentLevel.OnLevel();
			UIManager.Instance.CloseUI(GLOBALCONST.RESOURCE_UI_NAME_LOADING);
		}

		m_CurrentLevel.OnUpdate();
	}

	//切換情境
	public void ChangeLevel(E_PlayLevel level,params object[] Objects)
	{
		m_PlayLevel = level;
		if (m_CurrentLevel != null)
			m_CurrentLevel.OnClear();
		switch (level)
		{
			//case E_PlayLevel.Role:
			//    m_CurrentLevel = CreateRoleLevel.Instance;
			//    break;
			case E_PlayLevel.Menu:
			m_CurrentLevel = MenuLevel.Instance;
			break;
			case E_PlayLevel.Play:
			m_CurrentLevel = PlayLevel.Instance;
			break;
		}
		if (m_CurrentLevel != null)
		{
			UIManager.Instance.ShowUI(GLOBALCONST.RESOURCE_UI_NAME_LOADING);
			m_CurrentLevel.OnLoad();
			m_LevelLoadResource = true;
		}
	}

	public void OnLoad()
	{
		
	}
}
