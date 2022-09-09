using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace NetWork
{
    public enum E_ConnectStatus
    {
        None = 0,
        Connectting,
        Failed,
    }

    public class ClientSocket
    {        
        private Action<ClientSocket> m_RecycleMethod = null;
        public Socket WorkSocket = null;// Client  Socket.
        public EndPoint RemoteEnd
        {
            get;
            private set;
        }
        public E_ConnectStatus Status = E_ConnectStatus.None;
        public byte ConnectCount = 0;
		public string Name;
        public NetBuffer ReceiveData;
        public NetBuffer SendData;

        private sPacketHead mHead;
        public sPacketHead Head{get{return mHead;}}

        public void Init(NetBuffer receiveBuffer ,NetBuffer sendBuffer , Action<ClientSocket> recycleMethod)
        {
            Status = E_ConnectStatus.None;
            ReceiveData = receiveBuffer;
            ReceiveData.Owner = this;
            SendData = sendBuffer;
            SendData.Owner = this;
            m_RecycleMethod = recycleMethod;

            //if (Socket.OSSupportsIPv6)
            //    client.WorkSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp); // IPv6
            //else
            WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // IPv4 
			//WorkSocket.ReceiveTimeout = 1;
			//WorkSocket.SendTimeout = 1;
			WorkSocket.NoDelay = true;
			//WorkSocket.SendBufferSize = Const.BUFFER_SIZE;
        }
        public void Recycle()
        {
            if (m_RecycleMethod != null)
                m_RecycleMethod(this);
        }

        public void SetEndPoint(EndPoint endPoint)
        {
            RemoteEnd = endPoint;
        }

        public void SetHead(int mainNumber , int subNumber)
        {
            /*SendData.Reset();
            SetData(Const.NET_STATE_NORMAL);
            SetData(Const.HEAD_OF_PROTOCOL);
            SetData(mainNumber);
            SetData(subNumber);*/
            SendData.Reset();
            mHead.MainID = mainNumber;
            mHead.SubID = subNumber;
        }        
        public void SetReciveBuffer(NetBuffer buffer)
        {
            ReceiveData = buffer;
            buffer.Owner = this;
        }
        public void SetSendBuffer(NetBuffer buffer)
        {
            SendData = buffer;
            buffer.Owner = this;
        }

        public void SetData(bool value)
        {
            SendData.SetData(value);
        }
        //public bool ReadBool()
        //{
        //    return ReceiveData.ReadBool();
        //}

        public void SetData(int value)
        {
            SendData.SetData(value);
        }
        //public int ReadInt()
        //{
        //    return ReceiveData.ReadInt();
        //}

        public void SetData(string value)
        {
            SendData.SetData(value);
        }
        //public string ReadString()
        //{
        //    return ReceiveData.ReadString();
        //}

        public void SetData(byte value)
        {
            SendData.SetData(value);
        }
        //public byte ReadByte()
        //{
        //    return ReceiveData.ReadByte();
        //}

        public void SetData(ulong value)
        {
            SendData.SetData(value);
        }
        //public ulong ReadUlong()
        //{
        //    return ReceiveData.ReadUlong();
        //}

        public void SetData(uint value)
        {
            SendData.SetData(value);
        }
        //public ulong ReadUint()
        //{
        //    return ReceiveData.ReadUint();
        //}

        public void SetData(ushort value)
        {
            SendData.SetData(value);
        }
        //public ulong ReadUshort()
        //{
        //    return ReceiveData.ReadUshort();
        //}

        //public byte[] ReadByteAy(int iSize)
        //{
        //    return ReceiveData.ReadByteAy(iSize);
        //}

        public void SetData(byte[] iValue ,int index = 0 , int size = -1)
        {
            if (size < 0)
                size = iValue.Length;
            SendData.SetData(iValue , index , size);
        }        

        //public sPacketHead ReadHead()
        //{
        //    return ReceiveData.ReadHead();
        //}

        public void SetHead(NetBuffer iBuffer)
        {
            SendData.SetHead(iBuffer);
        }

//        public void SetData<T>(T value)
//        {
//            SendData.SetData<T>(value);
//        }

    }
}
