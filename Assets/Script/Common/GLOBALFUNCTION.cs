using System.Collections.Generic;
using UnityEngine;
using XEResources;
using System.Linq;

public static class GLOBALFUNCTION
{
	public static class FORMULA
	{
		public static float GetTotalAttribute(float baseAttribute,float fixBuff = 0,float pcntBuff = 0)
		{
			float A = baseAttribute + fixBuff;
			float B = 1 + pcntBuff;
			float result = A * B;
			return result;

		}
		public static float GetCombinedTotalAtk(float heroTotalAtk,float armyTotalAtk,float skillDmgIncPcnt = 0)
		{
			float A = 1 + (armyTotalAtk / 20);
			float B = skillDmgIncPcnt + 1;
			float result = heroTotalAtk * A * B;
			return result;
		}

		public static float GetCombinedTotalDef(float heroTotalDef,float armyTotalDef,float skillDmgRedPcnt = 0)
		{
			float A = 1 + (armyTotalDef / 20);
			float B = (1 - skillDmgRedPcnt) / 2;
			float result = heroTotalDef * A * B;
			return result;
		}


		public static float GetDmgPcntCorrection(float skillDmgPcnt = 1,float critDmgPcnt = 1.3f,float skillCritDmgPlusPcnt = 0.3f)
		{
			float A = critDmgPcnt + skillCritDmgPlusPcnt;
			float result = skillDmgPcnt * A;
			return result;
		}
		public static float GetFinalDmgDealtByBasicAtk(float combinedTotalAtk,float combinedTotalDef,float dmgPcntCorrection)
		{
			float result = combinedTotalAtk - (combinedTotalDef * dmgPcntCorrection);
			return result;
		}

		public static float GetTotalCritPcnt(float armyCritPcnt,float heroTotalAtk)
		{
			float A = 1 + (heroTotalAtk / 10f);
			float result = armyCritPcnt * A;
			return result;
		}

		public static float GetTotalCritDefPcnt(float armyCritDefPcnt,float heroTotalDef)
		{
			float A = 1 + (heroTotalDef / 10f);
			A /= 2;
			float result = armyCritDefPcnt * A;
			return result;
		}

		public static float GetFinalCritChance(float totalCritPcnt,float totalCritDefPcnt)
		{
			return totalCritPcnt / totalCritDefPcnt;
		}

	}
	//------------------END OF FORMULA-------------------------------



	public static class GACHA
	{
		public static List<PlayerHeroData> GetRewardPlayerHero(E_GachaType type)
		{
			List<PlayerHeroData> result = new List<PlayerHeroData>();
			GachaPoolData poolData = TableManager.Instance.GachaPoolDataTable.GetGachaPool(type);
			if (poolData != null)
			{
				int usedProbability = 0;
				List<ProbableHero> probableList = poolData.GetProbableHeroes();
				int count = GetGachaCount(type);
				Debug.Log("Count type = " + count);

				//For fixed gacha
				if (count == probableList.Count)
				{
					for (int i = 0; i < probableList.Count; i++)
					{
						PlayerHeroData reward = GLOBALFUNCTION.GetPlayerHeroFromHeroData(probableList[i].Hero,true);
						GLOBALFUNCTION.GetPlayerHeroFromHeroData(probableList[i].Hero);
						result.Add(reward);
					}
					for (int i = 0; i < result.Count; i++)
					{
						Debug.Log(string.Format("HeroID = {0}, HeroImageName = {1}",result[i].HeroTableID,result[i].GetHeroData().HeroImageName));
					}
					Debug.Log("Hero Count after adding = " + PlayerDataManager.Instance.PlayerData.OwnedHeros.Count);
					return result;
				}
				else
				{
					Debug.Log("Count = " + count + ". ProbableCount" + probableList.Count);
				}

				//Modify Probability for random.
				for (int i = 0; i < probableList.Count; i++)
				{
					probableList[i].Probability += usedProbability;
					usedProbability = probableList[i].Probability;
				}
				List<ProbableHero> sortedProbableList = probableList.OrderByDescending(probableHero => probableHero.Probability).ToList();
				for (int i = 0; i < sortedProbableList.Count; i++)
				{
					Debug.Log(string.Format("Index = {0}, HeroID = {1}, Probability = {2}",i,sortedProbableList[i].Hero.ID,sortedProbableList[i].Probability));
				}


				for (int i = 0; i < count; i++)
				{
					int RandomNum = Random.Range(1,poolData.GetTotalProbability());
					Debug.Log("Random Number = " + RandomNum);
					for (int j = 0; j < sortedProbableList.Count; j++)
					{
						if ((sortedProbableList[j].Probability == RandomNum) ||
						((sortedProbableList[j + 1].Probability < RandomNum) && (RandomNum < sortedProbableList[j].Probability)))
						{
							PlayerHeroData reward = GLOBALFUNCTION.GetPlayerHeroFromHeroData(sortedProbableList[j].Hero,true);
							GLOBALFUNCTION.GetPlayerHeroFromHeroData(sortedProbableList[j].Hero);
							result.Add(reward);
							break;
						}
					}
				}
			}
			
			for (int i = 0; i < result.Count; i++)
			{
				Debug.Log(string.Format("HeroID = {0}, HeroImageName = {1}",result[i].HeroTableID,result[i].GetHeroData().HeroImageName));
			}

			return result;
		}

