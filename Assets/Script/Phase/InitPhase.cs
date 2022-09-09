using XEResources;
using UnityEngine;

public class InitPhase : Singleton<InitPhase>, IMainPhase
{

	public void OnClear()
	{
	}

	public void OnLoad()
	{
        //Debug.Log("InitPhase.OnLoad()");
		ResourceManager.Instance.Initialize();
		UIManager.Instance.Initialize();
		CNetManager.Instance.Initialize();
	}

	public void OnStatus()
	{
        //Debug.Log("InitPhase.OnStatus()");
        UIManager.Instance.ShowUI(GLOBALCONST.UI_LOGIN);
		//UIManager.Instance.ShowUI(GLOBALCONST.UI_MESSAGEBOX);
        //UIMessageBox.ShowMessageBox("xxxx", E_MessageBox.Yes);
		//	Main.Instance.ChangeGameStatus(E_GameStatus.Version);
	}

	public void OnUpdate()
	{

	}
}
