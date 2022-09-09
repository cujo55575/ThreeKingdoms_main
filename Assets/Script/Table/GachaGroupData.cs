using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GachaGroupData : TableDataBase
{
	public int ID;
	public int HeroID1;
	public int Probability1;
	public int HeroID2;
	public int Probability2;
	public int HeroID3;
	public int Probability3;
	public int HeroID4;
	public int Probability4;
	public int HeroID5;
	public int Probability5;
	public int HeroID6;
	public int Probability6;
	public int HeroID7;
	public int Probability7;
	public int HeroID8;
	public int Probability8;
	public int HeroID9;
	public int Probability9;
	public int HeroID10;
	public int Probability10;

	public override int key()
	{
		return ID;
	}

	public int GetTotalProbability()
	{
		return (Probability1 + Probability2 + Probability3 + Probability4 + Probability5 +
		Probability6 + Probability7 + Probability8 + Probability9 + Probability10);
	}

	public List<ProbableHero> GetProbableHeroes()
	{
		List<ProbableHero> result = new List<ProbableHero>();
		HeroData hero1 = TableManager.Instance.HeroDataTable.GetData(HeroID1);
		if (hero1 != null)
		{
			result.Add(new ProbableHero(Probability1,hero1));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID1,ID));
		}

		HeroData hero2 = TableManager.Instance.HeroDataTable.GetData(HeroID2);
		if (hero2 != null)
		{
			result.Add(new ProbableHero(Probability2,hero2));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID2,ID));
		}

		HeroData hero3 = TableManager.Instance.HeroDataTable.GetData(HeroID3);
		if (hero3 != null)
		{
			result.Add(new ProbableHero(Probability3,hero3));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID3,ID));
		}

		HeroData hero4 = TableManager.Instance.HeroDataTable.GetData(HeroID4);
		if (hero4 != null)
		{
			result.Add(new ProbableHero(Probability4,hero4));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID4,ID));
		}

		HeroData hero5 = TableManager.Instance.HeroDataTable.GetData(HeroID5);
		if (hero5 != null)
		{
			result.Add(new ProbableHero(Probability5,hero5));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID5,ID));
		}

		HeroData hero6 = TableManager.Instance.HeroDataTable.GetData(HeroID6);
		if (hero6 != null)
		{
			result.Add(new ProbableHero(Probability6,hero6));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID6,ID));
		}

		HeroData hero7 = TableManager.Instance.HeroDataTable.GetData(HeroID7);
		if (hero7 != null)
		{
			result.Add(new ProbableHero(Probability7,hero7));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID7,ID));
		}

		HeroData hero8 = TableManager.Instance.HeroDataTable.GetData(HeroID8);
		if (hero8 != null)
		{
			result.Add(new ProbableHero(Probability8,hero8));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID8,ID));
		}

		HeroData hero9 = TableManager.Instance.HeroDataTable.GetData(HeroID9);
		if (hero9 != null)
		{
			result.Add(new ProbableHero(Probability9,hero9));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID9,ID));
		}

		HeroData hero10 = TableManager.Instance.HeroDataTable.GetData(HeroID10);
		if (hero10 != null)
		{
			result.Add(new ProbableHero(Probability10,hero10));
		}
		else
		{
			Debug.Log(string.Format("HeroData Null for ID = {0} at GachaGroupData ID = {1}",HeroID10,ID));
		}

		return result;
	}
}

[Serializable]
public class GachaGroupDataTable : TableBase<GachaGroupData>
{
	
}

public class ProbableHero
{
	public int Probability;
	public HeroData Hero;

	public ProbableHero(int probability,HeroData hero)
	{
		Probability = probability;
		Hero = hero;
	}
}