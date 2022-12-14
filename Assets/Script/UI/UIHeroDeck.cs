using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class UIHeroDeck : UIBase
{
    public Button btnTotalPowerHighToLow;
    public Button btnTotalPowerLowToHigh;
    public Button btnHeroLevelHighToLow;
    public Button btnHeroLevelLowToHigh;
    public Button btnHeroRankHighToLow;
    public Button btnHeroRankLowToHigh;
    public Button bthHeroGradeHighToLow;
    public Button btnHeroGradeLowToHigh;
    public Button btnFilterHero;
    public Button btnHeroLevelUpInterface;
    public Button btnCloseHeroLevelUpInterface;
    public Button btnCloseHeroRankUpInterface;
    public Button btnHeroLevelUpgrade;
    public Button btnHeroRankUpInterface;
    public Button btnHeroLevelUpOnce;
    public RawImage imgHero;
    public Image[] Stars;
    public Text txtHeroLevel;
    public Text txtArmyHP;
    public Text txtHeroConfig;
    public Text txtArmyConfig;
    public Text txtArmyInfoTab;
    public Text txtArmySkillTab;
    public Text txtArmyUpgrade;
    public Text txtHeroName;
    public Text txtGacha;
    public Text txtHeroExp;
    public Text txtHeroRank;
    public Text txtHeroLevelForHeroLevelUPInterface;
    public Text txtWarningMessage;
    public Text txtHeroItemAmount;
    public Text txtHeroItemAmoutForUIHeroLevelUpInterface;
    public Text txtLevelUpOnce;
    public Text txtMaxLevelMsg;
    public Text txtHeroHpForHeroLevelUPInterface, txtHeroHPValueForHeroLevelUPInterface, txtNextHeroHPValueForHeroLevelUPInterface;
    public Text txtHeroAtkForHeroLevelUPInterface, txtHeroAtkValueForHeroLevelUPInterface, txtNextHeroAtkValueForHeroLevelUPInterface;
    public Text txtHeroDefForHeroLevelUPInterface, txtHeroDefValueForHeroLevelUPInterface, txtNextHeroDefValueForHeroLevelUPInterface;
    public Text txtHeroAtkSpeedForHeroLevelUPInterface,txtHeroAtkSpeedValueForHeroLevelUPInterface, txtNextHeroAtkSpeedValueForHeroLevelUPInterface;
    public Text txtHeroCritForHeroLevelUPInterface, txtHeroCritValueForHeroLevelUPInterface, txtNextHeroCritValueForHeroLevelUPInterface;
    public Text txtHeroCritDefForHeroLevelUPInterface, txtHeroCritDefValueForHeroLevelUPInterface, txtNextHeroCritDefValueForHeroLevelUPInterface;
    public Text txtHeroMoveSpeedForHeroLevelUPInterface, txtHeroMoveSpeedValueForHeroLevelUPInterface, txtNextHeroMoveSpeedValueForHeroLevelUPInterface;
    public Text txtHeroAtkRangeForHeroLevelUPInterface, txtHeroAtkRangeValueForHeroLevelUPInterface, txtNextHeroAtkRangeValueForHeroLevelUPInterface;
    public GameObject heroFilterPanel;
    public GameObject PrefCardItem;
    public GameObject cardScrollViewOBJ;
    public GameObject objUICard1;
    public GameObject HeroandSkillInfo;
    public GameObject objUIHeroLevelUpInterface;
    public GameObject objUIHeroRankUpInterface;
    public GameObject btnLeftForUICard;
    public GameObject btnRightForUICard;
    public GameObject objWarningMessage;
    public Transform CardScrollView;
    private List<ItemDeckCard> itemCardRef;
    public ArmyPanelController _armyPanelControllerRef;
    public ArmySkillPanelController _armySkillPanelController;
    public SkillInfoPanelController1 _skillInfoPanelController1;
    public UICard1 _uiCardRef;
    public UICardForLeveUpAndRankUpPanel _UiCardRefForUIHeroLevelUPInterface;
    public UICardForLeveUpAndRankUpPanel _uiCardRefForUIHeroRankUPInterface;
    public List<PlayerHeroData> _OwnedHeroCards = new List<PlayerHeroData>();
    private List<PlayerHeroData> newPlayerHeroDataListRef = new List<PlayerHeroData>();
    public List<HeroArmyData> _heroArmies;
    private CardCountData m_CardCountData;
    public int _heroIndex;
    public int _heroArmyIndex=0;
    public Shader grayscale;
    public Material[] matLevelCabbages=new Material[6];
    public TweenScale Scaler;
    public static UIHeroDeck _uiHeroDeck;
    public UISkillIconDescriptionController _uiSkillIconDescriptionController;
    protected override void OnInit()
    {
        _uiHeroDeck = this;
        if ((float)Screen.width / (float)Screen.height >= 1.8f)
        {
            cardScrollViewOBJ.transform.localScale = Vector3.one * 0.89f;
            objUICard1.transform.localScale = Vector3.one * 0.9f;
            //cardScrollViewOBJ.transform.localPosition = new Vector3(0, -50f, 0);
            HeroandSkillInfo.transform.localScale = Vector3.one * 0.9f;
        }
        else
        {
            cardScrollViewOBJ.transform.localScale = Vector3.one * 1f;
        }
        base.OnInit();

        TextFiller();

        btnFilterHero.onClick.AddListener(() => ShowButtonListForFilterHero(!heroFilterPanel.activeSelf));
        btnHeroLevelHighToLow.onClick.AddListener(HeroLevelHighToLow);
        btnHeroLevelLowToHigh.onClick.AddListener(HeroLevelLowToHigh);
        btnTotalPowerHighToLow.onClick.AddListener(TotalCombatPowerHighToLow);
        btnTotalPowerLowToHigh.onClick.AddListener(TotalCombatPowerLowToHigh);
        btnHeroLevelUpInterface.onClick.AddListener(updateUIHeroLevelUPInterface);
        btnHeroRankUpInterface.onClick.AddListener(updateUIHeroRankUPInterface);
        btnCloseHeroLevelUpInterface.onClick.AddListener(closeUIHeroLevelUPInterface);
        btnCloseHeroRankUpInterface.onClick.AddListener(closeUIHeroRankUPInterface);
        btnHeroLevelUpgrade.onClick.AddListener(() => UpgradeConfirm(_OwnedHeroCards[_heroIndex], m_CardCountData));
        btnHeroLevelUpOnce.onClick.AddListener(()=>HeroLevelUpOnce(_OwnedHeroCards[_heroIndex], m_CardCountData));
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        _OwnedHeroCards =new List<PlayerHeroData>();
        _armyPanelControllerRef.ArmyInfo();
        if (_OwnedHeroCards.Count == 0)
        {
            _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
            m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
            _skillInfoPanelController1.RefreshSkillItem(_OwnedHeroCards[_heroIndex]);
            _uiSkillIconDescriptionController.CreateSkillIconDescriptions(_OwnedHeroCards[_heroIndex]);
        }
        RefreshCard(_OwnedHeroCards);
        for (int i = 0; i < itemCardRef.Count; i++)
        {
            itemCardRef[i].UpdateData(_OwnedHeroCards[i]);
        }
        SoundManager.Instance.PlayBGM("BGM00002");
        _armyPanelControllerRef.RefreshData(_OwnedHeroCards[_heroIndex]);
        _skillInfoPanelController1.RefreshSkillItem(_OwnedHeroCards[_heroIndex]);
        _skillInfoPanelController1.RefreshStatus(_OwnedHeroCards[_heroIndex]);
    }
    void SwitchCards(E_KingdomType _kingdomType)
    {
        _OwnedHeroCards = new List<PlayerHeroData>();
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; i++)
        {
            PlayerHeroData data = PlayerDataManager.Instance.PlayerData.OwnedHeros[i];
            if ((E_KingdomType)data.GetHeroData().KingdomID == _kingdomType)
            {
                _OwnedHeroCards.Add(data);
            }
        }
        for (int j = 0; j < _OwnedHeroCards.Count; j++)
        {
            _OwnedHeroCards[j].ID = j + 1;
        }
        RefreshCard(_OwnedHeroCards);
    }
    public void refreshPlayerHerdDataList()
    {
        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        for (int i = 0; i < _OwnedHeroCards.Count; i++)
        {
            _OwnedHeroCards[i].ID = i + 1;
        }
    }

    void HeroLevelHighToLow()
    {
        //descending
        //_OwnedHeroCards.Sort((x, y) => y.HeroLevel.CompareTo(x.HeroLevel));
        newPlayerHeroDataListRef = _OwnedHeroCards.OrderByDescending(s => s.HeroLevel).ToList();
        RefreshCard(newPlayerHeroDataListRef);
    }
    void HeroLevelLowToHigh()
    {
        //ascending
        //_OwnedHeroCards.Sort((x, y) => x.HeroLevel.CompareTo(y.HeroLevel));
        newPlayerHeroDataListRef = _OwnedHeroCards.OrderBy(s => s.HeroLevel).ToList();
        RefreshCard(newPlayerHeroDataListRef);
    }
    void TotalCombatPowerHighToLow()
    {
        //descending
        //_OwnedHeroCards.Sort((x, y) => y.GetCombatPowerSelf().CompareTo(x.GetCombatPowerSelf()));
        newPlayerHeroDataListRef = _OwnedHeroCards.OrderByDescending(s => s.GetCombatPower()).ToList();
        RefreshCard(newPlayerHeroDataListRef);
    }
    void TotalCombatPowerLowToHigh()
    {
        //ascending
        //_OwnedHeroCards.Sort((x, y) => x.GetCombatPowerSelf().CompareTo(y.GetCombatPowerSelf()));
        newPlayerHeroDataListRef = _OwnedHeroCards.OrderBy(s => s.GetCombatPower()).ToList();
        RefreshCard(newPlayerHeroDataListRef);
    }

    public void RefreshCard(List<PlayerHeroData> tempPlayerHeroDataList)
    {
        foreach (Transform child in CardScrollView)
        {
            Destroy(child.gameObject);
        }
        itemCardRef = new List<ItemDeckCard>();
        for (int i = 0; i < tempPlayerHeroDataList.Count; i++)
        {
            GameObject go = Instantiate(PrefCardItem);
            go.SetActive(true);
            itemCardRef.Add(go.GetComponentInChildren<ItemDeckCard>());
            go.transform.SetParent(CardScrollView, false);
            ItemDeckCard itemDeckCard = go.GetComponent<ItemDeckCard>();
            itemDeckCard.UpdateData(tempPlayerHeroDataList[i]);
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
    public void ShowButtonListForFilterHero(bool isTrue)
    {
        heroFilterPanel.SetActive(isTrue);
    }
    private void ClickCard(ItemDeckCard item)
    {
        RefreshUI(item.PlayerHeroData);
        ShowButtonListForFilterHero(false);
    }

    private void RefreshUI(PlayerHeroData data)
    {
        _heroArmies = data.GetAllArmiesSelf();
        _armyPanelControllerRef.m_heroIndex = _uiCardRef.heroIndex = _heroIndex = data.ID - 1;
        _armyPanelControllerRef._heroArmyDataRef = _OwnedHeroCards[_heroIndex].GetEquippedArmySelf();
        refreshUICardPlayerHeroData();
        refreshArmyPanelPlayerHeroData();
        _uiCardRef.RefreshData(data);
        RefreshHeroInfo(data);
        _heroArmies = new List<HeroArmyData>();
        _heroArmies = data.GetAllArmiesSelf();
        for (int i = 0; i < _heroArmies.Count; i++)
        {
            _armyPanelControllerRef.armies.Add(_heroArmies[i]);
        }
        for (int i = 0; i < _heroArmies.Count; i++)
        {
            if (E_EquipableStatus.Equipped == (E_EquipableStatus)_heroArmies[i].HeroArmyStatusType)
            {
                _heroArmyIndex = i;
                _armyPanelControllerRef.RefreshArmyInfo(_heroArmies[i]);
                _armySkillPanelController.RefreshData(_heroArmies[i]);
                break;
            }
        }
        _armyPanelControllerRef.RefreshData(data);
        _skillInfoPanelController1.RefreshSkillItem(data);
        _skillInfoPanelController1.RefreshStatus(data);
    }
    void refreshUICardPlayerHeroData()
    {
        _uiCardRef._OwnedHeroCards = new List<PlayerHeroData>();
        for (int i = 0; i < _OwnedHeroCards.Count; i++)
        {
            _uiCardRef._OwnedHeroCards.Add(_OwnedHeroCards[i]);
        }
    }
    void refreshArmyPanelPlayerHeroData()
    {
        _armyPanelControllerRef._OwnedHeroCards = new List<PlayerHeroData>();
        for(int i=0;i<_OwnedHeroCards.Count;i++)
        {
            _armyPanelControllerRef._OwnedHeroCards.Add(_OwnedHeroCards[i]);
        }
    }
    public void RefreshHeroInfo(PlayerHeroData _pDataPlayerRef)
    {
        _heroArmies = _pDataPlayerRef.GetAllArmiesSelf();
        AttributeData attributeDataRef = _heroArmies[_heroArmyIndex].GetArmyAttribute(_heroArmies[_heroArmyIndex].ArmyLevel);
        txtArmyHP.text = attributeDataRef.Hp.ToString();
        imgHero.texture = _pDataPlayerRef.GetHeroData().GetHeroTexture();
        txtHeroName.text = _pDataPlayerRef.GetHeroName();
        txtHeroLevel.text = _pDataPlayerRef.HeroLevel.ToString();
        txtHeroItemAmount.text = _pDataPlayerRef.FragmentCount.ToString() + "/" + m_CardCountData.GetCostByLevel(_pDataPlayerRef.HeroLevel);
        for (int i = 0; i < Stars.Length; i++)
        {
            matLevelCabbages[i] = new Material(grayscale);
            Stars[i].material = matLevelCabbages[i];
            if (i < _pDataPlayerRef.HeroLevel - 1)
            {
                Stars[i].gameObject.SetActive(true);
                matLevelCabbages[i].SetFloat("_EffectAmount", 0);
            }
            else
            {
                Stars[i].gameObject.SetActive(false);
                matLevelCabbages[i].SetFloat("_EffectAmount", 1);
            }
        }
       
        if (_pDataPlayerRef.HeroLevel==6)
        {
            btnHeroLevelUpgrade.gameObject.SetActive(false);
            btnHeroLevelUpOnce.gameObject.SetActive(false);
            txtMaxLevelMsg.gameObject.SetActive(true);
            txtHeroItemAmoutForUIHeroLevelUpInterface.gameObject.SetActive(false);
        }
        else
        {
            btnHeroLevelUpgrade.gameObject.SetActive(true);
            btnHeroLevelUpOnce.gameObject.SetActive(true);
            txtMaxLevelMsg.gameObject.SetActive(false);
            txtHeroItemAmoutForUIHeroLevelUpInterface.gameObject.SetActive(true);
        }
    }
    void updateUIHeroRankUPInterface()
    {
        objUIHeroRankUpInterface.SetActive(true);
        _uiCardRefForUIHeroRankUPInterface.RefreshData(_OwnedHeroCards[_heroIndex]);
        UIMessageBox.ShowMessageBox("Coming Soon!", E_MessageBox.Yes, CloseMessageCallback);
    }
    public void closeUIHeroRankUPInterface()
    {
        objUIHeroRankUpInterface.SetActive(false);
        btnLeftForUICard.SetActive(true);
        btnRightForUICard.SetActive(true);
    }
    void updateUIHeroLevelUPInterface()
    {
        if (_OwnedHeroCards[_heroIndex].HeroLevel == 6)
        {
            objWarningMessage.SetActive(true);
            txtWarningMessage.text = "You Have Reached Maximum Level";
        }
        txtHeroItemAmoutForUIHeroLevelUpInterface.text = _OwnedHeroCards[_heroIndex].FragmentCount.ToString() + "/" + m_CardCountData.GetCostByLevel(_OwnedHeroCards[_heroIndex].HeroLevel);
        objUIHeroLevelUpInterface.SetActive(true);
        _UiCardRefForUIHeroLevelUPInterface.RefreshData(_OwnedHeroCards[_heroIndex]);
        ShowHeroAttributeData(_OwnedHeroCards[_heroIndex]);
     }
    public void closeUIHeroLevelUPInterface()
    {
        objUIHeroLevelUpInterface.SetActive(false);
        btnLeftForUICard.SetActive(true);
        btnRightForUICard.SetActive(true);
        objWarningMessage.SetActive(false);
    }
    public void HeroLevelUpOnce(PlayerHeroData pData, CardCountData _cardCountData)
    {
        if (pData.FragmentCount >= _cardCountData.GetCostByLevel(pData.HeroLevel) || pData.HeroLevel == 6)
        {
            for(int i=1;i<6;i++)
            {
                pData.FragmentCount -= _cardCountData.GetCostByLevel(pData.HeroLevel);
                pData.HeroLevel += 1;
                RefreshHeroInfo(pData);
                for (int j = 0; j < itemCardRef.Count; j++)
                {
                    itemCardRef[j].UpdateData(_OwnedHeroCards[j]);
                }
               if(pData.FragmentCount<_cardCountData.GetCostByLevel(pData.HeroLevel)||pData.FragmentCount==0)
                {
                    break;
                }
            }
            _skillInfoPanelController1.RefreshSkillItem(pData);
            _skillInfoPanelController1.RefreshStatus(pData);
            _uiCardRef.RefreshData(pData);
            _UiCardRefForUIHeroLevelUPInterface.RefreshData(pData);
            txtHeroLevelForHeroLevelUPInterface.text = pData.HeroLevel.ToString();
            txtHeroItemAmoutForUIHeroLevelUpInterface.text = pData.FragmentCount.ToString() + "/" + _cardCountData.GetCostByLevel(_OwnedHeroCards[_heroIndex].HeroLevel);
            txtHeroItemAmount.text = pData.FragmentCount.ToString() + "/" + _cardCountData.GetCostByLevel(pData.HeroLevel);
            closeUIHeroLevelUPInterface();
        }
        else
        {
            StartCoroutine(showWarningInfo());
        }
    }
    public void UpgradeConfirm(PlayerHeroData pData, CardCountData _cardCountData)
    {
        if (pData.FragmentCount >= _cardCountData.GetCostByLevel(pData.HeroLevel) || pData.HeroLevel == 6)
        {
            pData.FragmentCount -= _cardCountData.GetCostByLevel(pData.HeroLevel);
            pData.HeroLevel += 1;
            RefreshHeroInfo(pData);
            for (int i = 0; i < itemCardRef.Count; i++)
            {
                itemCardRef[i].UpdateData(_OwnedHeroCards[i]);
            }
            _skillInfoPanelController1.RefreshSkillItem(pData);
            _skillInfoPanelController1.RefreshStatus(pData);
            _uiCardRef.RefreshData(pData);
            _UiCardRefForUIHeroLevelUPInterface.RefreshData(pData);
            txtHeroLevelForHeroLevelUPInterface.text = pData.HeroLevel.ToString();
            txtHeroItemAmoutForUIHeroLevelUpInterface.text = pData.FragmentCount.ToString() + "/" + _cardCountData.GetCostByLevel(pData.HeroLevel);
            txtHeroItemAmount.text = pData.FragmentCount.ToString() + "/" + _cardCountData.GetCostByLevel(pData.HeroLevel);
            //PlayerDataManager.Instance.SavePlayerData();
            closeUIHeroLevelUPInterface();
        }
        else
        {
            StartCoroutine(showWarningInfo());
        }
    }
    IEnumerator showWarningInfo()
    {
        objWarningMessage.SetActive(true);
        txtWarningMessage.text = "Not Enough FragmentCout.You Can't Upgrade Now";
        yield return new WaitForSeconds(1.5f);
        objWarningMessage.gameObject.SetActive(false);
        closeUIHeroLevelUPInterface();
    }
    void ShowHeroAttributeData(PlayerHeroData _myPlayerHeroData)
    {
        AttributeData _myAttData = _myPlayerHeroData.GetHeroAttribute();
        AttributeData _myAttDataForNextLevelAttData = _myPlayerHeroData.GetHeroData().GetHeroAttribute(_myPlayerHeroData.HeroLevel + 1);

        txtHeroHPValueForHeroLevelUPInterface.text = _myAttData.Hp.ToString();
        txtHeroAtkValueForHeroLevelUPInterface.text = _myAttData.Atk.ToString();
        txtHeroDefValueForHeroLevelUPInterface.text = _myAttData.Def.ToString();
        txtHeroAtkSpeedValueForHeroLevelUPInterface.text = _myAttData.AtkSpeed.ToString();
        txtHeroCritValueForHeroLevelUPInterface.text = _myAttData.Crit.ToString();
        txtHeroDefValueForHeroLevelUPInterface.text = _myAttData.CritDef.ToString();
        txtHeroAtkRangeValueForHeroLevelUPInterface.text = _myAttData.AtkRange.ToString();
        txtHeroMoveSpeedValueForHeroLevelUPInterface.text = _myAttData.MoveSpeed.ToString();

        txtNextHeroHPValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.Hp.ToString();
        txtNextHeroAtkValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.Atk.ToString();
        txtNextHeroDefValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.Def.ToString();
        txtNextHeroAtkSpeedValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.AtkSpeed.ToString();
        txtNextHeroCritValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.Crit.ToString();
        txtNextHeroDefValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.CritDef.ToString();
        txtNextHeroAtkRangeValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.AtkRange.ToString();
        txtNextHeroMoveSpeedValueForHeroLevelUPInterface.text = _myAttDataForNextLevelAttData.MoveSpeed.ToString();
    }
    void TextFiller()
    {
        heroFilterPanel.SetActive(false);
        objUIHeroLevelUpInterface.SetActive(false);
        objUIHeroRankUpInterface.SetActive(false);
        objWarningMessage.SetActive(false);
        txtMaxLevelMsg.gameObject.SetActive(false);
        txtHeroConfig.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TITLE_BAR);
        txtArmyConfig.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_ARMYCONFIG);
        txtArmyInfoTab.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHANGEARMY_BTN_SWITCH_ARMYCONDITIONPANEL);
        txtArmySkillTab.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_CHANGEARMY_SKILLINFO_TITLE);
        txtArmyUpgrade.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHeroDeck_UPGRADE);
        txtGacha.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_HEROINFOPANEL_GACHA);

        txtHeroDefForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF);
        txtHeroAtkForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ATK);
        txtHeroAtkSpeedForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_SPEED);
        txtHeroMoveSpeedForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_SPEED);
        txtHeroAtkRangeForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_RANGE);
        txtHeroCritDefForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF);
        txtHeroHpForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIArmy_HP);
        txtHeroCritForHeroLevelUPInterface.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL);
    }
    public void CloseMessageCallback(bool sure, object[] param)
    {
        Debug.Log("YesMessageCallbackCalled.");
        closeUIHeroRankUPInterface();
    }
}
