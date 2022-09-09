using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueMessageGroupData : TableDataBase
{
	public int ID;
	public int DialogueMessageDataID1;
	public int DialogueMessageDataID2;
	public int DialogueMessageDataID3;
	public int DialogueMessageDataID4;
	public int DialogueMessageDataID5;
	public int DialogueMessageDataID6;
	public int DialogueMessageDataID7;
	public int DialogueMessageDataID8;
	public int DialogueMessageDataID9;
	public int DialogueMessageDataID10;
	public override int key()
	{
		return ID;
	}

	public List<DialogueMessageData> GetAllDialogueMessageDataFromGroup()
	{
		List<DialogueMessageData> result = new List<DialogueMessageData>();

		DialogueMessageData data1 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID1);
		if (data1 != null)
		{
			result.Add(data1);
		}

		DialogueMessageData data2 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID2);
		if (data2 != null)
		{
			result.Add(data2);
		}

		DialogueMessageData data3 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID3);
		if (data3 != null)
		{
			result.Add(data3);
		}

		DialogueMessageData data4 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID4);
		if (data4 != null)
		{
			result.Add(data4);
		}

		DialogueMessageData data5 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID5);
		if (data5 != null)
		{
			result.Add(data5);
		}
		DialogueMessageData data6 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID6);
		if (data6 != null)
		{
			result.Add(data6);
		}
		DialogueMessageData data7 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID7);
		if (data7 != null)
		{
			result.Add(data7);
		}
		DialogueMessageData data8 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID8);
		if (data8 != null)
		{
			result.Add(data8);
		}
		DialogueMessageData data9 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID9);
		if (data9 != null)
		{
			result.Add(data9);
		}
		DialogueMessageData data10 = TableManager.Instance.DialogueMessageDataTable.GetData(DialogueMessageDataID10);
		if (data10 != null)
		{
			result.Add(data10);
		}
		return result;
	}
}

[Serializable]
public class DialogueMessageGroupDataTable : TableBase<DialogueMessageGroupData>
{
}