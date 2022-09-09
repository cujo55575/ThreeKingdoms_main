using System;
using System.Collections.Generic;
using System.Text;
using Common.Tool;
using NetWork;

namespace Common.Tool.Mem
{
	//通用儲存空間
	public class CommonMem : Singleton<CommonMem>
	{
		private BufferPool mPool;

		public int InitSize {get{return mPool.InitSize;}}
		public int TotalSize{get{return mPool.TotalSize;}}

		//
		public CommonMem()
		{
			mPool = new BufferPool(10, Const.BUFFER_SIZE);
		}

		//取出空間
		public byte[] GetBuffer()
		{
			return mPool.GetBuffer();
		}

		//放回空間
		public void BackBuffer(byte[] iBuffer)
		{
			mPool.BackBuffer(iBuffer);
		}
	}
}