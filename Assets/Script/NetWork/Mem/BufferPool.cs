using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tool.Mem
{
	//緩存池
	public class BufferPool
	{
		private static List<BufferPool> mBufferPools = new List<BufferPool>();

		private Queue<byte[]> mBuffers;  //緩存
		private int mInitCount;          //初始數量
		private int mInitSize;           //初始大小
		private int mMissCount;          //失誤次數(1次1倍)

		public int InitSize {get{return mInitSize;}}
		public int TotalSize{get{return mInitCount * (1 + mMissCount) * mInitSize;}}

		//
		public BufferPool(int iInitCount, int iInitSize)
		{
			mBuffers = new Queue<byte[]>();
			mInitCount = iInitCount;
			mInitSize = iInitSize;
			mMissCount = 0;

			for(int i = 0; i < mInitCount; i++)
				mBuffers.Enqueue(new byte[mInitSize]);

			lock(mBufferPools)
			{
				mBufferPools.Add(this);
			}
		}

		//結束
		public void Free()
		{
			lock(mBufferPools)
			{
				mBufferPools.Remove(this);
			}
		}

		//取出緩存
		public byte[] GetBuffer()
		{
			byte[] aBuffer = null;

			lock(mBuffers)
			{
				if(mBuffers.Count == 0)
				{
					for(int i = 0; i < mInitCount; i++)
						mBuffers.Enqueue(new byte[mInitSize]);

					mMissCount++;
				}

				aBuffer = mBuffers.Dequeue();
			}

			return aBuffer;
		}

		//放回緩存
		public void BackBuffer(byte[] iBuffer)
		{
			if(iBuffer == null)
				return;

			lock(mBuffers)
			{
				mBuffers.Enqueue(iBuffer);
			}
		}
	}
}
