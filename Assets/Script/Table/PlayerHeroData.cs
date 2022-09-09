using System;
using System.Collections.Generic;
using UnityEngine;
using Common.Player;

[Serializable]
public class PlayerHeroData : TableDataBase
{
    public int ID;
    public int PlayerTableID;
    public int HeroTableID;
    public int HeroLevel;
    public int FragmentCount;
    public byte Skill1StatusType;
    public byte Skill2StatusType;
    public byte Skill3StatusType;
    public byte Skill4StatusType;
    public byte Skill5StatusType;
    public byte Skill6StatusType;
    public int HeroArmy1;
    public int HeroArmy2;
    public int HeroArmy3;
    public int HeroArmy4;
    public int HeroArmy5;
    public int HeroArmy6;
    public int HeroArmy7;
    public int HeroArmy8;
    public int HeroArmy9;
    public int HeroArmy10;
    public int HeroArmy11;
    public int HeroArmy12;
    public int HeroArmy13;
    public int HeroArmy14;
    public int HeroArmy15;
    public int HeroArmy16;
    public int HeroArmy17;
    public int HeroArmy18;
    public int HeroArmy19;
    public int HeroArmy20;

    public override int key()
    {
        return ID;
    }
    public int heroRank()
    {
        return HeroLevel;
    }
    public AttributeData GetHeroAttribute()
    {
        HeroData heroData = TableManager.Instance.HeroDataTable.GetData(HeroTableID);
        AttributeLinkData attributeLink = TableManager.Instance.AttributeLinkDataTable.GetData(heroData.AttributeLinkTableID);
        return attributeLink.GetAttribute(HeroLevel);
    }

    public string GetHeroName()
    {
        HeroData heroData = TableManager.Instance.HeroDataTable.GetData(HeroTableID);
        return TableManager.Instance.LocaleStringDataTable.GetString(heroData.HeroNameID);
    }

    public HeroData GetHeroData()
    {
        return TableManager.Instance.HeroDataTable.GetData(HeroTableID);
    }

    public List<SkillData> GetEquippedSkills()
    {
        List<SkillData> allSkills = GetHeroData().GetSkills();
        List<SkillData> result = new List<SkillData>();
        if (Skill1StatusType > 2)
        {
            result.Add(allSkills[0]);
        }
        if (Skill2StatusType > 2)
        {
            result.Add(allSkills[1]);
        }
        if (Skill3StatusType > 2)
        {
            result.Add(allSkills[2]);
        }
        if (Skill4StatusType > 2)
        {
            result.Add(allSkills[3]);
        }
        if (Skill5StatusType > 2)
        {
            result.Add(allSkills[4]);
        }
        if (Skill6StatusType > 2)
        {
            result.Add(allSkills[5]);
        }
        return result;
    }
    public List<SkillData> GetEquippedSkillsNPC()
    {
        List<SkillData> allSkills = GetHeroData().GetSkills();
        List<SkillData> result = new List<SkillData>();
        if (Skill1StatusType == 2)
        {
            result.Add(allSkills[0]);
        }
        if (Skill2StatusType == 2)
        {
            result.Add(allSkills[1]);
        }
        if (Skill3StatusType == 2)
        {
            result.Add(allSkills[2]);
        }
        if (Skill4StatusType == 2)
        {
            result.Add(allSkills[3]);
        }
        if (Skill5StatusType == 2)
        {
            result.Add(allSkills[4]);
        }
        if (Skill6StatusType == 2)
        {
            result.Add(allSkills[5]);
        }
        return result;
    }
    public SkillData GetNextSkill(int HeroLevel)
    {
        List<SkillData> allSkills = GetHeroData().GetSkills();
        return allSkills[HeroLevel];
    }
    public Dictionary<byte, SkillData> GetEquippedSkillsWithIndex()
    {
        List<SkillData> allSkills = GetHeroData().GetSkills();
        Dictionary<byte, SkillData> result = new Dictionary<byte, SkillData>();
        if (Skill1StatusType > 2)
        {
            result.Add(Skill1StatusType, allSkills[0]);
        }
        if (Skill2StatusType > 2)
        {
            result.Add(Skill2StatusType, allSkills[1]);
        }
        if (Skill3StatusType > 2)
        {
            result.Add(Skill3StatusType, allSkills[2]);
        }
        if (Skill4StatusType > 2)
        {
            result.Add(Skill4StatusType, allSkills[3]);
        }
        if (Skill5StatusType > 2)
        {
            result.Add(Skill5StatusType, allSkills[4]);
        }
        if (Skill6StatusType > 2)
        {
            result.Add(Skill6StatusType, allSkills[5]);
        }
        return result;
    }
    public bool IsEquiped(int index)
    {
        switch (index)
        {
            case 0:
                return (bool)(Skill1StatusType > 2);
            case 1:
                return (bool)(Skill2StatusType > 2);
            case 2:
                return (bool)(Skill3StatusType > 2);
            case 3:
                return (bool)(Skill4StatusType > 2);
            case 4:
                return (bool)(Skill5StatusType > 2);
            case 5:
                return (bool)(Skill6StatusType > 2);
        }
        return false;
    }
    public List<HeroArmyData> GetAllArmies()
    {
        List<HeroArmyData> result = new List<HeroArmyData>();
        HeroArmyData armyData1 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy1);
        if (armyData1 != null) { result.Add(armyData1); }

