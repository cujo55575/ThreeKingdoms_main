using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArmy : UIBase
{
    public Transform ModelParent;
    public Button BtnClose;
    public Button BtnLevelUp;
    public Button BtnChangeArmy;
    public Button BtnIntroduction;
    public Button BtnConfirm;
    public Button BtnCancel;
    public Button BtnBack;
    public UIArmyUpgradePopUpController ArmyUpgradeControl;
    public GameObject InformationPopUp;
    public ArmyDetail ArmyDetail;
    public Text TxtMoney;
    public Text TxtLvlUpTitle;
    public Text TxtLvlUpDesc;
    public Text TxtArmyName;
    public Text TxtArmyDesc;
    public Text txtLevelUP;
    public Text txtChangeArmy;
    // public Text txtPopConfirmbtn;
    public Text txtIntroudction;
    public Text txtHP;
    public Text txtATK;
    public Text txtDEF;
    public Text txtA_SPEED;
    public Text txtCrittical;
    public Text txtCrittical_DEF;
    public Text txtMove_Speed;
    public Text txtATTACK_RANGE;
    // public Text txtOKButton;
    public Text txtLevel;
    public UICard CardUI;

    public GameObject[] characterModels;

    //public int characterIndex, uiArmyIndex;

    private PlayerHeroData m_PlayerHeroData;
    protected override void OnUpdate()
    {
        base.OnUpdate();

        //if (characterIndex == uiArmyIndex)
        //{ characterModels[characterIndex].SetActive(true); }
        //else
        //{
        //	characterModels[characterIndex].SetActive(false);
        //}

    }
    protected override void OnInit()
    {
        base.OnInit();
        BtnClose.onClick.AddListener(CloseUI);

        BtnLevelUp.onClick.AddListener(LevelUp);
        BtnChangeArmy.onClick.AddListener(ChangeArmy);
        BtnIntroduction.onClick.AddListener(ShowIntroduction);
        BtnConfirm.onClick.AddListener(ConfirmLevelUp);
        BtnCancel.onClick.AddListener(ClosePopUp);
        BtnBack.onClick.AddListener(ClosePopUp);


    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        if (Objects != null)
        {
            PlayerHeroData playerHeroData = (PlayerHeroData)Objects[0];
            RefreshData(playerHeroData);
        }
        txtLevelUP.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIARMY_LEVELUP);
        txtChangeArmy.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIARMY_CHANGEARMY);
        txtIntroudction.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIARMY_INTRODUCTION);
        // txtPopConfirmbtn.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ARMYCONFIRMBUTTON);

        txtHP.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIArmy_HP);
        txtATK.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_ATK);
        txtDEF.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_DEF);
        txtA_SPEED.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_SPEED);
        txtCrittical.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL);
        txtCrittical_DEF.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_CRITICAL);
        txtMove_Speed.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_SPEED);
        txtATTACK_RANGE.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIChangeArmy_A_RANGE);

        ArmyUpgradeControl.gameObject.SetActive(false);

    }

    private void LevelUp()
    {
        ModelParent.gameObject.SetActive(false);
        //UIMessageBox.ShowMessageBox("Coming Soon!", E_MessageBox.Yes, CloseMessageCallback);
        ArmyUpgradeControl.Refresh(m_PlayerHeroData);
        ArmyUpgradeControl.gameObject.SetActive(true);
    }
    public void CloseMessageCallback(bool sure, object[] param)
    {
        Debug.Log("YesMessageCallbackCalled.");
        ClosePopUp();
    }

    private void ChangeArmy()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_ARMY);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_CHANGE_ARMY, m_PlayerHeroData);
        GameObject directionalLight = GameObject.Find("Directional Light");
        Light light = directionalLight.GetComponent<Light>();
        light.enabled = false;

    }

    private void ShowIntroduction()
    {
        //InformationPopUp.SetActive(true);
        ModelParent.gameObject.SetActive(false);
        UIMessageBox.ShowMessageBox("Coming Soon!", E_MessageBox.Yes, CloseMessageCallback);
    }


    public void ConfirmLevelUp()
    {

    }
    public void ClosePopUp()
    {
        ArmyUpgradeControl.gameObject.SetActive(false);
        InformationPopUp.SetActive(false);
        ModelParent.gameObject.SetActive(true);
    }
    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_ARMY);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK, m_PlayerHeroData);
        //PlayerDataManager.Instance.SavePlayerData();
    }

    public void RefreshData(PlayerHeroData heroData)
    {
        m_PlayerHeroData = heroData;
        CardUI.RefreshData(m_PlayerHeroData);
        HeroArmyData HeroArmyData = m_PlayerHeroData.GetEquippedArmySelf();
        txtLevel.text = string.Format(TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEVEL_TEXT_STRING_ID), m_PlayerHeroData.GetEquippedArmySelf().ArmyLevel);



        ArmyDetail.RefreshData(HeroArmyData);
        ArmyData armyData = HeroArmyData.GetArmyData();
        for (int i = 0; i < ModelParent.childCount; i++)
        {
            Destroy(ModelParent.GetChild(i).gameObject);
        }

        GameObject m_ModelObject = armyData.GetModelObject();
        m_ModelObject = Instantiate(m_ModelObject, ModelParent);

        Vector3 position;
        Vector3 rotation;
        Vector3 scale;

        switch ((E_ArmyType)armyData.ArmyType)
        {
            case E_ArmyType.Archer:
                position = new Vector3(10f, 15f, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 90f;
                break;
            case E_ArmyType.Footman:
                position = new Vector3(10f, 15f, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 90f;
                break;
            case E_ArmyType.Horseman:
                position = new Vector3(10f, 15f, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 60f;
                break;
            default:
                position = new Vector3(-275f, -160f, 0);
                rotation = Vector3.up * 180f;
                scale = Vector3.one * 90f;
                break;
        }
        m_ModelObject.transform.localPosition = position;
        m_ModelObject.transform.rotation = Quaternion.Euler(rotation);
        m_ModelObject.transform.localScale = scale;

        m_ModelObject.layer = 5;
    }
}
