using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeArmy : UIBase
{
    public Button BtnClose;
    public Button BtnUpgrade;
    public Button BtnSwithArmyInfo;
    public Button BtnPrevious;
    public Button BtnNext;
    public Button BtnEquipArmy;
    public Text thisArmyneedred;
    public Text thisarmyneedgreen;
    public Text txtThisArmyNeedUnlock;
    public Text txtTiteBar;
    public Text txtArmySkill;
    public Text txtSwitchArmyInfo;
    public Text _heroName;
    public GameObject SpotLightObject;
    public GameObject ArmyPlaceHolderPrefab;
    public GameObject lockInfoPanel;
    public GameObject ArmyDataPanel;
    public GameObject unlockConditionalPanel;
    public GameObject ObjCost;
    public RawImage heroImage;
    public RawImage imgbtnEquip;
    public Sprite disableEquipbuttonImage;
    public Sprite enableEquipButtonImage;
    public Transform ArmyParent;
    public TweenRotation ArmyParentRotator;
    public UIChangeArmyInformationPanelController _UIChangeArmyInfoPanelController;
    public UIChangeArmySkillInfoPanelController SkillPanelControl;
    public List<HeroArmyData> armies = new List<HeroArmyData>();
    private PlayerHeroData m_PlayerHeroData;
    private PlayerHeroData heroData;
    private int m_SelectedArmyIndex;
    private bool m_Rotating = false;
    private bool isArmyCondition;
    private float m_Timer;
    public UITopBar _uiTopBar;
    //Grayscale
    public Shader grayscale;
    public Material matBtnEquipArmy;
    public List<ClickOn> clickOnList = new List<ClickOn>();
    private Animator m_AttackingAnimator;
    private enum E_Direction
    {
        Left, Right
    }
    protected override void OnInit()
    {
        base.OnInit();
        armies = new List<HeroArmyData>();
        BtnClose.onClick.AddListener(CloseUI);
        BtnEquipArmy.onClick.AddListener(OnClickEquipArmy);
        BtnUpgrade.onClick.AddListener(OnClickUpgrade);
        BtnSwithArmyInfo.onClick.AddListener(() => switchArmyInfo(isArmyCondition));
        BtnNext.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Left);
        });
        BtnPrevious.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Right);
        });
        ArmyParentRotator.FinishMethod = delegate
        {
            RotateFinish();
        };
        matBtnEquipArmy = new Material(grayscale);
        BtnEquipArmy.image.material = matBtnEquipArmy;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (m_AttackingAnimator != null && m_AttackingAnimator.GetBool("IsAttacking"))
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_AttackingAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                m_AttackingAnimator.SetBool("IsAttacking", false);
                m_AttackingAnimator = null;
            }
        }
    }
       protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        if (Objects != null && Objects.Length == 1)
		{
			heroData = (PlayerHeroData)Objects[0];
			RefreshData(heroData);
        }
        _uiTopBar.RefreshData();
        txtTiteBar.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_BTN_ARMYCONFIG);
        txtArmySkill.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UI_CHANGEARMY_SKILLINFO_TITLE);
        txtThisArmyNeedUnlock.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYNEEDUNLOCKBY);
        thisArmyneedred.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYName) + TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEVEL_TEXT_STRING_ID);
        thisarmyneedgreen.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYName) + TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEVEL_TEXT_STRING_ID);
    }
    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_CHANGE_ARMY);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK,m_PlayerHeroData);
        GameObject directionalLight = GameObject.Find("Directional Light");
        Light light = directionalLight.GetComponent<Light>();
        light.enabled = true;
		//PlayerDataManager.Instance.SavePlayerData();
	}
    private void OnClickUpgrade()
    {
        armies = m_PlayerHeroData.GetAllArmiesSelf();
        AttributeData attribDataCurrentLevel = armies[m_SelectedArmyIndex].GetArmyAttribute(armies[m_SelectedArmyIndex].ArmyLevel);
        PlayerDataManager.Instance.PlayerData.BattlePoint -= (int)attribDataCurrentLevel.UpgradeCost;
        _uiTopBar.RefreshData();
        armies[m_SelectedArmyIndex].SetArmyLevel(armies[m_SelectedArmyIndex].ArmyLevel + 1);
        m_PlayerHeroData.ReCheckArmyUnlockConditions();
        RefreshUI(armies[m_SelectedArmyIndex]);


        ArmyParent.transform.GetChild(m_SelectedArmyIndex).gameObject.GetComponentInChildren<ClickOn>().Refresh(armies[m_SelectedArmyIndex]);
        for (int i = 0; i < armies.Count; i++)
        {
            clickOnList[i].Refresh(armies[i]);
        }
    }
    private void switchArmyInfo(bool _isArmyCondition)
    {
        if (_isArmyCondition)
        {
            ArmyDataPanel.SetActive(true);
            unlockConditionalPanel.SetActive(false);
            txtSwitchArmyInfo.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHANGEARMY_BTN_SWITCH_ARMYDATAPANEL);
            isArmyCondition = false;
        }
        else
        {
            ArmyDataPanel.SetActive(false);
            unlockConditionalPanel.SetActive(true);
            txtSwitchArmyInfo.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHANGEARMY_BTN_SWITCH_ARMYCONDITIONPANEL);
            isArmyCondition = true;
        }
    }
    private void OnClickEquipArmy()
    {
        //Debug.Log("OnClickEquipArmy()");
        StartCoroutine(delayLeftRightBtn());
        List<HeroArmyData> armies = m_PlayerHeroData.GetAllArmiesSelf();
        for (int i = 0; i < armies.Count; i++)
        {
            if (i == m_SelectedArmyIndex)
            {
                HeroArmyData equippedArmy = m_PlayerHeroData.GetEquippedArmySelf();
                if (equippedArmy != null)
                {
                    equippedArmy.HeroArmyStatusType = (byte)E_EquipableStatus.Unlocked;
                }
                armies[i].HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
				//PlayerDataManager.Instance.SavePlayerData();
				break;
            }
        }
        m_AttackingAnimator = ArmyParent.transform.GetChild(m_SelectedArmyIndex).gameObject.GetComponentInChildren<Animator>();
        Debug.Log(m_AttackingAnimator.gameObject.name);
        m_AttackingAnimator.SetBool("IsAttacking", true);
        m_Timer = 0;
        Debug.Log("m_SelectedArmyIndex="+m_SelectedArmyIndex);
        BtnEquipArmy.interactable = false;
        // BtnEquipArmy.GetComponentInChildren<GameObject>.image.sprite = disableEquipbuttonImage;
        imgbtnEquip.texture = disableEquipbuttonImage.texture;
    }
    IEnumerator delayLeftRightBtn()
    {
        BtnNext.interactable = false;
        BtnPrevious.interactable = false;
        yield return new WaitForSeconds(1f);
        BtnNext.interactable = true;
        BtnPrevious.interactable = true;
    }

    private void RefreshData(PlayerHeroData playerHeroData)
    {
        clickOnList = new List<ClickOn>();
        for (int i = 0; i < ArmyParent.childCount; i++)
        {
            Destroy(ArmyParent.GetChild(i).gameObject);
        }
        m_SelectedArmyIndex = 0;
        m_PlayerHeroData = playerHeroData;
        List<HeroArmyData> armies = m_PlayerHeroData.GetAllArmiesSelf();
        float constRot = 360f / armies.Count;
        _heroName.text = playerHeroData.GetHeroName();
        heroImage.texture = playerHeroData.GetHeroData().GetHeroTexture();
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
        RefreshUI(armies[m_SelectedArmyIndex]);
        ArmyParentRotator.gameObject.transform.localEulerAngles = Vector3.up * -armyParentYRotation;
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
        Debug.Log("Equipped army level is " + m_PlayerHeroData.GetAllArmiesSelf()[m_SelectedArmyIndex].ArmyLevel);
        RefreshUI(m_PlayerHeroData.GetAllArmiesSelf()[m_SelectedArmyIndex]);
        RotateArmyParent(direction);
        ChangeScaleSelected();
    }
    private void RefreshUI(HeroArmyData armyData)
    {
        bool isArmyLocked = (E_EquipableStatus.Locked == (E_EquipableStatus)armyData.HeroArmyStatusType);
        bool isArmyEquip = (E_EquipableStatus.Equipped == (E_EquipableStatus)armyData.HeroArmyStatusType);
        bool isArmyLevelMax = armyData.IsArmyAtMaxLevel();
        Debug.Log(armyData.GetArmyData().ModelName + isArmyLocked);
        SpotLightObject.SetActive(!isArmyLocked);
        BtnEquipArmy.interactable = (!isArmyLocked) && (!isArmyEquip);
        ShowOrHideActivateButton(isArmyLocked);
        if (isArmyLocked)
        {
            BtnUpgrade.gameObject.SetActive(false);
            BtnSwithArmyInfo.gameObject.SetActive(true);
            ObjCost.SetActive(false);
            switchArmyInfo(!isArmyLocked);
        }
        else
        {
            if (isArmyLevelMax)
            {
                BtnUpgrade.gameObject.SetActive(false);
                BtnSwithArmyInfo.gameObject.SetActive(false);
                ObjCost.SetActive(false);
            }
            else
            {
                BtnUpgrade.gameObject.SetActive(true);
                BtnSwithArmyInfo.gameObject.SetActive(false);
                ArmyDataPanel.SetActive(true);
                unlockConditionalPanel.SetActive(true);
                ObjCost.SetActive(true);
            }
            switchArmyInfo(true);
        }

        if (BtnEquipArmy.interactable)
        {
            imgbtnEquip.texture = enableEquipButtonImage.texture;
        }
        else
        {
            imgbtnEquip.texture = disableEquipbuttonImage.texture;
        }
        lockInfoPanel.SetActive(false);
        SkillPanelControl.RefreshData(armyData);
        _UIChangeArmyInfoPanelController.RefreshData(armyData, isArmyLocked, m_PlayerHeroData);
    }
    void ShowOrHideActivateButton(bool _isArmyLocked)
    {
        if (_isArmyLocked)
        {
            BtnEquipArmy.gameObject.SetActive(false);
        }
        else
        {
            BtnEquipArmy.gameObject.SetActive(true);
        }
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
        BtnNext.interactable = false;
        BtnPrevious.interactable = false;
        m_Rotating = true;
    }

    private void RotateFinish()
    {
        Debug.Log("RotateFinish");
        BtnNext.interactable = true;
        BtnPrevious.interactable = true;
        m_Rotating = false;
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
}
