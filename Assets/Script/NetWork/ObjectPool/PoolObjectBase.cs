using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tool.ObjectPool
{
	//池物件基礎
	public abstract class PoolObjectBase
	{
		private int mConstID;

		public int ConstID{get{return mConstID;}}

		//
		public PoolObjectBase()
		{
			mConstID = -1;
		}

		//開始
		public abstract void InitBase();

		//結束
		public abstract void FreeBase();

		//設定固定編號
		public void SetConstID(int iID)
		{
			if(mConstID != -1)
				return;

			mConstID = iID;
		}
	}
}
