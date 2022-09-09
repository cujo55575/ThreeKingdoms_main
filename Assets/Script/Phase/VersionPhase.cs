public class VersionPhase : Singleton<VersionPhase>, IMainPhase
{
	private enum E_Status
	{
		None = 0,
		CheckVersion,
		AskVersion,
		DownloadVersion,
		CheckResource,
		AskResource,
		DownloadResource,
		Finish,
	}

	private E_Status m_Status = E_Status.None;



	public void OnClear()
	{
	}

	public void OnStatus()
	{
		ChangeStatus(E_Status.Finish);
	}

	public void OnUpdate()
	{
	}

	private void ChangeStatus(E_Status Status)
	{
		switch (Status)
		{
			case E_Status.CheckVersion:
			break;
			case E_Status.AskVersion:
			break;
			case E_Status.DownloadVersion:
			break;
			case E_Status.CheckResource:
			break;
			case E_Status.AskResource:
			break;
			case E_Status.DownloadResource:
			break;
			case E_Status.Finish:
			TableManager.Instance.LoadTable();  //讀取表格
			PlayerDataManager.Instance.Initialize();
			Main.Instance.ChangeGameStatus(E_GameStatus.Login); //登入

			break;
		}
	}

	public void OnLoad()
	{
	}
}
