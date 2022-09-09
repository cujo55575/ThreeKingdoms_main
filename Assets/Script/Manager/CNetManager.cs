using System.Net;
using NetWork;
using UnityEngine;
using Common.Engine;
using Common.Player;
using System;
using System.Globalization;
using System.Collections.Generic;

public class CNetManager : ClientNetManager<CNetManager>
{
    //private ClientSocket m_LoginServer = null;  //Login and get GS server ip
    //private ClientSocket m_GameServer = null;   //Lobby, Cultivate
    //private ClientSocket m_BattleServer = null; //Room , Board game part

    //private int m_LoginServer_Port = 11000;
    //private string m_GameServer_IP = string.Empty;
    //private int m_GameServer_Port = 0;
   

    //private Action<bool> m_LSConnectCallback;
    //private Action<bool> m_GSConnectCallback;
    //private Action<bool> m_BSConnectCallback;
  
    public override void Initialize()
    {        
        //base.Initialize();
        //m_LoginServer = new ClientSocket();
        //m_LoginServer.Name = "LoginServer";
        //m_LoginServer.Init(BufferPool.GetBuffer() , BufferPool.GetBuffer() , null);

        //m_GameServer = new ClientSocket();
        //m_GameServer.Name = "GameServer";
        //m_GameServer.Init(BufferPool.GetBuffer(), BufferPool.GetBuffer(), null);        

        //m_BattleServer = new ClientSocket();
        //m_BattleServer.Name = "BattleServer";
        //m_BattleServer.Init(BufferPool.GetBuffer(), BufferPool.GetBuffer(), null);

        //RegisterProtocol(1, 1, LS2C_1_1);//login loginserver
        //RegisterProtocol(1, 2, GS2C_1_2);//login gameserver
		//RegisterProtocol (1, 3, GS2C_1_3);//send player name
       // RegisterProtocol (1, 4, GS2C_1_4);//receive playerdata
  //      RegisterProtocol (1, 5, GS2C_1_5);//receive playerdata
		//RegisterProtocol (6, 1, GS2C_6_1);//get rank
		//RegisterProtocol (6, 2, GS2C_6_2);//get player rank
  //      RegisterProtocol (6, 3, GS2C_6_3);//get player rank
  //      RegisterProtocol(12, 0, GS2C_12_0);//get fixed gache
		//RegisterProtocol(12, 1, GS2C_12_1);//get 1x gache
  //      RegisterProtocol(12, 2, GS2C_12_2);//get 10x gache
    }

