using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Player;

public class UIGacha : UIBase
{
    public Button BtnClose;
    public Button BtnPanelClose;
    public Button BtnOneDraw;
    public Button BtnTenDraw;
    public Button BtnChangeBuyResource;
    public Sprite btnfreebtnOneDraw, btnPaidbtnOneDraw, btnswitchfree, btnswitchpaid;
    public Image paidImgOneDraw, freeImgOneDraw, paidIconOneDraw, freeIconOneDraw;
    public Image paidImgTenDraw, freeImgTenDraw, paidIconTenDraw, freeIconTenDraw;
    public bool IsBuyResourceBambooFragment;
    public Text txtGaha, txtOneDraw, txtTenDraw, txtSwitchFreeorDraw;
    public GameObject GacahBuyPopup;
    public GameObject GachaReward;
    public GameObject[] CardList;
    public GameObject VideoObject;
    public Image Mask;
    private string BackUIName;

    private List<int> m_FixedGachaIDs = new List<int>() { 1, 2, 3, 4, 5 };
    // private bool m_ToGiveFixedGacha;
    private bool m_FromOtherUI = true;

    private const int BAMBOO_ROLL_1X = 100;
    private const int BAMBOO_ROLL_10X = 1000;
    private const int BAMBOO_FRAGMENT_1X = 500;
    private const int BAMBOO_FRAGMENT_10X = 5000;
    //Grayscale
    public Shader GrayScale;
    public Material _gsMaterial;

    private E_GachaType m_GachaType = E_GachaType.None;

    protected override void OnInit()
    {
        base.OnInit();
        _gsMaterial = new Material(GrayScale);
        BtnClose.onClick.AddListener(CloseUI);
        BtnPanelClose.onClick.AddListener(OnClickRewardPanel);
        BtnOneDraw.onClick.AddListener(Buy1X);
        BtnTenDraw.onClick.AddListener(Buy10X);
        //BtnTenDraw.onClick.AddListener(ComingSoonMsg);
        BtnChangeBuyResource.onClick.AddListener(ChangeResourceToBuy);
    }
    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        BackUIName = (string)Objects[0];
        m_GachaType = (E_GachaType)Objects[1];
        if (m_GachaType == E_GachaType.Fixed || m_GachaType == E_GachaType.BattleLose || m_GachaType == E_GachaType.BattleWin)
        {
            VideoObject.SetActive(true);
            List<PlayerHeroData> rewards = GLOBALFUNCTION.GACHA.GetRewardPlayerHero(m_GachaType);
            ShowReward(rewards);
            if (m_GachaType == E_GachaType.Fixed)
            {
                PlayerDataManager.Instance.PlayerData.SavedArmyFormation = new List<ArmyBattleFormation>();
                //CampaignLevelData data = new CampaignLevelData();
                PlayerDataManager.Instance.PlayerData.CampaignLevelData = new CampaignLevelData();
                PlayerDataManager.Instance.PlayerData.CampaignLevelData.PlayerCurrentLevelID = 1;
                PlayerDataManager.Instance.PlayerData.CampaignLevelData.PlayerCampaignFormation = new List<ArmyBattleFormation>();

                PlayerDataManager.Instance.PlayerData.TowerLevelData = new TowerLevelData();
                PlayerDataManager.Instance.PlayerData.TowerLevelData.PlayerCurrentLevelID = 1;
                PlayerDataManager.Instance.PlayerData.TowerLevelData.PlayerTowerModeFormation = new List<ArmyBattleFormation>();

                for (int i = 0; i < rewards.Count; i++)
                {
                    ArmyBattleFormation formation = new ArmyBattleFormation
                    {
                        PlayerHeroID = rewards[i].key(),
                        GridX = i / 2,
                        GridY = ((i % 2) == 0 ? 1 : 2)

                    };

                    PlayerDataManager.Instance.PlayerData.SavedArmyFormation.Add(formation);
                    PlayerDataManager.Instance.PlayerData.CampaignLevelData.PlayerCampaignFormation.Add(formation);
                    PlayerDataManager.Instance.PlayerData.TowerLevelData.PlayerTowerModeFormation.Add(formation);
                }
                //PlayerDataManager.Instance.SavePlayerData();
            }
        }

