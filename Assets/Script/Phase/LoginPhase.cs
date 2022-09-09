using UnityEngine;
using System.Collections;
using System;

public class LoginPhase : Singleton<LoginPhase>, IMainPhase
{
	
    enum E_Status
    {
        None = 0,
        Loading,
        PlatformLogin,    
        ConnectLS,
        LoginLS,
        ConnectGS,
        LoginGS,     
        Waitting,
        Fail
    }



	private E_Status m_Status = E_Status.None;
    private string m_Account = string.Empty;
    private string m_Token = string.Empty;
    private E_Platform m_Platform = E_Platform.None;

	public void OnClear()
	{
        //UIManager.Instance.CloseUI(GLOBALCONST.RESOURCE_UI_NAME_LOGIN);
	}

	public void OnStatus()
	{
        Debug.Log("LoginPhase.OnStatus()");
        UIManager.Instance.CloseAllUI();
        UIManager.Instance.ShowUI(GLOBALCONST.UI_LOGIN);
	}

	public void OnLoad()
	{
        Debug.Log("LoginPhase.OnLoad()");
       // UIManager.Instance.ShowUI(GLOBALCONST.UI_LOGIN);
        //SceneData Data = TableManager.Instance.SceneDataTable.GetData(GLOBALCONST.SCENE_ID_LOGIN);
        //ResourceManager.Instance.LoadScene(Data.SceneName);
	}

	public void OnUpdate()
	{
        //Debug.Log("LoginPhase.OnUpdate("+m_Status+")");
        switch (m_Status)
        {
            case E_Status.PlatformLogin:
                m_Status = E_Status.Waitting;
                PlatformLogin_Callback();
                break;
            case E_Status.ConnectLS:
                m_Status = E_Status.Waitting;
                //CNetManager.Instance.ConnectLoginServer(ConnectLSCallback);
                break;
            case E_Status.LoginLS:
                m_Status = E_Status.Waitting;
               // CNetManager.Instance.C2LS_1_1(m_Account , m_Token , m_Platform);
                break;
            case E_Status.ConnectGS:
                m_Status = E_Status.Waitting;
               // CNetManager.Instance.ConnectGameServer(ConnectGSCallback);
                break;
            case E_Status.LoginGS:
                m_Status = E_Status.Waitting;
                //CNetManager.Instance.C2GS_1_2(PlayerDataManager.Instance.PlayerSN, PlayerDataManager.Instance.Token);
                break;
            case E_Status.Fail:
                m_Status = E_Status.None;
                UIManager.Instance.CloseUI(GLOBALCONST.RESOURCE_UI_NAME_LOADING);
                break;
        }
	}

	public void UserLogin()
	{
        Debug.Log("LoginPhase.UserLogin()");
        m_Status = E_Status.PlatformLogin;
        m_Platform = E_Platform.FB;
        //Todo Call FBSDK to login account;    

        m_Account = SystemInfo.deviceUniqueIdentifier + "_fb1";
		//PlayerDataManager.Instance.LoadPlayerSave(account);
		//Main.Instance.ChangeGameStatus(E_GameStatus.Play);
		//PlayPhase.Instance.ChangeLevel(E_PlayLevel.Menu);
		//PlayerDataManager.Instance.RoleLoginGame();
	}

    //social network login callback
    private void PlatformLogin_Callback()
    {
        //m_Account = SystemInfo.deviceUniqueIdentifier;
        m_Token = string.Format("{0}", DateTime.Now.ToString());
		Debug.Log ("m_Token="+m_Token);
        m_Status = E_Status.ConnectLS;
    }

    //Connect to LS
    private void ConnectLSCallback(bool success)
    {
        //UIMessageBox.ShowMessageBox("xxx!", E_MessageBox.Yes);
        //UIManager.Instance.ShowUI(GLOBALCONST.UI_MESSAGEBOX);
        if (success) {
            m_Status = E_Status.LoginLS;
            Debug.Log("Connect LoginServer SUCCESS");
            //UIMessageBox.ShowMessageBox("Connect LoginServer Success!", E_MessageBox.Yes);
        }   
        else
        {
            m_Status = E_Status.Fail;
            //Todo , hints for connect failed.
			Debug.LogError("Connect LoginServer failed");
            UnityMainThreadDispatcher.Instance().Enqueue(
                () => UIMessageBox.ShowMessageBox("Connect LoginServer Failed!", E_MessageBox.Yes, DoReconnect)
            );
        }
    }

    public void DoReconnect(bool sure, object[] param)
    {
        Debug.Log("doReconnect()");
        m_Status = E_Status.ConnectLS;
    }

    ////LS login result
    //public void LSLoginResult(bool success , ulong playerSN, string token)
    //{
    //    CNetManager.Instance.CloseLoginServer();
    //    if (success)
    //    {
    //        PlayerDataManager.Instance.PlayerSN = playerSN;
    //        PlayerDataManager.Instance.Token = token;

    //        m_Status = E_Status.ConnectGS;
    //    }
    //    else
    //    {            
    //        m_Status = E_Status.None;
    //        UnityMainThreadDispatcher.Instance().Enqueue(
    //            () => UIMessageBox.ShowMessageBox("LoginServer Login Failed! Errorcode="+token, E_MessageBox.Yes, DoReconnect)
    //        );
    //    }        
    //}

    ////connect to gs
    //private void ConnectGSCallback(bool success)
    //{
    //    if (success) {    //Connect SUCCESS
    //        m_Status = E_Status.LoginGS;
    //        Debug.Log("Connect GS SUCCESS");
    //    }
    //    else
    //    {
    //        m_Status = E_Status.Fail;
    //        //Todo , hints for connect failed.
    //        Debug.LogError("Connect GS failed");
    //        UnityMainThreadDispatcher.Instance().Enqueue(
    //            () => UIMessageBox.ShowMessageBox("Connect GameServer Failed!", E_MessageBox.Yes)
    //        );
    //    }
    //}

    ////GS login resutl
    //public void GSLoginResult(bool success)
    //{
    //    m_Status = E_Status.None;
    //    if (success)
    //    {
    //        Debug.Log("GSLoginResult("+success+")");
    //        UIManager.Instance.CloseUI(GLOBALCONST.UI_LOGIN);
    //        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
    //        /*
    //        if (string.IsNullOrEmpty(PlayerDataManager.Instance.PlayerData.Name) || PlayerDataManager.Instance.PlayerData.CharacterBackpack.Characters.Count == 0)   //first time login , Create Character
    //        {
    //            Main.Instance.ChangeGameStatus(E_GameStatus.Play);
    //            PlayPhase.Instance.ChangeLevel(E_PlayLevel.Character);
    //        }
    //        else //enter to menu
    //        {
    //            Main.Instance.ChangeGameStatus(E_GameStatus.Play);
    //            PlayPhase.Instance.ChangeLevel(E_PlayLevel.Menu);
    //        }
    //        */
    //    }
    //    else
    //    {
    //        //UIMessageBox.ShowMessageBox("GameServer Login Failed!", E_MessageBox.Yes);
    //        UnityMainThreadDispatcher.Instance().Enqueue(
    //            () => UIMessageBox.ShowMessageBox("GameServer Login Failed!", E_MessageBox.Yes)
    //        );
    //    }
    //}

}
