using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanelController : MonoBehaviour
{
    public GameObject skillDescriptionPanelParent, heroInfoWarning, skill, MaxSkill, getSkill,
     skillUpgradeAvailablePanel, highestLevelInfoPanel, heroInfoPanel;
    public Text SkillDescriptionText, SkillLevelText, _txtSkillDescription, _skillNameForSkillDescription,
     _txtSkillName, _txtSkillUseCount, skillNameForUnlockingSkill, txtHerorank, txtUnlockingSkill,
      txtHeroCardCount, txthighestLevelInfo, txtSkillTrigger;
    public RawImage _skillRawImageHeroInfoPanel;
    public Transform skillInfoPanel;
    public List<SkilliconController> _skilliconController = new List<SkilliconController>();
    PlayerHeroData newHeroData;
    private CardCountData m_CardCountData;
    PlayerHeroData myHeroData;
    void Start()
    {
        ReleasePressOnSkillIcon();
    }
    void Update()
    {
        Dictionary<byte, SkillData> skills = myHeroData.GetEquippedSkillsWithIndex();
        string txtSuserCount = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TOTAL_SKILLUSE);
        _txtSkillUseCount.text = string.Format(txtSuserCount, skills.Count);
    }
    public void RefreshStatus(PlayerHeroData HeroData)
    {
        myHeroData = HeroData;
        Dictionary<byte, SkillData> skills = HeroData.GetEquippedSkillsWithIndex();
        string txtSuserCount = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TOTAL_SKILLUSE);
        _txtSkillUseCount.text = string.Format(txtSuserCount, skills.Count);
    }
    public void StartPressOnSkillIcon(int index, SkillData data, PlayerHeroData HeroData)
    {
        newHeroData = HeroData;
        skillDescriptionPanelParent.SetActive(true);
        List<SkillData> _skills = HeroData.GetHeroData().GetSkills();
        _skillNameForSkillDescription.text = _skills[index].GetSkillName();
        SkillDescriptionText.text = _skills[index].GetSkillDescription();
        string _skillLevelText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UNLOCKSKILL_LEVEL);
        SkillLevelText.text = string.Format(_skillLevelText, _skills[index].UnlockLevel);
        if (HeroData.HeroLevel >= data.UnlockLevel)
        {
            heroInfoWarning.SetActive(false);
        }
        else
        {
            heroInfoWarning.SetActive(true);
        }
        Dictionary<byte, SkillData> skills = HeroData.GetEquippedSkillsWithIndex();
        int skillType = (int)_skills[index].SkillType;
        int skillTriggerProbability = _skills[index].Chance;
        if (skillType == 0)
        {
            string triggerText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TRIGGER_PROBABILITY);
            txtSkillTrigger.text = skillTriggerProbability.ToString() + triggerText;
        }
        else
        {
            txtSkillTrigger.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_PASSIVE_SKILL);
        }
    }
    public void RefreshSkillItem(PlayerHeroData heroData)
    {
        foreach (Transform child in skillInfoPanel)
        {
            Destroy(child.gameObject);
        }
        _skilliconController.Clear();
        ReleasePressOnSkillIcon();
        heroInfoPanel.SetActive(true);
        highestLevelInfoPanel.SetActive(false);
        List<SkillData> m_Skills = heroData.GetHeroData().GetSkills();
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        txtHeroCardCount.text = heroData.FragmentCount.ToString() + "/" + m_CardCountData.GetCostByLevel(heroData.HeroLevel);
        txtHerorank.text = heroData.heroRank().ToString();
        for (int i = 0; i < m_Skills.Count; i++)
        {
            GameObject go = Instantiate(skill);
            go.transform.SetParent(this.transform, false);
            SkilliconController itemDeckCard = go.GetComponent<SkilliconController>();
            Texture icon = m_Skills[i].GetSkillTexture();
            itemDeckCard.UpdateData(m_Skills[i], heroData, i);
            _skilliconController.Add(itemDeckCard);
            go.SetActive(true);
        }
        if (heroData.HeroLevel < 6)
        {
            skillUpgradeAvailablePanel.SetActive(true);
            highestLevelInfoPanel.SetActive(false);
            getSkill.SetActive(true);
            MaxSkill.SetActive(false);
            SkillData _skillData = heroData.GetNextSkill(heroData.HeroLevel);
            _skillRawImageHeroInfoPanel.texture = _skillData.GetSkillTexture();
            _txtSkillName.text = _skillData.GetSkillName();
            skillNameForUnlockingSkill.text = _skillData.GetSkillName();
            string txtUnlockskilllevel = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UNLOCKSKILL_LEVEL);
            txtUnlockingSkill.text = string.Format(txtUnlockskilllevel, _skillData.unlockLevel().ToString());
            _txtSkillDescription.text = _skillData.GetSkillDescription();
        }
        else
        {
            heroInfoPanel.SetActive(false);
            highestLevelInfoPanel.SetActive(true);
            txthighestLevelInfo.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_HIGHEST_LEVEL_TEXT);
            getSkill.SetActive(false);
            skillUpgradeAvailablePanel.SetActive(false);
            MaxSkill.SetActive(true);
        }
    }
    public void ReleasePressOnSkillIcon()
    {
        skillDescriptionPanelParent.SetActive(false);
    }
}