		public static int GetGachaCount(E_GachaType type)
		{
			switch (type)
			{
				default: return 5;
			}
		}
	}

	public static string ToJsonString(DeckObject deckObject)
	{
		DeckJson deckJson = ToDeckJson(deckObject);
		string result = JsonUtility.ToJson(deckJson,true);
		Debug.Log(result);
		return result;
	}

	public static string ToJsonString(List<Card> cards)
	{
		DeckJson deckJson = new DeckJson();
		for (int i = 0; i < cards.Count; i++)
		{
			deckJson.Cards.Add(ToCardJson(cards[i]));
		}
		string result = JsonUtility.ToJson(deckJson,true);
		return result;
	}
	public static CardObject ToCardFromCardJson(CardJson cJson)
	{
		CardObject card = new CardObject();
		card.Card.Atk = cJson.Atk;
		card.Card.Def = cJson.Def;
		card.Card.IsUsed = false;
		card.Card.CardName = cJson.Name;
		card.Card.RareUp = cJson.RareUp;
		card.Card.CardTexture = ResourceManager.Instance.Load<Sprite>("Image/Sprite/" + cJson.TextureName);
		return card;
	}
	public static DeckJson ToDeckJson(DeckObject deckObject)
	{
		DeckJson result = new DeckJson();
		for (int i = 0; i < deckObject.Deck.Cards.Count; i++)
		{
			CardJson cardJson = ToCardJson(deckObject.Deck.Cards[i]);
			result.Cards.Add(cardJson);
		}
		return result;
	}

	public static CardJson ToCardJson(CardObject cardObject)
	{
		CardJson result = new CardJson
		{
			Name = cardObject.Card.CardName,
			TextureName = cardObject.Card.CardTexture.name,
			Atk = cardObject.Card.Atk,
			Def = cardObject.Card.Def,
			IsUsed = cardObject.Card.IsUsed
		};
		result.PosX = cardObject.Card.PosX;
		result.PosY = cardObject.Card.PosY;
		result.Scale = cardObject.Card.Scale;

		return result;
	}

	public static CardJson ToCardJson(Card card)
	{
		CardJson result = new CardJson
		{
			Name = card.CardName,
			TextureName = card.CardTexture.name,
			Atk = card.Atk,
			Def = card.Def,
			IsUsed = card.IsUsed
		};
		result.PosX = card.PosX;
		result.PosY = card.PosY;
		result.Scale = card.Scale;
		return result;
	}

	public static Card ToCard(CardJson cardJson)
	{
		Sprite sprite = ResourceManager.Instance.Load<Sprite>("Image/Sprite/" + cardJson.TextureName);
		Debug.Log(sprite.name);
		Card result = new Card
		{
			CardName = cardJson.Name,
			CardTexture = sprite,
			Atk = cardJson.Atk,
			Def = cardJson.Def,
			RareUp = cardJson.RareUp,
			IsUsed = cardJson.IsUsed
		};
		result.PosX = cardJson.PosX;
		result.PosY = cardJson.PosY;
		result.Scale = cardJson.Scale;
		return result;
	}

	public static int GetIntRandom(int min,int max)
	{
		return Random.Range(min,max);
	}