        Mask.color = Color.clear;
        txtGaha.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_GACHA);
        txtOneDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_DRAWONE_BTN_PAID);
        txtTenDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_DRAWTEN_BTN_PAID);
        txtSwitchFreeorDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_BTN_SWITCH_PAID);
        IsBuyResourceBambooFragment = false;
        if (UIManager.UIInstance<UITopBar>() != null)
        {
            UIManager.UIInstance<UITopBar>().RefreshData();
        }
        RefreshUI();
    }

    public void ShowReward(List<PlayerHeroData> Rewards)
    {
        SoundManager.Instance.StopBGM();
        GacahBuyPopup.SetActive(false);
        GachaReward.SetActive(true);
        int Count = Rewards.Count;

        for (int i = 0; i < CardList.Length; i++)
        {
            if (i < Count)
            {
                CardList[i].SetActive(true);
                CardList[i].GetComponent<UICard>().RefreshData(Rewards[i]);
            }
            else
            {
                CardList[i].SetActive(false);
            }

        }
    }

    //1 time
    private void Buy1X()
    {
        m_FromOtherUI = false;
        List<PlayerHeroData> rewards = new List<PlayerHeroData>();
        if (IsBuyResourceBambooFragment)
        {
            if (PlayerDataManager.Instance.PlayerData.BambooFragment >= BAMBOO_FRAGMENT_1X)
            {
                PlayerDataManager.Instance.PlayerData.BambooFragment -= BAMBOO_FRAGMENT_1X;
                rewards = GLOBALFUNCTION.GACHA.GetRewardPlayerHero(E_GachaType.BambooFragment1X);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (PlayerDataManager.Instance.PlayerData.BambooRoll >= BAMBOO_ROLL_1X)
            {
                PlayerDataManager.Instance.PlayerData.BambooRoll -= BAMBOO_ROLL_1X;
                rewards = GLOBALFUNCTION.GACHA.GetRewardPlayerHero(E_GachaType.BambooRoll1X);
            }
            else
            {
                return;
            }
        }
        // VideoObject.SetActive(true);
        ShowReward(rewards);
        PlayerDataManager.Instance.SavePlayerData();
    }
    //10 time
    private void ComingSoonMsg()
    {
        UIMessageBox.ShowMessageBox("Coming Soon!", E_MessageBox.Yes, CloseMessageCallback);
    }
    private void Buy10X()
    {
        m_FromOtherUI = false;
        if (IsBuyResourceBambooFragment)
        {
            if (PlayerDataManager.Instance.PlayerData.BambooFragment >= BAMBOO_FRAGMENT_10X)
            {
                //PlayerDataManager.Instance.PlayerData.BambooFragment -= BAMBOO_FRAGMENT_10X;
                //PlayerDataManager.Instance.SavePlayerData();
                byte kind = 1;
                // CNetManager.Instance.C2GS_12_2(kind);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (PlayerDataManager.Instance.PlayerData.BambooRoll >= BAMBOO_ROLL_10X)
            {
                //PlayerDataManager.Instance.PlayerData.BambooRoll -= BAMBOO_ROLL_10X;
                //PlayerDataManager.Instance.SavePlayerData();
                byte kind = 2;
                //CNetManager.Instance.C2GS_12_2(kind);
            }
            else
            {
                return;
            }
        }
        //VideoObject.SetActive(true);
        //RefreshUI();
        //TopBar.RefreshData();
        //ShowReward(5);
    }
    public void CloseMessageCallback(bool sure, object[] param)
    {
        Debug.Log("YesMessageCallbackCalled.");
    }
    private void CloseBuyPanel()
    {
        GacahBuyPopup.SetActive(false);
    }
    private void OnClickRewardPanel()
    {
        if (m_FromOtherUI)
        {
            CloseUI();
        }
        else
        {
            GachaReward.SetActive(false);
            GacahBuyPopup.SetActive(true);
        }
    }

    private void CloseUI()
    {
        m_FromOtherUI = true;
        GachaReward.SetActive(false);
        GacahBuyPopup.SetActive(true);
        UIManager.Instance.CloseUI(GLOBALCONST.UI_GACHA);
        if (BackUIName != "")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            UIManager.Instance.ShowUI(BackUIName);
            BackUIName = "";
        }
        else
        {
            SoundManager.Instance.PlayBGM("BGM00001");
        }
    }
    protected override void OnUpdate()
    {
        Mask.color = Color.Lerp(Mask.color, Color.clear, Time.deltaTime);
        base.OnUpdate();
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (UIManager.UIInstance<UITopBar>() != null)
        {
            UIManager.UIInstance<UITopBar>().RefreshData();
        }
        if (IsBuyResourceBambooFragment)
        {
            //do bamboo fragment .Gold color
            BtnOneDraw.interactable = PlayerDataManager.Instance.PlayerData.BambooFragment >= BAMBOO_FRAGMENT_1X;
            // BtnTenDraw.interactable = PlayerDataManager.Instance.PlayerData.BambooFragment >= BAMBOO_FRAGMENT_10X;
            BtnTenDraw.interactable = false;

            txtOneDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_DRAWONE_BTN_FREE);
            txtTenDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_DRAWTEN_BTN_FREE);
            txtSwitchFreeorDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_BTN_SWITCH_FREE);
            BtnChangeBuyResource.image.sprite = btnswitchfree;
            BtnOneDraw.image.sprite = btnfreebtnOneDraw;
            BtnTenDraw.image.sprite = btnfreebtnOneDraw;

            freeImgOneDraw.gameObject.SetActive(true);
            freeIconOneDraw.gameObject.SetActive(true);
            freeImgTenDraw.gameObject.SetActive(true);
            freeIconTenDraw.gameObject.SetActive(true);

            paidImgOneDraw.gameObject.SetActive(false);
            paidIconOneDraw.gameObject.SetActive(false);
            paidImgTenDraw.gameObject.SetActive(false);
            paidIconTenDraw.gameObject.SetActive(false);



            if (BtnOneDraw.interactable == false)
            {
                freeImgOneDraw.material = _gsMaterial;
                freeIconOneDraw.material = _gsMaterial;
                BtnOneDraw.image.material = _gsMaterial;
                _gsMaterial.SetFloat("_EffectAmount", 1);
            }
            else if (BtnOneDraw.interactable == true)
            {
                Material _myMaterial = new Material(GrayScale);
                freeImgOneDraw.material = _myMaterial;
                freeIconOneDraw.material = _myMaterial;
                BtnOneDraw.image.material = _myMaterial;
                _myMaterial.SetFloat("_EffectAmount", 0);
            }
            if (BtnTenDraw.interactable == false)
            {
                // freeImgTenDraw.material = _gsMaterial;
                freeIconTenDraw.material = _gsMaterial;
                BtnTenDraw.image.material = _gsMaterial;
                _gsMaterial.SetFloat("_EffectAmount", 1);
            }
            else if (BtnTenDraw.interactable == true)
            {
                Material _myMaterial = new Material(GrayScale);
                freeImgTenDraw.material = _myMaterial;
                freeIconTenDraw.material = _myMaterial;
                BtnTenDraw.image.material = _myMaterial;
                _myMaterial.SetFloat("_EffectAmount", 0);
            }
        }
        else
        {
            //do blue color bamboo roll
            BtnOneDraw.interactable = PlayerDataManager.Instance.PlayerData.BambooRoll >= BAMBOO_ROLL_1X;
            // BtnTenDraw.interactable = PlayerDataManager.Instance.PlayerData.BambooRoll >= BAMBOO_ROLL_10X;
            BtnTenDraw.interactable = false;

            txtOneDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_DRAWONE_BTN_PAID);
            txtTenDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_DRAWTEN_BTN_PAID);
            txtSwitchFreeorDraw.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIGACHA_BTN_SWITCH_PAID);
            BtnChangeBuyResource.image.sprite = btnswitchpaid;
            BtnOneDraw.image.sprite = btnPaidbtnOneDraw;
            BtnTenDraw.image.sprite = btnPaidbtnOneDraw;

            freeImgOneDraw.gameObject.SetActive(false);
            freeIconOneDraw.gameObject.SetActive(false);
            freeImgTenDraw.gameObject.SetActive(false);
            freeIconTenDraw.gameObject.SetActive(false);

            paidImgOneDraw.gameObject.SetActive(true);
            paidIconOneDraw.gameObject.SetActive(true);
            paidImgTenDraw.gameObject.SetActive(true);
            paidIconTenDraw.gameObject.SetActive(true);
            if (BtnOneDraw.interactable == false)
            {
                paidImgOneDraw.material = _gsMaterial;
                paidIconOneDraw.material = _gsMaterial;
                BtnOneDraw.image.material = _gsMaterial;
                _gsMaterial.SetFloat("_EffectAmount", 1);
            }
            else if (BtnOneDraw.interactable == true)
            {
                Material _myMaterial = new Material(GrayScale);
                paidImgOneDraw.material = _myMaterial;
                paidIconOneDraw.material = _myMaterial;
                BtnOneDraw.image.material = _myMaterial;
                _myMaterial.SetFloat("_EffectAmount", 0);
            }
            if (BtnTenDraw.interactable == false)
            {
                // paidImgTenDraw.material = _gsMaterial;
                paidIconTenDraw.material = _gsMaterial;
                BtnTenDraw.image.material = _gsMaterial;
                _gsMaterial.SetFloat("_EffectAmount", 1);
            }
            else if (BtnTenDraw.interactable == true)
            {
                Material _myMaterial = new Material(GrayScale);
                paidImgTenDraw.material = _myMaterial;
                paidIconTenDraw.material = _myMaterial;
                BtnTenDraw.image.material = _myMaterial;
                _myMaterial.SetFloat("_EffectAmount", 0);
            }
        }
    }
    void ChangeResourceToBuy()
    {
        IsBuyResourceBambooFragment = !IsBuyResourceBambooFragment;
        RefreshUI();
    }
    protected override void PlayerDataRefreshHandler()
    {
        base.PlayerDataRefreshHandler();
        RefreshUI();
    }
}
