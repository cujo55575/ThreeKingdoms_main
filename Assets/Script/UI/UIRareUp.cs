using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRareUp : UIBase
{
    public Button BtnClose;
    public Button BtnRareUp;

    public Text TxtGetSkill;
    public Text TxtSkillName;
    public Text TxtCardNameTitle;
    public Text TxtCardName;
    public Text TxtCardAmount;
    public Text txtGetSKill;
    public Text txtConfirmPOPUPBUTTON;

    public RawImage ImgSkill;
    public RawImage ImgCard;
    public RawImage[] CurrentStars;
    public RawImage[] UpgradedStars;

    public UICard CardUI;
    private PlayerHeroData CardData;

    public Texture EnableTexture;
    public Texture DisableTexture;

    public GameObject GetSkill;
    public GameObject MaxSkill;

    public UIRareUpConfirmPanelController ConfrimPanelControl;
    private CardCountData m_CardCountData;


    protected override void OnInit()
    {
        base.OnInit();
        BtnClose.onClick.AddListener(CloseUI);
        BtnRareUp.onClick.AddListener(RareUp);
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        CardData = (PlayerHeroData)Objects[0];

        RefreshRareUp();
        txtGetSKill.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIARMY_GETSKILL);
    }
    public void RefreshRareUp()
    {
        BtnRareUp.interactable = true;

        CardUI.RefreshData(CardData);

        FillStar(CurrentStars, CardData.HeroLevel - 1);
        FillStar(UpgradedStars, CardData.HeroLevel);

        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);

        TxtCardAmount.text = "X " + CardData.FragmentCount.ToString() + "/" + m_CardCountData.GetCostByLevel(CardData.HeroLevel);

        TxtCardNameTitle.text = CardData.GetHeroName();
        TxtCardName.text = CardData.GetHeroName();
        ImgCard.texture = CardData.GetHeroData().GetHeroTexture();
        if (CardData.HeroLevel < 6)
        {
            SkillData skill = CardData.GetNextSkill(CardData.HeroLevel);
            RefreshSkillInfo(skill);
            GetSkill.SetActive(true);
            MaxSkill.SetActive(false);
        }
        else
        {
            GetSkill.SetActive(false);
            MaxSkill.SetActive(true);
        }

        if (CardData.FragmentCount >= m_CardCountData.GetCostByLevel(CardData.HeroLevel))
        {
            BtnRareUp.interactable = true;
            TxtCardAmount.color = Color.white;
        }
        else
        {
            BtnRareUp.interactable = false;
            TxtCardAmount.color = Color.red;
        }
        if (CardData.HeroLevel == 6)
        {
            BtnRareUp.interactable = false;
        }
    }
    private void RefreshSkillInfo(SkillData skill)
    {
        ImgSkill.texture = skill.GetSkillTexture();
        TxtSkillName.text = skill.GetSkillName();
    }

    private void RareUp()
    {
        ConfrimPanelControl.ShowConfrimPanel();
        txtConfirmPOPUPBUTTON.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);
        //UIMessageBox.ShowMessageBox("Coming Soon!",E_MessageBox.Yes,CloseMessageCallback);
    }
    public void CloseMessageCallback(bool sure, object[] param)
    {
        Debug.Log("YesMessageCallbackCalled.");
        CloseUI();
    }

    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_RAREUP);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK, CardData);
        //PlayerDataManager.Instance.SavePlayerData();
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

    public void UpgradeConfirm()
    {
        CardData.FragmentCount -= m_CardCountData.GetCostByLevel(CardData.HeroLevel);
        CardData.HeroLevel += 1;
        RefreshRareUp();
    }
}
