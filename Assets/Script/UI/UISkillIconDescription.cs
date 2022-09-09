using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillIconDescription : MonoBehaviour
{
    public Text txtskillNameForSkillDescription;
    public Text txtSkillDescriptionText;
    public Text SkillLevelText;
    public Text txtSkillTrigger;
    public RawImage ImgSkillIconForSkillDescriptionPanel;
    public GameObject MaskForSkillDescriptionPanel;
    public GameObject selectCircleForSkillDescriptionPanel;
    public GameObject unSelectCircleForSkillDescriptionPanel;
    public GameObject heroInfoWarning;
    public GameObject objskillDescription;
    private int SkillIndex;
    public bool IsEquiped;
    public void StartPressOnSkillIcon(int index, SkillData data, PlayerHeroData HeroData)
    {
        objskillDescription.SetActive(true);
        List<SkillData> _skills = HeroData.GetHeroData().GetSkills();
        txtskillNameForSkillDescription.text = _skills[index].GetSkillName();
        txtSkillDescriptionText.text = _skills[index].GetSkillDescription();
        ImgSkillIconForSkillDescriptionPanel.texture = data.GetSkillTexture();
         //IsEquiped = HeroData.IsEquiped(SkillIndex);
        if (HeroData.HeroLevel >= data.UnlockLevel)
        {
            MaskForSkillDescriptionPanel.SetActive(false);
            if (IsEquiped)
            {
                selectCircleForSkillDescriptionPanel.SetActive(true);
                unSelectCircleForSkillDescriptionPanel.SetActive(false);
            }
            else
            {
                selectCircleForSkillDescriptionPanel.SetActive(false);
                unSelectCircleForSkillDescriptionPanel.SetActive(true);
            }
        }
        else
        {
            MaskForSkillDescriptionPanel.SetActive(true);
        }
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
    public void StartPressOnSkillIconForUIArmy(int index, SkillData data, HeroArmyData _heroArmydata)
    {
        objskillDescription.SetActive(true);
        List<SkillData> _skills = _heroArmydata.GetSkills();
        txtskillNameForSkillDescription.text = _skills[index].GetSkillName();
        txtSkillDescriptionText.text = _skills[index].GetSkillDescription();
        ImgSkillIconForSkillDescriptionPanel.texture = data.GetSkillTexture();
        if (_heroArmydata.ArmyLevel >= data.UnlockLevel)
        {
            MaskForSkillDescriptionPanel.SetActive(false);
            if (IsEquiped)
            {
                selectCircleForSkillDescriptionPanel.SetActive(true);
                unSelectCircleForSkillDescriptionPanel.SetActive(false);
            }
            else
            {
                selectCircleForSkillDescriptionPanel.SetActive(false);
                unSelectCircleForSkillDescriptionPanel.SetActive(true);
            }
        }
        else
        {
            MaskForSkillDescriptionPanel.SetActive(true);
        }
        string _skillLevelText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UNLOCKSKILL_LEVEL);
        SkillLevelText.text = string.Format(_skillLevelText, _skills[index].UnlockLevel);
        if (_heroArmydata.ArmyLevel >= data.UnlockLevel)
        {
            heroInfoWarning.SetActive(false);
        }
        else
        {
            heroInfoWarning.SetActive(true);
        }
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
    public void ReleasePressOnSkillIcon()
    {
        objskillDescription.SetActive(false);
    }
}
