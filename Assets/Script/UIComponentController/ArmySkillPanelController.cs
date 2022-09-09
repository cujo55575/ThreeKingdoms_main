using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmySkillPanelController : MonoBehaviour
{
    public Text txtSkillNameForSkillInfoPanelController;
    public GameObject SkillPrefab;
    public RawImage skillicon;
    private HeroArmyData m_HeroArmyData;
    private List<SkillData> m_Skills;
    private List<SkillIconObjController> m_SkillObjectControl = new List<SkillIconObjController>();
    public UISkillIconDescriptionController _uiSkillIconDescriptionController;
    public List<UISkillIconDescription> _uiSkillIconDescriptionList = new List<UISkillIconDescription>();
    public void RefreshData(HeroArmyData heroArmyData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        m_SkillObjectControl.Clear();
        m_HeroArmyData = heroArmyData;
        m_Skills = m_HeroArmyData.GetSkills();
        for (int i = 0; i < m_Skills.Count; i++)
        {
            GameObject go = Instantiate(SkillPrefab, transform);
            SkillIconObjController control = go.GetComponent<SkillIconObjController>();
            Texture icon = m_Skills[i].GetSkillTexture();
            bool isLocked = (GetSkillStatus(i) == E_EquipableStatus.Locked);
            control.RefreshData(m_Skills[i], icon, isLocked, i,m_HeroArmyData);
            m_SkillObjectControl.Add(control);
            go.SetActive(true);
        }
        _uiSkillIconDescriptionController.CreateSkillIconDescriptionsForArmyPanel(heroArmyData);
        if (_uiSkillIconDescriptionList.Count != 0)
        {
            for (int i = 0; i < m_SkillObjectControl.Count; i++)
            {
                m_SkillObjectControl[i]._uiSkillIconDescripton = _uiSkillIconDescriptionList[i];
            }
        }
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