    public override void Uninitialize()
    {
        base.Uninitialize();

        //CloseClient(m_LoginServer);
        //CloseClient(m_GameServer);
        //CloseClient(m_BattleServer);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

	//    public void ConnectLoginServer(Action<bool> callback)
	//    {
	//        //IPAddress IP = IPAddress.Parse(GLOBALVALUE.LOGIN_SERVER_URL);
	//        //IPEndPoint EPoint = new IPEndPoint(IP , m_LoginServer_Port);
	//        //Debug.Log("ConnectLoginServer("+EPoint.ToString()+")");
	//        //ConnectCallBackNotify -= ConnectLoginServerCallback;
	//        //ConnectCallBackNotify += ConnectLoginServerCallback;
	//        // m_LSConnectCallback -= callback;
	//        //m_LSConnectCallback += callback;
	//        //StartConnect(m_LoginServer, EPoint);        
	//    }

	//    private void ConnectLoginServerCallback(ClientSocket socket)
	//    {
	//        if (socket == null)
	//            return;
	//        if (socket != m_LoginServer)
	//            return;
	//        ConnectCallBackNotify -= ConnectLoginServerCallback; 

	//        if (socket.WorkSocket.Connected) {
	//			//Debug.Log("ConnectLoginServerCallback success");
	//        } else {
	//			Debug.Log("ConnectLoginServerCallback failed");
	//        }
	//        if (m_LSConnectCallback != null)
	//            m_LSConnectCallback(socket.WorkSocket.Connected);
	//    }

	//    public void CloseLoginServer()
	//    {
	//        CloseClient(m_LoginServer);
	//    }    

	//    public void ConnectGameServer(Action<bool> callback)
	//    {
	//        if (string.IsNullOrEmpty(m_GameServer_IP))
	//            return;
	//        IPAddress IP = IPAddress.Parse(m_GameServer_IP);
	//        IPEndPoint EPoint = new IPEndPoint(IP, m_GameServer_Port);
	//        Debug.Log("ConnectGameServer("+EPoint.ToString()+")");
	//        ConnectCallBackNotify -= ConnectGameServerCallback;
	//        ConnectCallBackNotify += ConnectGameServerCallback;
	//        m_GSConnectCallback -= callback;
	//        m_GSConnectCallback += callback;
	//        StartConnect(m_GameServer, EPoint);
	//    }

	//    private void ConnectGameServerCallback(ClientSocket socket)
	//    {
	//        if (socket == null)
	//            return;
	//        if (socket != m_GameServer)
	//            return;
	//        ConnectCallBackNotify -= ConnectGameServerCallback;

	//        if (socket.WorkSocket.Connected) {
	//			//Debug.Log("ConnectGameServerCallback success");        
	//        } else {
	//			Debug.Log("ConnectGameServerCallback failed");
	//        }
	//        if (m_GSConnectCallback != null)
	//            m_GSConnectCallback(socket.WorkSocket.Connected);
	//    }

	//    private void ConnectBattleServerCallback(ClientSocket socket)
	//    {
	//        if (socket == null)
	//            return;
	//        if (socket != m_BattleServer)
	//            return;
	//        ConnectCallBackNotify -= ConnectBattleServerCallback;
	//        if (socket.WorkSocket.Connected)
	//        {
	//			Debug.Log("ConnectBattleServerCallback success");
	//        }
	//        else
	//        {
	//			Debug.LogError("ConnectBattleServerCallback failed");
	//        }

	//        if (m_BSConnectCallback != null)
	//            m_BSConnectCallback(socket.WorkSocket.Connected);
	//        else
	//            Debug.Log("m_BSConnectCallback is null");
	//    }


	//    private void ConnectBSCallback(bool success)
	//    {
	//		m_BSConnectCallback -= ConnectBSCallback;
	//    }

	//    public void C2LS_1_1(string account , string token , E_Platform platform)
	//    {
	//        Debug.Log("Login to LoginServer by C2LS_1_1()");
	//        m_LoginServer.SetHead(1, 1);
	//        m_LoginServer.SetData(account);
	//        m_LoginServer.SetData(token);
	//        m_LoginServer.SetData((byte)platform);
	//        Send(m_LoginServer);
	//    }

	//    public void LS2C_1_1(NetBuffer buffer)
	//    {
	//        bool LoginResult = buffer.ReadBool();
	//        if (LoginResult)
	//        {
	//            m_GameServer_IP = buffer.ReadString();
	//            m_GameServer_Port = buffer.ReadInt();
	//            ulong PlayerSN = buffer.ReadUlong();
	//            string Token = buffer.ReadString();

	//			Debug.Log(string.Format("LS2C 1_1 LSLoginSuccess - {0}", PlayerSN));

	//            LoginPhase.Instance.LSLoginResult(true , PlayerSN, Token);
	//        }
	//        else
	//        {
	//            int ErrorCode = buffer.ReadInt();

	//            LoginPhase.Instance.LSLoginResult(false, 0, ErrorCode.ToString());

	//			Debug.LogError(string.Format("Errorcode - {0}", ErrorCode));
	//        }
	//    }

	//    public void C2GS_1_2(ulong playerSN , string Token)
	//    {
	//#if OFFLINE_DATA
	//		PlayerDataManager.Instance.LoadPlayerData();
	//		LoginPhase.Instance.GSLoginResult(true);
	//		return;
	//#endif
	//		Debug.Log(string.Format("Login GameServer by using C2GS_1_2({0},{1})", playerSN, Token));
	//        //Login GameServer
	//        //C2GS	1_2	PlayerSN(ulong) + Token(string)
	//        m_GameServer.SetHead(1,2);
	//        m_GameServer.SetData(playerSN);
	//        m_GameServer.SetData(Token);
	//        Send(m_GameServer);
	//    }

	//   // public void GS2C_1_2(NetBuffer buffer)
	//   // {
	//   //     //GS2C    1_2 LoginResult(bool) : true + PlayerData(PlayerData)
	//   //     //                              : false + ErrorCode(int)
	//   //     bool LoginResult = buffer.ReadBool();
	//   //     if (LoginResult)
	//   //     {
	//   //         PlayerData pData = SocketTool.GetData_PlayerData(ref buffer);
	//   //         PlayerDataManager.Instance.PlayerData = pData;
	//			//Debug.Log("GS2C 1_2 GSLoginSuccess !!! PlayerData="+pData);
	//   //         LoginPhase.Instance.GSLoginResult(true);
	//   //     }
	//   //     else
	//   //     {
	//   //         int ErrorCode = buffer.ReadInt();
	//			//Debug.LogError(string.Format("Errorcode {0}", ErrorCode));

	//   //         LoginPhase.Instance.GSLoginResult(false);
	//   //     }
	//   // }

	//	//Send Player Name Result
	//	//					true 
	//	//					false + errorcode(int)
	//	public void GS2C_1_3(NetBuffer buffer)
	//	{
	//		bool success = buffer.ReadBool();
	//		int error = buffer.ReadInt ();
	//        PlayerDataManager.Instance.PlayerData.IsNewPlayer = false;
	//        Debug.Log("GS2C_1_3 result="+success);
	//        if(m_callback!=null) {
	//            m_callback(true);
	//            m_callback = null;
	//        }
	//	}

	//    //Send PlayerData
	//	public void C2GS_1_4(PlayerData pData, Action<bool> callback = null)
	//	{
	//	#if OFFLINE_DATA
	//		PlayerDataManager.Instance.SavePlayerData();
	//		return;
	//	#endif
	//		m_GameServer.SetHead (1, 4);
	//		SocketTool.SetData_PlayerData(ref m_GameServer, pData);
	//        if(callback!=null)
	//            m_callback += callback;
	//		Send (m_GameServer);

	//		Debug.Log ("C2GS_1_4 Send PlayerData : " + pData);
	//	}

	// //   /// <summary>
	// //   /// recieive update PlayerData
	// //   /// </summary>
	// //   /// <param name="buffer"></param>
	// //   public void GS2C_1_4(NetBuffer buffer)
	//	//{
	// //       bool success = buffer.ReadBool();
	// //       if(success) {
	//	//        PlayerData pData = SocketTool.GetData_PlayerData(ref buffer);
	// //           PlayerDataManager.Instance.PlayerData = pData;
	// //           Debug.Log("************** GS2C_1_4 receive PlayerData="+pData);
	// //           UIGacha gacha = UIManager.UIInstance<UIGacha>();
	// //           if (gacha != null)
	// //               gacha.RefreshUI();
	// //       }
	// //       else
	// //       {
	// //           int errorcode = buffer.ReadInt();
	// //           Debug.Log("update PlayerData errorcode="+errorcode);
	// //       }
	//	//}

	//    //delete PlayerData
	//	public void C2GS_1_5(Action<bool> callback = null)
	//	{
	//		m_GameServer.SetHead (1, 5);
	//        if(callback!=null)
	//            m_callback += callback;
	//		Send (m_GameServer);

	//		Debug.Log ("C2GS_1_5 delete PlayerData : ");
	//	}

	//    /// <summary>
	//    /// delete PlayerData result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_1_5(NetBuffer buffer)
	//	{
	//        bool success = buffer.ReadBool();
	//        if(success) {


	//            Debug.Log("************** GS2C_1_5 delete success");
	//            UIGacha gacha = UIManager.UIInstance<UIGacha>();
	//            if (gacha != null)
	//                gacha.RefreshUI();
	//        }
	//        else
	//        {
	//            int errorcode = buffer.ReadInt();
	//            Debug.Log("delete PlayerData error, code="+errorcode);
	//        }
	//	}

	//    private Action<bool> m_callback = null;
	//    /// <summary>
	//    /// get Rank
	//    /// </summary>
	//    /// <param name="kind"></param>
	public void C2GS_6_1(Action<bool> callback = null)
	{
#if OFFLINE_DATA
		Debug.Log("Calledxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
		const string FAKE_PLAYER_SCRIPTABLE_OBJECT_DIRECTORY = "ScriptableObject/Players";
		PlayerObject[] datas = Resources.LoadAll<PlayerObject>(FAKE_PLAYER_SCRIPTABLE_OBJECT_DIRECTORY);
		PlayerDataManager.Instance.RankPlayers = new List<PlayerObject>(datas);

		if (callback != null)
		{
			callback(true);
		}
		return;

#endif
		//m_GameServer.SetHead(6,1);
		//Debug.Log("CNetManager : C2GS_6_1 -");
		//if (callback != null)
		//	m_callback += callback;
		//Send(m_GameServer);
	}

	//    /// <summary>
	//    /// get Rank Result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_6_1(NetBuffer buffer)
	//    {
	//        bool result = buffer.ReadBool();
	//        int count = buffer.ReadInt();

	//        List<PlayerObject> datas = new List<PlayerObject>();
	//        Debug.Log(string.Format("CNetManager : GS2C_6_1 - count = {0}", count));
	//        for (int i = 0; i < count; i++)
	//        {
	//            PlayerObject data = new PlayerObject();
	//            data.Rank = buffer.ReadInt();
	//            data.PlayerSN =  buffer.ReadUlong();
	//            data.Name =  buffer.ReadString();
	//            data.Level = buffer.ReadByte();
	//            data.RankPoint = buffer.ReadInt();
	//            data.WinCount = buffer.ReadInt();
	//            data.LoseCount = buffer.ReadInt();
	//            data.PlayerIconName = buffer.ReadString();
	//            data.OwnedHeros = SocketTool.GetData_OwnedHeros(ref buffer);
	//            data.PlayerHeroArmies = SocketTool.GetData_HeroArmies(ref buffer);
	//            data.SavedArmyFormation = SocketTool.GetData_ArmyFormation(ref buffer);
	//            datas.Add(data);
	//        }

	//        PlayerDataManager.Instance.RankPlayers = datas;
	//        if(m_callback!=null) {
	//            m_callback(true);
	//            m_callback = null;
	//        }
	//        //UIManager.Instance.ShowUI(GLOBALCONST.UI_LEADER_BOARD);
	//    }

	//    /// <summary>
	//    /// get Player Rank
	//    /// </summary>
	//	public void C2GS_6_2(Action<bool> callback = null)
	//    {
	//        m_GameServer.SetHead(6, 2);
	//        Debug.Log(string.Format("CNetManager : C2GS_6_2"));
	//        if(callback!=null)
	//            m_callback += callback;
	//        Send(m_GameServer);
	//    }

	//    /// <summary>
	//    /// get Player Rank result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_6_2(NetBuffer buffer)
	//    {
	//        bool result = buffer.ReadBool();

	//        if(result) {
	//            List<PlayerObject> datas = new List<PlayerObject>();
	//            Debug.Log(string.Format("CNetManager : GS2C_6_2 - success = {0}", result));
	//            PlayerObject data = new PlayerObject();
	//            data.Rank = buffer.ReadInt();
	//            data.PlayerSN =  buffer.ReadUlong();
	//            data.Name =  buffer.ReadString();
	//            data.Level = buffer.ReadByte();
	//            data.RankPoint = buffer.ReadInt();
	//            data.WinCount = buffer.ReadInt();
	//            data.LoseCount = buffer.ReadInt();
	//            data.PlayerIconName = buffer.ReadString();
	//            data.OwnedHeros = SocketTool.GetData_OwnedHeros(ref buffer);
	//            data.PlayerHeroArmies = SocketTool.GetData_HeroArmies(ref buffer);
	//            data.SavedArmyFormation = SocketTool.GetData_ArmyFormation(ref buffer);
	//            PlayerDataManager.Instance.MyRankData = data;
	//        } else
	//        {

	//        }

	//        if(m_callback!=null) {
	//            m_callback(true);
	//            m_callback = null;
	//        }
	//    }


	//    /// <summary>
	//    /// send rank battle result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_6_3(NetBuffer buffer)
	//    {
	//        bool result = buffer.ReadBool();

	//        if(result) {
	//            Debug.Log(string.Format("CNetManager : GS2C_6_3 - success = {0}", result));
	//        } else {

	//        }

	//        if(m_callback!=null) {
	//            m_callback(true);
	//            m_callback = null;
	//        }
	//    }

	//    /// <summary>
	//    /// get fixed gacha result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_12_0(NetBuffer buffer)
	//    {
	//		Debug.Log("12_0 returned from server");
	//        bool result = buffer.ReadBool();
	//        int error = buffer.ReadInt();
	//        if(result) {
	//            List<PlayerHeroData> rewards = SocketTool.GetData_OwnedHeros(ref buffer);
	//            UIGacha gacha = UIManager.UIInstance<UIGacha>();
	//            if (gacha != null)
	//                gacha.ShowReward(rewards);
	//        } else {
	//            Debug.Log("GS2C_12_0 : result - " + result + ", err - " + error);
	//        }
	//    }


	//    /// <summary>
	//    /// buy gacha 1x result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_12_1(NetBuffer buffer)
	//    {
	//		Debug.Log("12_1 returned from server");
	//        bool result = buffer.ReadBool();
	//        int error = buffer.ReadInt();
	//        if(result) {
	//            List<PlayerHeroData> rewards = SocketTool.GetData_OwnedHeros(ref buffer);
	//            /*
	//             * dont do that
	//             * new ownedHeros will receive in GS2C_1_4
	//             * 
	//			for (int i = 0; i < rewards.Count; i++)
	//			{
	//				bool isNew = true;
	//				for (int j = 0; j < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; j++)
	//				{
	//					if (PlayerDataManager.Instance.PlayerData.OwnedHeros[j].key() == rewards[i].key())
	//					{
	//						isNew = false;
	//						PlayerDataManager.Instance.PlayerData.OwnedHeros[j].FragmentCount++;
	//						break;
	//					}
	//				}
	//				if (isNew)
	//				{
	//					PlayerDataManager.Instance.PlayerData.OwnedHeros.Add(rewards[i]);
	//				}
	//			}


	//			C2GS_1_4(PlayerDataManager.Instance.PlayerData);
	//            */

	//            UIGacha gacha = UIManager.UIInstance<UIGacha>();
	//            if (gacha != null)
	//                gacha.ShowReward(rewards);
	//        } else {
	//            Debug.Log("GS2C_12_1 : result - " + result + ", err - " + error);
	//        }
	//    }

	//    /// <summary>
	//    /// buy gacha 10x result
	//    /// </summary>
	//    /// <param name="buffer"></param>
	//    public void GS2C_12_2(NetBuffer buffer)
	//    {
	//       bool result = buffer.ReadBool();
	//        int error = buffer.ReadInt();
	//        if(result) {
	//            List<PlayerHeroData> rewards = SocketTool.GetData_OwnedHeros(ref buffer);

	//            UIGacha gacha = UIManager.UIInstance<UIGacha>();
	//            if (gacha != null)
	//                gacha.ShowReward(rewards);
	//        } else {
	//            Debug.Log("GS2C_12_2 : result - " + result + ", err - " + error);
	//        }
	//    }

}