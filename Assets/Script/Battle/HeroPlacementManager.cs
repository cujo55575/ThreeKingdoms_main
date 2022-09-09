using Common.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroPlacementManager : MonoBehaviour
{
	public GameObject Grid;
	public int offset = 100;

	public GameObject[,] Grids;
	public Team.TeamType yourTeam;

	public GameObject BattleManager;
	public int LeadersCount = 0;
	public Button BtnFight;
	public Image imgFight;

	public Sprite ImageW;
	public Sprite ImageY;
	public Sprite TextImageW;
	public Sprite TextImageY;

	public GameObject[] Armies;
	public Sprite[] ArmyFaces;

	private Leader SelectedLeader;
	private Leader RemoveableLeader;
	// public GameObject SwipeScreen;
	public Vector3 LastPosition;
	public GameObject GlowEffect;

	public RectTransform box;
	public RectTransform DragCard;
	public Text txtHeroName;
	public RawImage DragCardIcon;

	public Text txtOurArmyText;
	public string TextOurArmy;

	public FormulaObject formulaObject;

	private Vector3 mousePos;
	private bool Swipe = false;

	public List<string> mapList=new List<string>();

	public GameObject TextBar;

	public Canvas cardsCanvas;
	public Canvas m_Canvas;
	private void Awake()
	{
		QualitySettings.SetQualityLevel(0,true);
		if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.RankedMatch || GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.FriendlyMatch)
		{
			int rand = Random.Range(0,mapList.Count);
			GLOBALVALUE.CURRENT_MAP_NAME = mapList[rand];
		}
        UnityEngine.SceneManagement.SceneManager.LoadScene(GLOBALVALUE.CURRENT_MAP_NAME, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    public void OnLevelWasLoaded(int level)
    {
        Invoke("OnFinishLoading",0.1f);
    }
    void OnFinishLoading()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MATCHMAKING);
        StartMethod();

		if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.CampaignMatch)
		{
			LevelData levelData = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData();

			List<DialogueData> dialogues = levelData.GetAllDialogues();
			if (dialogues.Count > 0)
			{
				for (int i = 0; i < dialogues.Count; i++)
				{
					if ((E_DialougeTriggerType)dialogues[i].DialogueTriggerType == E_DialougeTriggerType.BeforeBattle)
					{
						UIManager.Instance.ShowUI(GLOBALCONST.UI_DIALOGUE,dialogues[i]);
						cardsCanvas.enabled = false;
						m_Canvas.enabled = false;
						return;
					}
				}
			}
		}
	}
    public void SwitchModel(PlayerHeroData data,HeroArmyData HeroData,BattleCardMono bcm)
    {
        List<Leader> Teamates = new List<Leader>();
        Leader[] leaders = GameObject.FindObjectsOfType<Leader>();
        for (int i = 0; i < leaders.Length; i++)
        {
            if (leaders[i].team.team == yourTeam)
            {
                Teamates.Add(leaders[i]);
            }
        }
        GameObject gridIns=null;
        for (int i=0;i<Teamates.Count;i++)
        {
            if(Teamates[i].m_PlayerHeroData==data)
            {
                gridIns=Teamates[i].getGrid();
                Destroy(Teamates[i].gameObject);
                break;
            }
        }
        if(gridIns!=null)
        {
            GameObject ins = Instantiate(Armies[(int)HeroData.GetArmyData().ArmyType], gridIns.transform.position, Quaternion.identity);
            Leader leaderScript = ins.GetComponent<Leader>();

            leaderScript.UICard = bcm.gameObject;
            leaderScript.FaceSprite = bcm.data.GetHeroData().GetHeroTexture();
            leaderScript.HeroName = bcm.data.GetHeroName();

            leaderScript.RegisterAttributes(bcm.data);
            leaderScript.gameObject.SetActive(true);
        }
    }
    void StartMethod()
    {
        TextOurArmy = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.OUR_ARMY_TEXT_ID);
        UIManager.Instance.ShowUI("UIBattleCanvas");
        SoundManager.Instance.PlayBGM("BGM00003");
		Grids = new GameObject[4,4];
		for (int i = 0; i < 4; i++)
		{
			float StartX = offset * -1.5f;
			float StartY = offset * -1.5f;
			for (int j = 0; j < 4; j++)
			{
				GameObject ins=Instantiate(Grid,transform.position+Vector3.up,Quaternion.identity);
				ins.transform.parent = this.transform;
				ins.SetActive(true);
				ins.transform.Translate(StartY + j * offset,0,StartX + i * offset);
				ins.name=i+","+j;

				Grids[i,j] = ins;
			}
		}
        GenerateCards();
    }
    public GameObject Card;
    public int Count = 10;
    public Transform Layout;
    public ScrollRect Scroll;
    public List<BattleCardMono> cards;
    private void GenerateCards()
    {
        cards = new List<BattleCardMono>();
        for(int i=0;i<PlayerDataManager.Instance.PlayerData.OwnedHeros.Count;i++)
        {
            GameObject ins = Instantiate(Card);
            ins.transform.SetParent(Layout, false);
            ins.SetActive(true);

            BattleCardMono bcm = ins.GetComponent<BattleCardMono>();
            bcm.data = PlayerDataManager.Instance.PlayerData.OwnedHeros[i];
            bcm.Manager = this;
            cards.Add(bcm);
        }
    
        if(PlayerDataManager.Instance.PlayerData.SavedArmyFormation!=null && PlayerDataManager.Instance.PlayerData.SavedArmyFormation.Count==5)
        {
            for(int i=0;i<PlayerDataManager.Instance.PlayerData.SavedArmyFormation.Count;i++)
            {
                ArmyBattleFormation formation = PlayerDataManager.Instance.PlayerData.SavedArmyFormation[i];
                BattleCardMono bcm=cards.Find(x => x.data.ID == formation.PlayerHeroID);

				HeroArmyData equippedArmy = bcm.data.GetEquippedArmySelf();
				ArmyData armyData = equippedArmy.GetArmyData();

				int aindex = (int)armyData.ArmyType;
                int gx = formation.GridX;
                if(gx>3)
                    gx = 0;
                int gy = formation.GridY;
                if(gy>3)
                    gy = 0;
                Debug.LogError(string.Format("aindex={0}, gx={1}, gy={2}",aindex, gx, gy));
                GameObject ins = Instantiate(Armies[aindex], Grids[gx,gy].transform.position, Quaternion.identity);
                Leader leaderScript = ins.GetComponent<Leader>();
				leaderScript.gameObject.SetActive(false);

                leaderScript.UICard = bcm.gameObject;
                leaderScript.FaceSprite = bcm.data.GetHeroData().GetHeroTexture();
                leaderScript.HeroName = bcm.data.GetHeroName();

                leaderScript.RegisterAttributes(bcm.data);
                leaderScript.gameObject.SetActive(true);

                bcm.IsInBattle = true;
                //bcm.gameObject.SetActive(false);
            }
            LeadersCount = 5;
            Scroll.enabled = false;
        }
        
        
    }
    private GameObject SelectedCard=null;
    public GameObject SelectedArmy = null;
    public GameObject OutsidePanel;
    public void ChooseCard(GameObject _Card)
    {
        if(SelectedCard!=null)
        {
            return;
        }
        SelectedCard = _Card;
        DragCardIcon.texture = _Card.GetComponent<BattleCardMono>().Icon.texture;
        txtHeroName.text = _Card.GetComponent<BattleCardMono>().txtHeroName.text;

        CancelRemovableLeader();
    }
    public void OnPointerEnter()
    {
        if(SelectedArmy==null)
        {
            Scroll.enabled = true;
        }
        
    }
    public void OnPointerExit()
    {
        Scroll.enabled = false;
    }
    public Leader SwitchLeader;
    public void RemoveLeader()
    {
        RemoveableLeader.GetComponent<Leader>().UICard.GetComponent<BattleCardMono>().IsInBattle = false;
        LeadersCount--;
        SwitchLeader = RemoveableLeader;
        SwitchLeader.formator.ChangeShader();
        Destroy(SwitchLeader.m_formatorDetector);
        RemoveableLeader = null;
        SelectedArmy = null;
    }
    public void CloseBattleCardInfo()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLECARDINFO);
    }
    private void Update()
	{
        txtOurArmyText.text = TextOurArmy + " " + LeadersCount + "/" + 5;
        if (UIManager.UIInstance<UIBattleCardInfo>() != null)
        {
            OutsidePanel.SetActive(UIManager.UIInstance<UIBattleCardInfo>().gameObject.activeSelf);
        }

        if (LeadersCount == 5)
        {
            BtnFight.interactable = true;
            BtnFight.image.sprite = ImageY;
            imgFight.sprite = TextImageY;
            imgFight.color = Color.white;
        }
        else
        {
            BtnFight.interactable = false;
            BtnFight.image.sprite = ImageW;
            imgFight.sprite = TextImageW;
            imgFight.color = new Color32(200,200,200,200);
        }
        /*if (RemoveableLeader!=null)
        {
            GlowEffect.transform.position = RemoveableLeader.transform.position;
            GlowEffect.SetActive(true);
        }
        else
        {
            GlowEffect.SetActive(false);
        }*/
        if(SelectedCard!=null && !Scroll.enabled)
        {
            if(Input.GetMouseButton(0))
            {
                RaycastHit hit;
                UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLECARDINFO);
                SelectedCard.GetComponent<BattleCardMono>().IsInBattle = true;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask("Grid")))
                {
                    if (SelectedArmy == null)
                    {
						BattleCardMono bcm = SelectedCard.GetComponent<BattleCardMono>();
						SelectedArmy = Instantiate(Armies[(int)bcm.data.GetEquippedArmySelf().GetArmyData().ArmyType],hit.transform.position,Quaternion.identity);
                        Leader leaderScript = SelectedArmy.GetComponent<Leader>();
                        

                        leaderScript.UICard = SelectedCard;
                        leaderScript.FaceSprite=bcm.data.GetHeroData().GetHeroTexture();
                        leaderScript.HeroName = bcm.data.GetHeroName();

                        leaderScript.RegisterAttributes(bcm.data);
                    }
                    else if(SelectedArmy!=null)
                    {
                        SelectedArmy.SetActive(true);
                        SelectedArmy.transform.position = new Vector3(hit.transform.position.x,SelectedArmy.transform.position.y,hit.transform.position.z);
                    }
                    SelectedArmy.SetActive(true);
                    Scroll.enabled = false;
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                if (SelectedArmy==null)
                {
                    SelectedCard.GetComponent<BattleCardMono>().IsInBattle=false;
                    SelectedCard = null;
                    if (SwitchLeader != null)
                    {
                        Destroy(SwitchLeader.gameObject);
                        SwitchLeader = null;
                    }
                }
                else
                {

                    Vector2 localpoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(Scroll.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localpoint);           
                    if (localpoint.y<=95f)
                    {
                        Destroy(SelectedArmy);
                        SelectedCard.GetComponent<BattleCardMono>().IsInBattle = false;
                        SelectedArmy = null;
                        SelectedCard = null;

                    }
                    else
                    {
                        Collider[] cols = Physics.OverlapBox(SelectedArmy.transform.position, Vector3.one * 5, Quaternion.identity, LayerMask.GetMask("HeroPosition"));
                        if (cols != null)
                        {
                            if (cols.Length > 1)
                            {
                                if(SwitchLeader !=null)
                                {
                                    for(int i=0;i<cols.Length;i++)
                                    {
                                        GameObject anotherArmy = cols[i].GetComponentInParent<Leader>().gameObject;
                                        if (anotherArmy!=null)
                                        {
                                            if(anotherArmy!=SelectedArmy)
                                            {                                                
                                                anotherArmy.transform.position = SwitchLeader.getGrid().transform.position;
                                                break;
                                            }
                                        }
                                    }
                                    SelectedArmy = null;
                                    SelectedCard = null;

                                    LeadersCount++;
                                }
                                else
                                {
                                    for (int i = 0; i < cols.Length; i++)
                                    {
                                        GameObject anotherArmy = cols[i].GetComponentInParent<Leader>().gameObject;
                                        if (anotherArmy != null)
                                        {
                                            if (anotherArmy != SelectedArmy)
                                            {
                                                Destroy(anotherArmy);
                                                anotherArmy.GetComponent<Leader>().UICard.GetComponent<BattleCardMono>().IsInBattle = false;
                                                break;
                                            }
                                        }
                                    }
                                    SelectedArmy = null;
                                    SelectedCard = null;
                                }
                                //Destroy(SelectedArmy);
                                //SelectedCard.GetComponent<BattleCardMono>().IsInBattle = false;
                                
                            }
                            else if (LeadersCount == 5)
                            {
                                Destroy(SelectedArmy);
                                SelectedCard.GetComponent<BattleCardMono>().IsInBattle = false;
                                SelectedArmy = null;
                                SelectedCard = null;
                            }
                            else
                            {
                                SelectedArmy = null;
                                SelectedCard = null;

                                LeadersCount++;

                            }
                        }
                        else
                        {
                            SelectedArmy = null;
                            SelectedCard = null;

                            LeadersCount++;
                        }
                    }
                    if(SwitchLeader!=null)
                    {
                        Destroy(SwitchLeader.gameObject);
                        SwitchLeader = null;
                    }
                }         
            }
        }
        if (Input.GetMouseButtonDown(0))
		{
			bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null;
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,2000,LayerMask.GetMask("HeroPosition")) && noUIcontrolsInUse)
			{
				UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLECARDINFO);
				SelectedLeader = hit.collider.transform.parent.GetComponent<Leader>();
				if (SelectedLeader != null && SelectedLeader.GetComponent<Team>().team == yourTeam)
				{
					LastPosition = Input.mousePosition;
					RemoveableLeader = SelectedLeader;
				}
			}
			else if (noUIcontrolsInUse)
			{
				mousePos = Input.mousePosition;
				Swipe = true;
			}
		}
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,2000,LayerMask.GetMask("Grid")))
			{
                if (SelectedLeader != null && RemoveableLeader!=null)
                {
                    if(Vector3.Distance(Input.mousePosition,LastPosition)>=10)
                    {
                        BattleCardMono bcm = RemoveableLeader.UICard.GetComponent<BattleCardMono>();
                        RemoveLeader();
                        ChooseCard(bcm.gameObject);
                        Scroll.enabled = false;
                    }
                }
			}
		}
        if (SelectedCard != null && SelectedCard.GetComponent<BattleCardMono>().IsInBattle)
        {
            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(box, Input.mousePosition, Camera.main, out localpoint);
            DragCard.anchoredPosition = localpoint+new Vector2(-100,100);
            DragCard.gameObject.SetActive(true);
        }
        else
        {
            DragCard.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0))
		{
			if (Swipe)
			{
				Swipe = false;
				Vector3 swipeVector = Input.mousePosition-mousePos;

				if (Mathf.Abs(swipeVector.y) >50)
				{
					if (swipeVector.y < 0)
					{
						UIBattleCanvas.ShowEnemyBattleField();
					}
					else
					{
						UIBattleCanvas.ShowOwnBattleField();
					}
				}
		
				return;
			}
            if (SwitchLeader != null)
            {
                Destroy(SwitchLeader.gameObject);
                SwitchLeader = null;
            }
            if (RemoveableLeader!=null)
            {
                BattleCardMono bcm = RemoveableLeader.UICard.GetComponent<BattleCardMono>();
                RemoveableLeader = null;
                bcm.OnClick();
                Scroll.enabled = true;
            }
			if (SelectedLeader != null)
			{
				Collider[] cols = Physics.OverlapBox(SelectedLeader.transform.position,Vector3.one * 5,Quaternion.identity,LayerMask.GetMask("HeroPosition"));
				if (cols != null)
				{
					if (cols.Length == 2)
					{
						SelectedLeader.transform.position = LastPosition;
					}
					SelectedLeader = null;
				}
			}
            if(SelectedCard!=null)
            {
                SelectedCard.GetComponent<BattleCardMono>().IsInBattle=false;
            }
            SelectedCard = null;
		}
	}
    public void CancelRemovableLeader()
    {
        RemoveableLeader = null;
    }
	public void Fight()
	{
		UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLECARDINFO);
		UIManager.Instance.CloseUI(GLOBALCONST.UI_ENEMYCARDINFO);
		BattleManager.SetActive(true);

		List<Leader> Teamates = new List<Leader>();
		Leader[] leaders = GameObject.FindObjectsOfType<Leader>();
		for (int i = 0; i < leaders.Length; i++)
		{
			leaders[i].GameStart();
			if (leaders[i].team.team == yourTeam)
			{
				Teamates.Add(leaders[i]);
			}
		}
		PlayerDataManager.Instance.PlayerData.SavedArmyFormation = new List<Common.Player.ArmyBattleFormation>();
		for (int i = 0; i < Teamates.Count; i++)
		{
			GameObject grid = Teamates[i].getGrid();
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (Grids[x,y].Equals(grid))
					{
						Common.Player.ArmyBattleFormation data = new Common.Player.ArmyBattleFormation();
						data.PlayerHeroID = Teamates[i].m_PlayerHeroData.ID;
						data.GridX = x;
						data.GridY = y;
						PlayerDataManager.Instance.PlayerData.SavedArmyFormation.Add(data);
					}
				}
			}
		}
		PlayerDataManager.Instance.SavePlayerData();

		Projector[] projectors = GameObject.FindObjectsOfType<Projector>();
		for (int i = 0; i < projectors.Length; i++)
		{
			projectors[i].transform.parent = null;
			projectors[i].GetComponent<ProjectorSelfDestroy>().enabled = true;
		}

		gameObject.SetActive(false);
		GameObject.FindObjectOfType<EnemyPlacement>().gameObject.SetActive(false);

		GameObject.FindObjectOfType<CameraController>().Init();
		//SwipeScreen.SetActive(false);

		UIManager.Instance.ShowUI("UIBattleHPBar",Teamates);

		GlowEffect.SetActive(false);

		SoundManager.Instance.PlayBGM("BGM00004",true);
		if (CavalryCheck(Teamates))
		{
			SoundManager.Instance.PlaySound("MS00001_2");
			SoundManager.Instance.PlaySound("MS00001");
		}
		else
		{
			SoundManager.Instance.PlaySound("MS00001");
		}
		
	}
    private bool CavalryCheck(List<Leader> mates)
    {
        for(int i=0;i<mates.Count;i++)
        {
            List<GameObject> objects = mates[i].formator.Units;
            for (int x = 0; x < objects.Count; x++)
            {
                if (objects[i].name == "Cavalry(Clone)")
                {
                    return true;
                }
            }
        }
        return false;
    }
}
