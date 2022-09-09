using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tool.ObjectPool
{
	//物件池基礎
	public class ObjectPoolBase<T> where T : PoolObjectBase
	{
		private Queue<T> mObjects;
		private int mCount;

		public ObjectPoolBase()
		{
			mObjects = new Queue<T>();
			mCount = 0;
		}

		//取出物件
		public virtual T GetObject()
		{
			T aObject = null;

			if(mObjects.Count == 0)
			{
				aObject = Activator.CreateInstance<T>();
				aObject.SetConstID(mCount);
				mCount++;
			}
			else
			{
				aObject = mObjects.Dequeue();
			}

			aObject.InitBase();

			return aObject;
		}

		//放回物件
		public virtual void BackObject(T iObject)
		{
			if(iObject == null)
				return;

			iObject.FreeBase();

			mObjects.Enqueue(iObject);
		}
	}
}
