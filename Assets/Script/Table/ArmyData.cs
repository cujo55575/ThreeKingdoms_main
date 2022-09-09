using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ArmyData : TableDataBase
{
    public int ID;
    public int ArmyNameID;//LocalStringDataTableID
    public byte ArmyType;///<see cref="E_ArmyType"/>
	public byte ArmyJobType;///<see cref="E_ArmyJobType"/>
	public byte ArmyMoveType;///<see cref="E_ArmyMoveType"/>
	public byte ArmyAttackType;///<see cref="E_ArmyAttackType"/>
	public int AttributeLinkTableID;
    public string ModelName;
    public int SkillTableID1;
    public int SkillTableID2;
    public int SkillTableID3;
    public int SkillTableID4;
    public int SkillTableID5;
    public int SkillTableID6;
    public int ArmyUnlockTableID1;
    public int ArmyUnlockTableID2;
    public int ArmyUnlockTableID3;
    public byte ArmyDamageType;///<see cref="E_DamageType"/>

    public override int key()
    {
        return ID;
    }

    public GameObject GetModelObject()
    {
        GameObject go = Resources.Load<GameObject>("Prefab/" + ModelName);
        if (go == null)
        {
            Debug.Log(string.Format("Cannot Load GameObject at Path = Resources/Prefab/{0}", ModelName));
            return null;
        }
        else
        {
            return go;
        }
    }

    public string GetArmyMoveTypeName()
    {
        switch ((E_ArmyMoveType)ArmyMoveType)
        {
            case E_ArmyMoveType.Cavalry:
                return TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ARMY_MOVE_TYPE_CAVALRY_STRING_ID);

            case E_ArmyMoveType.Infantry:
                return TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ARMY_MOVE_TYPE_INFANTRY_STRING_ID);

            default:
                return string.Empty;
        }
    }
    public Texture GetArmyTexture()
    {
        Texture result = null;
        result = Resources.Load<Texture>("Prefab/ChangeArmyTexture/" + ModelName);

        if (result == null)
        {
            Debug.Log(string.Format("Cannot Load Texture at Path = Resources/Prefab/ChangeArmyTexture/{0}", ModelName));
        }
        return result;
    }

    public Texture GetArmyJobTypeTexture()
    {
        switch ((E_ArmyJobType)ArmyJobType)
        {
            case E_ArmyJobType.Assasin:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_ASSASIN_TEXTURE);

            case E_ArmyJobType.Attacker:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_ATTACKER_TEXTURE);

            case E_ArmyJobType.Defender:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_DEFENDER_TEXTURE);

            default:
                return null;
        }
    }

    public Texture GetArmyJobTypeTitleTexture()
    {
        switch ((E_ArmyJobType)ArmyJobType)
        {
            case E_ArmyJobType.Assasin:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_ASSASIN_TITLE_TEXTURE);

            case E_ArmyJobType.Attacker:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_ATTACKER_TITLE_TEXTURE);

            case E_ArmyJobType.Defender:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_DEFENDER_TITLE_TEXTURE);

            default:
                return null;
        }
    }
    public Texture GetArmyJobTypeBGTexture()
    {
        switch ((E_ArmyJobType)ArmyJobType)
        {
            case E_ArmyJobType.Assasin:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_ASSASIN_BG_TEXTURE);

            case E_ArmyJobType.Attacker:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_ATTACKER_BG_TEXTURE);

            case E_ArmyJobType.Defender:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_JOB_TYPE_DEFENDER_BG_TEXTURE);

            default:
                return null;
        }
    }
    public Texture GetArmyMoveTypeTexture()
    {
        switch ((E_ArmyMoveType)ArmyMoveType)
        {
            case E_ArmyMoveType.Cavalry:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_MOVE_TYPE_CAVALRY_ICON_NAME);

            case E_ArmyMoveType.Infantry:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_MOVE_TYPE_INFANTRY_ICON_NAME);

            default:
                return null;
        }
    }


    public string GetArmyAttackTypeName()
    {
        switch ((E_ArmyAttackType)ArmyAttackType)
        {
            case E_ArmyAttackType.Meelee:
                return TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ARMY_ATK_TYPE_MEELEE_STRING_ID);

            case E_ArmyAttackType.Range:
                return TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ARMY_ATK_TYPE_RANGE_STRING_ID);

            default:
                return string.Empty;
        }
    }

    public Texture GetArmyAttackTypeTexture()
    {
        switch ((E_ArmyAttackType)ArmyAttackType)
        {
            case E_ArmyAttackType.Meelee:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_ATK_TYPE_MEELEE_ICON_NAME);

            case E_ArmyAttackType.Range:
                return Resources.Load<Texture>(GLOBALCONST.FOLDER_IMAGE + GLOBALCONST.FOLDER_ICON + GLOBALCONST.ARMY_ATK_TYPE_RANGE_ICON_NAME);

            default:
                return null;
        }
    }

    public string GetArmyName()
    {
        string localizedArmyName = TableManager.Instance.LocaleStringDataTable.GetString(ArmyNameID);
        if (String.IsNullOrEmpty(localizedArmyName))
        {
            Debug.Log(string.Format("Localized army names cannot be found for HeroDataID = {0}, ArmyNameID = {1}",
            ID, ArmyNameID));
            return string.Empty;
        }
        else
            return localizedArmyName;
    }

    public List<ArmyUnlockData> GetUnlockConditions()
    {
        List<ArmyUnlockData> result = new List<ArmyUnlockData>();
        ArmyUnlockData data1 = TableManager.Instance.ArmyUnlockDataTable.GetData(ArmyUnlockTableID1);


        if (data1 != null)
        {
            result.Add(data1);
        }

        ArmyUnlockData data2 = TableManager.Instance.ArmyUnlockDataTable.GetData(ArmyUnlockTableID2);
        if (data2 != null)
        {
            result.Add(data2);
        }

        ArmyUnlockData data3 = TableManager.Instance.ArmyUnlockDataTable.GetData(ArmyUnlockTableID3);
        if (data3 != null)
        {
            result.Add(data3);
        }

        return result;
    }

    public AttributeData GetArmyAttribute(int level)
    {
        if (level > GLOBALCONST.MAX_ARMY_LEVEL)
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
public class ArmyDataTable : TableBase<ArmyData>
{

}
