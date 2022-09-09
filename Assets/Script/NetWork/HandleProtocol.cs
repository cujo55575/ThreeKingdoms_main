using System;
using System.Collections.Generic;
using System.Text;
using NetWork;

namespace Common.Net
{
	//處理協定
	public class HandleProtocol
	{
		private Dictionary<int, Dictionary<int, Action<NetBuffer>>> mEvents;

		//
		public HandleProtocol()
		{
			mEvents = new Dictionary<int, Dictionary<int, Action<NetBuffer>>>();
		}

		//註冊事件
		public void Register(int iMainID, int iSubID, Action<NetBuffer> iEvent)
		{
			if(iEvent == null)
				return;

			if(mEvents.ContainsKey(iMainID) == false)
			{
				Dictionary<int, Action<NetBuffer>> aDic = new Dictionary<int, Action<NetBuffer>>();
				mEvents.Add(iMainID, aDic);
			}

			if(mEvents[iMainID].ContainsKey(iSubID) == false)
			{
				mEvents[iMainID].Add(iSubID, iEvent);
			}
			else
			{   
				//Log2.Ins.Warn(string.Format("協定{0}-{1}重複註冊", iMainID, iSubID));
			}
		}

		//取得註冊事件
		public Action<NetBuffer> GetRegister(int iMainID, int iSubID)
		{
			if(mEvents.ContainsKey(iMainID) == false)
				return null;
			if(mEvents[iMainID].ContainsKey(iSubID) == false)
				return null;

			return mEvents[iMainID][iSubID];
		}
	}
}
