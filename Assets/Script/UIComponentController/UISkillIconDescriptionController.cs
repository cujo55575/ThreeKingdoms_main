using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillIconDescriptionController : MonoBehaviour
{
    private Transform uiSkillIconDescriptionPanel;
    public GameObject objSkillIconDescritpion;
    public SkillInfoPanelController1 _skillInfoPanelController1;
    public ArmySkillPanelController _armySkillPanelController;
    private List<SkillData> m_Skills = new List<SkillData>();
    private GridLayoutGroup gLayOutGroupRef;
    public void Start()
    {
        uiSkillIconDescriptionPanel = gameObject.transform;
        gLayOutGroupRef = GetComponent<GridLayoutGroup>();
    }
    private void Update()
    {
       if(m_Skills.Count==3)
        {
            gLayOutGroupRef.padding.left = -250;
            gLayOutGroupRef.spacing= new Vector2(190,100);
        }
        else
        {
            gLayOutGroupRef.padding.left = -190;
            gLayOutGroupRef.spacing = new Vector2(350, 100);
        }
    }
    public void CreateSkillIconDescriptions(PlayerHeroData newPlayerHeroData)
    {
        List<SkillData> m_Skills = newPlayerHeroData.GetHeroData().GetSkills();
        CardCountData m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        for (int i = 0; i < m_Skills.Count; i++)
        {
            GameObject go = Instantiate(objSkillIconDescritpion);
            go.transform.SetParent(this.transform, false);
            _skillInfoPanelController1._uiSkillIconDescriptionList.Add(go.GetComponent<UISkillIconDescription>());
            go.SetActive(true);
        }
    }
    public void CreateSkillIconDescriptionsForArmyPanel(HeroArmyData newHeroArmyData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        m_Skills = newHeroArmyData.GetSkills();
        _armySkillPanelController._uiSkillIconDescriptionList = new List<UISkillIconDescription>();
        CardCountData m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        for (int i = 0; i < m_Skills.Count; i++)
        {
            GameObject go = Instantiate(objSkillIconDescritpion);
            go.transform.SetParent(this.transform, false);
            _armySkillPanelController._uiSkillIconDescriptionList.Add(go.GetComponent<UISkillIconDescription>());
            go.SetActive(true);
        }
    }
}
