using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIMainMenu : UIBase
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnLSTOM;
    public Button btnRSTOM;
    public Button BtnBattleStart;
    public Button BtnAchievement;
    public Button BtnFriendList;
    public Button BtnGacha;
    public Button BtnDeck;
    public Button BtnChooseProfile;
    public Button BtnAccountCreatePanel;
    public Button BtnAccountNameConfirm;
    public Button BtnAccountNameErrorClose;
    public InputField accountNameInputField;
    public Text txtMessage;
    public Text errorMessage;
    public Text txtName;
    public Text txtonlyallowcharacters;
    public Text txtsummoner;
    public Text TxtPlayerName;
    public Text TxtPlayerLevelValue;
    public Text txtTipsTitle;
    public GameObject accountCreateTextBoxOBJ;
    public GameObject errormsgforAccountName;
    public GameObject _accountCreatePanel;
    public GameObject bgDisable;
    public GameObject objModePanel;
    public GameObject objBackground;
    public RawImage ImgProfile;
    public RawImage imgLSTOM;
    public RawImage imgRSTOM;
    public Transform _selectedModePanel;
    private int btnAccountNameIndex = 0;
    public int battleModeIndex = 0;
    private string _accountName;
    public List<ModePanelController> modePanelControllerList = new List<ModePanelController>();

    private enum E_BattleMode
    {
        PVP, PVE, UnLimited
    }
    private string[] ModeBg = new string[3];
    private string[] ModeIcon_Button = new string[3];
    private string[] ModeIcon = new string[3];
    protected override void OnInit()
    {
        objBackground.SetActive(false);
        ModeBg = new string[] { "background_mainpage", "bg_PVPMode", "bg_PVEMode", "bg_UnlimitedMode" };
        ModeIcon_Button = new string[] { "UI_pvp", "UI_campaign", "UI_school" };
        ModeIcon = new string[] { "Frame_Pvp", "Frame_campaign(c)", "Frame_school" };

        base.OnInit();

        objModePanel.SetActive(false);

        UpdateSelectedMode(battleModeIndex);


        txtMessage.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ACCOUNT_CREATION_INTRO_MESSAGE);
        BtnAccountNameConfirm.GetComponentInChildren<Text>().text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);

        txtTipsTitle.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMAIN_ACCOUNTCREATE_TITLE);
        BtnAchievement.onClick.AddListener(ShowAchievement);
        BtnFriendList.onClick.AddListener(ShowFriendList);
        BtnDeck.onClick.AddListener(ShowHeroDeck);
        BtnGacha.onClick.AddListener(ShowGacha);
        BtnChooseProfile.onClick.AddListener(ChooseProfileIcon);
        BtnAccountCreatePanel.onClick.AddListener(() => accountCreateTextBox(btnAccountNameIndex));
        BtnAccountNameConfirm.onClick.AddListener(accountCreatConfirmBtn);
        BtnAccountNameErrorClose.onClick.AddListener(accountNameErrorMsgCloseBtn);
        BtnBattleStart.onClick.AddListener(BattleStart);
    }
    protected override void OnShow(params object[] Objects)
    {
        _accountCreatePanel.SetActive(PlayerDataManager.Instance.PlayerData.IsNewPlayer);

        base.OnShow(Objects);

        battleModeIndex = 0;
        SoundManager.Instance.PlayBGM("BGM00001");

        RefreshUI(battleModeIndex);
        RefreshPlayerInformation();
    }
    public void swipeControl(bool isSwipe)
    {
        if (isSwipe)
        {
            battleModeIndex++;
        }
        else
        {
            battleModeIndex--;
        }
        if (battleModeIndex >= Enum.GetNames(typeof(E_BattleMode)).Length)
        {
            battleModeIndex = 0;
        }
        else if (battleModeIndex < 0)
        {
            battleModeIndex = Enum.GetNames(typeof(E_BattleMode)).Length - 1;
        }
        RefreshUI(battleModeIndex);
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            SoundManager.Instance.PlayBGM("BGM00001");
        }
    }
    public void ShowAchievement()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MAIN_MENU);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_ACHIEVEMENT);
    }
    public void ShowFriendList()
    {
        // UIManager.Instance.ShowUI(GLOBALCONST.UI_FRIEND_LIST);
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MAIN_MENU);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_HeroInterface);
    }
    public void ShowGacha()
    {
        //UIManager.Instance.CloseUI(GLOBALCONST.UI_MAIN_MENU);
        UIManager.Instance.CloseUI(GLOBALCONST.UITopBar);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_GACHA, string.Empty, E_GachaType.None);
        UIManager.Instance.ShowUI(GLOBALCONST.UITopBar);
    }
    public void ShowHeroDeck()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MAIN_MENU);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_HeroDeck, PlayerDataManager.Instance.PlayerData.OwnedHeros);
        UIManager.UIInstance<UITopBar>().RefreshData();
    }
    public void ChooseProfileIcon()
    {
        UIManager.Instance.ShowUI(GLOBALCONST.UI_CHOOSE_PROFILE);
    }
    //for clicking on panel and do the job
    public void OnPanelClick()
    {
        accountCreateTextBox(btnAccountNameIndex);
    }
    public void accountCreateTextBox(int i)
    {
        if (i == 0)
        {
            //Open AccountCreate
            accountCreateTextBoxOBJ.SetActive(true);
            txtMessage.gameObject.SetActive(false);
            BtnAccountCreatePanel.gameObject.SetActive(false);
            txtName.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMainMenu_Name);
            txtonlyallowcharacters.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMainMenu_ONLYCHARACTERS);
            txtsummoner.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMainMenu_Summoner_REGUSTERATION);
        }
        else if (i == 1)
        {
            txtMessage.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ACCOUNT_CREATION_CARD_WARNING_MESSAGE);
            btnAccountNameIndex = 2;
        }
        else if (i == 2)
        {
            txtMessage.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ACCOUNT_CREATION_PLAYGAME_MESSAGE);
            btnAccountNameIndex = 3;
        }
        else if (i == 3)
        {
            PlayerDataManager.Instance.PlayerData.Name = _accountName;
#if OFFLINE_DATA
            PlayerDataManager.Instance.PlayerData.IsNewPlayer = false;
            RefreshPlayerInformation();
            _accountCreatePanel.SetActive(false);
            UIManager.Instance.ShowUI(GLOBALCONST.UI_GACHA, GLOBALCONST.UI_MAIN_MENU, E_GachaType.Fixed);
            PlayerDataManager.Instance.SavePlayerData();

            return;
#endif
        }
    }
    public void BattleStart()
    {
        if (battleModeIndex == 0)
        {
            UIManager.Instance.CloseAllUI();
            GLOBALVALUE.CURRENT_MATCH_MODE = E_MatchMode.RankedMatch;
            UIManager.Instance.ShowUI(GLOBALCONST.UI_MATCHMAKING);
        }
        else if (battleModeIndex == 1)
        {
            LevelData levelData = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData();
            if (levelData == null)
            {
                return;
            }
            UIManager.Instance.CloseAllUI();
            GLOBALVALUE.CURRENT_MATCH_MODE = E_MatchMode.CampaignMatch;
            GLOBALVALUE.CURRENT_MAP_NAME = levelData.MapName;
            UIManager.Instance.ShowUI(GLOBALCONST.UI_MATCHMAKING);
        }
        else if (battleModeIndex == 2)
        {
            TowerModeLevelData levelData = PlayerDataManager.Instance.PlayerData.TowerLevelData.GetPlayerCurrentLevelData();
            if (levelData == null)
            {
                return;
            }
            UIManager.Instance.CloseAllUI();
            GLOBALVALUE.CURRENT_MATCH_MODE = E_MatchMode.TowerMatch;
            GLOBALVALUE.CURRENT_MAP_NAME = levelData.MapName;
            UIManager.Instance.ShowUI(GLOBALCONST.UI_MATCHMAKING);
        }
    }
    private void SendNamecallback(bool success)
    {
        RefreshPlayerInformation();
        _accountCreatePanel.SetActive(false);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_GACHA, GLOBALCONST.UI_MAIN_MENU, E_GachaType.Fixed);
    }

    void accountCreatConfirmBtn()
    {
        _accountName = accountNameInputField.text;
        if (_accountName.Length == 0)
        {
            accountCreateTextBoxOBJ.SetActive(false);
            BtnAccountCreatePanel.gameObject.SetActive(true);
            txtMessage.gameObject.SetActive(true);
            txtMessage.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ACCOUNT_CREATION_ENTER_NAME_MESSAGE);
            btnAccountNameIndex = 0;
        }
        else if (_accountName.Length <= 12)
        {
            accountCreateTextBoxOBJ.SetActive(false);
            BtnAccountCreatePanel.gameObject.SetActive(true);
            txtMessage.gameObject.SetActive(true);
            string accountNAMEVAL = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ACCOUNT_CREATOION_PLAYERNAME_INTRO_MESSAGE);
            txtMessage.text = string.Format(accountNAMEVAL, _accountName);
            btnAccountNameIndex = 1;
        }
        else
        {
            errormsgforAccountName.SetActive(true);
            errorMessage.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ACCOUNT_NAME_TOOLONG_ERROR_MESSAGE);
        }

    }
    void accountNameErrorMsgCloseBtn()
    {
        errormsgforAccountName.SetActive(false);
    }

    protected override void PlayerDataRefreshHandler()
    {
        base.PlayerDataRefreshHandler();
        RefreshPlayerInformation();
    }
    public void RefreshPlayerInformation()
    {
        ImgProfile.texture = GLOBALFUNCTION.GetPlayerIcon(PlayerDataManager.Instance.PlayerData.PlayerIconName);
        TxtPlayerName.text = PlayerDataManager.Instance.PlayerData.Name;
        string levelFormat = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.LEVEL_TEXT_STRING_ID);
        TxtPlayerLevelValue.text = string.Format(levelFormat, PlayerDataManager.Instance.PlayerData.Level);
    }
    IEnumerator delayLeftRightBtns()
    {
        btnLeft.interactable = false;
        btnRight.interactable = false;
        yield return new WaitForSeconds(0.3f);
        btnLeft.interactable = true;
        btnRight.interactable = true;
    }
    IEnumerator DelayPanel()
    {
        bgDisable.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        bgDisable.SetActive(false);
    }
    public void showPanel()
    {
        bgDisable.SetActive(true);
    }
    public void hidePanel()
    {
        StartCoroutine(DelayPanel());
    }
    void UpdateSelectedMode(int modeIndex)
    {
        foreach (Transform child in _selectedModePanel)
        {
            Destroy(child.gameObject);
        }
        modePanelControllerList = new List<ModePanelController>();
        for (int i = 0; i < Enum.GetNames(typeof(E_BattleMode)).Length; i++)
        {
            GameObject go = Instantiate(objModePanel);
            modePanelControllerList.Add(go.GetComponentInChildren<ModePanelController>());
            modePanelControllerList[i].index = i;
            go.transform.SetParent(_selectedModePanel.transform, false);
            go.SetActive(true);
        }
    }
    public void RefreshUI(int index)
    {
        switch (index)
        {
            case 0:
                //UI_PVP
                modePanelControllerList[0].objPVE_Panel.SetActive(false);
                modePanelControllerList[0].objPVP_Panel.SetActive(true);
                modePanelControllerList[0].txtPVPSeasonValue.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMAINMENU_LEVELNAME);

                modePanelControllerList[0].ImgBackgroundForMode.texture = GetModeTextureFromResource(ModeBg[0]);
                modePanelControllerList[0].ImgMode.texture = GetModeTextureFromResource(ModeIcon[0]);

                imgLSTOM.texture = GetModeTextureFromResource(ModeIcon_Button[2]); ;
                imgRSTOM.texture = GetModeTextureFromResource(ModeIcon_Button[1]);
                imgRSTOM.rectTransform.localPosition = new Vector3(733, 25, 0);
                imgLSTOM.rectTransform.localPosition = new Vector3(-733, 25, 0);

                break;
            case 1:
                //UI_PVE
                modePanelControllerList[1].objPVE_Panel.SetActive(true);
                modePanelControllerList[1].objPVP_Panel.SetActive(false);
                modePanelControllerList[1].txtChapterNumber.text = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData().GetActString();

                modePanelControllerList[1].txtChapterName.text = PlayerDataManager.Instance.PlayerData.CampaignLevelData.GetPlayerCurrentLevelData().LevelNameString();
                modePanelControllerList[1].ImgBackgroundForMode.texture = GetModeTextureFromResource(ModeBg[0]);
                modePanelControllerList[1].ImgMode.texture = GetModeTextureFromResource(ModeIcon[1]);


                imgRSTOM.texture = GetModeTextureFromResource(ModeIcon_Button[0]);
                imgLSTOM.texture = GetModeTextureFromResource(ModeIcon_Button[2]);
                imgRSTOM.rectTransform.localPosition = new Vector3(-800, 25, 0);
                imgLSTOM.rectTransform.localPosition = new Vector3(800, 25, 0);
                break;
            case 2:
                //UI_School;
                modePanelControllerList[2].objPVE_Panel.SetActive(false);
                modePanelControllerList[2].objPVP_Panel.SetActive(false);


                modePanelControllerList[2].ImgBackgroundForMode.texture = GetModeTextureFromResource(ModeBg[0]);
                modePanelControllerList[2].ImgMode.texture = GetModeTextureFromResource(ModeIcon[2]);
                modePanelControllerList[2].txtChapterNumber.gameObject.SetActive(false);

                imgRSTOM.texture = GetModeTextureFromResource(ModeIcon_Button[1]);
                imgLSTOM.texture = GetModeTextureFromResource(ModeIcon_Button[0]);
                imgRSTOM.rectTransform.localPosition = new Vector3(-800, 25, 0);
                imgLSTOM.rectTransform.localPosition = new Vector3(733, 25, 0);
                break;
        }
    }
    public Texture GetBGTextureFromResource(string playerImageName)
    {
        Texture texture = Resources.Load<Texture>("Image/Background/" + playerImageName);
        return texture;
    }
    public Texture GetModeTextureFromResource(string playerImageName)
    {
        Texture texture = Resources.Load<Texture>("Image/New Images/" + playerImageName);
        return texture;
    }
    public Texture GetModeWordTextureFromResource(string playerImageName)
    {
        Texture texture = Resources.Load<Texture>("Image/ImageWord/" + playerImageName);
        return texture;
    }
}
