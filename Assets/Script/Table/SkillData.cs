using System;
using UnityEngine;

[Serializable]
public class SkillData : TableDataBase
{
    public int ID;
    public int NameID;
    public string IconName;
    public byte SkillType;///<see cref="E_SkillType"/>
	public int UnlockLevel;
    public int DescriptionID;
    public int Chance;
    public byte ElementType;
    public int SkillEffectTableID1;
    public int SkillEffectTableID2;
    public int SkillEffectTableID3;
    public int SkillTriggerID;

    public override int key()
    {
        return ID;
    }
    public int unlockLevel()
    {
        return UnlockLevel;
    }
    public Texture GetSkillTexture()
    {
        Texture result = Resources.Load<Texture>("Image/Icon/" + IconName);
        if (result == null)
        {
            Debug.Log("Cannot Load SkillIcon named " + IconName);
        }
        else
        {
            Debug.Log(result.name);
        }
        return result;
    }
    public string GetSkillName()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(NameID);

        return result;
    }

    public string GetSkillDescription()
    {
        string result = TableManager.Instance.LocaleStringDataTable.GetString(DescriptionID);

        return result;
    }
    public GameObject GetVFX()
    {
        string VfxName = TableManager.Instance.SkillEffectDataTable.GetData(SkillEffectTableID1).GetVFXName();
        GameObject result = (GameObject)Resources.Load("Effects/" + VfxName);
        if (result == null)
        {
            Debug.Log(VfxName);
        }
        return result;
    }
    public string GetSFXName()
    {
        string SfxName = TableManager.Instance.SkillEffectDataTable.GetData(SkillEffectTableID1).GetSfxName();
        return SfxName;
    }

}

[Serializable]
public class SkillDataTable : TableBase<SkillData>
{

}
