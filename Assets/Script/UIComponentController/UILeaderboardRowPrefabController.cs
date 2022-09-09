using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderboardRowPrefabController : MonoBehaviour
{
    public RawImage IconProfile;
	public Text TxtPlayerRank;
	public Text TxtPlayerName;
	public Text TxtWin;
	public Text TxtWinCount;
	public Text TxtLose;
	public Text TxtLoseCount;
	public Text TxtRankPoint;
	public Text TxtRankPointValue;
	public Text TxtFriendlyMatch;
	public Button BtnFriendlyMatch;

	private PlayerObject m_PlayerObjectData;

	private void Start()
	{
		BtnFriendlyMatch.onClick.AddListener(OnClickFriendlyMatch);
	}

	public void RefreshData(PlayerObject dataObjct, int rank,bool isDifferentPlayer)
	{
		m_PlayerObjectData = dataObjct;
		IconProfile.texture = GLOBALFUNCTION.GetPlayerIcon(m_PlayerObjectData.Data.PlayerIconName);
		string rankString = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_LEADERBOARD_RANK_FORMAT);
		TxtPlayerRank.text = string.Format(rankString,rank);
		TxtPlayerName.text = m_PlayerObjectData.Data.Name;
		TxtWin.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_LEADERBOARD_WIN_COUNT);
		TxtWinCount.text = m_PlayerObjectData.Data.WinCount.ToString();
		TxtLose.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_LEADERBOARD_LOSE_COUNT);
		TxtLoseCount.text = m_PlayerObjectData.Data.LoseCount.ToString();
		TxtRankPoint.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_LEADERBOARD_RANK_POINT);
		TxtRankPointValue.text = m_PlayerObjectData.Data.RankPoint.ToString();
		TxtFriendlyMatch.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_LEADERBOARD_FRIENDLY_MATCH);
        //BtnFriendlyMatch.gameObject.SetActive(!dataObjct.Data.IsRealPlayer);
        BtnFriendlyMatch.gameObject.SetActive(!isDifferentPlayer);
    }

	private void OnClickFriendlyMatch()
	{
		Debug.Log("Friendly Match Start with Fake Player Name" + m_PlayerObjectData.Data.Name);
		GLOBALVALUE.CURRENT_ENEMY = m_PlayerObjectData;
		GLOBALVALUE.CURRENT_MATCH_MODE = E_MatchMode.FriendlyMatch;
		UIManager.Instance.CloseAllUI();
		UIManager.Instance.ShowUI("UIMatchMaking");
	}
}
