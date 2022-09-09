using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPlacement : MonoBehaviour
{

    public HeroPlacementManager heroPlacementManger;
    public BattleManager battleManager;
    private List<PlayerObject> m_PlayerObjects;

    public GameObject Grid;
    public int offset = 100;

    public GameObject[,] Grids;
    public Team.TeamType yourTeam;

	public bool AllowCheckingInfo = false;
    private void Start()
    {
        Grids = new GameObject[4, 4];
        for (int i = 0; i < 4; i++)
        {
            float StartX = offset * -1.5f;
            float StartY = offset * -1.5f;
            for (int j = 0; j < 4; j++)
            {
                GameObject ins = Instantiate(Grid, transform.position + Vector3.up, Quaternion.identity);
                ins.transform.parent = this.transform;
                ins.SetActive(true);
                ins.transform.Translate(StartY + j * offset, 0, StartX + i * offset);
                ins.name = i + "," + j;

                Grids[i, j] = ins;
            }
        }
        transform.Rotate(0,180f,0);


		switch (GLOBALVALUE.CURRENT_MATCH_MODE)
		{
			case E_MatchMode.FriendlyMatch:
			case E_MatchMode.RankedMatch:

				PlayerObject enemy;
				if (GLOBALVALUE.CURRENT_ENEMY != null)
				{
					Debug.Log(string.Format("Okay haha GLOBALVALUE.CURRENT_ENEMY={0}",GLOBALVALUE.CURRENT_ENEMY.Data.Name));
					enemy = GLOBALVALUE.CURRENT_ENEMY;
					battleManager.enemy = enemy;
				}
				else
				{
					m_PlayerObjects = PlayerDataManager.Instance.RankPlayers;

					int rand = Random.Range(0,m_PlayerObjects.Count);
					battleManager.enemy = m_PlayerObjects[rand];
					enemy = battleManager.enemy;
				}
				List<Common.Player.ArmyBattleFormation> saveFormation = enemy.Data.SavedArmyFormation;
				Debug.Log("Formation Count : " + saveFormation.Count);
				if (saveFormation != null)
				{

					for (int i = 0; i < saveFormation.Count; i++)
					{
						CreateLeader(saveFormation[i],enemy,i);
					}
				}
			break;

			case E_MatchMode.CampaignMatch:

				LevelData levelData = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData();
				
				NpcBattleFormation formation1 = new NpcBattleFormation();
				formation1.NpcID = levelData.Npc1ID;
				formation1.GridX = levelData.Npc1PosX;
				formation1.GridY = levelData.Npc1PosY;

				NpcBattleFormation formation2 = new NpcBattleFormation();
				formation2.NpcID = levelData.Npc2ID;
				formation2.GridX = levelData.Npc2PosX;
				formation2.GridY = levelData.Npc2PosY;

				NpcBattleFormation formation3 = new NpcBattleFormation();
				formation3.NpcID = levelData.Npc3ID;
				formation3.GridX = levelData.Npc3PosX;
				formation3.GridY = levelData.Npc3PosY;

				NpcBattleFormation formation4 = new NpcBattleFormation();
				formation4.NpcID = levelData.Npc4ID;
				formation4.GridX = levelData.Npc4PosX;
				formation4.GridY = levelData.Npc4PosY;

				NpcBattleFormation formation5 = new NpcBattleFormation();
				formation5.NpcID = levelData.Npc5ID;
				formation5.GridX = levelData.Npc5PosX;
				formation5.GridY = levelData.Npc5PosY;

				CreateNpcLeader(formation1);
				CreateNpcLeader(formation2);
				CreateNpcLeader(formation3);
				CreateNpcLeader(formation4);
				CreateNpcLeader(formation5);
			break;

			case E_MatchMode.TowerMatch:

				TowerModeLevelData towerLevelData = PlayerDataManager.Instance.PlayerData.TowerLevelData.GetPlayerCurrentLevelData();

				NpcBattleFormation f1 = new NpcBattleFormation();
				f1.NpcID = towerLevelData.Npc1ID;
				f1.GridX = towerLevelData.Npc1PosX;
				f1.GridY = towerLevelData.Npc1PosY;

				NpcBattleFormation f2 = new NpcBattleFormation();
				f2.NpcID = towerLevelData.Npc2ID;
				f2.GridX = towerLevelData.Npc2PosX;
				f2.GridY = towerLevelData.Npc2PosY;

				NpcBattleFormation f3 = new NpcBattleFormation();
				f3.NpcID = towerLevelData.Npc3ID;
				f3.GridX = towerLevelData.Npc3PosX;
				f3.GridY = towerLevelData.Npc3PosY;

				NpcBattleFormation f4 = new NpcBattleFormation();
				f4.NpcID = towerLevelData.Npc4ID;
				f4.GridX = towerLevelData.Npc4PosX;
				f4.GridY = towerLevelData.Npc4PosY;

				NpcBattleFormation f5 = new NpcBattleFormation();
				f5.NpcID = towerLevelData.Npc5ID;
				f5.GridX = towerLevelData.Npc5PosX;
				f5.GridY = towerLevelData.Npc5PosY;

				CreateNpcLeader(f1);
				CreateNpcLeader(f2);
				CreateNpcLeader(f3);
				CreateNpcLeader(f4);
				CreateNpcLeader(f5);
			break;

			default:
			break;
		}

    }

	public class NpcBattleFormation
	{
		public int NpcID;
		public int GridX;
		public int GridY;
	}

	void CreateNpcLeader(NpcBattleFormation formation)
	{
		NpcData npcData = TableManager.Instance.NpcDataTable.GetData(formation.NpcID);
		if (npcData == null)
		{
			return;
		}
		ArmyData armyData=TableManager.Instance.ArmyDataTable.GetData(npcData.NpcArmyID);
		if (armyData == null)
		{
			return;
		}

		int aindex = (int)armyData.ArmyType;

		int gx = formation.GridX;
		if (gx > 3)
			gx = 0;
		int gy = formation.GridY;
		if (gy > 3)
			gy = 0;
		Debug.Log("Enemy Formation Index = " + gx + "," + gy);
		GameObject ins = Instantiate(heroPlacementManger.Armies[aindex],Grids[gx,gy].transform.position,Grids[gx,gy].transform.rotation);
		Leader leaderScript = ins.GetComponent<Leader>();
		leaderScript.gameObject.SetActive(false);

		leaderScript.FaceSprite = npcData.GetNPCImage();
		leaderScript.HeroName = npcData.GetNPCName();

		leaderScript.FillNpcData(npcData,armyData);

		leaderScript.GetComponent<Team>().team = yourTeam;
		leaderScript.gameObject.SetActive(true);

		if (battleManager.tex2 == null)
		{
			battleManager.tex2 = npcData.GetNPCImage();
			battleManager.name2 = npcData.GetNPCName();
		}
	}
    void CreateLeader(Common.Player.ArmyBattleFormation formation, PlayerObject enemy, int i)
    {
		PlayerHeroData data;
		
		HeroArmyData equippedArmy;


#if OFFLINE_DATA
		//data = TableManager.Instance.PlayerHeroDataTable.GetData(formation.PlayerHeroID);
		//equippedArmy = data.GetEquippedArmy();

		data = enemy.Data.OwnedHeros.Find(x => x.ID == formation.PlayerHeroID);
		equippedArmy = data.GetEquippedArmyFromEnemy(enemy);
#else
		data = enemy.Data.OwnedHeros.Find(x => x.ID == formation.PlayerHeroID);
		equippedArmy = data.GetEquippedArmyFromEnemy(enemy);
#endif
		ArmyData armyData = equippedArmy.GetArmyData();

		int aindex = (int)armyData.ArmyType;

        //data = TableManager.Instance.PlayerHeroDataTable.GetData(formation.PlayerHeroID);
        //int aindex = (int)data.GetEquippedArmy().GetArmyData().ArmyType;
        int gx = formation.GridX;
        if(gx>3)
            gx = 0;
        int gy = formation.GridY;
        if(gy>3)
            gy = 0;
        Debug.LogError(string.Format("{0} enemy aindex={1}, gx={2}, gy={3}", i, aindex, gx, gy));
        GameObject ins = Instantiate(heroPlacementManger.Armies[aindex], Grids[gx,gy].transform.position, Grids[gx,gy].transform.rotation);
        Leader leaderScript = ins.GetComponent<Leader>();
		leaderScript.gameObject.SetActive(false);

        leaderScript.FaceSprite = data.GetHeroData().GetHeroTexture();
        leaderScript.HeroName = data.GetHeroName();

        leaderScript.RegisterAttributes(data,equippedArmy,true);

        leaderScript.GetComponent<Team>().team=yourTeam;
        leaderScript.gameObject.SetActive(true);
    }
    private void Update()
    {
		if (!AllowCheckingInfo)
		{
			return;
		}
        if (Input.GetMouseButtonDown(0))
        {
            bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask("HeroPosition")) && noUIcontrolsInUse)
            {
                Leader leader=hit.collider.transform.parent.GetComponent<Leader>();
                if(leader.team.team==Team.TeamType.TeamTwo)
                {
					if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.FriendlyMatch || GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.RankedMatch)
					{
						if (UIManager.UIInstance<UIEnemyCardInfo>() != null && UIManager.UIInstance<UIEnemyCardInfo>().gameObject.activeSelf)
						{
							UIManager.UIInstance<UIEnemyCardInfo>().ShowData(leader.m_PlayerHeroData,leader.m_ArmyData);
						}
						else
						{
							UIManager.Instance.ShowUI(GLOBALCONST.UI_ENEMYCARDINFO,leader.m_PlayerHeroData,leader.m_ArmyData);
						}
					}
					else
					{
						if (UIManager.UIInstance<UIEnemyCardInfo>() != null && UIManager.UIInstance<UIEnemyCardInfo>().gameObject.activeSelf)
						{
							UIManager.UIInstance<UIEnemyCardInfo>().ShowData(leader.NpcData);
						}
						else
						{
							UIManager.Instance.ShowUI(GLOBALCONST.UI_ENEMYCARDINFO,leader.NpcData);
						}
					}
                }
            }
            else if(noUIcontrolsInUse)
            {
                UIManager.Instance.CloseUI(GLOBALCONST.UI_ENEMYCARDINFO);
            }
        }
    }
}
