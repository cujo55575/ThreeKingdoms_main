using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public List<Leader> teamOne;
    public List<Leader> teamOneDeads;
	public List<Leader> teamTwo;
    public List<Leader> teamTwoDeads;

    public PlayerObject enemy;

	public Canvas SwipeCanvas;

    private AudioSource audioSource;

	public string name2 = null;
	public Texture tex2 = null;

	private float VibrateTimer=0.0f;
	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
		teamOne = new List<Leader>();
        teamOneDeads = new List<Leader>();
		teamTwo = new List<Leader>();
        teamTwoDeads = new List<Leader>();

        string name1= PlayerDataManager.Instance.PlayerData.Name;
		
        
        Texture tex1 = GLOBALFUNCTION.GetPlayerIcon(PlayerDataManager.Instance.PlayerData.PlayerIconName);
		
		if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.RankedMatch || GLOBALVALUE.CURRENT_MATCH_MODE==E_MatchMode.FriendlyMatch)
		{
			tex2 = GLOBALFUNCTION.GetPlayerIcon(enemy.Data.PlayerIconName);
			name2 = enemy.Data.Name;
		}
		Leader[] leaders = GameObject.FindObjectsOfType<Leader>();

		for (int i = 0; i < leaders.Length; i++)
		{
			if (leaders[i].team.team == Team.TeamType.TeamOne)
			{
				teamOne.Add(leaders[i]);
			}
			else
            {
                teamTwo.Add(leaders[i]);
			}
		}

        UIManager.UIInstance<UIBattleCanvas>().OnGameStart();
        UIManager.Instance.ShowUI(GLOBALCONST.UI_BATTLE_START_ANIMATION,name1,name2,tex1,tex2);
    }
	public void Vibrate(float duration)
	{
		if (VibrateTimer < duration)
		{
			VibrateTimer = duration;
		}
	}
	private void Update()
	{
		if (VibrateTimer > 0)
		{
			Handheld.Vibrate();
			VibrateTimer -= Time.deltaTime;
		}		
		List<Leader> tempLeaders=new List<Leader>();
		for (int i = 0; i < teamOne.Count; i++)
		{
			if (teamOne[i].HP <= 0)
			{
				tempLeaders.Add(teamOne[i]);
			}
		}

		for (int i = 0; i < tempLeaders.Count; i++)
		{
			teamOne.Remove(tempLeaders[i]);
            teamOneDeads.Add(tempLeaders[i]);
		}

		tempLeaders = new List<Leader>();
		for (int i = 0; i < teamTwo.Count; i++)
		{
			if (teamTwo[i].HP <= 0)
			{
				tempLeaders.Add(teamTwo[i]);
			}
		}

		for (int i = 0; i < tempLeaders.Count; i++)
		{
			teamTwo.Remove(tempLeaders[i]);
            teamTwoDeads.Add(tempLeaders[i]);
        }

        bool PlayOverallSound=false;

        for(int i=0;i<teamOne.Count;i++)
        {
            if(teamOne[i].GetComponent<Animator>().GetBool(teamOne[i].AttackingParam))
            {
                PlayOverallSound = true;
            }
        }
        for (int i = 0; i < teamTwo.Count; i++)
        {
            if (teamTwo[i].GetComponent<Animator>().GetBool(teamTwo[i].AttackingParam))
            {
                PlayOverallSound = true;
            }
        }

        audioSource.enabled = PlayOverallSound;

        if (teamOne.Count == 0 && teamTwo.Count == 0)
		{
			SwipeCanvas.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
			CancelAllInvoke();
			UIManager.Instance.CloseAllUI();
            Time.timeScale =0.3f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			Invoke("Draw",1.0f);
		}
		else if (teamOne.Count == 0)
		{
			SwipeCanvas.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
            SoundManager.Instance.StopBGM();
			CancelAllInvoke();
            UIManager.Instance.CloseAllUI();

            if(GLOBALVALUE.CURRENT_MATCH_MODE==E_MatchMode.RankedMatch)
            {
                PlayerDataManager.Instance.PlayerData.LoseCount++;
                enemy.Data.WinCount++;

                enemy.Data.RankPoint += (25 - (3 * (5 - teamTwo.Count)));

                Debug.Log((25 - (3 * (5 - teamTwo.Count))));

                //PlayerDataManager.Instance.SavePlayerData();
            }
			Time.timeScale = 0.3f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;

			if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.CampaignMatch)
			{
				LevelData levelData = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData();

				List<DialogueData> dialogues = levelData.GetAllDialogues();
				if (dialogues.Count > 0)
				{
					for (int i = 0; i < dialogues.Count; i++)
					{
						if ((E_DialougeTriggerType)dialogues[i].DialogueTriggerType == E_DialougeTriggerType.AfterBattleLose)
						{
							UIManager.Instance.ShowUI(GLOBALCONST.UI_DIALOGUE,dialogues[i]);
							//return;
						}
					}
				}
			}

			Invoke("Lose",1.0f);
		}
		else if (teamTwo.Count==0)
		{
			SwipeCanvas.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
            SoundManager.Instance.StopBGM();
			CancelAllInvoke();
			UIManager.Instance.CloseAllUI();
            if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.RankedMatch)
            {
                PlayerDataManager.Instance.PlayerData.WinCount++;
                enemy.Data.LoseCount++;

                PlayerDataManager.Instance.PlayerData.RankPoint+=(25 - (3 * (5 - teamOne.Count)));

                Debug.Log((25 - (3 * (5 - teamOne.Count))));

                //PlayerDataManager.Instance.SavePlayerData();
            }
			Time.timeScale = 0.3f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;


			if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.CampaignMatch)
			{
				LevelData levelData = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData();

				List<DialogueData> dialogues = levelData.GetAllDialogues();
				if (dialogues.Count > 0)
				{
					for (int i = 0; i < dialogues.Count; i++)
					{
						if ((E_DialougeTriggerType)dialogues[i].DialogueTriggerType == E_DialougeTriggerType.AfterBattleWin)
						{
							UIManager.Instance.ShowUI(GLOBALCONST.UI_DIALOGUE,dialogues[i]);
							//return;
						}
					}
				}
			}

			Invoke("Win",1.0f);
		}
	}
    public Leader GetDeadLeader(Team.TeamType team)
    {
        if(team==Team.TeamType.TeamOne)
        {
            if(teamOneDeads.Count>0)
            {
                int rand = Random.Range(0,teamOneDeads.Count);
                Leader tempLeader = teamOneDeads[rand];

                teamOneDeads.RemoveAt(rand);
                teamOne.Add(tempLeader);

                return tempLeader;
            }
        }
        else
        {
            if (teamTwoDeads.Count > 0)
            {
                int rand = Random.Range(0, teamTwoDeads.Count);
                Leader tempLeader = teamTwoDeads[rand];

                teamTwoDeads.RemoveAt(rand);
                teamTwo.Add(tempLeader);

                return tempLeader;
            }
        }
        return null;
    }
    public void RefreshEnemies()
    {
        if (teamOne.Count > 0)
        {
            for (int i = 0; i < teamOne.Count; i++)
            {
                teamOne[i].FindTargets();
            }
        }

        if (teamTwo.Count > 0)
        {
            for (int i = 0; i < teamTwo.Count; i++)
            {
                teamTwo[i].FindTargets();
            }
        }
    }
	public List<Leader> GetTeamates(Team.TeamType team)
	{
		if (team == Team.TeamType.TeamOne)
		{
			return teamOne;
		}
		else if (team==Team.TeamType.TeamTwo)
		{
			return teamTwo;
		}
		return null;
	}
	public void Win()
	{
        Debug.Log("Win()");
		PlayerScore p1 = new PlayerScore("","Player 1",1,0,0);
		PlayerScore p2 = new PlayerScore("","Player 2",0,0,0);
		GameResult result = new GameResult(p1,p2);
		UIManager.Instance.ShowUI("UIGameResult",result);
        UIManager.Instance.CloseUI("UIBattleHPBar");
    }
	public void Lose()
	{
        Debug.Log("Lose()");
		PlayerScore p1 = new PlayerScore("","Player 1",0,0,0);
		PlayerScore p2 = new PlayerScore("","Player 2",1,0,0);
		GameResult result = new GameResult(p1,p2);
		UIManager.Instance.ShowUI("UIGameResult",result);
        UIManager.Instance.CloseUI("UIBattleHPBar");
    }
	public void Draw()
	{
        Debug.Log("Draw()");
		PlayerScore p1 = new PlayerScore("","Player 1",1,0,0);
		PlayerScore p2 = new PlayerScore("","Player 2",1,0,0);
		GameResult result = new GameResult(p1,p2);

		UIManager.Instance.ShowUI("UIGameResult",result);
        UIManager.Instance.CloseUI("UIBattleHPBar");
    }

	public void CancelAllInvoke()
	{
		Camera.main.GetComponent<Animator>().ResetTrigger("FadeIn");
		Camera.main.GetComponent<Animator>().SetBool("IsShaking",false);
		Camera.main.GetComponent<Animator>().CrossFade("idle",0);
		
		if (teamOne.Count > 0)
		{
			for (int i = 0; i < teamOne.Count; i++)
			{
				teamOne[i].CancelInvoke();
			}
		}
		if (teamTwo.Count > 0)
		{
			for (int i = 0; i < teamTwo.Count; i++)
			{
				teamTwo[i].CancelInvoke();
			}
		}
	}
}
