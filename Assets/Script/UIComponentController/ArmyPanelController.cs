using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyPanelController : MonoBehaviour
{
    public Button BtnLeft;
    public Button BtnRight;
    public Button BtnArmyInfo;
    public Button BtnArmySkill;
    public Button BtnUpgrade;
    public Button BtnLevelUpOnce;
    public Button BtnEquip;
    public Button BtnInfromations;
    public Button btnUIArmyLevelUpInterface;
    public Button btnUICloseUIArmyLevelUpInterface;
    public Button btnUnlockArmy;
    public RawImage imgArmyJobType;
    public RawImage imgArmyType;
    public RawImage RawimgArmyJobTypeBackground;
    public RawImage imgMoveType;
    public RawImage imgAttackType;
    public Texture imgSelected;
    public Texture imgUnSelected;
    public Text txtArmyJobType;
    public Text txtArmyLevel;
    public Text txtArmyHP;
    public Text txtAtk, txtAtkValue;
    public Text txtDef, txtDefValue;
    public Text txtCrit, txtCritValue;
    public Text txtASpeed, txtASpeedValue;
    public Text txtARange, txtARangeValue;
    public Text txtMSpeed, txtMSpeedValue;
    public Text txtAtkForArmyLevelUpInteface, txtAtkValueForArmyLevelUpinteface, txtNextAtkValueForArmyLevelUpInteface;
    public Text txtDefForArmyLevelUpInteface, txtDefValueForArmyLevelUpInteface, txtNextDefValueForArmyLevelUpInteface;
    public Text txtCritForArmyLevelUpInteface, txtCritValueForArmyLevelUpInteface, txtNextCritValueForArmyLevelUpInteface;
    public Text txtASpeedForArmyLevelUpInteface, txtAspeedValueForArmyLevelUpInteface, txtNextAspeedValueForArmyLevelUpInteface;
    public Text txtARangeForArmyLevelUpInteface, txtARangeValueForArmyLevelUpInteface, txtNextARangeValueForArmyLevelUpInteface;
    public Text txtMSpeedForArmyLevelUpInteface, txtMSpeedValueForArmyLevelUpInteface, txtNextMSpeedValueForArmyLevelUpInteface;
    public Text UnlockConditionDescription;
    public Text UnlockCondition1;
    public Text UnlockCondition2;
    public Text UnlockCondition3;
    public Text txtArmyName;
    public Text txtArmyLevelForUIArmyLeveUpInterface;
    public Text txtEquip;
    public Text txtCurrentandTotalItem;
    public Text txtWarning;
    public Text txtMaxLevelMessage;
    public GameObject ArmyPlaceHolderPrefab;
    public GameObject SpotLightObject;
    public GameObject objArmyInfo;
    public GameObject objArmySkill;
    public GameObject armyconfigPanel;
    public GameObject armyPlateParent;
    public GameObject TopBar;
    public GameObject armyParentBg;
    public GameObject objArmyIconandDataPanel;
    public GameObject UnlockConditonParent;
    public GameObject unpgradeEffect;
    public GameObject objUnclickedForArmyInfo;
    public GameObject objUnlickedForSkillInfo;
    public GameObject objclickedForArmyInfo;
    public GameObject objclickedForSkillInfo;
    public GameObject objUIArmyLevelUpInterface;
    public GameObject objNextArmyStatus;
    public GameObject objWarning;
    public GameObject objWarningMsgFroArmylevelUnlock;
    private GameObject go;
    public List<PlayerHeroData> _OwnedHeroCards;
    public List<ArmyUnlockData> _armyUnlockData;
    public List<HeroArmyData> armies;
    private List<ClickOn> clickOnList = new List<ClickOn>();
    public UIDeckArmyPanelController ArmyPanelObject;
    private PlayerHeroData m_PlayerHeroData;
    public HeroArmyData _heroArmyDataRef;
    public ArmySkillPanelController _armySkillPanelController;
    public UIHeroDeck _uiHeroDeckRef;
    public UITopBar _uiTopBarRef;
    private ArmyData _armyData;
    public AttributeData _attributeDataRef;
    public TweenRotation ArmyParentRotator;
    public Transform ArmyParent;
    public Transform upgradeEffectTrans;
    private Animator m_AttackingAnimator;
    public int m_SelectedArmyIndex=0;
    public int m_heroIndex=0;
    private float m_Timer;
    private bool m_Rotating = false;
    private bool isThisInformationPanel = false;
    private enum E_Direction
    {
        Left, Right
    }
    void Awake()
    {
        //effectCircularWall.SetActive(false);
        if(armyPlateParent!=null)
        {
            if ((float)Screen.width / (float)Screen.height >= 1.8f)
            {
                armyPlateParent.transform.localScale = Vector3.one * 1.68f;
                Vector3 temp = new Vector3(0, -40, 0);
                TopBar.transform.position += temp;
                armyconfigPanel.transform.localPosition += new Vector3(0, 47, 0);
                BtnEquip.gameObject.transform.localPosition += new Vector3(0, 44, 0);
            }
            else
            {
                armyPlateParent.transform.localScale = Vector3.one * 2.1f;
            }
        }
    }
    void Start()
    {
        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        armies = _OwnedHeroCards[m_heroIndex].GetAllArmies();
        _heroArmyDataRef = armies[m_SelectedArmyIndex];

        RefreshData(_OwnedHeroCards[m_heroIndex]);
        RefreshArmyInfo(_heroArmyDataRef);
        _armySkillPanelController.RefreshData(_heroArmyDataRef);
        ArmyInfo();
        unitTextFill();
        BtnRight.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Left);
        });
        BtnLeft.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Right);
        });
        BtnUpgrade.onClick.AddListener(OnClickUpgrade);
        btnUnlockArmy.onClick.AddListener(OnClickUpgrade);
        BtnLevelUpOnce.onClick.AddListener(levelUpOnce);
        BtnEquip.onClick.AddListener(OnClickEquipArmy);
        BtnArmyInfo.onClick.AddListener(ArmyInfo);
        BtnArmySkill.onClick.AddListener(ArmySkill);
        BtnInfromations.onClick.AddListener(()=>unlockCondition(isThisInformationPanel));
        btnUIArmyLevelUpInterface.onClick.AddListener(()=>updateUIArmyLevelUpInterface(armies[m_SelectedArmyIndex]));
        btnUICloseUIArmyLevelUpInterface.onClick.AddListener(closeUIArmyLevelUpInteface);
        ArmyParentRotator.FinishMethod = delegate
        {
            RotateFinish();
        };
    }
    private void Update()
    {
        if(txtCurrentandTotalItem.gameObject)
        {
            txtCurrentandTotalItem.text = PlayerDataManager.Instance.PlayerData.BattlePoint.ToString() + "/" + _attributeDataRef.UpgradeCost.ToString();
        }
        if (m_AttackingAnimator != null && m_AttackingAnimator.GetBool("IsAttacking"))
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_AttackingAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                m_AttackingAnimator.SetBool("IsAttacking", false);
                m_AttackingAnimator = null;
            }
        }
        if ((PlayerDataManager.Instance.PlayerData.BattlePoint < (int)_heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel).UpgradeCost))
        {
            txtCurrentandTotalItem.color = Color.red;
        }
        else
        {
            txtCurrentandTotalItem.color = Color.black;
        }
    }
    private void OnClickChangeSelectedHero(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Left:
                m_SelectedArmyIndex--;
                break;
            case E_Direction.Right:
                m_SelectedArmyIndex++;
                break;
            default:
                m_SelectedArmyIndex--;
                break;
        }
        if (m_SelectedArmyIndex >= m_PlayerHeroData.GetAllArmiesSelf().Count)
        {
            m_SelectedArmyIndex = 0;
        }
        else if (m_SelectedArmyIndex < 0)
        {
            m_SelectedArmyIndex = m_PlayerHeroData.GetAllArmiesSelf().Count - 1;
        }
        Destroy(go);
        _uiHeroDeckRef._heroArmyIndex = m_SelectedArmyIndex;
        _heroArmyDataRef = armies[m_SelectedArmyIndex];
        RotateArmyParent(direction);
        RefreshUI(m_PlayerHeroData.GetAllArmiesSelf()[m_SelectedArmyIndex]);
        RefreshArmyInfo(_heroArmyDataRef);
        _armySkillPanelController.RefreshData(_heroArmyDataRef);
        ChangeScaleSelected();
        checkUnlockableCondition(m_SelectedArmyIndex);
        isThisInformationPanel = false;
    }
    public void RefreshData(PlayerHeroData playerHeroData)
    {
        clickOnList = new List<ClickOn>();
        for (int i = 0; i < ArmyParent.childCount; i++)
        {
            Destroy(ArmyParent.GetChild(i).gameObject);
        }
        m_SelectedArmyIndex = 0;
        m_PlayerHeroData = playerHeroData;
        armies = m_PlayerHeroData.GetAllArmiesSelf();
        float constRot = 360f / armies.Count;
        for (int i = 0; i < armies.Count; i++)
        {
            GameObject ins = Instantiate(ArmyPlaceHolderPrefab, transform.position, Quaternion.identity);
            ins.transform.SetParent(ArmyParent, false);
            ins.transform.localRotation = Quaternion.Euler(0, i * constRot + 180, 0);
            ins.transform.localPosition = Vector3.zero;
            ins.transform.GetComponentInChildren<ClickOn>().InstantiateArmyObject(armies[i]);
            clickOnList.Add(ins.transform.GetComponentInChildren<ClickOn>());
        }
        float armyParentYRotation = 0;
        m_SelectedArmyIndex = 0;
        for (int i = 0; i < armies.Count; i++)
        {
            if (E_EquipableStatus.Equipped == (E_EquipableStatus)armies[i].HeroArmyStatusType)
            {
                armyParentYRotation = i * constRot;
                m_SelectedArmyIndex = i;
                break;
            }
        }
        RefreshUI(_heroArmyDataRef);
        ArmyParentRotator.gameObject.transform.localEulerAngles = Vector3.up * -armyParentYRotation;
    }
    public void RefreshArmyInfo(HeroArmyData _heroData)
    {
        _armyData = _heroData.GetArmyData();
        _attributeDataRef = _heroData.GetArmyAttribute(_heroData.ArmyLevel);
        txtArmyName.text = _armyData.GetArmyName();
        imgArmyJobType.texture = _armyData.GetArmyJobTypeTexture();
        RawimgArmyJobTypeBackground.texture = _armyData.GetArmyJobTypeTitleTexture();
        imgArmyType.texture = _armyData.GetArmyTexture();
        txtArmyJobType.text = _armyData.GetArmyName();
        imgAttackType.texture = _armyData.GetArmyAttackTypeTexture();
        imgMoveType.texture = _armyData.GetArmyMoveTypeTexture();
        fillArmyInfoValue(_attributeDataRef);

        if(_heroData.ArmyLevel==0)
        {
            txtArmyLevel.text = string.Format("Lv : {0}", _heroData.ArmyLevel.ToString());
            txtArmyLevelForUIArmyLeveUpInterface.text = _heroData.ArmyLevel.ToString();
            BtnUpgrade.gameObject.SetActive(false);
            objNextArmyStatus.SetActive(false);
            BtnLevelUpOnce.gameObject.SetActive(false);
            btnUnlockArmy.gameObject.SetActive(true);
            txtMaxLevelMessage.gameObject.SetActive(false);
        }
        else if (_heroData.ArmyLevel>0 &&_heroData.ArmyLevel<=40)
        {
            BtnUpgrade.gameObject.SetActive(true);
            BtnLevelUpOnce.gameObject.SetActive(true);
            btnUnlockArmy.gameObject.SetActive(false);
            objNextArmyStatus.SetActive(true);
            txtMaxLevelMessage.gameObject.SetActive(false);
            txtArmyLevel.text = string.Format("Lv : {0}", _heroData.ArmyLevel.ToString());
            txtArmyLevelForUIArmyLeveUpInterface.text = _heroData.ArmyLevel.ToString();
            txtArmyHP.text = _heroData.GetArmyAttribute(_heroData.ArmyLevel).Hp.ToString();
        }
        if(_heroData.ArmyLevel==40)
        {
            objNextArmyStatus.SetActive(false);
            BtnUpgrade.gameObject.SetActive(false);
            BtnLevelUpOnce.gameObject.SetActive(false);
            btnUnlockArmy.gameObject.SetActive(false);
            txtMaxLevelMessage.gameObject.SetActive(true);
        }
    }
    private void checkUnlockableCondition(int _heroIndex)
    {
        List<ArmyUnlockData> _armyUnlockDataList = _heroArmyDataRef.GetArmyData().GetUnlockConditions();
        bool[] isAllConditionUnlock = new bool[_armyUnlockDataList.Count];
        for(int i=0;i<_armyUnlockDataList.Count;i++)
        {
            isAllConditionUnlock[i] = m_PlayerHeroData.CheckUnlockConditionMet(_armyUnlockDataList[i]);
            if(isAllConditionUnlock[i])
            {
                BtnInfromations.gameObject.SetActive(false);
                btnUIArmyLevelUpInterface.gameObject.SetActive(true);
            }
            else
            {
                BtnInfromations.gameObject.SetActive(true);
                btnUIArmyLevelUpInterface.gameObject.SetActive(false);
            }
        }
    }
    public void unlockCondition(bool _isThisInformationPanel)
    {
        if (!_isThisInformationPanel)
        {
            UnlockConditonParent.SetActive(false);
            objArmyIconandDataPanel.SetActive(true);
            BtnInfromations.GetComponentInChildren<Text>().text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_BTN_CONDITION);
            isThisInformationPanel = true;
        }
        else
        {
            UnlockConditonParent.SetActive(true);
            objArmyIconandDataPanel.SetActive(false);
            BtnInfromations.GetComponentInChildren<Text>().text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_BTN_INFORMATION);
            isThisInformationPanel = false;
        }
    }
    private void RefreshUI(HeroArmyData _hData)
    {
        bool isArmyLocked = (E_EquipableStatus.Locked == (E_EquipableStatus)_hData.HeroArmyStatusType);
        bool isArmyEquip = (E_EquipableStatus.Equipped == (E_EquipableStatus)_hData.HeroArmyStatusType);
        bool isArmyLevelMax = _hData.IsArmyAtMaxLevel();
        SpotLightObject.SetActive(!isArmyLocked);
        BtnEquip.interactable = (!isArmyEquip);
        ShowOrHideActivateButton(isArmyLocked);
        ArmyPanelObject.RefreshData(_hData);
        RefreshArmyInfo(_hData);
        if (BtnEquip.interactable)
        {
            txtEquip.text = "Send To Battle";
            txtEquip.color = Color.white;
        }
        else
        {
            txtEquip.text = "Already Send To Battle";
            txtEquip.color = Color.red;
        }
        if (isArmyLocked)
        {
            UnlockCondition1.gameObject.SetActive(false);
            UnlockCondition2.gameObject.SetActive(false);
            UnlockCondition3.gameObject.SetActive(false);
            List<ArmyUnlockData> unlocks = _hData.GetArmyData().GetUnlockConditions();
            for (int i = 0; i < unlocks.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        UnlockCondition1.text = GetFormattedUnlockedString(unlocks[0], m_PlayerHeroData);
                        UnlockCondition1.gameObject.SetActive(true);
                        break;

                    case 1:
                        UnlockCondition2.text = GetFormattedUnlockedString(unlocks[1], m_PlayerHeroData);
                        UnlockCondition2.gameObject.SetActive(true);
                        break;

                    case 2:
                        UnlockCondition3.text = GetFormattedUnlockedString(unlocks[2], m_PlayerHeroData);
                        UnlockCondition3.gameObject.SetActive(true);
                        break;

                    default:
                        break;
                }
            }
            UnlockConditonParent.SetActive(true);
            BtnInfromations.gameObject.SetActive(true);
            btnUIArmyLevelUpInterface.gameObject.SetActive(false);
            objArmyIconandDataPanel.SetActive(false);
        }
        else
        {
            UnlockConditonParent.SetActive(false);
            BtnInfromations.gameObject.SetActive(false);
            btnUIArmyLevelUpInterface.gameObject.SetActive(true);
            objArmyIconandDataPanel.SetActive(true);
        }
    }
    void ShowOrHideActivateButton(bool _isArmyLocked)
    {
        if (_isArmyLocked)
        {
            BtnEquip.gameObject.SetActive(false);
        }
        else
        {
            BtnEquip.gameObject.SetActive(true);
        }
    }
    private void OnClickUpgrade()
    {
        if (PlayerDataManager.Instance.PlayerData.BattlePoint >= (int)_heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel).UpgradeCost&&_heroArmyDataRef.ArmyLevel <=40) 
        {
            AttributeData attribDataCurrentLevel = _heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel);
            PlayerDataManager.Instance.PlayerData.BattlePoint -= (int)attribDataCurrentLevel.UpgradeCost;

            if (_heroArmyDataRef.ArmyLevel == 0 && btnUnlockArmy.gameObject)
            {
                _heroArmyDataRef.HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
                btnUnlockArmy.gameObject.SetActive(false);
                StartCoroutine(showWarningMessage());
            }
            _heroArmyDataRef.SetArmyLevel(_heroArmyDataRef.ArmyLevel + 1);
            _uiTopBarRef.RefreshData();
            //m_PlayerHeroData.ReCheckArmyUnlockConditions();
            LevelUpArmy(_heroArmyDataRef);
            closeUIArmyLevelUpInteface();
            showUpgradeEffect();
        }
        else
        {
            StartCoroutine(showWarningMessageForArmyUpgrade());
            if(_heroArmyDataRef.ArmyLevel==0)
            {
                txtWarning.text = "Don't Have Enough Item For Unlock";
            }
            else
            {
                txtWarning.text = "Don't Have Enough Item To Upgrade";
            }
        }
    }
    private void levelUpOnce()
    {
        if (PlayerDataManager.Instance.PlayerData.BattlePoint >=(int)_heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel).UpgradeCost && _heroArmyDataRef.ArmyLevel<=40)
        {
            for (int i = 1; i <40; i++)
            {
                AttributeData attribDataCurrentLevel = _heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel);
                PlayerDataManager.Instance.PlayerData.BattlePoint -= (int)attribDataCurrentLevel.UpgradeCost;
                if (_heroArmyDataRef.ArmyLevel == 0 && btnUnlockArmy.gameObject)
                {
                    _heroArmyDataRef.HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
                    btnUnlockArmy.gameObject.SetActive(false);
                    StartCoroutine(showWarningMessage());
                }
                _heroArmyDataRef.SetArmyLevel(_heroArmyDataRef.ArmyLevel + 1);
                _uiTopBarRef.RefreshData();
                if (PlayerDataManager.Instance.PlayerData.BattlePoint < (int)attribDataCurrentLevel.UpgradeCost || PlayerDataManager.Instance.PlayerData.BattlePoint <= 0)
                {
                    break;
                }
            }
            LevelUpArmy(_heroArmyDataRef);
            closeUIArmyLevelUpInteface();
            showUpgradeEffect();
        }
        else
        {
            StartCoroutine(showWarningMessageForArmyUpgrade());
            if (_heroArmyDataRef.ArmyLevel == 0)
            {
                txtWarning.text = "Don't Have Enough Item For Unlock";
            }
            else
            {
                txtWarning.text = "Don't Have Enough Item To Upgrade";
            }
        }
    }
   private void LevelUpArmy(HeroArmyData _heroArmyDataRef)
    {
        RefreshUI(_heroArmyDataRef);
        ArmyParent.transform.GetChild(m_SelectedArmyIndex).gameObject.GetComponentInChildren<ClickOn>().Refresh(_heroArmyDataRef);
        RefreshArmyInfo(_heroArmyDataRef);
        for (int i = 0; i < armies.Count; i++)
        {
            clickOnList[i].Refresh(armies[i]);
        }
        fillArmyInfoValue(_heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel));
    }
    private void OnClickEquipArmy()
    {
        List<HeroArmyData> _armiesRef = m_PlayerHeroData.GetAllArmiesSelf();
        for (int i = 0; i < _armiesRef.Count; i++)
        {
            if (i == m_SelectedArmyIndex)
            {
                HeroArmyData equippedArmy = m_PlayerHeroData.GetEquippedArmySelf();
                if (equippedArmy != null)
                {
                    equippedArmy.HeroArmyStatusType = (byte)E_EquipableStatus.Unlocked;
                }
                _armiesRef[i].HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
                
                break;
            }
        }
        m_AttackingAnimator = ArmyParent.transform.GetChild(m_SelectedArmyIndex).gameObject.GetComponentInChildren<Animator>();
        m_AttackingAnimator.SetBool("IsAttacking", true);
        m_Timer = 0;
        BtnEquip.interactable = false;
        txtEquip.text = "Already Send To Battle";
        txtEquip.color = Color.red;
    }
    private void RotateFinish()
    {
        Debug.Log("RotateFinish");
        BtnRight.interactable = true;
        BtnLeft.interactable = true;
        m_Rotating = false;
    }
    private void RotateArmyParent(E_Direction direction)
    {
        if (m_Rotating)
        {
            return;
        }
        float degrePerTurn = 360f / m_PlayerHeroData.GetAllArmiesSelf().Count;
        ArmyParentRotator.From = ArmyParentRotator.transform.localEulerAngles;
        switch (direction)
        {
            case E_Direction.Left:
                ArmyParentRotator.To = ArmyParentRotator.transform.localEulerAngles + (Vector3.up * degrePerTurn);
                break;

            case E_Direction.Right:
                ArmyParentRotator.To = ArmyParentRotator.transform.localEulerAngles - (Vector3.up * degrePerTurn);
                break;

            default:
                ArmyParentRotator.To = ArmyParentRotator.transform.localEulerAngles + (Vector3.up * degrePerTurn);
                break;

        }
        ArmyParentRotator.Duration = 0.5f;

        Debug.Log(string.Format("From {0} To {1}", ArmyParentRotator.From.ToString(), ArmyParentRotator.To.ToString()));
        ArmyParentRotator.TweenStart();
        BtnRight.interactable = false;
        BtnLeft.interactable = false;
        m_Rotating = true;
    }
    public void ArmyInfo()
    {
        objclickedForArmyInfo.SetActive(true);
        objUnclickedForArmyInfo.SetActive(false);
        objclickedForSkillInfo.SetActive(false);
        objUnlickedForSkillInfo.SetActive(true);
        objArmyInfo.SetActive(true);
        objArmySkill.SetActive(false);
    }
    void ArmySkill()
    {
        objclickedForArmyInfo.SetActive(false);
        objUnclickedForArmyInfo.SetActive(true);
        objclickedForSkillInfo.SetActive(true);
        objUnlickedForSkillInfo.SetActive(false);
        _armySkillPanelController.RefreshData(_heroArmyDataRef);
        objArmyInfo.SetActive(false);
        objArmySkill.SetActive(true);
    }
    void fillArmyInfoValue(AttributeData _attributeData)
    {
        txtAtkValue.text = _attributeData.Atk.ToString();
        txtDefValue.text = _attributeData.Def.ToString();
        txtCritValue.text = _attributeData.Crit.ToString();
        txtASpeedValue.text = _attributeData.AtkSpeed.ToString();
        txtARangeValue.text = _attributeData.AtkRange.ToString();
        txtMSpeedValue.text = _attributeData.MoveSpeed.ToString();

        txtAtkValueForArmyLevelUpinteface.text = _attributeData.Atk.ToString();
        txtDefValueForArmyLevelUpInteface.text= _attributeData.Def.ToString();
        txtCritValueForArmyLevelUpInteface.text= _attributeData.Crit.ToString();
        txtAspeedValueForArmyLevelUpInteface.text = _attributeData.AtkSpeed.ToString();
        txtARangeValueForArmyLevelUpInteface.text= _attributeData.AtkRange.ToString();
        txtMSpeedValueForArmyLevelUpInteface.text= _attributeData.MoveSpeed.ToString();

        txtCurrentandTotalItem.gameObject.SetActive(true);

        if (_heroArmyDataRef.ArmyLevel > 0 && _heroArmyDataRef.ArmyLevel< 40)
        {

            AttributeData nextAttributeData = _heroArmyDataRef.GetArmyAttribute(_heroArmyDataRef.ArmyLevel + 1);
            txtNextAtkValueForArmyLevelUpInteface.text = nextAttributeData.Atk.ToString();
            txtNextDefValueForArmyLevelUpInteface.text = nextAttributeData.Def.ToString();
            txtNextCritValueForArmyLevelUpInteface.text = nextAttributeData.Crit.ToString();
            txtNextAspeedValueForArmyLevelUpInteface.text = nextAttributeData.AtkSpeed.ToString();
            txtNextARangeValueForArmyLevelUpInteface.text = nextAttributeData.AtkRange.ToString();
            txtNextMSpeedValueForArmyLevelUpInteface.text = nextAttributeData.MoveSpeed.ToString();
        }
        else if(_heroArmyDataRef.ArmyLevel == 40)
        {
            txtCurrentandTotalItem.gameObject.SetActive(false);
        }
    }
    void unitTextFill()
    {
        string dot = " : ";
        txtAtk.text =TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ATK) + dot;
        txtDef.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF) + dot;
        txtCrit.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL) + dot;
        txtMSpeed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_SPEED) + dot;
        txtASpeed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_SPEED) + dot;
        txtARange.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_RANGE) + dot;

        txtAtkForArmyLevelUpInteface.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ATK) + dot;
        txtDefForArmyLevelUpInteface.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF) + dot;
        txtCritForArmyLevelUpInteface.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL) + dot;
        txtMSpeedForArmyLevelUpInteface.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_SPEED) + dot;
        txtASpeedForArmyLevelUpInteface.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_SPEED) + dot;
        txtARangeForArmyLevelUpInteface.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_RANGE) + dot;


        txtEquip.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_EQUIP);
        UnlockConditionDescription.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYNEEDUNLOCKBY);



        objWarning.SetActive(false);
        objWarningMsgFroArmylevelUnlock.SetActive(false);
        txtMaxLevelMessage.gameObject.SetActive(false);
        objUIArmyLevelUpInterface.SetActive(false);
      
    }
    private void ChangeScaleSelected()
    {
        for (int i = 0; i < clickOnList.Count; i++)
        {
            if (i == m_SelectedArmyIndex)
            {
                clickOnList[i].IncreaseModelSize();
            }
            else
            {
                clickOnList[i].DecreaseModelSizeToNormal();
            }
        }
    }
    private string GetFormattedUnlockedString(ArmyUnlockData unlock, PlayerHeroData heroData)
    {
        bool isConditionMet = heroData.CheckUnlockConditionMet(unlock);
        string levelTextString = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEVEL_TEXT_STRING_ID);
        string unlockStringFormat;
        if (isConditionMet)
        {
            unlockStringFormat = "{1} " + levelTextString;
        }
        else
        {
            unlockStringFormat = "<color=red>{1} " + levelTextString + "</color>";
        }
        string armyName = heroData.GetArmyNameFromType((E_ArmyType)unlock.ArmyType);
        return string.Format(unlockStringFormat, unlock.Level, armyName);
    }
    private void updateUIArmyLevelUpInterface(HeroArmyData _heroArmydata)
    {
        objUIArmyLevelUpInterface.SetActive(true);
        armyPlateParent.SetActive(false);
        fillArmyInfoValue(_heroArmydata.GetArmyAttribute(_heroArmydata.ArmyLevel));
    }
    private void closeUIArmyLevelUpInteface()
    {
        objUIArmyLevelUpInterface.SetActive(false);
        armyPlateParent.SetActive(true);
        objWarning.SetActive(false);
    }
    public void showUpgradeEffect()
    {
        go = Resources.Load("Effects/UIHeroDeckEffect/UPgradeEffect") as GameObject;
        if (_armyData.ArmyMoveType == (byte)E_ArmyMoveType.Cavalry)
        {
            go.transform.localScale = Vector3.one * 1.45f;
        }
        else
        {
            go.transform.localScale = Vector3.one * 0.75f;
        }
        go = Instantiate(go);
        go.transform.SetParent(upgradeEffectTrans, false);
    }
    IEnumerator showWarningMessage()
    {
        objWarningMsgFroArmylevelUnlock.SetActive(true);
        objWarningMsgFroArmylevelUnlock.GetComponentInChildren<Text>().text = "You Have Unlocked A New Army";
        yield return new WaitForSeconds(2.5f);
        objWarningMsgFroArmylevelUnlock.SetActive(false);
    }
    IEnumerator showWarningMessageForArmyUpgrade()
    {
        objWarning.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        objWarning.SetActive(false);
    }
}
