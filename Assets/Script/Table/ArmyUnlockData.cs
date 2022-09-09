using System;
[Serializable]
public class ArmyUnlockData : TableDataBase
{
	public int ID;
	public byte ArmyType;///<see cref="E_ArmyType"/>
	public int Level;

	public override int key()
	{
		return ID;
	}
}

[Serializable]
public class ArmyUnlockDataTable : TableBase<ArmyUnlockData>
{
}
