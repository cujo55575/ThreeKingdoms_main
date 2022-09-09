using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArmyUpgradePopUpController : MonoBehaviour
{
    public UIArmy UIArmyReference;

    public Text TxtTitle;
    public Text TxtContent;
    public Text TxtBPCost;
    public Text TxtConfirm;
    public Text TxtCancel;

    public RawImage IcnBattlePoint;

    public Button BtnConfirm;
    public Button BtnCancel;

    private PlayerHeroData m_PlayerHeroData;
    private HeroArmyData m_HeroArmyData;

    private const int NOT_ENOUGH_MESSAGE_ID = 57023;
    private const int ENOUGH_MESSAGE_ID = 57022;
    private const int ARMY_MAX_LEVEL_MESSAGE_ID = 57024;
    private const int TITLE_MESSAGE_ID = 57012;

    private const string BP_COUNT_STRING_FORMAT = "x{0}";

    void Start()
    {
        TxtTitle.text = TableManager.Instance.LocaleStringDataTable.GetString(TITLE_MESSAGE_ID);
        TxtConfirm.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);
        TxtCancel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_CANCELBTN);
        BtnConfirm.onClick.AddListener(OnClickConfirm);
        BtnCancel.onClick.AddListener(OnClickCancel);
    }

    public void Refresh(PlayerHeroData data)
    {
        m_PlayerHeroData = data;
        m_HeroArmyData = m_PlayerHeroData.GetEquippedArmySelf();
        RefreshUI();
    }

    private void RefreshUI()
    {
        Debug.Log("Upgrade Cost = " + m_HeroArmyData.GetArmyAttribute(m_HeroArmyData.ArmyLevel).UpgradeCost.ToString());
        Debug.Log("BP Left = " + PlayerDataManager.Instance.PlayerData.BattlePoint);
        Debug.Log("Army Level = " + m_HeroArmyData.ArmyLevel);
        bool isResourceEnough = PlayerDataManager.Instance.PlayerData.BattlePoint > m_HeroArmyData.GetArmyAttribute(m_HeroArmyData.ArmyLevel).UpgradeCost;
        bool isArmyAtMaxLevel = m_HeroArmyData.IsArmyAtMaxLevel();
        BtnConfirm.gameObject.SetActive(isResourceEnough && !isArmyAtMaxLevel);
        IcnBattlePoint.gameObject.SetActive(isResourceEnough && !isArmyAtMaxLevel);
        TxtBPCost.gameObject.SetActive(isResourceEnough && !isArmyAtMaxLevel);
        TxtBPCost.text = string.Format(BP_COUNT_STRING_FORMAT, m_HeroArmyData.GetArmyAttribute(m_HeroArmyData.ArmyLevel).UpgradeCost.ToString());
        int contentStringID = isArmyAtMaxLevel ? ARMY_MAX_LEVEL_MESSAGE_ID : (isResourceEnough ? ENOUGH_MESSAGE_ID : NOT_ENOUGH_MESSAGE_ID);
        TxtContent.text = TableManager.Instance.LocaleStringDataTable.GetString(contentStringID);
    }

    private void OnClickConfirm()
    {
        m_HeroArmyData.SetArmyLevel(m_HeroArmyData.ArmyLevel + 1);
        PlayerDataManager.Instance.PlayerData.BattlePoint -= (int)m_HeroArmyData.GetArmyAttribute(m_HeroArmyData.ArmyLevel).UpgradeCost;
        //PlayerDataManager.Instance.SavePlayerData();
        gameObject.SetActive(false);
        UIArmyReference.ClosePopUp();
        UIArmyReference.RefreshData(m_PlayerHeroData);
    }

    private void OnClickCancel()
    {
        gameObject.SetActive(false);
    }
}