	public static List<PlayerObject> SortPlayerObjectList(List<PlayerObject> list)
	{
		if (list.Count <= 1)
			return list;
		int pivotPosition = list.Count / 2;
		PlayerObject pivotValue = list[pivotPosition];
		list.RemoveAt(pivotPosition);
		List<PlayerObject> smaller = new List<PlayerObject>();
		List<PlayerObject> greater = new List<PlayerObject>();
		foreach (PlayerObject item in list)
		{
			if (item.Data.RankPoint < pivotValue.Data.RankPoint)
			{
				smaller.Add(item);
			}
			else if (item.Data.RankPoint == pivotValue.Data.RankPoint)
			{
				if (item.Data.WinCount > pivotValue.Data.WinCount)
				{
					smaller.Add(item);
				}
				else
				{
					greater.Add(item);
				}
			}
			else
			{
				greater.Add(item);
			}
		}
		List<PlayerObject> sorted = SortPlayerObjectList(smaller);
		sorted.Add(pivotValue);
		sorted.AddRange(SortPlayerObjectList(greater));
		for (int i = 0; i < sorted.Count; i++)
		{
			if (sorted[i] == null)
			{
				Debug.Log("YYYYYYYYYYYYYYYYYY" + i);
			}
		}
		return sorted;
	}

	public static int PlayerObjectSort(PlayerObject p1,PlayerObject p2)
	{
		//higher rankpoint = stronger
		int rankPoint = p1.Data.RankPoint.CompareTo(p2.Data.RankPoint);
		if (rankPoint != 0)
		{
			return rankPoint;
		}
		//LowerWinCount = stronger
		return p2.Data.WinCount.CompareTo(p1.Data.WinCount);
	}

	public static Texture GetPlayerIcon(string IconName)
	{
		return Resources.Load<Texture>("Image/Sprite/" + IconName);
	}

	public static int GetPlayerRankInLeaderBoard()
	{
		for (int i = 0; i < PlayerDataManager.Instance.RankPlayers.Count; i++)
		{
			if (PlayerDataManager.Instance.PlayerData.PlayerSN.Equals(PlayerDataManager.Instance.RankPlayers[i].Data.PlayerSN))
			{
				return i + 1;
			}
		}
			return -1;
	}

