using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChooseProfile : UIBase
{
    public Text TxtTitle;
    public Text txtPlayerName;
    public Text txtPlayerLevel;
    public Text txtPlayerId;
    public Text txtPlayerNameValue;
    public Text txtPlayerLevelValue;
    public Text txtPlayerIdValue;
    public Text txtCancel_MainProfile;
    public Text txtConfirm_MainProfile;
    //public Text TxtConfirm;
    //public Text txtCacnel;
    //public Text txtConfirm_ProfileIcon;
    //public Text txtCancel_ProfileIcon;

    public Button BtnConfirm_ProfileIconChoose;
    public Button btnConfirm_MainParofile;
    public Button btnCancel_ProfileIcon;
    public Button btnCancel_MainProfile;
    public Button btnChangeProfileIcon;
    public Button btnEdit;
    public Button btnClose;

    public RawImage imgBlackCircle;
    public RawImage imgIcon;

    public GameObject PrefIconItem;
    public GameObject objProfileIcon_Panel;
    public GameObject objMainProfile_Panel;

    public ToggleGroup TGPIcon;

    public InputField inputField_PlayerName;

	private List<string> m_IconNames = new List<string>()
	{
		"H47","H46","H41","H42","H22",
		"H56","H53","H45","H43","H37"
	};
    public RawImage MainProfileIcon;

    public List<ItemProfileIcon> itemProfileList = new List<ItemProfileIcon>();

    public int itemProfileIndex;
    protected override void OnInit()
    {
        base.OnInit();

        TxtTitle.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHOOSE_PROEILE_TITLE);
        txtPlayerName.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHOOSE_PROFILE_PLAYER_NAME);
        txtPlayerLevel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHOOSE_PROFILE_PLAYER_LEVEL);
        txtPlayerId.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UICHOOSE_PROFILE_PLAYER_ID);
        txtConfirm_MainProfile.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);
        txtCancel_MainProfile.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_CANCELBTN);

        //txtConfirm_ProfileIcon.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);
        //TxtConfirm.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_STRING_ID);
        //txtCacnel.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CANECL_STRING_ID);
        //txtCancel_ProfileIcon.text= TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_CANCELBTN);


        PrefIconItem.gameObject.SetActive(false);
        objProfileIcon_Panel.SetActive(false);
        objMainProfile_Panel.SetActive(true);
        btnCancel_MainProfile.gameObject.SetActive(false);
        btnConfirm_MainParofile.gameObject.SetActive(false);
        inputField_PlayerName.gameObject.SetActive(false);
        //btnChangeProfileIcon.interactable = false;
       


        btnClose.onClick.AddListener(ClosePanel);
        btnCancel_MainProfile.onClick.AddListener(CancelPanel);
        btnCancel_ProfileIcon.onClick.AddListener(CancelPanel);
        btnChangeProfileIcon.onClick.AddListener(ShowProfileIconChoosePanel);
        btnEdit.onClick.AddListener(EditProfile);
        btnConfirm_MainParofile.onClick.AddListener(ConfirmEdit);
        BtnConfirm_ProfileIconChoose.onClick.AddListener(ConfirmChange);
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        imgBlackCircle.color = new Color(255, 255, 0, 255);


        MainProfileIcon.texture= GLOBALFUNCTION.GetPlayerIcon(PlayerDataManager.Instance.PlayerData.PlayerIconName);
        txtPlayerNameValue.text = PlayerDataManager.Instance.PlayerData.Name.ToString();
        txtPlayerLevelValue.text = PlayerDataManager.Instance.PlayerData.Level.ToString();
    }
    private void ShowProfileIconChoosePanel()
    {
        objProfileIcon_Panel.SetActive(true);
        objMainProfile_Panel.SetActive(false);
        RefreshProfileIcon();
    }
    private void EditProfile()
    {
        if (inputField_PlayerName.gameObject.activeSelf)
        {
            imgBlackCircle.color = new Color(255, 255, 0, 255);
            btnChangeProfileIcon.interactable = true;
            btnCancel_MainProfile.gameObject.SetActive(false);
            btnConfirm_MainParofile.gameObject.SetActive(false);
            inputField_PlayerName.gameObject.SetActive(false);
        }
        else
        {
            imgBlackCircle.color = new Color(255, 255, 255, 255);
            btnChangeProfileIcon.interactable = false;
            btnCancel_MainProfile.gameObject.SetActive(true);
            btnConfirm_MainParofile.gameObject.SetActive(true);
            inputField_PlayerName.gameObject.SetActive(true);
        }
    }
    private void RefreshProfileIcon()
    {
        itemProfileList = new List<ItemProfileIcon>();
        foreach (Transform child in TGPIcon.transform)
        {
            Destroy(child.gameObject);
        }
		PrefIconItem.SetActive(false);
		for (int i = 0; i < m_IconNames.Count; i++)
        {
			GameObject go = Instantiate(PrefIconItem);
			go.SetActive(true);
			go.transform.SetParent(TGPIcon.transform,false);
            ItemProfileIcon itemProfile = go.GetComponent<ItemProfileIcon>();
			itemProfile.IconName = m_IconNames[i];
			itemProfile.IconProfile.texture = Resources.Load<Texture>("Image/Sprite/" + m_IconNames[i]);
            itemProfileList.Add(itemProfile);
            itemProfileList[i].index = i;
		}
	}
    private void CancelPanel()
    {
        if (objProfileIcon_Panel.activeSelf)
        {
            objProfileIcon_Panel.SetActive(false);
            objMainProfile_Panel.SetActive(true);
        }
        else if (objMainProfile_Panel.activeSelf)
        {
            btnCancel_MainProfile.gameObject.SetActive(false);
            btnConfirm_MainParofile.gameObject.SetActive(false);
            btnChangeProfileIcon.interactable = true;
            inputField_PlayerName.gameObject.SetActive(false);
            imgBlackCircle.color = new Color(255, 255, 0, 255);
        }
    }
    private void ClosePanel()
    {
        if (objProfileIcon_Panel.activeSelf)
        {
            objProfileIcon_Panel.SetActive(false);
            objMainProfile_Panel.SetActive(true);
        }
        else if(objMainProfile_Panel.activeSelf)
        {
            btnChangeProfileIcon.interactable = transform;
            inputField_PlayerName.gameObject.SetActive(false);
            imgBlackCircle.color = new Color(255, 255, 0, 255);
            UIManager.Instance.CloseUI(GLOBALCONST.UI_CHOOSE_PROFILE);
        }
    }
    private void ConfirmChange()
    {
        PlayerDataManager.Instance.PlayerData.PlayerIconName = itemProfileList[itemProfileIndex].IconName;
        imgIcon.texture= GLOBALFUNCTION.GetPlayerIcon(PlayerDataManager.Instance.PlayerData.PlayerIconName);
        UIManager.UIInstance<UIMainMenu>().RefreshPlayerInformation();
        PlayerDataManager.Instance.SavePlayerData();
        ClosePanel();
    }
    private void ConfirmEdit()
    {
        if(inputField_PlayerName.text.Length==0)
        {
            btnCancel_MainProfile.gameObject.SetActive(false);
            btnConfirm_MainParofile.gameObject.SetActive(false);
            btnChangeProfileIcon.interactable = true;
            inputField_PlayerName.gameObject.SetActive(false);
            imgBlackCircle.color = new Color(255, 255, 0, 255);
        }
         if (inputField_PlayerName.text.Length <= 12 && inputField_PlayerName.text.Length > 0)
        {
            btnCancel_MainProfile.gameObject.SetActive(false);
            btnConfirm_MainParofile.gameObject.SetActive(false);
            inputField_PlayerName.gameObject.SetActive(false);
            txtPlayerNameValue.text = inputField_PlayerName.text;
            PlayerDataManager.Instance.PlayerData.Name = inputField_PlayerName.text;
            UIManager.UIInstance<UIMainMenu>().RefreshPlayerInformation();
            PlayerDataManager.Instance.SavePlayerData();
            //ClosePanel();
        }
    }
}
