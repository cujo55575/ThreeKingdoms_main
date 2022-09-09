using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Common.Net;

namespace NetWork
{
    public abstract class ClientNetManager<T> : Singleton<T>
    {
        private int m_BufferPoolSize;
        private HandleProtocol mHP;
        private PacketHandler mPacketHandler;
        private BufferPool m_BufferPool;

        protected BufferPool BufferPool
        {
            get { return m_BufferPool; }
        }

        protected int BufferPoolSize
        {
            set { m_BufferPoolSize = value; }
        }

        public ClientNetManager()
        {
            mHP = new HandleProtocol();
            mPacketHandler = new PacketHandler(mHP);
            m_BufferPool = NetBuffer.BufferPool;
        }

        public Action<ClientSocket> ConnectCallBackNotify;        

        //連接回調
        private void ConnectCallback(IAsyncResult asyncResult)
        {
            // Retrieve the socket from the state object.  
            ClientSocket Client = asyncResult.AsyncState as ClientSocket;
			//Debug.Log (string.Format("ConnectCallback from {0}", Client.Name));
            try
            {                
                // Complete the connection.  
                Client.WorkSocket.EndConnect(asyncResult);                
                //Common.Engine.Tool.ShowInfo(string.Format("Socket connected to {0}",Client.WorkSocket.RemoteEndPoint.ToString()));
                
                if (Client.WorkSocket.Connected)
                {                    
                    Client.ConnectCount = 0;
                    Client.Status = E_ConnectStatus.None;

                    Client.WorkSocket.BeginReceive(Client.ReceiveData.Buffer, 0, Client.ReceiveData.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), Client);

                    if (ConnectCallBackNotify != null)
                        ConnectCallBackNotify(Client);
                }                    
                else
                {
                    if (Client.ConnectCount >= Const.RECONNECT_TIMES)
                    {
                        Client.Status = E_ConnectStatus.Failed;
                        Client.ConnectCount = 0;

                        if (ConnectCallBackNotify != null)
                            ConnectCallBackNotify(Client);
                        return;
                    }
                    Client.ConnectCount++;
                    StartConnect(Client , Client.RemoteEnd);
                }                
            }
            catch (Exception e)
            {
				Debug.LogError(string.Format("Error={0}", e.Message));

                //if (Client.ConnectCount >= Const.RECONNECT_TIMES)
                //{
                Client.Status = E_ConnectStatus.Failed;
                //Client.ConnectCount = 0;

				if (ConnectCallBackNotify != null) {
					//other thread to main thread call
					UnityMainThreadDispatcher.Instance().Enqueue(
						() => ConnectCallBackNotify (Client)
					);
				}
				Client.ConnectCount++;
            }
        }
        //接收訊息回調
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
			//Debug.Log("ReceiveCallback protocol");            
            ClientSocket Client = asyncResult.AsyncState as ClientSocket;
            try
            {
				//Debug.Log(string.Format("ReceiveCallback protocol from {0}", Client.Name));
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.                  
                if (!Client.WorkSocket.Connected)
                {
                    //遠方已斷線
					Debug.LogError(string.Format("{0} 連線已中斷", Client.Name));
                    CloseClient(Client);
                    return;                    
                }

                int BytesRead = Client.WorkSocket.EndReceive(asyncResult);  
				//Debug.Log(string.Format("Receive {0} bytes from {1}" , BytesRead, Client.Name));  
                if (BytesRead > 0)
                {                                        
                    Client.ReceiveData.ReceivedSize = BytesRead;
                    Client.ReceiveData.Reset();
                    mPacketHandler.doDecodePacketHandle(ref Client);
                    Client.SetReciveBuffer(m_BufferPool.GetBuffer());
                    Client.WorkSocket.BeginReceive(Client.ReceiveData.Buffer, 0, Client.ReceiveData.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), Client);                                      
                }
                else
                {
					Debug.LogError(string.Format("{0} 連線已中斷", Client.Name));
                    //遠端已斷線
                    CloseClient(Client);
					//Debug.LogError("遠端已斷線");
                }                                    
            }
            catch (Exception e)
            {
                //Common.Engine.Tool.ShowInfo(e.Message);
                Common.Engine.Tool.ShowInfo("連線已中斷");
                //遠端已斷線
                CloseClient(Client);
            }
        }
        //發送回調
        private void SendCallback(IAsyncResult asyncResult)
        {
            //Common.Engine.Tool.ShowInfo("SendCallback");
            // Retrieve the socket from the state object.  
            ClientSocket Client = asyncResult.AsyncState as ClientSocket;
            try
            {
                // Complete sending the data to the remote device.  
                int bytesSent = Client.WorkSocket.EndSend(asyncResult);
                //Common.Engine.Tool.ShowInfo(string.Format("Sent {0} bytes to server.", bytesSent));                
            }
            catch (Exception e)
            {
                //Common.Engine.Tool.ShowInfo(e.ToString());
				Debug.LogError(string.Format("{0} 連線已中斷", Client.Name));
                //遠端已斷線
                CloseClient(Client);
            }
        }

        //主要使用這個開啟
        protected void StartConnect(ClientSocket client , EndPoint endPoint)
        {
			//Debug.Log (string.Format("Start Connect to {0}", endPoint));
			if (client == null)
                return;
            if (client.WorkSocket != null)
            {                                
                if (client.WorkSocket.Connected)                
                    CloseClient(client);                
            }
            client.SetEndPoint(endPoint);
            client.Status = E_ConnectStatus.Connectting;   
                 
            //if (Socket.OSSupportsIPv6)
            //    client.WorkSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp); // IPv6
            //else
			//if(client.WorkSocket ==null)
			client.WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // IPv4 
			if(client.WorkSocket !=null)
            	client.WorkSocket.BeginConnect(endPoint , new AsyncCallback(ConnectCallback), client);
        }

        //使用這個關閉
        protected void CloseClient(ClientSocket client)
        {
			//Debug.Log (string.Format("a CloseClient({0})", client.Name));
			if (client == null)
                return;
            if (client.WorkSocket == null)
                return;
			//Debug.Log (string.Format("b CloseClient({0})", client.Name));
            if (!client.WorkSocket.Connected)
                return;

			//Debug.Log (string.Format("b CloseClient({0})", client.Name));

			if (client.Name == "GameServer") {
				string message = string.Format ("{0} Disconnected...", client.Name);
				UnityMainThreadDispatcher.Instance ().Enqueue (
					() => Main.Instance.ChangeGameStatus (E_GameStatus.Login)
				);
				//UnityMainThreadDispatcher.Instance ().Enqueue (
				//	() => UIMessageBox.ShowMessageBox (message, null)
				//);
			}
			//Debug.Log (string.Format("c CloseClient({0})", client.Name));

			//if (client.WorkSocket!=null && client.WorkSocket.Connected) {
			if(client.WorkSocket!=null)
			{
				try {
					if(client.WorkSocket.Connected)
					{
						client.WorkSocket.Shutdown(SocketShutdown.Both);
					}
					else
					{
						client.WorkSocket.Close();
					}
				}
				catch{
				}
				finally{
					client.WorkSocket = null;
				}
			}
			//	client.WorkSocket.Shutdown (SocketShutdown.Both);
			//	client.WorkSocket.Close ();
			//client.WorkSocket = null;
			//Debug.Log (string.Format("c CloseClient({0})", client.Name));
			//}

            //if (Socket.OSSupportsIPv6)
            //    client.WorkSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp); // IPv6
            //else
            //client.WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // IPv4 

            Common.Engine.Tool.ShowInfo("Close Connect");
        }
        //發送協議
        public void Send(ClientSocket client)
        {            
            if (client == null)
                return;
            if (client.WorkSocket == null)
                return;
            //NetBuffer buf = null;

            if (!client.WorkSocket.Connected)
            {
                //遠方已斷線
                CloseClient(client);
                return;
            }
           
            try
            {
                //buf = client.SendData;
                //buf.Owner = client;
				client.SendData.SetHead(client.SendData);
				client.WorkSocket.BeginSend(client.SendData.Buffer, 0, client.SendData.IndexPosition, SocketFlags.None, new AsyncCallback(SendCallback), client);
            }
            catch (Exception e)
            {
                Common.Engine.Tool.ShowInfo(e.StackTrace);
            }
            finally
            {
                //if(buf!=null)
                ///    BufferPool.RecycleBuffer(buf);
            }
        }

        protected void RegisterProtocol(int iMainID, int iSubID, Action<NetBuffer> iEvent)
        {
            mHP.Register(iMainID, iSubID, iEvent);
        }

        public override void Initialize()
        {
            base.Initialize();
            BufferPool.CreateNewBuffers(m_BufferPoolSize);
        }

        public virtual void OnUpdate()
        {

        }
    }
}
