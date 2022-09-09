using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderBoard : UIBase
{
    public Button BtnClose;
    public Transform PlayerScrollView;
	public UILeaderboardRowPrefabController RowPrefabController;
	private const int MAX_LEADERBOARD_COUNT = 20;

	private List<PlayerObject> m_PlayerObjects;

    protected override void OnInit()
    {
        base.OnInit();
        BtnClose.onClick.AddListener(CloseUI);
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
		RowPrefabController.gameObject.SetActive(false);
		for (int i = 0; i < PlayerScrollView.childCount; i++)
		{
			Destroy(PlayerScrollView.GetChild(i).gameObject);
		}



		//PlayerObject realPlayerObject = GLOBALFUNCTION.ConvertPlayerDataToPlayerObject();
		m_PlayerObjects = PlayerDataManager.Instance.RankPlayers;
        if(m_PlayerObjects!=null) {
		    for (int i = 0; i < m_PlayerObjects.Count; i++)
		    {
			    if (m_PlayerObjects[i] == null)
			    {
				    Debug.Log("LeaderBoard OnShow Null before real player add" + i);
			    }
		    }
		    //m_PlayerObjects.Add(realPlayerObject);
		    for (int i = 0; i < m_PlayerObjects.Count; i++)
		    {
			    if (m_PlayerObjects[i] == null)
			    {
				    Debug.Log("LeaderBoard OnShow Null" + i);
			    }
		    }
		    //m_PlayerObjects = GLOBALFUNCTION.SortPlayerObjectList(m_PlayerObjects);
		    //m_PlayerObjects.Sort(GLOBALFUNCTION.PlayerObjectSort);
		    //m_PlayerObjects.Reverse();
		    for (int i = 0; i < MAX_LEADERBOARD_COUNT; i++)
		    {
                if(i>=m_PlayerObjects.Count)
                    break;
			    GameObject go = Instantiate(RowPrefabController.gameObject,PlayerScrollView);
			    UILeaderboardRowPrefabController control = go.GetComponent<UILeaderboardRowPrefabController>();
                    control.RefreshData(m_PlayerObjects[i], i + 1, PlayerDataManager.Instance.PlayerData.PlayerSN.Equals(PlayerDataManager.Instance.RankPlayers[i].Data.PlayerSN));
                go.SetActive(true);
		    }
        }
    }

    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_LEADER_BOARD);
    }
}
