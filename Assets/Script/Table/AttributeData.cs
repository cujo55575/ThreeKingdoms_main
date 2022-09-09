using System;

[Serializable]
public class AttributeData : TableDataBase
{
    public int ID;
    public uint UpgradeCost;
    public byte CostResourceType;///<see cref="E_ResourceType"/>
	public int Hp;
    public int Atk;
    public int Def;
    public float AtkSpeed;
    public int Crit;
    public int CritDef;
    public float MoveSpeed;
    public int AtkRange;
	public int Intelligence;
    public override int key()
    {
        return ID;
    }
}

[Serializable]
public class AttributeDataTable : TableBase<AttributeData>
{
}
