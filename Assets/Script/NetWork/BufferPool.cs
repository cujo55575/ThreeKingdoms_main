using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using Common.Tool.Mem;
using Common.Tool.ObjectPool;

namespace NetWork
{
	//檔頭
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sPacketHead
	{
		public int MainID;  //主協定編號
		public int SubID;   //副協定編號
		public int Size;    //資料大小
	}

	public class NetBuffer : PoolObjectBase
    {
		private static readonly sPacketHead mHead;

		public static readonly int HeadSize = Marshal.SizeOf(mHead);

		public static BufferPool BufferPool = new BufferPool();

        //private static byte[] m_TempBuffer = new byte[Const.BUFFER_SIZE]; //設定header用的Buffer

		private ClientSocket m_Owner;
		private byte[] m_Buffer;      
		private MemoryStream m_StreamData;
		private BinaryReader m_BR_ReciveData;
		private BinaryWriter m_BW_SendData;
		private int m_Receivedize;

        public int IndexPosition
        {
            get { return(int)m_StreamData.Position; }
        }


        public int ReceivedSize
        {
            set { m_Receivedize = value; }
            get { return m_Receivedize; }
        }

        public byte[] Buffer
        {
            get { return m_Buffer; }
            set {   m_Buffer = value;   }
        }
        public ClientSocket Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

		public NetBuffer()
		{
			//m_Buffer = new byte[Const.BUFFER_SIZE];
			m_Buffer = CommonMem.Instance.GetBuffer();
			m_StreamData = new MemoryStream(m_Buffer);            
			m_BR_ReciveData = new BinaryReader(m_StreamData, Encoding.Unicode);
			m_BW_SendData = new BinaryWriter(m_StreamData, Encoding.Unicode);
		}

		//開始
		public override void InitBase()
		{
			Clear();
		}

		//結束
		public override void FreeBase()
		{
			Clear();
		}

		//清除
		private void Clear()
		{
			Reset();
			m_Owner = null;
		}

        public void Init()
        {
        }  

        public void Reset()
        {
            m_StreamData.Position = 0;
        }

		public void Seek(int iPos)
		{
			m_StreamData.Position += iPos;
		}


        public void SetData(bool value)
        {
            m_BW_SendData.Write(value);
        }
        public bool ReadBool()
        {
            return m_BR_ReciveData.ReadBoolean();
        }
        public void SetData(int value)
        {
            m_BW_SendData.Write(value);
        }
        public int ReadInt()
        {
            return m_BR_ReciveData.ReadInt32();
        }
        public void SetData(string value)
        {
            m_BW_SendData.Write(value);
        }
        public string ReadString()
        {
            return m_BR_ReciveData.ReadString();
        }
        public void SetData(byte value)
        {
            m_BW_SendData.Write(value);
        }
        public byte ReadByte()
        {
            return m_BR_ReciveData.ReadByte();
        }
        public void SetData(ulong value)
        {
            m_BW_SendData.Write(value);
        }
        public ulong ReadUlong()
        {
            return m_BR_ReciveData.ReadUInt64();
        }
        public void SetData(uint value)
        {
            m_BW_SendData.Write(value);
        }
        public uint ReadUint()
        {
            return m_BR_ReciveData.ReadUInt32();
        }
        public void SetData(ushort value)
        {
            m_BW_SendData.Write(value);
        }
        public ushort ReadUshort()
        {
            return m_BR_ReciveData.ReadUInt16();
        }
			
		//讀byte array
		public byte[] ReadByteAy(int iSize)
		{
			return m_BR_ReciveData.ReadBytes(iSize);
		}

		//寫byte array
		public void SetData(byte[] iValue , int index , int size)
		{
            m_BW_SendData.Write(iValue, index, size);            
        }

		//讀檔頭
		public sPacketHead ReadHead()
		{         
			sPacketHead aHead = new sPacketHead();
			aHead.MainID = ReadInt();
			aHead.SubID = ReadInt();
			aHead.Size = ReadInt();

			return aHead;
		}

		//寫檔頭
		public void SetHead(NetBuffer iBuffer)
		{            
			int Size = iBuffer.IndexPosition;

			byte[] aBuffer = CommonMem.Instance.GetBuffer();

			//Array.Copy(iBuffer.Buffer, m_TempBuffer, Size);            
			Array.Copy(iBuffer.Buffer, aBuffer, Size);    

			iBuffer.Reset();
			iBuffer.SetData(iBuffer.Owner.Head.MainID);
			iBuffer.SetData(iBuffer.Owner.Head.SubID);
			iBuffer.SetData(Size);  

			if(Const.BUFFER_SIZE < iBuffer.IndexPosition + Size)
			{
				//Common.Engine.Tool.ShowInfo(string.Format("buffer寫入{0}超過緩衝{1}", iBuffer.IndexPosition + Size, Const.BUFFER_SIZE));
				Debug.LogError(string.Format("buffer寫入{0}超過緩衝{1}", iBuffer.IndexPosition + Size, Const.BUFFER_SIZE));
                return;
			}

			//iBuffer.SetData(m_TempBuffer,0, Size);
			iBuffer.SetData(aBuffer, 0, Size);
			CommonMem.Instance.BackBuffer(aBuffer);

			//Common.Engine.Tool.ShowInfo(string.Format("發送協定{0}-{1}, 大小{2}", iBuffer.Owner.Head.MainID, iBuffer.Owner.Head.SubID, Size + 12));
			//Debug.Log(string.Format("發送協定封包 {0}-{1}, 長度{2}", iBuffer.Owner.Head.MainID, iBuffer.Owner.Head.SubID, Size + 12));
		}
    }

	public class BufferPool : ObjectPoolBase<NetBuffer>
    {
        //private List<NetBuffer> m_List = new List<NetBuffer>();

        public void CreateNewBuffers(int iNum = 10)
        {
			/*
            for (int i = 0; i < numbers; i++)
            {
                NetBuffer Content = new NetBuffer();
                Content.Init();
                m_List.Add(Content);
            }
            */
        }

        public NetBuffer GetBuffer()
        {
			/*
            NetBuffer Content = null;
            if (m_List.Count <= 0)
                CreateNewBuffers();            
            Content = m_List[0];
            m_List.RemoveAt(0);

            return Content;
            */
			return base.GetObject();
        }

		public void RecycleBuffer(NetBuffer iObject)
        {
			/*
            content.Reset();
            content.Owner = null;
            m_List.Add(content);   
            */
			base.BackObject(iObject);          
        }
    }
}
