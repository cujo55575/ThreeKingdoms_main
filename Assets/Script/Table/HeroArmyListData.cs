using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class HeroArmyListData : TableDataBase
{
	public int ID;
	public int Army1ID;
	public int Army2ID;
	public int Army3ID;
	public int Army4ID;
	public int Army5ID;
	public int Army6ID;
	public int Army7ID;
	public int Army8ID;
	public int Army9ID;
	public int Army10ID;
	public int Army11ID;
	public int Army12ID;
	public int Army13ID;
	public int Army14ID;
	public int Army15ID;
	public int Army16ID;
	public int Army17ID;
	public int Army18ID;
	public int Army19ID;
	public int Army20ID;

	public override int key()
	{
		return ID;
	}

	public List<ArmyData> GetArmies()
	{
		List<ArmyData> result = new List<ArmyData>();
		ArmyData army1 = TableManager.Instance.ArmyDataTable.GetData(Army1ID);
		if (army1 != null)
		{
			result.Add(army1);
		}

		ArmyData army2 = TableManager.Instance.ArmyDataTable.GetData(Army2ID);
		if (army2 != null)
		{
			result.Add(army2);
		}

		ArmyData army3 = TableManager.Instance.ArmyDataTable.GetData(Army3ID);
		if (army3 != null)
		{
			result.Add(army3);
		}

		ArmyData army4 = TableManager.Instance.ArmyDataTable.GetData(Army4ID);
		if (army4 != null)
		{
			result.Add(army4);
		}

		ArmyData army5 = TableManager.Instance.ArmyDataTable.GetData(Army5ID);
		if (army5 != null)
		{
			result.Add(army5);
		}
		ArmyData army6 = TableManager.Instance.ArmyDataTable.GetData(Army6ID);
		if (army6 != null)
		{
			result.Add(army6);
		}

		ArmyData army7 = TableManager.Instance.ArmyDataTable.GetData(Army7ID);
		if (army7 != null)
		{
			result.Add(army7);
		}

		ArmyData army8 = TableManager.Instance.ArmyDataTable.GetData(Army8ID);
		if (army8 != null)
		{
			result.Add(army8);
		}

		ArmyData army9 = TableManager.Instance.ArmyDataTable.GetData(Army9ID);
		if (army9 != null)
		{
			result.Add(army9);
		}

		ArmyData army10 = TableManager.Instance.ArmyDataTable.GetData(Army10ID);
		if (army10 != null)
		{
			result.Add(army10);
		}
		ArmyData army11 = TableManager.Instance.ArmyDataTable.GetData(Army11ID);
		if (army11 != null)
		{
			result.Add(army11);
		}

		ArmyData army12 = TableManager.Instance.ArmyDataTable.GetData(Army12ID);
		if (army12 != null)
		{
			result.Add(army12);
		}

		ArmyData army13 = TableManager.Instance.ArmyDataTable.GetData(Army13ID);
		if (army13 != null)
		{
			result.Add(army13);
		}

		ArmyData army14 = TableManager.Instance.ArmyDataTable.GetData(Army14ID);
		if (army14 != null)
		{
			result.Add(army14);
		}

		ArmyData army15 = TableManager.Instance.ArmyDataTable.GetData(Army15ID);
		if (army15 != null)
		{
			result.Add(army15);
		}
		ArmyData army16 = TableManager.Instance.ArmyDataTable.GetData(Army16ID);
		if (army16 != null)
		{
			result.Add(army16);
		}

		ArmyData army17 = TableManager.Instance.ArmyDataTable.GetData(Army17ID);
		if (army17 != null)
		{
			result.Add(army17);
		}

		ArmyData army18 = TableManager.Instance.ArmyDataTable.GetData(Army18ID);
		if (army18 != null)
		{
			result.Add(army18);
		}

		ArmyData army19 = TableManager.Instance.ArmyDataTable.GetData(Army19ID);
		if (army19 != null)
		{
			result.Add(army19);
		}

		ArmyData army20 = TableManager.Instance.ArmyDataTable.GetData(Army20ID);
		if (army20 != null)
		{
			result.Add(army20);
		}

		return result;

	}
}

[Serializable]
public class HeroArmyListDataTable : TableBase<HeroArmyListData>
{
	
}