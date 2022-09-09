using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroArmyData : TableDataBase
{
	public int ID;
	public int HeroTableID;
	public int ArmyTableID;
	public int ArmyLevel;
	public byte HeroArmyStatusType;///<see cref="E_EquipableStatus"/>
	public byte Skill1StatusType;///<see cref="E_EquipableStatus"/>
	public byte Skill2StatusType;///<see cref="E_EquipableStatus"/>
	public byte Skill3StatusType;///<see cref="E_EquipableStatus"/>
	public byte Skill4StatusType;///<see cref="E_EquipableStatus"/>
	public byte Skill5StatusType;///<see cref="E_EquipableStatus"/>
	public byte Skill6StatusType;///<see cref="E_EquipableStatus"/>

	public override int key()
	{
		return ID;
	}

	public ArmyData GetArmyData()
	{
		ArmyData armyData = TableManager.Instance.ArmyDataTable.GetData(ArmyTableID);
		if (armyData == null)
		{
			Debug.Log(string.Format("Army Data Null for HeroArmyDataID = {0}, ArmyTableID = {1}",ID,ArmyTableID));
			return null;
		}
		else
		{
			return armyData;
		}
	}

	public AttributeData GetArmyAttribute(int level)
	{
		if (level > GLOBALCONST.MAX_ARMY_LEVEL)
			return null;
		ArmyData armyData = GetArmyData();
		AttributeLinkData attributeLink = TableManager.Instance.AttributeLinkDataTable.GetData(armyData.AttributeLinkTableID);
		if (attributeLink == null)
		{
			Debug.Log(string.Format("Attribute Link Data is null for ArmyData ID = {0}, AttributeLinkDataID = {1}",armyData.key(),armyData.AttributeLinkTableID));
		}
		return attributeLink.GetAttribute(level);
	}

	public Dictionary<int,SkillData> GetSkillsWithIndex()
	{
		Dictionary<int,SkillData> result = new Dictionary<int,SkillData>();
		SkillData SkillData1 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID1);
		if (SkillData1 != null)
		{
			result.Add(1,SkillData1);
		}

		SkillData SkillData2 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID2);
		if (SkillData2 != null)
		{
			result.Add(2,SkillData2);
		}

		SkillData SkillData3 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID3);
		if (SkillData3 != null)
		{
			result.Add(3,SkillData3);
		}

		SkillData SkillData4 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID4);
		if (SkillData4 != null)
		{
			result.Add(4,SkillData4);
		}

		SkillData SkillData5 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID5);
		if (SkillData5 != null)
		{
			result.Add(5,SkillData5);
		}

		SkillData SkillData6 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID6);
		if (SkillData6 != null)
		{
			result.Add(6,SkillData6);
		}
		return result;
	}

	public List<SkillData> GetSkills()
	{
		List<SkillData> result = new List<SkillData>();
		SkillData SkillData1 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID1);
		if (SkillData1 != null)
		{
			result.Add(SkillData1);
		}

		SkillData SkillData2 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID2);
		if (SkillData2 != null)
		{
			result.Add(SkillData2);
		}

		SkillData SkillData3 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID3);
		if (SkillData3 != null)
		{
			result.Add(SkillData3);
		}

		SkillData SkillData4 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID4);
		if (SkillData4 != null)
		{
			result.Add(SkillData4);
		}

		SkillData SkillData5 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID5);
		if (SkillData5 != null)
		{
			result.Add(SkillData5);
		}

		SkillData SkillData6 = TableManager.Instance.SkillDataTable.GetData(GetArmyData().SkillTableID6);
		if (SkillData6 != null)
		{
			result.Add(SkillData6);
		}

		return result;
	}

	/*
	 * HeroAmy Skills are auto equipped automatically as soon as unlocked
	 */
	public List<SkillData> GetEquipedSkills()
	{
		List<SkillData> allSkills = GetSkills();
		List<SkillData> result = new List<SkillData>();

		if (Skill1StatusType != (byte)E_EquipableStatus.Locked)
		{
			result.Add(allSkills[0]);
		}
		if (Skill2StatusType != (byte)E_EquipableStatus.Locked)
		{
			result.Add(allSkills[1]);
		}
		if (Skill3StatusType != (byte)E_EquipableStatus.Locked)
		{
			result.Add(allSkills[2]);
		}
		if (Skill4StatusType != (byte)E_EquipableStatus.Locked)
		{
			result.Add(allSkills[3]);
		}
		if (Skill5StatusType != (byte)E_EquipableStatus.Locked)
		{
			result.Add(allSkills[4]);
		}
		if (Skill6StatusType != (byte)E_EquipableStatus.Locked)
		{
			result.Add(allSkills[5]);
		}

		return result;
	}

	public bool IsArmyAtMaxLevel()
	{
		return ArmyLevel == GLOBALCONST.MAX_ARMY_LEVEL;
	}

	public void SetArmyLevel(int level)
	{
		if (level > GLOBALCONST.MAX_ARMY_LEVEL)
		{
			return;
		}
		ArmyLevel = level;
		Dictionary<int,SkillData> skills = GetSkillsWithIndex();
		foreach (KeyValuePair<int,SkillData> item in skills)
		{
			if (ArmyLevel >= item.Value.UnlockLevel)
			{
				ChangeSkillStatusAtIndex(item.Key,E_EquipableStatus.Unlocked);
			}
			else
			{
				ChangeSkillStatusAtIndex(item.Key,E_EquipableStatus.Locked);
			}
		}
	}

	private void ChangeSkillStatusAtIndex(int index,E_EquipableStatus newStatus)
	{
		switch (index)
		{
			case 1:
			Skill1StatusType = (byte)newStatus;
			break;
			case 2:
			Skill2StatusType = (byte)newStatus;
			break;
			case 3:
			Skill3StatusType = (byte)newStatus;
			break;
			case 4:
			Skill4StatusType = (byte)newStatus;
			break;
			case 5:
			Skill5StatusType = (byte)newStatus;
			break;
			case 6:
			Skill6StatusType = (byte)newStatus;
			break;
			default:
			break;

		}
	}

	public HeroArmyData GetCopyData()
	{
		HeroArmyData result = new HeroArmyData
		{
			ID = ID,
			HeroTableID = HeroTableID,
			ArmyTableID = ArmyTableID,
			ArmyLevel = ArmyLevel,
			HeroArmyStatusType = HeroArmyStatusType,
			Skill1StatusType = Skill1StatusType,
			Skill2StatusType = Skill2StatusType,
			Skill3StatusType = Skill3StatusType,
			Skill4StatusType = Skill4StatusType,
			Skill5StatusType = Skill5StatusType,
			Skill6StatusType = Skill6StatusType,
		};
		return result;
	}
}

	[Serializable]
	public class HeroArmyDataTable : TableBase<HeroArmyData>
	{
		public List<HeroArmyData> GetCopyDatas()
		{
			List<HeroArmyData> result = new List<HeroArmyData>();
			for (int i = 0; i < Datas.Count; i++)
			{
				result.Add(Datas[i].GetCopyData());
			}
			return result;
		}
	}

