using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.Player
{
  [Serializable]
  public class PlayerData
  {
    //public ulong PlayerSN;
    public string PlayerSN;

    public string Name;
    public byte Level;
    public uint Exp;

    public int BattlePoint;
    public int BambooRoll;
    public int BambooFragment;

    public int RankPoint;
    public int Rank;
    public int WinCount;
    public int LoseCount;
    public string PlayerIconName;
    public bool IsRealPlayer = false;
    public bool IsNewPlayer = true;
    public List<PlayerHeroData> OwnedHeros;
    public List<HeroArmyData> PlayerHeroArmies;
    public List<ArmyBattleFormation> SavedArmyFormation;
	public CampaignLevelData CampaignLevelData;
		public TowerLevelData TowerLevelData;
  }

	[Serializable]
	public class ArmyBattleFormation
	{
		public int PlayerHeroID;
		public int GridX;
		public int GridY;
	}

	[Serializable]
	public class CampaignLevelData
	{
		public List<ArmyBattleFormation> PlayerCampaignFormation;
		public int PlayerCurrentLevelID;

		public LevelData GetPlayerCurrentLevelData()
		{
			LevelData data = TableManager.Instance.LevelDataTable.GetData(PlayerCurrentLevelID);
			if (data == null)
			{
				UnityEngine.Debug.LogError("Level Data Null for id = " + PlayerCurrentLevelID);
			}
			return data;
		}

		public bool IsPlayerCompletedCampaign()
		{
			LevelData nextLevel = TableManager.Instance.LevelDataTable.GetData(GetPlayerCurrentLevelData().NextLvlID);
			return (nextLevel == null);
		}
	}

	[Serializable]
	public class TowerLevelData
	{
		public List<ArmyBattleFormation> PlayerTowerModeFormation;
		public int PlayerCurrentLevelID;

		public TowerModeLevelData GetPlayerCurrentLevelData()
		{
			TowerModeLevelData data = TableManager.Instance.TowerModeLevelDataTable.GetData(PlayerCurrentLevelID);
			if (data == null)
			{
				UnityEngine.Debug.LogError("Level Data Null for id = " + PlayerCurrentLevelID);
			}
			return data;
		}
	}

}
