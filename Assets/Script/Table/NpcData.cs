using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class NpcData : TableDataBase
{
	public int ID;
	public int NpcNameID;
	public string NpcImgName;
	public int NpcRare;
	public int NpcAttributeLinkID;
	public int NpcSkill1;
	public int NpcSkill2;
	public int NpcSkill3;
	public int NpcArmyID;
	public int ArmyLevel;

	public override int key()
	{
		return ID;
	}

	public Texture GetNPCImage()
	{
		Texture texture = Resources.Load<Texture>("Image/Sprite/" + NpcImgName);
		return texture;
	}

	public string GetNPCName()
	{
		return TableManager.Instance.LocaleStringDataTable.GetString(NpcNameID);
	}

	public AttributeData GetNPCAttribute()
	{
		AttributeLinkData aLinkData = TableManager.Instance.AttributeLinkDataTable.GetData(NpcAttributeLinkID);
		if (aLinkData == null)
		{
			Debug.Log(string.Format("AttributeLinkData null for NPCID = {0} and NPCAttributeLinkID = {1}",key(),NpcAttributeLinkID));
			return null;
		}
		AttributeData aData = aLinkData.GetAttribute(NpcRare);
		if (aData != null)
		{
			Debug.Log("Get with Attribute Link");
			return aData;
		}
		Debug.Log("GEt with direct Attribute");
		return TableManager.Instance.AttributeDataTable.GetData(NpcAttributeLinkID);
	}

	public List<SkillData> GetSkills()
	{
		List<SkillData> result = new List<SkillData>();
		SkillData data1 = TableManager.Instance.SkillDataTable.GetData(NpcSkill1);
		if (data1 != null)
		{
			result.Add(data1);
		}

		SkillData data2 = TableManager.Instance.SkillDataTable.GetData(NpcSkill2);
		if (data2 != null)
		{
			result.Add(data2);
		}

		SkillData data3 = TableManager.Instance.SkillDataTable.GetData(NpcSkill3);
		if (data3 != null)
		{
			result.Add(data3);
		}

		return result;
	}

	public ArmyData GetArmyData()
	{
		return TableManager.Instance.ArmyDataTable.GetData(NpcArmyID);
	}

	public int GetNPCCombatPower()
	{
		return GetNPCAttribute().Atk;
	}
}

[Serializable]
public class NpcDataTable : TableBase<NpcData>
{
}