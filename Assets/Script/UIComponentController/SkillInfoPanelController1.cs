using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanelController1 : MonoBehaviour
{
    public GameObject skill;
    public Text _txtSkillUseCount;
    public Text txtNextSkillName;
    public Transform skillInfoPanel;
    public List<SkillIconController1> _skilliconController1 = new List<SkillIconController1>();
    public UISkillIconDescriptionController _uiSkillIconDescriptionController;
    public List<UISkillIconDescription> _uiSkillIconDescriptionList = new List<UISkillIconDescription>();
    public Dictionary<byte, SkillData> skills = new Dictionary<byte, SkillData>();
    private CardCountData m_CardCountData;
    PlayerHeroData myHeroData;
    void Update()
    {
        RefreshStatus(myHeroData);
      }
    public void RefreshStatus(PlayerHeroData HeroData)
    {
        myHeroData = HeroData;
        _txtSkillUseCount.text = "Skill Use Count " + HeroData.GetEquippedSkillsWithIndex().Count.ToString() + "/3";
    }
    public void RefreshSkillItem(PlayerHeroData heroData)
    {
        foreach (Transform child in skillInfoPanel)
        {
            Destroy(child.gameObject);
        }
        _skilliconController1.Clear();
        List<SkillData> m_Skills = heroData.GetHeroData().GetSkills();
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        for (int i = 0; i < m_Skills.Count; i++)
        {
            GameObject go = Instantiate(skill);
            go.transform.SetParent(this.transform, false);
            SkillIconController1 itemDeckCard = go.GetComponent<SkillIconController1>();
            Texture icon = m_Skills[i].GetSkillTexture();
            itemDeckCard.UpdateData(m_Skills[i], heroData, i);
            _skilliconController1.Add(itemDeckCard);
            go.SetActive(true);
        }
        if(_uiSkillIconDescriptionList.Count!=0)
        {
            for (int i = 0; i < _skilliconController1.Count; i++)
            {
                _skilliconController1[i]._uiSkillIconDescripton = _uiSkillIconDescriptionList[i];
            }
        }
        if (heroData.HeroLevel < 6)
        {
            SkillData _skillData = heroData.GetNextSkill(heroData.HeroLevel);
            string txtUnlockskilllevel = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UNLOCKSKILL_LEVEL);
        }
        for(int i=0;i<_skilliconController1.Count;i++)
        {
            if (heroData.HeroLevel < _skilliconController1[i].Skill.UnlockLevel)
            {
                txtNextSkillName.text = string.Format(TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHERODECK_NEXTUNLOCKSKILLNAME) + _skilliconController1[i].Skill.GetSkillName());
                return;
            }
            else
                continue;
        }
    }
}
