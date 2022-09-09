using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResult : UIBase
{
    public GameObject WinImage;
    public GameObject LoseImage;
    public Text txtbamboofragment, txtbattlePoint;
    //public GameObject DrawImage;
    public Button BackButton;
    public UIPlayerScore PlayerOneScore;
    public UIPlayerScore PlayerTwoScore;
    public GameResult Result;

    private void SetWinLose(bool isWin, bool isDraw = false)
    {
        if (!isDraw)
        {
            WinImage.SetActive(isWin);
            LoseImage.SetActive(!isWin);
            //DrawImage.SetActive(false);
            if (isWin)
            {
                addBackButtomEvent();
                Invoke("WinSound", 1f);
            }
            else
            {
                addBackButtomEvent(3);
                Invoke("LoseSound", 1f);
            }
        }
        else
        {
            WinImage.SetActive(false);
            LoseImage.SetActive(false);
            //DrawImage.SetActive(true)
            addBackButtomEvent();
        }
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);

		Time.timeScale = 1;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;

		Result = (GameResult)Objects[0];
        PlayerOneScore.SetPlayerScore(Result.PlayerOneResult);
        PlayerTwoScore.SetPlayerScore(Result.PlayerTwoResult);

		int playerResult = Result.PlayerOneResult.TotalScore - Result.PlayerTwoResult.TotalScore;
		if (playerResult > 0) //playerWon
		{
			SetWinLose(true);
			if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.CampaignMatch)
			{
				LevelData currentLevel = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData();
				if (currentLevel != null)
				{
					PlayerDataManager.Instance.PlayerData.CampaignLevelData.PlayerCurrentLevelID = currentLevel.NextLvlID;
				}
			}

			if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.TowerMatch)
			{
				TowerModeLevelData currentLevel = PlayerDataManager.Instance.PlayerData.TowerLevelData.GetPlayerCurrentLevelData();
				if (currentLevel != null)
				{
					PlayerDataManager.Instance.PlayerData.TowerLevelData.PlayerCurrentLevelID = currentLevel.NextLvlID;
				}
			}
		}
		else if (playerResult < 0) //playerLose
		{
			SetWinLose(false);
		}
		else //draw
		{
			SetWinLose(true,true); //Set Draw First Param is Optional
		}

        int bambooCount = Random.Range(500, 1500);
        txtbamboofragment.text = bambooCount.ToString();
        PlayerDataManager.Instance.PlayerData.BambooFragment += bambooCount;

        int battlePoint = Random.Range(500, 1500);
        txtbattlePoint.text = battlePoint.ToString();
        PlayerDataManager.Instance.PlayerData.BattlePoint += battlePoint;
        //PlayerDataManager.Instance.SavePlayerData();

    }
    private void WinSound()
    {
        SoundManager.Instance.PlaySound("MS00003");
    }
    private void LoseSound()
    {
        SoundManager.Instance.PlaySound("MS00004");

    }
    protected override void OnInit()
    {
        base.OnInit();

    }
    private void addBackButtomEvent(int rewardCount = 5)
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(delegate
        {
			PlayerDataManager.Instance.SavePlayerData();
			UIManager.Instance.CloseAllUI();
			SoundManager.Instance.StopBGM();
			UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
			UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
            //UIManager.Instance.ShowUI(GLOBALCONST.UI_GACHA, GLOBALCONST.UI_MAIN_MENU, true, rewardCount);
        });
    }
}
