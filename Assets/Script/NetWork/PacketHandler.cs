using System;
using System.Collections.Generic;
using System.Text;
using Common.Net;
using UnityEngine;

namespace NetWork
{
    public class PacketHandler
    {
        private HandleProtocol mHP;
        private Dictionary<IntPtr, NetBuffer> remainBuffers = new Dictionary<IntPtr, NetBuffer>();

        public PacketHandler(HandleProtocol hp)
        {
            mHP = hp;
        }
        /**
		 * 分帧逻辑
		 * 
		 **/
        public void doDecodePacketHandle(ref ClientSocket Client)
        {
            NetBuffer preBuf = null;
            NetBuffer recvBuf = Client.ReceiveData;
            NetBuffer currentBuf = null;
            NetBuffer remainBuf = null;
            IntPtr sockethandle = Client.WorkSocket.Handle;

            try
            {
                if(remainBuffers.TryGetValue(sockethandle, out preBuf))
                {
                    Debug.Log(string.Format("have remain buff.ReceivedSize={0}", preBuf.ReceivedSize));
                    byte[] a = preBuf.Buffer;
                    byte[] b = recvBuf.Buffer;
                    int alen = preBuf.ReceivedSize;
                    int blen = recvBuf.ReceivedSize;

                    byte[] combined = new byte[alen + blen];
                    currentBuf = NetBuffer.BufferPool.GetBuffer();
                    Array.Copy(a, currentBuf.Buffer, alen);
                    Array.Copy(b, 0, currentBuf.Buffer, alen, blen);
                    currentBuf.Seek(0);
                    currentBuf.ReceivedSize = alen + blen;
					currentBuf.Owner = recvBuf.Owner;
                    remainBuffers.Remove(sockethandle);
                } else {
					//currentBuf = NetBuffer.BufferPool.GetBuffer();
					//Array.Copy(recvBuf.Buffer, currentBuf.Buffer, recvBuf.ReceivedSize);
					//currentBuf.ReceivedSize = recvBuf.ReceivedSize;
					//currentBuf.Owner = recvBuf.Owner;
                    currentBuf =  recvBuf;
                }
                
                remainBuf = DecodePacket(currentBuf);
                if(remainBuf != null)
                {
                    if(remainBuffers.ContainsKey(sockethandle))
                    {
                        remainBuffers[sockethandle] = remainBuf;
                    } else
                    {
                        remainBuffers.Add(sockethandle, remainBuf);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
            finally
            {
                if (preBuf != null)
                    NetBuffer.BufferPool.RecycleBuffer(preBuf);
                if (recvBuf != null)
                    NetBuffer.BufferPool.RecycleBuffer(recvBuf);
                //if (remainBuf != null)
                //    NetBuffer.BufferPool.RecycleBuffer(remainBuf);
            }
        }

        //解收到的封包
        private NetBuffer DecodePacket(NetBuffer iNB)
        {
            iNB.Reset();
            NetBuffer remainBuffer = null;

            int TotalSize = iNB.ReceivedSize;
            int szHead = NetBuffer.HeadSize;
            do
            {
                if (TotalSize < szHead)
                {//接收長度不足包頭大小
                    Debug.Log(string.Format("[半包]已接收總長度{0} < 包頭長度{1}, 下次處理", TotalSize, szHead));
                    remainBuffer = NetBuffer.BufferPool.GetBuffer();
                    remainBuffer.Owner = iNB.Owner;
                    Array.Copy(iNB.Buffer, iNB.IndexPosition, remainBuffer.Buffer, 0, TotalSize);
                    remainBuffer.ReceivedSize = TotalSize;
                    return remainBuffer;
                }
                else
                {
                    sPacketHead aHead = iNB.ReadHead();
                    Debug.Log(string.Format("解析包頭資訊 {0}_{1} size={2}", aHead.MainID, aHead.SubID, aHead.Size));
                    int len = aHead.Size;
                    if (TotalSize < szHead + len)
                    {//半包情形
                        Debug.Log(string.Format("[半包]已接收總長度{0} < 預接收長度{1}, 下次處理", TotalSize, szHead+len));
                        remainBuffer = NetBuffer.BufferPool.GetBuffer();
                        remainBuffer.Owner = iNB.Owner;
                        Array.Copy(iNB.Buffer, 0, remainBuffer.Buffer, 0, TotalSize);
                        remainBuffer.ReceivedSize = TotalSize;
                        return remainBuffer;
                    }
                    else
                    {//正常情況 或者黏包
                        if (TotalSize == szHead + len) {
                            Debug.Log(string.Format("[正常包]已接收總長度{0} == 預接收長度{1}", TotalSize, szHead + len));
                            NetBuffer tmpbuf = NetBuffer.BufferPool.GetBuffer();
                            tmpbuf.Owner = iNB.Owner;
                            Array.Copy(iNB.Buffer, iNB.IndexPosition, tmpbuf.Buffer, 0, aHead.Size);
                            iNB.Seek(aHead.Size);
                            TotalSize -= (szHead + len);
                            Debug.Log(string.Format("處理協定封包{0}_{1}, 長度={2}", aHead.MainID, aHead.SubID, aHead.Size+12));
                            Action<NetBuffer> aEvent = mHP.GetRegister(aHead.MainID, aHead.SubID);
						    if (aEvent != null) {
							    //from other thread to main thread
							    UnityMainThreadDispatcher.Instance().Enqueue(
								    () => aEvent(tmpbuf)
							    );
						    }
                        } else
                        {
                            Debug.LogError(string.Format("[黏包]已接收總長度{0} > 預接收長度{1}", TotalSize, szHead + len));
                            NetBuffer tmpbuf = NetBuffer.BufferPool.GetBuffer();
                            tmpbuf.Owner = iNB.Owner;
                            Array.Copy(iNB.Buffer, iNB.IndexPosition, tmpbuf.Buffer, 0, aHead.Size);
                            iNB.Seek(aHead.Size);
                            TotalSize -= (szHead + len);
                            Debug.Log(string.Format("處理協定封包{0}_{1}, 長度={2}", aHead.MainID, aHead.SubID, aHead.Size+12));
                            Action<NetBuffer> aEvent = mHP.GetRegister(aHead.MainID, aHead.SubID);
						    if (aEvent != null) {
							    //from other thread to main thread
							    UnityMainThreadDispatcher.Instance().Enqueue(
								    () => aEvent(tmpbuf)
							    );
						    }
                            Debug.Log(string.Format("剩餘封包長度{0}", TotalSize));
                        }
                    }
                }
            }
            while (TotalSize > 0);

            return remainBuffer;
        }
    }
}
