using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchBattle : UIBase
{
    public Button BtnLeaderBoard;
    public Button BtnBack;
    public Button BtnMatch;
    public UITopBar topBar;

    #region PlayerRankPanelVariables

    public GameObject PanelPlayerRank;
    public Text Name;
    public Text LeaderBoard;
    public Text playerRank;
    public Text TxtPlayerName;
    public Text TxtWin;
    public Text TxtWinValue;
    public Text TxtLose;
    public Text TxtLoseValue;
    public Text TxtPoint;
    public Text TxtPointValue;

    #endregion


    protected override void OnInit()
    {
        base.OnInit();

        BtnBack.onClick.AddListener(CloseUI);
        BtnMatch.onClick.AddListener(MatchBattle);
        BtnBack.onClick.AddListener(CloseUI);
        BtnLeaderBoard.onClick.AddListener(ShowLeaderBoard);

    }

    protected override void OnShow(params object[] Objects)
    {
        //TODO GET PLAYER RANK CNetManager.Instance.C2GS_6_2(Refresh);
        base.OnShow(Objects);
        Name.text = GLOBALCONST.PLAYER_NAME_TEMP;
        LeaderBoard.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.BUTTON_LEADERBOARD);
        //Refresh();
        //ValueShow();
    }
    void ValueShow()
    {
		TxtWinValue.text = PlayerDataManager.Instance.PlayerData.WinCount.ToString();
        TxtLoseValue.text = PlayerDataManager.Instance.PlayerData.LoseCount.ToString();
        TxtPointValue.text = PlayerDataManager.Instance.PlayerData.RankPoint.ToString();
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            SoundManager.Instance.PlayBGM("BGM00001");
        }
    }
    public void Refresh(bool success)
    {
        ShowPlayerRankPanel();
        topBar.RefreshData();
        ValueShow();
    }

    public void ShowPlayerRankPanel()
    {
        TxtPlayerName.text = PlayerDataManager.Instance.PlayerData.Name;
        // TxtPlayerRank.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEADERBOARD_RANK);

        TxtWin.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEADERBOARD_WIN);
        TxtLose.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEADERBOARD_LOSE);
        TxtPoint.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEADERBOARD_POINT);

		playerRank.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEADERBOARD_RANK) + ":" + GLOBALFUNCTION.GetPlayerRankInLeaderBoard();

        TxtWinValue.text = PlayerDataManager.Instance.PlayerData.WinCount.ToString();
        TxtLoseValue.text = PlayerDataManager.Instance.PlayerData.LoseCount.ToString();
        TxtPointValue.text = PlayerDataManager.Instance.PlayerData.RankPoint.ToString();
    }

    public void MatchBattle()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MATCH_BATTLE);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MATCHMAKING);
    }

    public void ShowLeaderBoard()
    {
        CNetManager.Instance.C2GS_6_1(ShowUI);
    }
    
    private void ShowUI(bool success)
    {
        UIManager.Instance.ShowUI(GLOBALCONST.UI_LEADER_BOARD);
    }

    public void ShowOption()
    {
        UIManager.Instance.ShowUI(GLOBALCONST.UI_OPTION);
    }

    public void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MATCH_BATTLE);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
    }
}