	public static PlayerHeroData GetPlayerHeroFromHeroData(HeroData heroData,bool onlyGetReset = false)
	{
		//Check if there is already PlayerHeroData is in playerData
		if (!onlyGetReset)
		{
			for (int i = 0; i < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; i++)
			{
				if (heroData.key() == PlayerDataManager.Instance.PlayerData.OwnedHeros[i].HeroTableID)
				{
					PlayerDataManager.Instance.PlayerData.OwnedHeros[i].FragmentCount += 1;
					return PlayerDataManager.Instance.PlayerData.OwnedHeros[i];
				}
			}
		}

		//Create new entry
		int nextID = 0;
		for (int i = 0; i < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; i++)
		{
			int ID = PlayerDataManager.Instance.PlayerData.OwnedHeros[i].key();
			if (nextID > ID)
			{
				continue;
			}
			nextID = ID;
		}
		nextID += 1;
		Debug.Log("nextID = "+nextID+" and Ownedhero count = "+PlayerDataManager.Instance.PlayerData.OwnedHeros.Count);
		
		PlayerHeroData data = new PlayerHeroData();
		data.ID = nextID;
		data.PlayerTableID = 0;
		data.HeroTableID = heroData.ID;
		data.HeroLevel = 1;
		data.FragmentCount = 1;
		data.Skill1StatusType = (byte)E_EquipableStatus.Unlocked;
		data.Skill2StatusType = (byte)E_EquipableStatus.Locked;
		data.Skill3StatusType = (byte)E_EquipableStatus.Locked;
		data.Skill4StatusType = (byte)E_EquipableStatus.Locked;
		data.Skill4StatusType = (byte)E_EquipableStatus.Locked;
		data.Skill5StatusType = (byte)E_EquipableStatus.Locked;
		data.Skill6StatusType = (byte)E_EquipableStatus.Locked;

		List<ArmyData> armyList = heroData.GetArmiesFromArmyList();
		Debug.Log("ArmyList Count = " + armyList.Count);
		for (int i = 0; i < armyList.Count; i++)
		{
			HeroArmyData heroArmyData = GetHeroArmyFromArmy(armyList[i],heroData);
			switch (i)
			{
				case 0:
				data.HeroArmy1 = heroArmyData.key();

				break;
				case 1:
				data.HeroArmy2 = heroArmyData.key();
				break;
				case 2:
				data.HeroArmy3 = heroArmyData.key();
				break;
				case 3:
				data.HeroArmy4 = heroArmyData.key();
				break;
				case 4:
				data.HeroArmy5 = heroArmyData.key();
				break;
				case 5:
				data.HeroArmy6 = heroArmyData.key();
				break;
				case 6:
				data.HeroArmy7 = heroArmyData.key();
				break;
				case 7:
				data.HeroArmy8 = heroArmyData.key();
				break;
				case 8:
				data.HeroArmy9 = heroArmyData.key();
				break;
				case 9:
				data.HeroArmy10 = heroArmyData.key();
				break;
				case 10:
				data.HeroArmy11 = heroArmyData.key();
				break;
				case 11:
				data.HeroArmy12 = heroArmyData.key();
				break;
				case 12:
				data.HeroArmy13 = heroArmyData.key();
				break;
				case 13:
				data.HeroArmy14 = heroArmyData.key();
				break;
				case 14:
				data.HeroArmy15 = heroArmyData.key();
				break;
				case 15:
				data.HeroArmy16 = heroArmyData.key();
				break;
				case 16:
				data.HeroArmy17 = heroArmyData.key();
				break;
				case 17:
				data.HeroArmy18 = heroArmyData.key();
				break;
				case 18:
				data.HeroArmy19 = heroArmyData.key();
				break;
				case 19:
				data.HeroArmy20 = heroArmyData.key();
				break;
				default:
				Debug.Log("This goes wrong. ..............");
				break;
			}
		}
		data.ReCheckArmyUnlockConditions();
		
		HeroArmyData heroArmyDataSelected = data.GetEquippedArmySelf();
		if (heroArmyDataSelected == null)
		{
			List<HeroArmyData> allArmies = data.GetAllArmiesSelf();
			if (allArmies.Count > 0)
			{
				for (int i = 0; i < allArmies.Count; i++)
				{
					E_ArmyType type = (E_ArmyType)allArmies[i].GetArmyData().ArmyType;
					Debug.Log("This have This type ......................." + type);
					if ((type == E_ArmyType.BasicFootman) || (type == E_ArmyType.HeavyFootman) || (type == E_ArmyType.Archer))
					{
						allArmies[i].HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
						Debug.Log("Basic Footman is equipped. type = " + type.ToString());
						break;
					}
				}

			}
			else
			{
				Debug.Log("This is ridiculous ,,,,........................... " + data.ID);
			}

			heroArmyDataSelected = data.GetEquippedArmySelf();
			if (heroArmyDataSelected == null)
			{
				Debug.Log("This is when PlayerData does not have basic footman ");
			}
		}
		if (!onlyGetReset)
		{
			PlayerDataManager.Instance.PlayerData.OwnedHeros.Add(data);
		}
		//PlayerDataManager.Instance.SavePlayerData();
		Debug.Log("Success");
		return data;
	}

	public static HeroArmyData GetHeroArmyFromArmy(ArmyData armyData,HeroData heroData,bool onlyGetReset = false)
	{
		if (!onlyGetReset)
		{
			//Check if there is already HeroArmy is in playerData
			for (int i = 0; i < PlayerDataManager.Instance.PlayerData.PlayerHeroArmies.Count; i++)
			{
				HeroArmyData heroArmyData = PlayerDataManager.Instance.PlayerData.PlayerHeroArmies[i];
				if (heroArmyData.ArmyTableID == armyData.key() && heroArmyData.HeroTableID == heroData.key())
				{
					return heroArmyData;
				}
			}
		}


		//Create new entry of HeroArmy in PlayerData
		int nextID = 0;
		for (int i = 0; i < PlayerDataManager.Instance.PlayerData.PlayerHeroArmies.Count; i++)
		{
			int ID = PlayerDataManager.Instance.PlayerData.PlayerHeroArmies[i].key();
			if (nextID > ID)
			{
				continue;
			}
			nextID = ID;
		}
		nextID += 1;
		HeroArmyData heroArmy = new HeroArmyData
		{
			ID = nextID,
			HeroTableID = heroData.key(),
			ArmyTableID = armyData.key(),
			ArmyLevel = 1,
			HeroArmyStatusType = armyData.ArmyType == (byte)E_ArmyType.Footman ? (byte)E_EquipableStatus.Equipped : (byte)E_EquipableStatus.Locked,
			Skill1StatusType = (byte)E_EquipableStatus.Equipped,
			Skill2StatusType = (byte)E_EquipableStatus.Locked,
			Skill3StatusType = (byte)E_EquipableStatus.Locked,
			Skill4StatusType = (byte)E_EquipableStatus.Locked,
			Skill5StatusType = (byte)E_EquipableStatus.Locked,
			Skill6StatusType = (byte)E_EquipableStatus.Locked
		};
		
		if (!onlyGetReset)
		{
			PlayerDataManager.Instance.PlayerData.PlayerHeroArmies.Add(heroArmy);
			Debug.Log("New HeroArmy Addded with ID = " + heroArmy.ID);
		}
		//PlayerDataManager.Instance.SavePlayerData();
		return heroArmy;
	}

