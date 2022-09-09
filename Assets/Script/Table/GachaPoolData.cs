using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GachaPoolData : TableDataBase
{
	public int ID;
	public string Description;
	public int GachaGroup1; 
	public int GachaGroup2; 
	public int GachaGroup3; 
	public int GachaGroup4; 
	public int GachaGroup5;


	public override int key()
	{
		return ID;
	}

	public int GetTotalProbability()
	{
		int result = 0;
		GachaGroupData gachaGroupData1 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup1);
		if (gachaGroupData1 != null)
		{
			result += gachaGroupData1.GetTotalProbability();
		}

		GachaGroupData gachaGroupData2 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup2);
		if (gachaGroupData2 != null)
		{
			result += gachaGroupData2.GetTotalProbability();
		}

		GachaGroupData gachaGroupData3 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup3);
		if (gachaGroupData3 != null)
		{
			result += gachaGroupData3.GetTotalProbability();
		}

		GachaGroupData gachaGroupData4 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup4);
		if (gachaGroupData4 != null)
		{
			result += gachaGroupData4.GetTotalProbability();
		}

		GachaGroupData gachaGroupData5 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup5);
		if (gachaGroupData5 != null)
		{
			result += gachaGroupData5.GetTotalProbability();
		}
		return result;
	}

	public List<ProbableHero> GetProbableHeroes()
	{
		List<ProbableHero> result = new List<ProbableHero>();

		GachaGroupData gachaGroupData1 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup1);
		if (gachaGroupData1 != null)
		{
			result.AddRange(gachaGroupData1.GetProbableHeroes());
		}

		GachaGroupData gachaGroupData2 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup2);
		if (gachaGroupData2 != null)
		{
			result.AddRange(gachaGroupData2.GetProbableHeroes());
		}

		GachaGroupData gachaGroupData3 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup3);
		if (gachaGroupData3 != null)
		{
			result.AddRange(gachaGroupData3.GetProbableHeroes());
		}

		GachaGroupData gachaGroupData4 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup4);
		if (gachaGroupData4 != null)
		{
			result.AddRange(gachaGroupData4.GetProbableHeroes());
		}

		GachaGroupData gachaGroupData5 = TableManager.Instance.GachaGroupDataTable.GetData(GachaGroup5);
		if (gachaGroupData5 != null)
		{
			result.AddRange(gachaGroupData5.GetProbableHeroes());
		}
		return result;
	}
}

[Serializable]
public class GachaPoolDataTable : TableBase<GachaPoolData>
{
	public GachaPoolData GetGachaPool(E_GachaType type)
	{
		GachaPoolData result = TableManager.Instance.GachaPoolDataTable.GetData((int)type);
		if (result == null)
		{
			Debug.Log(String.Format("No GachaPoolData found for ID = {0}",(int)type));
		}
		return result;
	}
}