        HeroArmyData armyData2 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy2);
        if (armyData2 != null)
        {
            result.Add(armyData2);
        }

        HeroArmyData armyData3 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy3);
        if (armyData3 != null)
        {
            result.Add(armyData3);
        }

        HeroArmyData armyData4 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy4);
        if (armyData4 != null)
        {
            result.Add(armyData4);
        }

        HeroArmyData armyData5 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy5);
        if (armyData5 != null)
        {
            result.Add(armyData5);
        }

        HeroArmyData armyData6 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy6);
        if (armyData6 != null)
        {
            result.Add(armyData6);
        }

        HeroArmyData armyData7 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy7);
        if (armyData7 != null)
        {
            result.Add(armyData7);
        }

        HeroArmyData armyData8 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy8);
        if (armyData8 != null)
        {
            result.Add(armyData8);
        }

        HeroArmyData armyData9 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy9);
        if (armyData9 != null)
        {
            result.Add(armyData9);
        }

        HeroArmyData armyData10 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy10);
        if (armyData10 != null)
        {
            result.Add(armyData10);
        }

        HeroArmyData armyData11 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy11);
        if (armyData11 != null)
        {
            result.Add(armyData11);
        }

        HeroArmyData armyData12 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy12);
        if (armyData12 != null)
        {
            result.Add(armyData12);
        }

        HeroArmyData armyData13 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy13);
        if (armyData13 != null)
        {
            result.Add(armyData13);
        }

        HeroArmyData armyData14 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy14);
        if (armyData14 != null)
        {
            result.Add(armyData14);
        }

        HeroArmyData armyData15 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy15);
        if (armyData15 != null)
        {
            result.Add(armyData15);
        }

        HeroArmyData armyData16 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy16);
        if (armyData16 != null)
        {
            result.Add(armyData16);
        }

        HeroArmyData armyData17 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy17);
        if (armyData17 != null)
        {
            result.Add(armyData17);
        }

        HeroArmyData armyData18 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy18);
        if (armyData18 != null)
        {
            result.Add(armyData18);
        }

        HeroArmyData armyData19 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy19);
        if (armyData19 != null)
        {
            result.Add(armyData19);
        }

        HeroArmyData armyData20 = TableManager.Instance.HeroArmyDataTable.GetData(HeroArmy20);
        if (armyData20 != null)
        {
            result.Add(armyData20);
        }
        return result;
    }

    public HeroArmyData GetEquippedArmy()
    {
        List<HeroArmyData> armies = GetAllArmies();
        for (int i = 0; i < armies.Count; i++)
        {
            if ((E_EquipableStatus)armies[i].HeroArmyStatusType == E_EquipableStatus.Equipped)
            {
                return armies[i];
            }
        }
		PlayerHeroData data = this;
		return null;
    }

    public HeroArmyData GetEquippedArmySelf()
    {

		List<HeroArmyData> armies = GetAllArmiesSelf();
		for (int i = 0; i < armies.Count; i++)
		{
			if ((E_EquipableStatus)armies[i].HeroArmyStatusType == E_EquipableStatus.Equipped)
			{
				return armies[i];
			}
		}
		
		return null;
	}

    public HeroArmyData GetEquippedArmyFromEnemy(PlayerObject enemy)
    {

		List<HeroArmyData> armies = GetAllArmiesFromEnemy(enemy);
		for (int i = 0; i < armies.Count; i++)
		{
			if ((E_EquipableStatus)armies[i].HeroArmyStatusType == E_EquipableStatus.Equipped)
			{
				return armies[i];
			}
		}

		return null;
	}


    public List<HeroArmyData> GetAllArmiesSelf()
    {
		//      PlayerData pData = PlayerDataManager.Instance.PlayerData;
		//List<HeroArmyData> heroArmyList = GetAllArmiesByPlayerData(pData);
		//if (heroArmyList == null || heroArmyList.Count == 0)
		//{
		//	Debug.Log("GetAllArmies got error ");
		//}
		//return heroArmyList;
		List<HeroArmyData> result = new List<HeroArmyData>();
		HeroArmyData armyData1 = GetArmyInData(HeroArmy1);
		if (armyData1 != null)
		{
			result.Add(armyData1);
		}
		else
		{
			
		}

		HeroArmyData armyData2 = GetArmyInData(HeroArmy2);
		if (armyData2 != null)
		{
			result.Add(armyData2);
		}

		HeroArmyData armyData3 = GetArmyInData(HeroArmy3);
		if (armyData3 != null)
		{
			result.Add(armyData3);
		}

		HeroArmyData armyData4 = GetArmyInData(HeroArmy4);
		if (armyData4 != null)
		{
			result.Add(armyData4);
		}

		HeroArmyData armyData5 = GetArmyInData(HeroArmy5);
		if (armyData5 != null)
		{
			result.Add(armyData5);
		}

		HeroArmyData armyData6 = GetArmyInData(HeroArmy6);
		if (armyData6 != null)
		{
			result.Add(armyData6);
		}

		HeroArmyData armyData7 = GetArmyInData(HeroArmy7);
		if (armyData7 != null)
		{
			result.Add(armyData7);
		}

		HeroArmyData armyData8 = GetArmyInData(HeroArmy8);
		if (armyData8 != null)
		{
			result.Add(armyData8);
		}

		HeroArmyData armyData9 = GetArmyInData(HeroArmy9);
		if (armyData9 != null)
		{
			result.Add(armyData9);
		}

		HeroArmyData armyData10 = GetArmyInData(HeroArmy10);
		if (armyData10 != null)
		{
			result.Add(armyData10);
		}

		HeroArmyData armyData11 = GetArmyInData(HeroArmy11);
		if (armyData11 != null)
		{
			result.Add(armyData11);
		}

		HeroArmyData armyData12 = GetArmyInData(HeroArmy12);
		if (armyData12 != null)
		{
			result.Add(armyData12);
		}

		HeroArmyData armyData13 = GetArmyInData(HeroArmy13);
		if (armyData13 != null)
		{
			result.Add(armyData13);
		}

		HeroArmyData armyData14 = GetArmyInData(HeroArmy14);
		if (armyData14 != null)
		{
			result.Add(armyData14);
		}

		HeroArmyData armyData15 = GetArmyInData(HeroArmy15);
		if (armyData15 != null)
		{
			result.Add(armyData15);
		}

		HeroArmyData armyData16 = GetArmyInData(HeroArmy16);
		if (armyData16 != null)
		{
			result.Add(armyData16);
		}

		HeroArmyData armyData17 = GetArmyInData(HeroArmy17);
		if (armyData17 != null)
		{
			result.Add(armyData17);
		}

		HeroArmyData armyData18 = GetArmyInData(HeroArmy18);
		if (armyData18 != null)
		{
			result.Add(armyData18);
		}

		HeroArmyData armyData19 = GetArmyInData(HeroArmy19);
		if (armyData19 != null)
		{
			result.Add(armyData19);
		}

		HeroArmyData armyData20 = GetArmyInData(HeroArmy20);
		if (armyData20 != null)
		{
			result.Add(armyData20);
		}

		return result;
    }

	private HeroArmyData GetArmyInData(int heroArmyID)
	{
		for (int i = 0; i < PlayerDataManager.Instance.PlayerData.PlayerHeroArmies.Count; i++)
		{
			if (heroArmyID == PlayerDataManager.Instance.PlayerData.PlayerHeroArmies[i].key())
			{
				return PlayerDataManager.Instance.PlayerData.PlayerHeroArmies[i];
			}
		}
		return null;
	}

    public List<HeroArmyData> GetAllArmiesFromEnemy(PlayerObject enemy)
    {
        return GetAllArmiesByPlayerObject(enemy);
    }
    
    public List<HeroArmyData> GetAllArmiesByPlayerObject(PlayerObject pObj)
    {
        List<HeroArmyData> result = new List<HeroArmyData>();

        HeroArmyData armyData1 = GetArmyFromPlayerObj(HeroArmy1, pObj);
        if (armyData1 != null)
        {
            result.Add(armyData1);
        }

        HeroArmyData armyData2 = GetArmyFromPlayerObj(HeroArmy2, pObj);
        if (armyData2 != null)
        {
            result.Add(armyData2);
        }

        HeroArmyData armyData3 = GetArmyFromPlayerObj(HeroArmy3, pObj);
        if (armyData3 != null)
        {
            result.Add(armyData3);
        }

        HeroArmyData armyData4 = GetArmyFromPlayerObj(HeroArmy4, pObj);
        if (armyData4 != null)
        {
            result.Add(armyData4);
        }

        HeroArmyData armyData5 = GetArmyFromPlayerObj(HeroArmy5, pObj);
        if (armyData5 != null)
        {
            result.Add(armyData5);
        }

        HeroArmyData armyData6 = GetArmyFromPlayerObj(HeroArmy6, pObj);
        if (armyData6 != null)
        {
            result.Add(armyData6);
        }

        HeroArmyData armyData7 = GetArmyFromPlayerObj(HeroArmy7, pObj);
        if (armyData7 != null)
        {
            result.Add(armyData7);
        }

        HeroArmyData armyData8 = GetArmyFromPlayerObj(HeroArmy8, pObj);
        if (armyData8 != null)
        {
            result.Add(armyData8);
        }

        HeroArmyData armyData9 = GetArmyFromPlayerObj(HeroArmy9, pObj);
        if (armyData9 != null)
        {
            result.Add(armyData9);
        }

        HeroArmyData armyData10 = GetArmyFromPlayerObj(HeroArmy10, pObj);
        if (armyData10 != null)
        {
            result.Add(armyData10);
        }

        HeroArmyData armyData11 = GetArmyFromPlayerObj(HeroArmy11, pObj);
        if (armyData11 != null)
        {
            result.Add(armyData11);
        }

        HeroArmyData armyData12 = GetArmyFromPlayerObj(HeroArmy12, pObj);
        if (armyData12 != null)
        {
            result.Add(armyData12);
        }

        HeroArmyData armyData13 = GetArmyFromPlayerObj(HeroArmy13, pObj);
        if (armyData13 != null)
        {
            result.Add(armyData13);
        }

        HeroArmyData armyData14 = GetArmyFromPlayerObj(HeroArmy14, pObj);
        if (armyData14 != null)
        {
            result.Add(armyData14);
        }

        HeroArmyData armyData15 = GetArmyFromPlayerObj(HeroArmy15, pObj);
        if (armyData15 != null)
        {
            result.Add(armyData15);
        }

        HeroArmyData armyData16 = GetArmyFromPlayerObj(HeroArmy16, pObj);
        if (armyData16 != null)
        {
            result.Add(armyData16);
        }

        HeroArmyData armyData17 = GetArmyFromPlayerObj(HeroArmy17, pObj);
        if (armyData17 != null)
        {
            result.Add(armyData17);
        }

        HeroArmyData armyData18 = GetArmyFromPlayerObj(HeroArmy18, pObj);
        if (armyData18 != null)
        {
            result.Add(armyData18);
        }

        HeroArmyData armyData19 = GetArmyFromPlayerObj(HeroArmy19, pObj);
        if (armyData19 != null)
        {
            result.Add(armyData19);
        }

        HeroArmyData armyData20 = GetArmyFromPlayerObj(HeroArmy20, pObj);
        if (armyData20 != null)
        {
            result.Add(armyData20);
        }
        return result;
    }

    public List<HeroArmyData> GetAllArmiesByPlayerData(PlayerData pData)
    {
        List<HeroArmyData> result = new List<HeroArmyData>();

        HeroArmyData armyData1 = GetArmyFromPlayerData(HeroArmy1, pData);
        if (armyData1 != null)
        {

            result.Add(armyData1);
        }

        HeroArmyData armyData2 = GetArmyFromPlayerData(HeroArmy2, pData);
        if (armyData2 != null)
        {
            result.Add(armyData2);
        }

        HeroArmyData armyData3 = GetArmyFromPlayerData(HeroArmy3, pData);
        if (armyData3 != null)
        {
            result.Add(armyData3);
        }

        HeroArmyData armyData4 = GetArmyFromPlayerData(HeroArmy4, pData);
        if (armyData4 != null)
        {
            result.Add(armyData4);
        }

        HeroArmyData armyData5 = GetArmyFromPlayerData(HeroArmy5, pData);
        if (armyData5 != null)
        {
            result.Add(armyData5);
        }

        HeroArmyData armyData6 = GetArmyFromPlayerData(HeroArmy6, pData);
        if (armyData6 != null)
        {
            result.Add(armyData6);
        }

        HeroArmyData armyData7 = GetArmyFromPlayerData(HeroArmy7, pData);
        if (armyData7 != null)
        {
            result.Add(armyData7);
        }

        HeroArmyData armyData8 = GetArmyFromPlayerData(HeroArmy8, pData);
        if (armyData8 != null)
        {
            result.Add(armyData8);
        }

        HeroArmyData armyData9 = GetArmyFromPlayerData(HeroArmy9, pData);
        if (armyData9 != null)
        {
            result.Add(armyData9);
        }

        HeroArmyData armyData10 = GetArmyFromPlayerData(HeroArmy10, pData);
        if (armyData10 != null)
        {
            result.Add(armyData10);
        }

        HeroArmyData armyData11 = GetArmyFromPlayerData(HeroArmy11, pData);
        if (armyData11 != null)
        {
            result.Add(armyData11);
        }

        HeroArmyData armyData12 = GetArmyFromPlayerData(HeroArmy12, pData);
        if (armyData12 != null)
        {
            result.Add(armyData12);
        }

        HeroArmyData armyData13 = GetArmyFromPlayerData(HeroArmy13, pData);
        if (armyData13 != null)
        {
            result.Add(armyData13);
        }

        HeroArmyData armyData14 = GetArmyFromPlayerData(HeroArmy14, pData);
        if (armyData14 != null)
        {
            result.Add(armyData14);
        }

        HeroArmyData armyData15 = GetArmyFromPlayerData(HeroArmy15, pData);
        if (armyData15 != null)
        {
            result.Add(armyData15);
        }

        HeroArmyData armyData16 = GetArmyFromPlayerData(HeroArmy16, pData);
        if (armyData16 != null)
        {
            result.Add(armyData16);
        }

        HeroArmyData armyData17 = GetArmyFromPlayerData(HeroArmy17, pData);
        if (armyData17 != null)
        {
            result.Add(armyData17);
        }

        HeroArmyData armyData18 = GetArmyFromPlayerData(HeroArmy18, pData);
        if (armyData18 != null)
        {
            result.Add(armyData18);
        }

        HeroArmyData armyData19 = GetArmyFromPlayerData(HeroArmy19, pData);
        if (armyData19 != null)
        {
            result.Add(armyData19);
        }

        HeroArmyData armyData20 = GetArmyFromPlayerData(HeroArmy20, pData);
        if (armyData20 != null)
        {
            result.Add(armyData20);
        }
        return result;
    }

    private HeroArmyData GetArmyFromPlayerData(int armyID, PlayerData pData)
    {
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.PlayerHeroArmies.Count; i++)
        {
            if (pData.PlayerHeroArmies[i].key() == armyID)
            {
                return pData.PlayerHeroArmies[i];
            }
        }
        return null;
    }

    private HeroArmyData GetArmyFromPlayerObj(int armyID, PlayerObject pData)
    {
        for (int i = 0; i < pData.Data.PlayerHeroArmies.Count; i++)
        {
            if (pData.Data.PlayerHeroArmies[i].key() == armyID)
            {
                return pData.Data.PlayerHeroArmies[i];
            }
        }
        return null;
    }

    public void ReCheckArmyUnlockConditions()
    {
        HeroArmyData equippedArmy = GetEquippedArmySelf();
        if (equippedArmy == null)
        {
            equippedArmy = GetHeroArmyFromType(E_ArmyType.Footman);
			if (equippedArmy != null)
			{
                equippedArmy.HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
			}
			if (equippedArmy == null)
			{
				equippedArmy = GetAllArmiesSelf()[0];
			}
        }
        List<HeroArmyData> armies = GetAllArmiesSelf();
        for (int i = 0; i < armies.Count; i++)
        {
            if (armies[i].key() == equippedArmy.key())
            {
                continue;
            }
            bool isUnlocked = true;
            List<ArmyUnlockData> unlocks = armies[i].GetArmyData().GetUnlockConditions();
            for (int j = 0; j < unlocks.Count; j++)
            {
                if (!CheckUnlockConditionMet(unlocks[j]))
                {
                    isUnlocked = false;
                    break;
                }
            }
            if (isUnlocked)
            {
                armies[i].HeroArmyStatusType = (byte)E_EquipableStatus.Unlocked;
            }
            else
            {
                armies[i].HeroArmyStatusType = (byte)E_EquipableStatus.Locked;
            }
        }
    }
    public bool isHeroAtMaxLevel()
    {
        return HeroLevel == GLOBALCONST.MAX_HERO_LEVEL;
    }
    private void SetPlayerHeroLevel(int level)
    {
        if (level > GLOBALCONST.MAX_HERO_LEVEL)
        {
            return;
        }
        HeroLevel = level;
        //List<HeroArmyData> armies = GetAllArmiesSelf();
        //for (int i = 0; i < armies.Count; i++)
        //{
        //    bool isUnlocked = true;
        //    List<ArmyUnlockData> unlocks = armies[i].GetArmyData().GetUnlockConditions();
        //    for (int j = 0; j < unlocks.Count; j++)
        //    {
        //        if (!CheckUnlockConditionMet(unlocks[j]))
        //        {
        //            isUnlocked = false;
        //            break;
        //        }
        //    }
        //    if (isUnlocked)
        //    {
        //        armies[i].HeroArmyStatusType = (byte)E_EquipableStatus.Unlocked;
        //    }
        //    else
        //    {
        //        armies[i].HeroArmyStatusType = (byte)E_EquipableStatus.Locked;
        //    }
        //}
        ////SetDefaultEquippedArmy to footman
        //HeroArmyData data = GetHeroArmyFromType(E_ArmyType.Footman);
        //data.HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;

    }

    public bool CheckUnlockConditionMet(ArmyUnlockData unlockData)
    {
        E_ArmyType armyType = (E_ArmyType)unlockData.ArmyType;
        HeroArmyData heroArmyData = GetHeroArmyFromType(armyType);
        if (heroArmyData == null)
        {
            return false;
        }
        return (heroArmyData.ArmyLevel >= unlockData.Level);
    }

    public string GetArmyNameFromType(E_ArmyType armyType)
    {
        HeroArmyData heroArmy = GetHeroArmyFromType(armyType);
        if (heroArmy == null)
        {
            UnityEngine.Debug.Log(
            string.Format("heroArmy Null for armyType = {0}, PlayerHeroID = {1}", armyType, ID));
            return string.Empty;
        }
        ArmyData army = heroArmy.GetArmyData();
        return TableManager.Instance.LocaleStringDataTable.GetString(army.ArmyNameID);
    }

    private HeroArmyData GetHeroArmyFromType(E_ArmyType type)
    {
        List<HeroArmyData> armies = GetAllArmiesSelf();
        for (int i = 0; i < armies.Count; i++)
        {
            if (type == (E_ArmyType)armies[i].GetArmyData().ArmyType)
            {
                return armies[i];
            }
        }
        return null;
    }

    public int GetCombatPower()
    {
        int result = 0;
        List<HeroArmyData> allArmies = GetAllArmies();
        for (int i = 0; i < allArmies.Count; i++)
        {
            if (E_EquipableStatus.Locked != (E_EquipableStatus)allArmies[i].HeroArmyStatusType)
            {
                result += allArmies[i].ArmyLevel;
            }
        }
        return result;
    }


    public int GetCombatPowerSelf()
    {
        int result = 0;
        List<HeroArmyData> allArmies = GetAllArmiesSelf();
        for (int i = 0; i < allArmies.Count; i++)
        {
            if (E_EquipableStatus.Locked != (E_EquipableStatus)allArmies[i].HeroArmyStatusType)
            {
                result += allArmies[i].ArmyLevel;
            }
        }
        return result;
    }

}

[Serializable]
public class PlayerHeroDataTable : TableBase<PlayerHeroData>
{
    public PlayerHeroData GetRandomData()
    {
        int rand = GLOBALFUNCTION.GetIntRandom(0, Datas.Count);
        //int rand = GLOBALFUNCTION.GetIntRandom(0,10);
        return Datas[rand];
    }
}
