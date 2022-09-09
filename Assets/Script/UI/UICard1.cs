using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MagneticScrollView;

public class UICard1 : MonoBehaviour
{
    public GameObject objUICard1;
    public GameObject ObjbuttonPanel;
    public RawImage RwImgHero;
    public Image ImgKingdomType;
    public Image[] Stars;
    public Sprite[] KingdomTypeImages;
    public Text TxtHeroName;
    public Text TxtAtk;
    public Text TxtDef;
    public Text txtCombatPower;
    public Text txtBtnHero;
    public Text txtbtnCardScrollView;
    public Text txtbtnArmy;
    public Text txtHeroLevel;
    public Text txtHeroLevelForUIHeroLevelUPInterface;
    public Button BtnRight, BtnLeft, btnHero, btnCardScrollView, btnArmy, btnBack;
    public List<PlayerHeroData> _OwnedHeroCards;
    public List<HeroArmyData> _heroArmies;
    public HeroArmyData _myHeroArmyRef;
    public ArmyPanelController _armyPanelController;
    public SkillInfoPanelController1 _skillInfoPanelController1;
    public UIHeroDeck _uiHeroDeckRef;
    private PlayerHeroData m_PlayerHeroData;
    public CardCountData m_CardCountData;
    public List<ItemDeckCard> _itemDeckCardRef = new List<ItemDeckCard>();
    public ArmySkillPanelController _armySkillPanelController;
    public int heroIndex = 0;
    private enum E_Direction
    {
        Left, Right
    }
    public TweenPosition uiCardTweenPos;
    public TweenPosition uiCardButtons;
    public Transform leftTransforUICard;
    public Transform rightTransForUICard;
    public Transform leftTransforUICardBtns;
    public Transform rightTransForUICardBtns;
    public MagneticScrollRect magneticScrollRect;
    public static UICard1 myUICard1;
    public bool isMainUIHeroDeck = false;
    public int uiIndex = 0;
    private void Awake()
    {
        myUICard1 = this;
    }
    private void Start()
    {
        BtnLeft.gameObject.SetActive(false);
        BtnRight.gameObject.SetActive(false);
        txtbtnArmy.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_BTN_UIARMYPANEL);
        txtbtnCardScrollView.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_BTN_MAINUIHERDECK);
        txtBtnHero.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_BTN_UIHEROSKILL);
        if ((float)Screen.width / (float)Screen.height >= 1.8f)
        {
            objUICard1.transform.localPosition = new Vector3(477.9999f, -60f, 0);
            leftTransforUICard.localPosition = new Vector3(-550, -60f, 0);
            rightTransForUICard.localPosition = new Vector3(478, -60, 0);

            ObjbuttonPanel.transform.localPosition = new Vector3(477.9999f, -10f, 0);
            leftTransforUICardBtns.localPosition = new Vector3(-550, -10, 0);
            rightTransForUICardBtns.localPosition = new Vector3(478, -10, 0);
        }
        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        if (BtnLeft != null || BtnRight != null)
        {
            BtnLeft.onClick.AddListener(delegate
            {
                OnClickChangeSelectedHero(E_Direction.Left);
            });
            BtnRight.onClick.AddListener(delegate
            {
                OnClickChangeSelectedHero(E_Direction.Right);
            });
        }
        btnBack.onClick.AddListener(CloseUI);
        btnHero.onClick.AddListener(ShowHero);
        btnArmy.onClick.AddListener(ShowArmy);
        btnCardScrollView.onClick.AddListener(ShowUICardScrollView);

        RefreshData(_OwnedHeroCards[heroIndex]);
    }
    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_HeroDeck);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
        PlayerDataManager.Instance.SavePlayerData();
        moveUICardBacktoRight(true);
    }
    private void ShowUICardScrollView()
    {
        btnTextureChange(0);
        if (uiIndex != 0 && uiIndex == 3)
        {
            StartCoroutine(delayButtons(1f));
        }
        if (uiIndex != 0 && uiIndex == 2)
        {

            StartCoroutine(delayButtons(0.4f));
        }
        uiIndex = 1;
        magneticScrollRect.ScrollRight(0);
        moveUICardBacktoRight(isMainUIHeroDeck);
    }
    private void ShowHero()
    {
        uiIndex = 2;
        btnTextureChange(1);
        StartCoroutine(delayButtons(0.5f));
        magneticScrollRect.ScrollLeft(1);
        moveUICard(isMainUIHeroDeck);
    }
    private void ShowArmy()
    {
        btnTextureChange(2);
        if (uiIndex == 1 || uiIndex == 0)
        {
            StartCoroutine(delayButtons(1.1f));
        }
        else if (uiIndex == 2)
        {
            StartCoroutine(delayButtons(0.4f));
        }
        uiIndex = 3;
        magneticScrollRect.ScrollLeft(2);
        moveUICard(isMainUIHeroDeck);
    }
    public void btnTextureChange(int _index)
    {
        if (_index == 0)
        {
            BtnRight.gameObject.SetActive(false);
            BtnLeft.gameObject.SetActive(false);
        }
        else if (_index == 1)
        {
            BtnRight.gameObject.SetActive(true);
            BtnLeft.gameObject.SetActive(true);
        }
        else if (_index == 2)
        {
            BtnRight.gameObject.SetActive(true);
            BtnLeft.gameObject.SetActive(true);
        }
    }
    public void moveUICard(bool _isMainUIHeroDeck)
    {
        if (_isMainUIHeroDeck == false)
        {
            uiCardTweenPos.From = rightTransForUICard.localPosition;
            uiCardTweenPos.To = leftTransforUICard.localPosition;
            uiCardTweenPos.Duration = 0.3f;
            uiCardTweenPos.TweenStart();

            uiCardButtons.From = rightTransForUICardBtns.localPosition;
            uiCardButtons.To = leftTransforUICardBtns.localPosition;
            uiCardButtons.Duration = 0.3f;
            uiCardButtons.TweenStart();

            isMainUIHeroDeck = true;
        }
    }
    public void moveUICardBacktoRight(bool _isMainUIHeroDeck)
    {
        if (_isMainUIHeroDeck == true)
        {
            uiCardTweenPos.From = leftTransforUICard.localPosition;
            uiCardTweenPos.To = rightTransForUICard.localPosition;
            uiCardTweenPos.Duration = 0.3f;
            uiCardTweenPos.TweenStart();

            uiCardButtons.From = leftTransforUICardBtns.localPosition;
            uiCardButtons.To = rightTransForUICardBtns.localPosition;
            uiCardButtons.Duration = 0.3f;
            uiCardButtons.TweenStart();

            isMainUIHeroDeck = false;
        }
    }
    public void RefreshData(PlayerHeroData playerHeroData)
    {
        heroIndex = playerHeroData.ID - 1;
        TxtHeroName.text = playerHeroData.GetHeroName();
        HeroData heroData = playerHeroData.GetHeroData();
        txtHeroLevel.text = playerHeroData.HeroLevel.ToString();
        if (txtHeroLevelForUIHeroLevelUPInterface != null)
        {
            txtHeroLevelForUIHeroLevelUPInterface.text = playerHeroData.HeroLevel.ToString();
        }
        RwImgHero.texture = heroData.GetHeroTexture();
        AttributeData attributeData = playerHeroData.GetHeroAttribute();
        TxtAtk.text = attributeData.Atk.ToString();
        TxtDef.text = attributeData.Def.ToString();
        ImgKingdomType.sprite = KingdomTypeImages[(int)heroData.KingdomID];
        if (txtCombatPower != null)
        {
            txtCombatPower.text = playerHeroData.GetCombatPowerSelf().ToString();
        }

        //StarBars refresh
        for (int i = 0; i < Stars.Length; i++)
        {
            if (i < playerHeroData.HeroLevel - 1)
            {
                Stars[i].gameObject.SetActive(true);
            }
            else
            {
                Stars[i].gameObject.SetActive(false);
            }
        }

    }
    private void OnClickChangeSelectedHero(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Right:
                heroIndex++;
                break;
            case E_Direction.Left:
                heroIndex--;
                break;
        }
        if (heroIndex >= _OwnedHeroCards.Count)
        {
            heroIndex = 0;
        }
        else if (heroIndex < 0)
        {
            heroIndex = _OwnedHeroCards.Count - 1;
        }
        _armyPanelController.m_heroIndex = _uiHeroDeckRef._heroIndex = heroIndex;
        RefreshData(_OwnedHeroCards[heroIndex]);
        _heroArmies = new List<HeroArmyData>();
        _heroArmies = _OwnedHeroCards[heroIndex].GetAllArmiesSelf();
        for (int i = 0; i < _heroArmies.Count; i++)
        {
            _armyPanelController.armies.Add(_heroArmies[i]);
        }
        for (int i = 0; i < _heroArmies.Count; i++)
        {
            if (E_EquipableStatus.Equipped == (E_EquipableStatus)_heroArmies[i].HeroArmyStatusType)
            {
                _uiHeroDeckRef._heroArmyIndex = i;
                _armyPanelController.RefreshArmyInfo(_heroArmies[i]);
                _armySkillPanelController.RefreshData(_heroArmies[i]);
                break;
            }
        }
        _armyPanelController.RefreshData(_OwnedHeroCards[heroIndex]);
        _armyPanelController._heroArmyDataRef = _OwnedHeroCards[heroIndex].GetEquippedArmySelf();
        _uiHeroDeckRef.RefreshHeroInfo(_OwnedHeroCards[heroIndex]);
        _skillInfoPanelController1.RefreshSkillItem(_OwnedHeroCards[heroIndex]);
        _skillInfoPanelController1.RefreshStatus(_OwnedHeroCards[heroIndex]);
    }
    IEnumerator delayButtons(float delayTime)
    {
        btnBack.interactable = false;
        BtnLeft.interactable = false;
        BtnRight.interactable = false;
        btnArmy.interactable = false;
        btnHero.interactable = false;
        btnCardScrollView.interactable = false;
        yield return new WaitForSeconds(delayTime);
        BtnLeft.interactable = true;
        BtnRight.interactable = true;
        btnArmy.interactable = true;
        btnHero.interactable = true;
        btnBack.interactable = true;
        btnCardScrollView.interactable = true;
    }
}
