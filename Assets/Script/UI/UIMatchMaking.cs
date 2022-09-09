using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIMatchMaking : UIBase
{
    // public const string MatchFindingStatusText = "搜索敵軍中";
    public const string MatchFoundStatusText = "Found The Enemy";
    public const string ConnectionBreakdownText = "No Connection";

    private const string m_LoadingSceneTextFormat = "Scene Name - {0} is at progress - {1}%";

    public GameObject MatchFoundGroup;
    public GameObject MatchFindingGroup;

    public GameObject CircularRotator;
    public GameObject SubRotator;
    public Text StatusText;
    public Text txtCancel;

    public enum MatchMakingState
    {
        MatchFinding,
        MatchFound,
        ConnectionBreakdown
    }
    public void ChangeMatchMakingState(MatchMakingState state)
    {
        CloseAllUI();
        switch (state)
        {
            case MatchMakingState.MatchFinding:
                Flag = 0;
                timer = 0;
                MatchFindingGroup.SetActive(true);
                SetStatus(TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_SEARCHENEMY));
                break;
            case MatchMakingState.MatchFound:
                MatchFoundGroup.SetActive(true);
                break;
            case MatchMakingState.ConnectionBreakdown:
                SetStatus(ConnectionBreakdownText);
                break;
        }
    }
    private void CloseAllUI()
    {
        MatchFoundGroup.SetActive(false);
        MatchFindingGroup.SetActive(false);
    }
    public void SetStatus(string paraText)
    {
        StatusText.text = paraText;
    }
    public void PressCancel()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MATCHMAKING);
        // UIManager.Instance.ShowUI(GLOBALCONST.UI_MATCH_BATTLE);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
        Debug.Log("Cancel Buttom has been pressed");
        //StartCoroutine(LoadScene());
    }

	  IEnumerator LoadScene()
    {
        yield return new WaitForSecondsRealtime(5f);
        GLOBALVALUE.CURRENT_ENEMY = GLOBALFUNCTION.GetEnemyBasedOnRank(10000);
        while (GLOBALVALUE.CURRENT_ENEMY == null)
        {
            GLOBALVALUE.CURRENT_ENEMY = GLOBALFUNCTION.GetEnemyBasedOnRank(10000);
            Debug.Log(string.Format("CURRENT_ENEMY={0}", GLOBALVALUE.CURRENT_ENEMY));
            yield return new WaitForSecondsRealtime(1);
        }
        GLOBALVALUE.CURRENT_MATCH_MODE = E_MatchMode.RankedMatch;
        ChangeMatchMakingState(MatchMakingState.MatchFound);
       // UIManager.Instance.CloseUI(GLOBALCONST.UI_MATCHMAKING);
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("BattleScene");

        //When the load is still in progress, output the Text and progress bar
        string MatchFoundText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_MATCHFOUND);
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            StatusText.text = MatchFoundText + Mathf.RoundToInt(asyncOperation.progress * 100) + "%";
            Debug.Log(string.Format("Progress :{0}%", asyncOperation.progress * 100));
            yield return null;
        }
    }
	IEnumerator LoadSceneSimple()
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("BattleScene");

		//When the load is still in progress, output the Text and progress bar
		string MatchFoundText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_MATCHFOUND);
		while (!asyncOperation.isDone)
		{
			//Output the current progress
			StatusText.text = MatchFoundText + Mathf.RoundToInt(asyncOperation.progress * 100) + "%";
			Debug.Log(string.Format("Progress :{0}%",asyncOperation.progress * 100));
			yield return null;
		}
	}
    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void OnDefocus()
    {
        base.OnDefocus();
    }

    protected override void OnEvent()
    {
        base.OnEvent();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnShow(params object[] Objects)
    {
		if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.RankedMatch)
		{
			ChangeMatchMakingState(MatchMakingState.MatchFinding);
			//CNetManager.Instance.C2GS_6_1();
			StartCoroutine(LoadScene());
		}
		else
		{
			ChangeMatchMakingState(MatchMakingState.MatchFound);
			StartCoroutine(LoadSceneSimple());
		}
        base.OnShow(Objects);
		
        txtCancel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_CANCELBTN);
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    float timer = 0.0f;
    int Flag = 0;

    protected override void OnUpdate()
    {
        if (MatchFindingGroup.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                Flag++;
            }
            if (Flag > 3)
            {
                Flag = 0;
            }
            string subText = "";
            string frontText = "";
            for (int i = 0; i < Flag; i++)
            {
                subText += ".";
                frontText += " ";
            }
            SetStatus(frontText + TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_SEARCHENEMY) + subText);
        }

        CircularRotator.transform.Rotate(0, 0, -150 * Time.deltaTime);
        SubRotator.transform.rotation = Quaternion.Euler(0, 0, CircularRotator.transform.rotation.z * -1);
        base.OnUpdate();
    }
}
