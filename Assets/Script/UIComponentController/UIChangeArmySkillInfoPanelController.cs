using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeArmySkillInfoPanelController : MonoBehaviour
{
    public GameObject DescriptionPanelParent;
    public Text SkillNameText;
    public Text txtSkillTrigger;
    public Text txtSkillNameForSkillInfoPanelController;
    public Text SkillLevelText;
    public Text SkillDescriptionText;
    public GameObject SkillPrefab;
    public RawImage skillicon;
    private HeroArmyData m_HeroArmyData;
    private List<SkillData> m_Skills;
    private List<UIChangeArmySkillObjectController> m_SkillObjectControl = new List<UIChangeArmySkillObjectController>();
    private void Start()
    {
        ReleasePressOnSkillIcon();
    }

    //Linked from Editor
    public void StartPressOnSkillIcon(int index, Texture mytexture)
    {
        Debug.Log("Called with " + index);
        SkillNameText.text = m_Skills[index].GetSkillName();
        string _skillLevel = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UNLOCKSKILL_LEVEL);
        SkillLevelText.text = string.Format(_skillLevel, m_Skills[index].UnlockLevel);

        int skillType = (int)m_Skills[index].SkillType;
        int skillTriggerProbability = m_Skills[index].Chance;
        if (skillType == 0)
        {
            string triggerText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_TRIGGER_PROBABILITY);
            txtSkillTrigger.text = skillTriggerProbability.ToString() + triggerText;
        }
        else
        {
            txtSkillTrigger.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_PASSIVE_SKILL);
        }

        SkillDescriptionText.text = m_Skills[index].GetSkillDescription();
        if (GetSkillStatus(index) != E_EquipableStatus.Locked)
        {
            SkillLevelText.color = Color.green;
        }
        else
        {
            SkillLevelText.color = Color.red;
        }
        DescriptionPanelParent.gameObject.SetActive(true);
        skillicon.texture = mytexture;
    }

    //Linked from Editor
    public void ReleasePressOnSkillIcon()
    {
        Debug.Log("Released");
        DescriptionPanelParent.gameObject.SetActive(false);
    }

    public void RefreshData(HeroArmyData heroArmyData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        m_SkillObjectControl.Clear();
        ReleasePressOnSkillIcon();

        m_HeroArmyData = heroArmyData;
        m_Skills = m_HeroArmyData.GetSkills();
        for (int i = 0; i < m_Skills.Count; i++)
        {
            GameObject go = Instantiate(SkillPrefab, transform);
            UIChangeArmySkillObjectController control = go.GetComponent<UIChangeArmySkillObjectController>();
            Texture icon = m_Skills[i].GetSkillTexture();
            bool isLocked = (GetSkillStatus(i) == E_EquipableStatus.Locked);
            control.RefreshData(m_Skills[i], icon, isLocked, i);
            m_SkillObjectControl.Add(control);
            go.SetActive(true);
        }
		//Canvas.ForceUpdateCanvases();
    }

    private E_EquipableStatus GetSkillStatus(int index)
    {
        switch (index)
        {
            case 0: return (E_EquipableStatus)m_HeroArmyData.Skill1StatusType;
            case 1: return (E_EquipableStatus)m_HeroArmyData.Skill2StatusType;
            case 2: return (E_EquipableStatus)m_HeroArmyData.Skill3StatusType;
            case 3: return (E_EquipableStatus)m_HeroArmyData.Skill4StatusType;
            case 4: return (E_EquipableStatus)m_HeroArmyData.Skill5StatusType;
            case 5: return (E_EquipableStatus)m_HeroArmyData.Skill6StatusType;
            default: return E_EquipableStatus.Locked;
        }
    }
}
