using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueData : TableDataBase
{
	public int ID;
	public int DialogueMessageGroupDataID1;
	public int DialogueMessageGroupDataID2;
	public int DialogueMessageGroupDataID3;
	public int DialogueMessageGroupDataID4;
	public int DialogueMessageGroupDataID5;
	public int DialogueMessageGroupDataID6;
	public int DialogueTriggerType;/// <see cref="E_DialougeTriggerType"/>
	public int TriggerTypeRelatedHeroID;
	public int WeatherID;

	public override int key()
	{
		return ID;
	}
	public List<DialogueMessageData> GetAllDialogueMessages()
	{
		List<DialogueMessageData> result = new List<DialogueMessageData>();

		DialogueMessageGroupData data1 = TableManager.Instance.DialogueMessageGroupDataTable.GetData(DialogueMessageGroupDataID1);
		if (data1 != null)
		{
			List<DialogueMessageData> dataFromGroup = data1.GetAllDialogueMessageDataFromGroup();
			result.AddRange(dataFromGroup);
		}

		DialogueMessageGroupData data2 = TableManager.Instance.DialogueMessageGroupDataTable.GetData(DialogueMessageGroupDataID2);
		if (data2 != null)
		{
			List<DialogueMessageData> dataFromGroup = data2.GetAllDialogueMessageDataFromGroup();
			result.AddRange(dataFromGroup);
		}

		DialogueMessageGroupData data3 = TableManager.Instance.DialogueMessageGroupDataTable.GetData(DialogueMessageGroupDataID3);
		if (data3 != null)
		{
			List<DialogueMessageData> dataFromGroup = data3.GetAllDialogueMessageDataFromGroup();
			result.AddRange(dataFromGroup);
		}

		DialogueMessageGroupData data4 = TableManager.Instance.DialogueMessageGroupDataTable.GetData(DialogueMessageGroupDataID4);
		if (data4 != null)
		{
			List<DialogueMessageData> dataFromGroup = data1.GetAllDialogueMessageDataFromGroup();
			result.AddRange(dataFromGroup);
		}

		DialogueMessageGroupData data5 = TableManager.Instance.DialogueMessageGroupDataTable.GetData(DialogueMessageGroupDataID5);
		if (data5 != null)
		{
			List<DialogueMessageData> dataFromGroup = data5.GetAllDialogueMessageDataFromGroup();
			result.AddRange(dataFromGroup);
		}

		DialogueMessageGroupData data6 = TableManager.Instance.DialogueMessageGroupDataTable.GetData(DialogueMessageGroupDataID6);
		if (data6 != null)
		{
			List<DialogueMessageData> dataFromGroup = data1.GetAllDialogueMessageDataFromGroup();
			result.AddRange(dataFromGroup);
		}

		return result;
	}

}
[Serializable]
public class DialogueDataTable : TableBase<DialogueData>
{
}
