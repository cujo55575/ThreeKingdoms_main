using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class HeroData : TableDataBase
{
    public int ID;
    public int HeroNameID;
    public string HeroImageName;
    public int KingdomID;
    public int AttributeLinkTableID;
    public int SkillTableID1;
    public int SkillTableID2;
    public int SkillTableID3;
    public int SkillTableID4;
    public int SkillTableID5;
    public int SkillTableID6;
	public int ArmyListID;


    public override int key()
    {
        return ID;
    }

    public Texture GetHeroTexture()
    {
        Texture texture = Resources.Load<Texture>("Image/Sprite/" + HeroImageName);
        return texture;
    }

    public List<SkillData> GetSkills()
    {
        List<SkillData> result = new List<SkillData>();
        result.Add(TableManager.Instance.SkillDataTable.GetData(SkillTableID1));
        result.Add(TableManager.Instance.SkillDataTable.GetData(SkillTableID2));
        result.Add(TableManager.Instance.SkillDataTable.GetData(SkillTableID3));
        result.Add(TableManager.Instance.SkillDataTable.GetData(SkillTableID4));
        result.Add(TableManager.Instance.SkillDataTable.GetData(SkillTableID5));
        result.Add(TableManager.Instance.SkillDataTable.GetData(SkillTableID6));
        return result;
    }

	public List<ArmyData> GetArmiesFromArmyList()
	{
		HeroArmyListData data = TableManager.Instance.HeroArmyListDataTable.GetData(ArmyListID);
		if (data == null)
		{
			Debug.LogError(string.Format("ArmyListData Null for HeroData ID = {0}, ArmyListDataID = {1}",ID,ArmyListID));
			return null;
		}

		return data.GetArmies();
	}
    public AttributeData GetHeroAttribute(int level)
    {
        if (level > GLOBALCONST.MAX_HERO_LEVEL)
            return null;
        AttributeLinkData attributeLink = TableManager.Instance.AttributeLinkDataTable.GetData(AttributeLinkTableID);
        if (attributeLink == null)
        {
            Debug.Log(string.Format("Attribute Link Data is null for ArmyData ID = {0}, AttributeLinkDataID = {1}", this.key(), AttributeLinkTableID));
        }
        return attributeLink.GetAttribute(level);
    }
}

[Serializable]
public class HeroDataTable : TableBase<HeroData>
{
	public HeroData GetRandomData()
	{
		int rand = GLOBALFUNCTION.GetIntRandom(0,Datas.Count);
		//int rand = GLOBALFUNCTION.GetIntRandom(0,10);
		return Datas[rand];
	}
}