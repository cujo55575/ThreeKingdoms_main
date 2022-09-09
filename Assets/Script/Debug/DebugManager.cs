using System.IO;
using UnityEditor;
using UnityEngine;
using XEResources;
using Common.Player;

public class DebugManager : MonoBehaviour
{

    protected static DebugManager ms_instance;
    public static DebugManager Instance
    {
        get
        {
            if (ms_instance == null)
            {
                ms_instance = Main.Instance.gameObject.AddComponent<DebugManager>();
            }
            return ms_instance;
        }
    }

	private Rect Rect_DebugWin = new Rect(0,0,300,500);
    private Rect Rect_AllWnd = new Rect(0, 130, 300, 450);
    private Rect Rect_CloseBtn = new Rect(250, 0, 50, 20);
    private Rect Rect_FullBtn = new Rect(0, 0, 300, 50);
    public static bool Show_DebugWin = false;

	// Use this for initialization
	private GameObject raycastBlocker;
    void Start()
    {
		raycastBlocker = GameObject.Find("RaycastBlock").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		raycastBlocker.SetActive(Show_DebugWin);
		if (Input.GetKeyUp(KeyCode.Space))
        {
            Show_DebugWin = !Show_DebugWin;
        }
        else if (Input.touchCount > 3)
        {
            Show_DebugWin = !Show_DebugWin;
        }
		
    }

    void OnGUI()
    {
        if (!Show_DebugWin)
            return;
        GUI.Window(0, Rect_DebugWin, doDebugWin, "--------------------Debug--------------------");

    }
	private string m_LvlIDString = "1";
	private int m_LvlID = 1;

	void doDebugWin(int windowId)
	{
		if (GUI.Button(Rect_CloseBtn,"x"))
		{
			Show_DebugWin = false;
		}
		if (GUI.Button(Rect_FullBtn,"Show DeckUI"))
		{
			UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK);
		}
		GUI.Label(new Rect(0,50,150,50),"LevelID");
		m_LvlIDString = GUI.TextField(new Rect(150,50,150,50),m_LvlIDString);
		if (int.TryParse(m_LvlIDString,out m_LvlID))
		{
			if (GUI.Button(new Rect(0,100,300,50),string.Format("Change Campaign to Level = {0}",m_LvlID)))
			{
				LevelData data = TableManager.Instance.LevelDataTable.GetData(m_LvlID);
				if (data != null)
				{
					PlayerDataManager.Instance.PlayerData.CampaignLevelData.PlayerCurrentLevelID = m_LvlID;
					PlayerDataManager.Instance.SavePlayerData();
				}
			}
		}

		
		if (GUI.Button(new Rect(0,150,300,50),"I am New Player"))
		{
			PlayerPrefs.SetInt(GLOBALCONST.IS_NEW_PLAYER,0);
			PlayerDataManager.Instance.SignOut();
		}
		if (GUI.Button(new Rect(0,200,300,50),"GIVE ME EVERYTHING!!!!!"))
		{
			PlayerDataManager.Instance.PlayerData.BambooRoll = 999999;
			PlayerDataManager.Instance.PlayerData.BambooFragment = 999999;
			PlayerDataManager.Instance.PlayerData.BattlePoint = 999999;
			//PlayerDataManager.Instance.SavePlayerData();
			UITopBar topbar = FindObjectOfType<UITopBar>();
			if (topbar != null)
			{
				topbar.RefreshData();
			}
		}

		if (GUI.Button(new Rect(0,250,300,50),"PlayerDataStatus"))
		{
			PlayerData data = PlayerDataManager.Instance.PlayerData;
			Debug.Log(data.ToString());
		}

		if (GUI.Button(new Rect(0,300,300,50),"FixGacha"))
		{
			GLOBALFUNCTION.GACHA.GetRewardPlayerHero(E_GachaType.Fixed);
		}

		if (GUI.Button(new Rect(0,350,300,50),"TestScene"))
		{
			UIManager.Instance.CloseAllUI();
			UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
		}
		if (GUI.Button(new Rect(0,400,300,50),"ShowUIDialogue"))
		{
			if (UIManager.UIInstance<UIDialogue>() == null || !UIManager.UIInstance<UIDialogue>().gameObject.activeInHierarchy)
			{
				UIManager.Instance.ShowUI(GLOBALCONST.UI_DIALOGUE);
			}
			else
			{
				UIManager.Instance.CloseUI(GLOBALCONST.UI_DIALOGUE);
			}
		}
		if (GUI.Button(new Rect(0,450,300,50),"Debug current Save army formation"))
		{
			for (int i = 0; i < PlayerDataManager.Instance.PlayerData.SavedArmyFormation.Count; i++)
			{
				ArmyBattleFormation formation = PlayerDataManager.Instance.PlayerData.SavedArmyFormation[i];
				Debug.Log(string.Format("SavedArmyFormation - Index = {0}, PlayerHeroID = {1}, GridX = {2}, GridY = {3}",i,formation.PlayerHeroID,formation.GridX,formation.GridY));
			}
		}

		
	}
}
