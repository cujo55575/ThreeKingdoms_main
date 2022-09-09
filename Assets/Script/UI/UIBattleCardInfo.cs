using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleCardInfo : UIBase
{
    public UICard card;
    public BattleCardMono bcm;
    public Text txtCombatPower;

    public List<SkillItem> skills;

    public GameObject skillSwitchPanel;
    public Button SkillReplaceButtom;

    public GameObject armySwitchPanel;
    public Button btnArmyReplace;

	public Text txtReplaceArmy;
	public Text txtReplaceSkill;

	public Text txtArmy;
	public Text txtSkill;
    protected override void OnInit()
    {
        base.OnInit();
        SkillReplaceButtom.onClick.AddListener(OnClickSkillReplace);
        btnArmyReplace.onClick.AddListener(OnClickCurrentArmyReplace);

		txtReplaceArmy.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.REPLACE_TEXT);
		txtReplaceSkill.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.REPLACE_TEXT);
		txtArmy.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CURRENT_ARMY_TEXT);
		txtSkill.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.HEROSKILL_TEXT);
    }
    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        if(bcm!=null)
        {
            bcm.SelectedImage.gameObject.SetActive(false);
        }
        bcm = (BattleCardMono)Objects[0];
        card.RefreshData(bcm.data);
        bcm.SelectedImage.SetActive(true);

        txtCombatPower.text= bcm.data.GetCombatPowerSelf().ToString();
        skillSwitchPanel.SetActive(false);
        armySwitchPanel.SetActive(false);
        RefreshSkill();
        RefreshArmy();
    }
    public void ShowData(BattleCardMono _bcm)
    {
        if (skillSwitchPanel.activeSelf)
        {
            //skillSwitchPanel.GetComponent<SkillSwitchPanel>().AssignValue();
        }
        if (bcm != null)
        {
            bcm.SelectedImage.gameObject.SetActive(false);
        }
        if(bcm==_bcm)
        {
            UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLECARDINFO);
            return;
        }
        bcm = _bcm;
        bcm.SelectedImage.SetActive(true);
        card.RefreshData(bcm.data);
        txtCombatPower.text = bcm.data.GetCombatPowerSelf().ToString();
        //skillSwitchPanel.SetActive(false);
        //armySwitchPanel.SetActive(false);
        RefreshSkill();
        RefreshArmy();
        armySwitchPanel.GetComponent<ArmySwitchPanel>().RefreshArmies();
        skillSwitchPanel.GetComponent<SkillSwitchPanel>().RefreshSkill();
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (skillSwitchPanel.activeSelf)
        {
            //skillSwitchPanel.GetComponent<SkillSwitchPanel>().AssignValue();
        }
        if (bcm != null)
        {
            bcm.SelectedImage.gameObject.SetActive(false);
            bcm = null;
        }
    }
    public void OnClickSkillReplace()
    {
        if(skillSwitchPanel.activeSelf)
        {
            //skillSwitchPanel.GetComponent<SkillSwitchPanel>().AssignValue();
            RefreshSkill();
            skillSwitchPanel.gameObject.SetActive(false);
        }
        else
        {
            skillSwitchPanel.gameObject.SetActive(true);
        }
        if(armySwitchPanel.activeSelf)
        {
            armySwitchPanel.SetActive(false);
        }
    }
    public void OnClickCurrentArmyReplace()
    {
        if (skillSwitchPanel.activeSelf)
        {
            //skillSwitchPanel.GetComponent<SkillSwitchPanel>().AssignValue();
            RefreshSkill();
            skillSwitchPanel.gameObject.SetActive(false);
        }

        if (armySwitchPanel.activeSelf)
        {
            armySwitchPanel.gameObject.SetActive(false);
        }
        else
        {
            armySwitchPanel.gameObject.SetActive(true);
        }
    }
    public void RefreshSkill()
    {
        List<SkillData> _skills=bcm.data.GetEquippedSkills();
        for(int i=0;i<skills.Count;i++)
        {
            if(i<_skills.Count)
            {
                skills[i].RefreshData(_skills[i]);
            }
            else
            {
                skills[i].RefreshData(null);
            }
        }
    }
    public Text txtCurrentArmyTitle;
    public Text txtCurrentArmyLevel;
    public RawImage imgArmyTexture;
    public RawImage imgJobIcon;
    public RawImage imgTitleIcon;

    public List<Sprite> jobIconSprites;
    public List<Sprite> jobTitleSprites;
    public void RefreshArmy()
    {
        //int jobType = (int)bcm.data.GetEquippedArmy().GetArmyData().ArmyJobType;
		imgJobIcon.texture = bcm.data.GetEquippedArmySelf().GetArmyData().GetArmyJobTypeTexture();
		imgTitleIcon.texture = bcm.data.GetEquippedArmySelf().GetArmyData().GetArmyJobTypeTitleTexture();


		imgArmyTexture.texture = bcm.data.GetEquippedArmySelf().GetArmyData().GetArmyTexture();
        txtCurrentArmyTitle.text = bcm.data.GetEquippedArmySelf().GetArmyData().GetArmyName();
        txtCurrentArmyLevel.text = string.Format("Lv.{0}",bcm.data.GetEquippedArmySelf().ArmyLevel);     
    }
    public void SwitchModel(HeroArmyData heroData)
    {
        GameObject.FindObjectOfType<HeroPlacementManager>().SwitchModel(bcm.data,heroData,bcm);

        armySwitchPanel.SetActive(false);

        RefreshArmy();
    }
}