	public static PlayerObject GetEnemyBasedOnRank(int rankRange)
	{
		Debug.Log("GetEnemyBasedOnRank()");
		int currentRange = rankRange;
		int selfRank = PlayerDataManager.Instance.PlayerData.RankPoint;
		List<PlayerObject> fakePlayers = PlayerDataManager.Instance.RankPlayers;
		Debug.Log("Fake player count = " + fakePlayers.Count);

		List<PlayerObject> compatablePlayers = new List<PlayerObject>();
		if (fakePlayers != null)
		{
			for (int i = 0; i < fakePlayers.Count; i++)
			{
				if (fakePlayers[i].Data.PlayerSN.Equals( PlayerDataManager.Instance.PlayerData.PlayerSN))
					continue;
				if (Mathf.Abs(fakePlayers[i].Data.RankPoint - selfRank) < currentRange)
				{
					compatablePlayers.Add(fakePlayers[i]);
				}
			}
			if (compatablePlayers.Count > 0)
			{
				int randomIndex = Random.Range(0,compatablePlayers.Count);
				Debug.Log("Compatible player count = " + compatablePlayers.Count + "Random Index" + randomIndex);
				return compatablePlayers[randomIndex];
			}

			currentRange *= 2;
			if (currentRange > 200000)
			{
				int randomIndex = Random.Range(0,fakePlayers.Count);
				return fakePlayers[randomIndex];
			}
			GetEnemyBasedOnRank(currentRange);
			int rdIndex = Random.Range(0,fakePlayers.Count);
			return fakePlayers[rdIndex];
		}
		else
		{
			return null;
		}
	}
	public static GameObject FindTargetToAttack(List<Team> enemies,E_ArmyMoveType ArmyMoveType,E_ArmyAttackType ArmyAttackType,E_ArmyJobType ArmyJobType,Vector3 SelfPosition)
	{
		if (enemies.Count == 0)
		{
			return null;
		}
		GameObject target=null;
		switch (ArmyJobType)
		{
			case E_ArmyJobType.Attacker:
			switch (ArmyAttackType)
			{
				case E_ArmyAttackType.Meelee:
				if (ArmyMoveType == E_ArmyMoveType.Infantry)
				{
					target = FindEnemyWithAttackType(enemies,E_ArmyAttackType.Meelee,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = FindEnemyWithAttackType(enemies,E_ArmyAttackType.Range,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				else
				{
					target = FindEnemyWithAttackType(enemies,E_ArmyAttackType.Range,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = FindEnemyWithAttackType(enemies,E_ArmyAttackType.Meelee,SelfPosition);
					if (target != null)
					{
						break;
					}			
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				break;
				case E_ArmyAttackType.Range:
				if (ArmyMoveType == E_ArmyMoveType.Infantry)
				{
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				else
				{
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				break;
			}
			break;
			case E_ArmyJobType.Defender:
			switch (ArmyAttackType)
			{
				case E_ArmyAttackType.Meelee:
				if (ArmyMoveType == E_ArmyMoveType.Infantry)
				{
					target = FindEnemyWithJobType(enemies,E_ArmyJobType.Attacker,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = FindEnemyWithJobType(enemies,E_ArmyJobType.Assasin,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				else
				{
					target = FindEnemyWithJobType(enemies,E_ArmyJobType.Assasin,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = FindEnemyWithJobType(enemies,E_ArmyJobType.Attacker,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				break;
				case E_ArmyAttackType.Range:
				if (ArmyMoveType == E_ArmyMoveType.Infantry)
				{
					target = FindEnemyWithJobType(enemies,E_ArmyJobType.Attacker,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = FindEnemyWithJobType(enemies,E_ArmyJobType.Defender,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				else
				{
					target = FindEnemyWithMoveType(enemies,E_ArmyMoveType.Cavalry,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				break;
			}
			break;
			case E_ArmyJobType.Assasin:
			switch (ArmyAttackType)
			{
				case E_ArmyAttackType.Meelee:
				if (ArmyMoveType == E_ArmyMoveType.Infantry)
				{
					target = FindEnemyWithMoveType(enemies,E_ArmyMoveType.Infantry,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				else
				{
					target = FindEnemyWithAttackType(enemies,E_ArmyAttackType.Range,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				break;
				case E_ArmyAttackType.Range:
				if (ArmyMoveType == E_ArmyMoveType.Infantry)
				{
					target = FindEnemyWithAttackType(enemies,E_ArmyAttackType.Meelee,SelfPosition);
					if (target != null)
					{
						break;
					}
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				else
				{
					target = GetClosetEnemy(enemies,SelfPosition);
				}
				break;
			}
			break;
		}
		return target;
	}
	public static GameObject GetClosetEnemy(List<Team> enemies,Vector3 SelfPosition)
	{
		if (enemies.Count == 0)
		{
			return null;
		}
		GameObject newTarget = enemies[0].gameObject;
		float tempDistance = Vector3.Distance(SelfPosition,newTarget.transform.position);
		for (int i = 1; i < enemies.Count; i++)
		{
			float dis = Vector3.Distance(SelfPosition,enemies[i].transform.position);
			if (dis < tempDistance)
			{
				tempDistance = dis;
				newTarget = enemies[i].gameObject;
			}
		}
		return newTarget;
	}
	public static GameObject GetClosetVisible(List<Team> enemies,Transform transform,float Range,float MaxAngle)
	{
		if (enemies.Count == 0)
		{
			return null;
		}
		GameObject newTarget = null;
		float tempAngle=360f;
		for (int i = 0; i < enemies.Count; i++)
		{
			float dis = Vector3.Distance(transform.position,enemies[i].transform.position);
			if (dis < Range)
			{
				Vector3 selfPoint = new Vector3(transform.position.x,0,transform.position.z);
				Vector3 enemyPoint = new Vector3(enemies[i].transform.position.x,0,enemies[i].transform.position.z);

				Vector3 direction = (enemyPoint - selfPoint).normalized;
				float angle=Vector3.Angle(transform.forward,direction);
				if (angle < tempAngle && angle<MaxAngle)
				{
					tempAngle = angle;
					newTarget = enemies[i].gameObject;
				}
			}
		}
		return newTarget;
	}
	public static GameObject FindEnemyWithAttackType(List<Team> enemies,E_ArmyAttackType attackType,Vector3 SelfPosition)
	{
		GameObject newTarget = null;


		List<Team> filteredEnemies = new List<Team>();

		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].GetComponent<Leader>().ArmyAttackType == attackType)
			{
				filteredEnemies.Add(enemies[i]);
			}
		}
		if (filteredEnemies.Count > 0)
		{
			newTarget = GetClosetEnemy(filteredEnemies,SelfPosition);
		}
		return newTarget;
	}
	public static GameObject FindEnemyWithJobType(List<Team> enemies,E_ArmyJobType jobType,Vector3 SelfPosition)
	{
		GameObject newTarget = null;


		List<Team> filteredEnemies = new List<Team>();

		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].GetComponent<Leader>().ArmyJobType == jobType)
			{
				filteredEnemies.Add(enemies[i]);
			}
		}
		if (filteredEnemies.Count > 0)
		{
			newTarget = GetClosetEnemy(filteredEnemies,SelfPosition);
		}
		return newTarget;
	}
	public static GameObject FindEnemyWithMoveType(List<Team> enemies,E_ArmyMoveType moveType,Vector3 SelfPosition)
	{
		GameObject newTarget = null;


		List<Team> filteredEnemies = new List<Team>();

		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i].GetComponent<Leader>().ArmyMoveType == moveType)
			{
				filteredEnemies.Add(enemies[i]);
			}
		}
		if (filteredEnemies.Count > 0)
		{
			newTarget = GetClosetEnemy(filteredEnemies,SelfPosition);
		}
		return newTarget;
	}
}
