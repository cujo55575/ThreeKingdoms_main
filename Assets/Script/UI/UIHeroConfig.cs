using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHeroConfig : UIBase, IPointerClickHandler
{
    public Button BtnBack, BtncloseUserInfoPanel, BtnUpgrade, Btnleft, Btnright;
    public Text heroName, txtUpgrade, txtHeroRank, txtTitleBar, txtBasicMsg, txtUpgradeMsg;
    public GameObject upgradeInfoPanel;
    public UICard UiCard;
    private PlayerHeroData CardData;
    public List<PlayerHeroData> _playerHeroData;
    public SkillInfoPanelController _skillInfoPanelController;
    private upgradeInfoPanel _parentCtrl;
    public int heroIndex;
    private HeroData heroData;
    public Texture EnableTexture;
    public Texture DisableTexture;
    public RawImage[] UpgradedStars;
    public RawImage upgradeInfoImage;
    private CardCountData m_CardCountData;

    //Grayscale
    public Shader grayscale;
    public Material matBtnUpgrade;
    private enum E_Direction
    {
        Left, Right
    }
    protected override void OnInit()
    {
        base.OnInit();
        _playerHeroData.Clear();
        BtnBack.onClick.AddListener(CloseUI);
        Btnleft.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Left);
        });
        Btnright.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Right);
        });

        matBtnUpgrade = new Material(grayscale);
        BtnUpgrade.image.material = matBtnUpgrade;

        BtnUpgrade.onClick.AddListener(openUserInfoPanel);


        BtncloseUserInfoPanel.onClick.AddListener(closeUserInfoPanel);
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        CardData = (PlayerHeroData)Objects[0];


        txtUpgrade.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UPGRADE_BTN);
        txtHeroRank.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_HERORANK);
        txtTitleBar.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TITLE_BAR);
        txtBasicMsg.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_BASICMESSAGE);

        _playerHeroData = (List<PlayerHeroData>)Objects[1];
        heroIndex = _playerHeroData.IndexOf(CardData);
        UiCard.RefreshData(CardData);
        _skillInfoPanelController.RefreshSkillItem(CardData);
        _skillInfoPanelController.RefreshStatus(CardData);
        heroName.text = _playerHeroData[heroIndex].GetHeroName();
        RefreshUpgradePanel();
    }
    public void RefreshUpgradePanel()
    {
        BtnUpgrade.interactable = true;
        matBtnUpgrade.SetFloat("_EffectAmount", 0);
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        if (CardData.FragmentCount >= m_CardCountData.GetCostByLevel(CardData.HeroLevel))
        {
            BtnUpgrade.interactable = true;
            matBtnUpgrade.SetFloat("_EffectAmount", 0);
        }
        else
        {
            BtnUpgrade.interactable = false;
            matBtnUpgrade.SetFloat("_EffectAmount", 1);
        }
        if (CardData.HeroLevel == 6)
        {
            BtnUpgrade.interactable = false;
            matBtnUpgrade.SetFloat("_EffectAmount", 1);
        }
    }
    void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_HeroConfig);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK, _playerHeroData[heroIndex]);
		//PlayerDataManager.Instance.SavePlayerData();
	}
    void closeUserInfoPanel()
    {
        upgradeInfoPanel.SetActive(false);
        GameObject _invisibleOBJ = GameObject.Find("InvisibleBG");
        _invisibleOBJ.SetActive(false);
    }
    void openUserInfoPanel()
    {
        upgradeInfoPanel.SetActive(true);
        txtUpgradeMsg.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UPGRADE_INFO);
        heroData = _playerHeroData[heroIndex].GetHeroData();
        Texture icon = heroData.GetHeroTexture();
        upgradeInfoImage.texture = icon;
        FillStar(UpgradedStars, _playerHeroData[heroIndex].HeroLevel);
        UpgradeConfirm(_playerHeroData[heroIndex]);
    }
    public void UpgradeConfirm(PlayerHeroData pData)
    {
        pData.FragmentCount -= m_CardCountData.GetCostByLevel(pData.HeroLevel);
        pData.HeroLevel += 1;
        RefreshUpgradePanel();
        UiCard.RefreshData(pData);
        _skillInfoPanelController.RefreshSkillItem(pData);
    }
    private void FillStar(RawImage[] starList, int starCount)
    {
        for (int i = 0; i < starList.Length; i++)
        {
            if ((i + 1) <= starCount)
            {
                starList[i].texture = EnableTexture;
            }
            else
            {
                starList[i].texture = DisableTexture;
            }
        }
    }
    public void setParentCtrl(upgradeInfoPanel ctrl)
    {
        _parentCtrl = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_parentCtrl != null)
            _parentCtrl.hide();
    }
    private void OnClickChangeSelectedHero(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Left:
                heroIndex--;
                break;
            case E_Direction.Right:
                heroIndex++;
                break;
        }
        if (heroIndex >= _playerHeroData.Count)
        {
            heroIndex = 0;
        }
        else if (heroIndex < 0)
        {
            heroIndex = _playerHeroData.Count - 1;
        }
        UiCard.RefreshData(_playerHeroData[heroIndex]);
        // _skillInfoPanelController.selectSkillIndex.Clear();
        _skillInfoPanelController.RefreshSkillItem(_playerHeroData[heroIndex]);
        _skillInfoPanelController.RefreshStatus(_playerHeroData[heroIndex]);
        heroName.text = _playerHeroData[heroIndex].GetHeroName();
        CardData = _playerHeroData[heroIndex];
        RefreshUpgradePanel();
    }

}
