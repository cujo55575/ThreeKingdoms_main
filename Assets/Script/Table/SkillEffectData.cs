using System;

[Serializable]
public class SkillEffectData : TableDataBase
{
	public int ID;
	public byte Target;///<see cref = "E_TargetType" />
	public int Range;
	public byte AffectType;///<see cref="E_AffectType"/>
	public int AffectRange;
	public float AffectValue;
	public byte AffectAttribute;///<see cref="E_AttributeType"/>
	public byte AffectValueType;///<see cref="E_AffectValueType"/>
	public float CooldownTime;
	public string EffectNameID;
	public string SFXNameID;
	public override int key()
	{
		return ID;
	} 
    public string GetVFXName()
    {
        return EffectNameID;
    }
    public string GetSfxName()
    {
        return SFXNameID;
    }
}

[Serializable]
public class SkillEffectDataTable : TableBase<SkillEffectData>
{
}