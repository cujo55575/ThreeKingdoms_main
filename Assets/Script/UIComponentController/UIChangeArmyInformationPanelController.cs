using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeArmyInformationPanelController : MonoBehaviour
{
    public GameObject UpgradeLevelObjectParent;
    public GameObject ArrowObject;
    public GameObject UnlockConditonParent;
    public Button btnUpgrade;
    public RawImage IcnArmyMoveType;
    public RawImage IcnArmyAtkType;
    public Text TxtArmyMoveType;
    public Text TxtArmyAttackType;
    public Text TxtMaxArmyLevelDescription;
    public Text UnlockConditionDescription;
    public Text UnlockCondition1;
    public Text UnlockCondition2;
    public Text UnlockCondition3;
    public Text txtUpgradeLevel, txtUpgradeLevelValue;
    public Text txtUpgradeHP, txtUpgradeHPValue;
    public Text txtUpgradeAtk, txtUpgradeAtkValue;
    public Text txtUpgradeDef, txtUpgradeDefValue;
    public Text txtUpgradeCrit, txtUpgradeCritValue;
    public Text txtUpgradeASpeed, txtUpgradeASpeedValue;
    public Text txtUpgradeARange, txtUpgradeARangeValue;
    public Text txtUpgradeMSpeed, txtUpgradeMSpeedValue;
    public Text txtLevel, txtLevelValue;
    public Text txtHP, txtHPValue;
    public Text txtAtk, txtAtkValue;
    public Text txtDef, txtDefValue;
    public Text txtCrit, txtCritValue;
    public Text txtAspeed, txtAspeedValue;
    public Text txtARange, txtARangeValue;
    public Text txtMSpeed, txtMSpeedValue;
    public Text txtEquipBTN;
    public Text txtArmyName;
    public Text txtCostForUpgrade;
    public RawImage ArmyTypeImage;
    public Sprite imgarmyTypeAtk;
    public Sprite imgarmyTypeAsn;
    public Sprite imgarmyTypeDef;
    AttributeData _attributeData;
    public Shader grayscale;
    public Material matBtnUpgrade;
    int upgradeCost;
    private HeroArmyData _heroArmydataReference;
    void Awake()
    {
        matBtnUpgrade = new Material(grayscale);
        btnUpgrade.image.material = matBtnUpgrade;
    }
    void Start()
    {
        UItextsFill();
    }
    void Update()
    {
        refershtxtCost(_attributeData);
    }

    public void RefreshData(HeroArmyData _heroArmyData, bool isArmyLocked, PlayerHeroData _playerHeroData)
    {
        _heroArmydataReference = _heroArmyData;
        ArmyData _armyData = _heroArmydataReference.GetArmyData();
        IcnArmyAtkType.texture = _armyData.GetArmyAttackTypeTexture();
        IcnArmyMoveType.texture = _armyData.GetArmyMoveTypeTexture();
        TxtArmyAttackType.text = _armyData.GetArmyAttackTypeName();
        TxtArmyMoveType.text = _armyData.GetArmyMoveTypeName();
        AttributeData attribDataCurrentLevel = _heroArmydataReference.GetArmyAttribute(_heroArmyData.ArmyLevel);
        AttributeData attributeDataNextLevel = _heroArmyData.GetArmyAttribute(_heroArmyData.ArmyLevel + 1);
        // RefershArmyTypeImage(_armyData);
        ArmyTypeImage.texture = _armyData.GetArmyJobTypeTexture();
        fillArmyInfoValue(attribDataCurrentLevel);
        bool toShowUpgradeObject = !(isArmyLocked || _heroArmyData.IsArmyAtMaxLevel() || attributeDataNextLevel == null);
        UpgradeLevelObjectParent.SetActive(toShowUpgradeObject);
        ArrowObject.SetActive(UpgradeLevelObjectParent.activeInHierarchy);
        if (UpgradeLevelObjectParent.activeSelf)
        {
            fillArmyInfoNextLevelValue(attributeDataNextLevel, attribDataCurrentLevel);
            UpgradeLevelObjectParent.SetActive(true);
        }

        if (isArmyLocked)
        {
            UnlockCondition1.gameObject.SetActive(false);
            UnlockCondition2.gameObject.SetActive(false);
            UnlockCondition3.gameObject.SetActive(false);
            List<ArmyUnlockData> unlocks = _heroArmydataReference.GetArmyData().GetUnlockConditions();
            for (int i = 0; i < unlocks.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        UnlockCondition1.text = GetFormattedUnlockedString(unlocks[0], _playerHeroData);
                        UnlockCondition1.gameObject.SetActive(true);
                        break;

                    case 1:
                        UnlockCondition2.text = GetFormattedUnlockedString(unlocks[1], _playerHeroData);
                        UnlockCondition2.gameObject.SetActive(true);

                        break;

                    case 2:
                        UnlockCondition3.text = GetFormattedUnlockedString(unlocks[2], _playerHeroData);
                        UnlockCondition3.gameObject.SetActive(true);
                        break;

                    default:
                        break;
                }
            }

            UnlockConditonParent.SetActive(true);
        }
        else
        {

            UnlockConditonParent.SetActive(false);
        }

        TxtMaxArmyLevelDescription.gameObject.SetActive(_heroArmyData.IsArmyAtMaxLevel());
        // CostObject.SetActive(!TxtMaxArmyLevelDescription.gameObject.activeSelf);
        // UpgradeObject.SetActive(!TxtMaxArmyLevelDescription.gameObject.activeSelf);
        // switchArmyInfoButton.SetActive(!TxtMaxArmyLevelDescription.gameObject.activeSelf);

        RefreshLevelAndCostData(_heroArmydataReference, _armyData, attribDataCurrentLevel, attributeDataNextLevel);
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
    void RefreshLevelAndCostData(HeroArmyData _heroArmyData, ArmyData _armydata, AttributeData _attriData, AttributeData _upgradeAttriData)
    {
        txtLevelValue.text = _heroArmyData.ArmyLevel.ToString();
		if (_heroArmyData.ArmyLevel == 0)
		{
			Debug.Log("ArmyLevel 0 for ID = "+_heroArmyData.key());
		}
        int _armyLevelUpgrade = _heroArmyData.ArmyLevel + 1;
        txtUpgradeLevelValue.text = _armyLevelUpgrade.ToString();
        txtArmyName.text = _armydata.GetArmyName();

        _attributeData = _attriData;
        refershtxtCost(_attributeData);
        // if (totalBP < upgradeCost)
        // {
        //     costStringFormat = "{0} / <color=red>{1}</color>";
        // }
        // else
        // {
        //     costStringFormat = "{0} / {1}";
        // }

        // txtCostForUpgrade.text = upgradeCost.ToString();
        // txtCostForUpgrade.text = string.Format(costStringFormat, upgradeCost, totalBP);

    }
    void refershtxtCost(AttributeData _aData)
    {
        int totalBP = PlayerDataManager.Instance.PlayerData.BattlePoint;
        upgradeCost = (int)_aData.UpgradeCost;
        string costStringFormat = string.Empty;
        btnUpgrade.interactable = (PlayerDataManager.Instance.PlayerData.BattlePoint >= (int)_aData.UpgradeCost);
        if (btnUpgrade.interactable)
        {
            matBtnUpgrade.SetFloat("_EffectAmount", 0);
        }
        else
        {
            matBtnUpgrade.SetFloat("_EffectAmount", 1);
        }
        if (totalBP < upgradeCost)
        {
            costStringFormat = "<color=red>{0}</color>";
        }
        else
        {
            costStringFormat = "{0}";
        }
        txtCostForUpgrade.text = string.Format(costStringFormat, upgradeCost);
    }
    // void RefershArmyTypeImage(ArmyData _ArmyData)
    // {
    //     if (_ArmyData.ArmyJobType == 0)
    //     {
    //         ArmyTypeImage.texture = imgarmyTypeAsn.texture;
    //     }
    //     else if (_ArmyData.ArmyJobType == 1)
    //     {
    //         ArmyTypeImage.texture = imgarmyTypeAtk.texture;
    //     }
    //     else if (_ArmyData.ArmyJobType == 2)
    //     {
    //         ArmyTypeImage.texture = imgarmyTypeDef.texture;
    //     }
    // }
    void fillArmyInfoValue(AttributeData _attributeData)
    {
        txtHPValue.text = _attributeData.Hp.ToString();
        txtAtkValue.text = _attributeData.Atk.ToString();
        txtDefValue.text = _attributeData.Def.ToString();
        txtCritValue.text = _attributeData.Crit.ToString();
        txtAspeedValue.text = _attributeData.AtkSpeed.ToString();
        txtARangeValue.text = _attributeData.AtkRange.ToString();
        txtMSpeedValue.text = _attributeData.MoveSpeed.ToString();
    }
    void fillArmyInfoNextLevelValue(AttributeData _attributeDataUpgrade, AttributeData _attributeOriginalData)
    {
        txtUpgradeHPValue.text = _attributeDataUpgrade.Hp.ToString();
        txtUpgradeAtkValue.text = _attributeDataUpgrade.Atk.ToString();
        txtUpgradeDefValue.text = _attributeDataUpgrade.Def.ToString();
        txtUpgradeCritValue.text = _attributeDataUpgrade.Crit.ToString();
        txtUpgradeASpeedValue.text = _attributeDataUpgrade.AtkSpeed.ToString();
        txtUpgradeARangeValue.text = _attributeDataUpgrade.AtkRange.ToString();
        txtUpgradeMSpeedValue.text = _attributeDataUpgrade.MoveSpeed.ToString();
        if (_attributeDataUpgrade.Hp > _attributeOriginalData.Hp)
        {
            txtUpgradeHPValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.Hp.ToString());
        }
        if (_attributeDataUpgrade.Atk > _attributeOriginalData.Atk)
        {
            txtUpgradeAtkValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.Atk.ToString());
        }
        if (_attributeDataUpgrade.Def > _attributeOriginalData.Def)
        {
            txtUpgradeDefValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.Def.ToString());
        }
        if (_attributeDataUpgrade.Crit > _attributeOriginalData.Crit)
        {
            txtUpgradeCritValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.Crit.ToString());
        }
        if (_attributeDataUpgrade.AtkSpeed > _attributeOriginalData.AtkSpeed)
        {
            txtUpgradeASpeedValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.AtkSpeed.ToString());
        }
        if (_attributeDataUpgrade.AtkRange > _attributeOriginalData.AtkRange)
        {
            txtUpgradeARangeValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.AtkRange.ToString());
        }
        if (_attributeDataUpgrade.MoveSpeed > _attributeOriginalData.MoveSpeed)
        {
            txtUpgradeMSpeedValue.text = string.Format("<color=green>{0}</color>", _attributeDataUpgrade.MoveSpeed.ToString());
        }
    }
    //Fill ALl text of ArmyType panel and AttackType panel;
    void UItextsFill()
    {
        txtEquipBTN.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIARMY_LEVELUP);
        txtUpgradeLevel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYLEVEL);
        txtLevel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYLEVEL);
        txtUpgradeHP.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIArmy_HP);
        txtHP.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIArmy_HP);
        txtUpgradeAtk.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ATK);
        txtDef.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF);
        txtUpgradeDef.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF);
        txtAtk.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ATK);
        txtUpgradeCrit.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL);
        txtCrit.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL);
        txtUpgradeCrit.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL);
        txtAspeed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_SPEED);
        txtUpgradeASpeed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_SPEED);
        txtARange.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_RANGE);
        txtUpgradeARange.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_RANGE);
        txtMSpeed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_SPEED);
        txtUpgradeMSpeed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_SPEED);
        UnlockConditionDescription.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYNEEDUNLOCKBY);
        TxtMaxArmyLevelDescription.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ARMY_MAX_LEVEL_DESCRIPTION_STRING_ID);

    }

}
