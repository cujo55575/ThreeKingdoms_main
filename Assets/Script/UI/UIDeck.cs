using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIDeck : UIBase
{
    public Button BtnBack, btnAllCard, btnWei, btnShu, btnWu, btnHao,
    btnheroFilter, btnHighCombatPower, btnLowCombaPower, btnHighHeroLevel, btnLowHeroLevel,
    btnHighGetTime, btnLowGetTime;
    public Button[] btnheroCards;
    public Sprite imgselectedButton;
    public Sprite imgUnselectedButton;
    public Text txtAllCard, txtWei, txtShu, txtWu, txtHao, txtHeroFilter, txtHighCombatPower, txtLowCombatPower,
    txtHighHeroLevel, txtLowHeroLevel, txtHeroConfig, txtArmyConfig, txtTitleBar;
    public Image[] _reddot;
    public Image _allCardReddot;
    public GameObject heroFilterPanel;
    public GameObject PrefCardItem;
    public Transform CardScrollView;
    private PlayerHeroData SelectedHero;
    private CardCountData m_CardCountData;
    public UICardDetailPanel CardDetailPanelObject;
    public UIDeckArmyPanelController ArmyPanelObject;
    public Text armyLevel, titleBar;
    private PlayerHeroData data;
    private List<PlayerHeroData> OwnedHeros=new List<PlayerHeroData>();
    private List<ItemDeckCard> itemCardRef;
    private bool isHeroFilterOpen = true;
    protected override void OnInit()
    {
        base.OnInit();
        SelectedHero = new PlayerHeroData();
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        txtAllCard.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_ALLCARD);
        txtArmyConfig.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_ARMYCONFIG);
        txtHao.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_HAO);
        txtWei.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_WEI);
        txtWu.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_WU);
        txtShu.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_SHU);
        txtHeroConfig.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TITLE_BAR);
        txtHeroFilter.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_HEROFILTER);
        txtHighCombatPower.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_HIGHCOMBATPOWER);
        txtLowCombatPower.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_LOWCOMBATPOWER);
        txtHighHeroLevel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_HIGHHEROLEVEL);
        txtLowHeroLevel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_LOWHEROLEVEL);
        txtTitleBar.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_ARMY_FORMATION_CONFIG_TITLE);

        BtnBack.onClick.AddListener(CloseUI);

        btnAllCard.onClick.AddListener(allCard);
        // btnShu.onClick.AddListener(() => SwitchCards(E_KingdomType.Shu));
        // btnWei.onClick.AddListener(() => SwitchCards(E_KingdomType.Wei));
        // btnWu.onClick.AddListener(() => SwitchCards(E_KingdomType.Wu));
        // btnHao.onClick.AddListener(() => SwitchCards(E_KingdomType.Other));
        btnheroCards[(int)E_KingdomType.Shu].onClick.AddListener(() => SwitchCards(E_KingdomType.Shu));
        btnheroCards[(int)E_KingdomType.Wei].onClick.AddListener(() => SwitchCards(E_KingdomType.Wei));
        btnheroCards[(int)E_KingdomType.Wu].onClick.AddListener(() => SwitchCards(E_KingdomType.Wu));
        btnheroCards[(int)E_KingdomType.Other].onClick.AddListener(() => SwitchCards(E_KingdomType.Other));
        btnheroFilter.onClick.AddListener(() => BtnHeroFilter(isHeroFilterOpen));
        btnHighCombatPower.onClick.AddListener(BtnHighToLowoCombatPower);
        btnLowCombaPower.onClick.AddListener(BtnLowToHighHeroCombatPower);
        btnHighHeroLevel.onClick.AddListener(BtnHighToLowHeroLevel);
        btnLowHeroLevel.onClick.AddListener(BtnLowToHighHeroLevel);
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        Common.Player.PlayerData newData = PlayerDataManager.Instance.PlayerData;
        if (OwnedHeros.Count == 0)
        {
            OwnedHeros = PlayerDataManager.Instance.PlayerData.OwnedHeros;
            refershTopBtnunSelected();
            btnAllCard.image.sprite = imgselectedButton;
        }
        RefreshCard();
        RefereshTopButtons();
        if (Objects.Length > 0)
        {
            data = (PlayerHeroData)Objects[0];
            RefreshUI(data);
        }
        SoundManager.Instance.PlayBGM("BGM00002");
        titleBar.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_ARMY_FORMATION_CONFIG_TITLE);

        for (int i = 0; i < OwnedHeros.Count; i++)
        {
            Debug.Log(string.Format("At Index = {0}, Hero Level = {1}, Fragment Count = {2}", i, OwnedHeros[i].HeroLevel, OwnedHeros[i].FragmentCount));
        }
    }
    public void RefereshTopButtons()
    {
        List<PlayerHeroData> _myOwnedHeros = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        _allCardReddot.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            _reddot[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _myOwnedHeros.Count; i++)
        {
            if (_myOwnedHeros[i].FragmentCount >= m_CardCountData.GetCostByLevel(_myOwnedHeros[i].HeroLevel) && !_myOwnedHeros[i].isHeroAtMaxLevel())
            {
                _allCardReddot.gameObject.SetActive(true);
                _reddot[_myOwnedHeros[i].GetHeroData().KingdomID].gameObject.SetActive(true);
            }
        }
    }
    private void RefreshCard()
    {
        foreach (Transform child in CardScrollView)
        {
            Destroy(child.gameObject);
        }
        itemCardRef = new List<ItemDeckCard>();
        CardDetailPanelObject._OwnedHeroCards.Clear();
        armyLevel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEVEL_TEXT_STRING_ID);
        for (int i = 0; i < OwnedHeros.Count; i++)
        {
            GameObject go = Instantiate(PrefCardItem);
            go.SetActive(true);
            itemCardRef.Add(go.GetComponentInChildren<ItemDeckCard>());
            go.transform.SetParent(CardScrollView, false);
            ItemDeckCard itemDeckCard = go.GetComponent<ItemDeckCard>();
            itemDeckCard.UpdateData(OwnedHeros[i]);
            CardDetailPanelObject._OwnedHeroCards.Add(OwnedHeros[i]);
            itemDeckCard.GetComponent<Button>().onClick.AddListener(delegate
            {
                ClickCard(itemDeckCard);
            });
            if (i == 0)
            {
                ClickCard(itemDeckCard);
            }
        }
    }
    void refershTopBtnunSelected()
    {
        btnAllCard.image.sprite = imgUnselectedButton;
        for (int j = 0; j < btnheroCards.Length; j++)
        {
            btnheroCards[j].image.sprite = imgUnselectedButton;
        }
    }
    void SwitchCards(E_KingdomType _kingdomType)
    {
        OwnedHeros = new List<PlayerHeroData>();
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; i++)
        {
            PlayerHeroData data = PlayerDataManager.Instance.PlayerData.OwnedHeros[i];
            if ((E_KingdomType)data.GetHeroData().KingdomID == _kingdomType)
            {
                OwnedHeros.Add(data);
            }
        }
        RefreshCard();
        refershTopBtnunSelected();
        btnheroCards[(int)_kingdomType].image.sprite = imgselectedButton;
    }
    void allCard()
    {
        OwnedHeros = new List<PlayerHeroData>();
        OwnedHeros = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        RefreshCard();
        refershTopBtnunSelected();
        btnAllCard.image.sprite = imgselectedButton;
    }
    void BtnLowToHighHeroLevel()
    {
        //ascending
        OwnedHeros.Sort((x, y) => x.HeroLevel.CompareTo(y.HeroLevel));
        RefreshCard();
        BtnHeroFilter(false);
    }
    void BtnHighToLowHeroLevel()
    {
        //descending
        OwnedHeros.Sort((x, y) => y.HeroLevel.CompareTo(x.HeroLevel));
        RefreshCard();
        BtnHeroFilter(false);
    }
    void BtnLowToHighHeroCombatPower()
    {
        //ascending
        OwnedHeros.Sort((x, y) => x.GetCombatPowerSelf().CompareTo(y.GetCombatPowerSelf()));
        RefreshCard();
        BtnHeroFilter(false);
    }
    void BtnHighToLowoCombatPower()
    {//descending
        OwnedHeros.Sort((x, y) => y.GetCombatPowerSelf().CompareTo(x.GetCombatPowerSelf()));
        RefreshCard();
        BtnHeroFilter(false);
    }
    private void ClickCard(ItemDeckCard item)
    {
        RefreshUI(item.PlayerHeroData);
    }

    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_DECK);
        OwnedHeros = new List<PlayerHeroData>();
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
		//PlayerDataManager.Instance.SavePlayerData();
	}


    private void RefreshUI(PlayerHeroData data)
    {

        CardDetailPanelObject.RefreshData(data);
        //for (int i = 0; i < itemCardRef.Count; i++)
        //{
        //    itemCardRef[i].selectedObj.SetActive(false);
        //}
        HeroArmyData armyData = data.GetEquippedArmySelf();
        if (armyData == null)
        {
            Debug.Log("EquippedHeroArmy NUll for PlayerHeroData ID " + data.key());
        }
        else
        {
            ArmyPanelObject.RefreshData(armyData);
        }
        SelectedHero = data;
    }

    void BtnHeroFilter(bool _isHeroFIlterOpen)
    {
        heroFilterPanel.SetActive(_isHeroFIlterOpen);
        if (_isHeroFIlterOpen == true)
        {
            isHeroFilterOpen = false;
        }
        else { isHeroFilterOpen = true; }

    }

    protected override void PlayerDataRefreshHandler()
    {
        base.PlayerDataRefreshHandler();
        Debug.Log("UIPlayerDataChanged Called");
    }
}
