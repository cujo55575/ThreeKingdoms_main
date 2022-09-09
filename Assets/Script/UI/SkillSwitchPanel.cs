using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSwitchPanel : MonoBehaviour
{
    public UIBattleCardInfo battleCardInfo;
    public Button btnOkay;
	public Text txtOkay;
    public List<SkillSwitchItem> skillSwitchItems;

    public Text txtSkillName;
    public Text txtSkillDes;
    public Text txtSkillTrigger;

    public GameObject SkillDesPanel;

    private byte Skill1StatusType;
    private byte Skill2StatusType;
    private byte Skill3StatusType;
    private byte Skill4StatusType;
    private byte Skill5StatusType;
    private byte Skill6StatusType;
    private void Awake()
    {
        btnOkay.onClick.AddListener(OnSkillComfirmClick);
		txtOkay.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);
    }
    public void RefreshSkillDesPanel(SkillData skillData)
    {

        txtSkillName.text = skillData.GetSkillName();
        txtSkillDes.text = skillData.GetSkillDescription();

        int skillType = (int)skillData.SkillType;
        int skillTriggerProbability = skillData.Chance;
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
    private void OnEnable()
    {
        if (battleCardInfo.bcm!=null) {
            Skill1StatusType = battleCardInfo.bcm.data.Skill1StatusType;
            Skill2StatusType = battleCardInfo.bcm.data.Skill2StatusType;
            Skill3StatusType = battleCardInfo.bcm.data.Skill3StatusType;
            Skill4StatusType = battleCardInfo.bcm.data.Skill4StatusType;
            Skill5StatusType = battleCardInfo.bcm.data.Skill5StatusType;
            Skill6StatusType = battleCardInfo.bcm.data.Skill6StatusType;
        }

        for(int i=0;i<skillSwitchItems.Count;i++)
        {
            skillSwitchItems[i].Index = i;
        }

        RefreshSkill();
    }
    public void AssignValue()
    {
        /*battleCardInfo.bcm.data.Skill1StatusType = Skill1StatusType;
        battleCardInfo.bcm.data.Skill2StatusType = Skill2StatusType;
        battleCardInfo.bcm.data.Skill3StatusType = Skill3StatusType;
        battleCardInfo.bcm.data.Skill4StatusType = Skill4StatusType;
        battleCardInfo.bcm.data.Skill5StatusType = Skill5StatusType;
        battleCardInfo.bcm.data.Skill6StatusType = Skill6StatusType;*/
        OnSkillComfirmClick();
    }
    public void RefreshSkill()
    {
        if (battleCardInfo.bcm==null) {
            return;
        }
        List<SkillData> AllSkills = battleCardInfo.bcm.data.GetHeroData().GetSkills();
        for(int i=0;i<skillSwitchItems.Count;i++)
        {
            if (i < battleCardInfo.bcm.data.HeroLevel)
            {
                skillSwitchItems[i].RefreshData(AllSkills[i]);
            }
            else
            {
                skillSwitchItems[i].RefreshData(null);
            }
        }
    }
    public void EquipSkill(E_EquipableStatus status, int index)
    {
        switch (index)
        {
            case 0:
                battleCardInfo.bcm.data.Skill1StatusType = (byte)status;
                break;
            case 1:
                battleCardInfo.bcm.data.Skill2StatusType = (byte)status;
                break;
            case 2:
                battleCardInfo.bcm.data.Skill3StatusType = (byte)status;
                break;
            case 3:
                battleCardInfo.bcm.data.Skill4StatusType = (byte)status;
                break;
            case 4:
                battleCardInfo.bcm.data.Skill5StatusType = (byte)status;
                break;
            case 5:
                battleCardInfo.bcm.data.Skill6StatusType = (byte)status;
                break;
        }
        for (int i = 0; i < skillSwitchItems.Count; i++)
        {
            skillSwitchItems[i].UpdateUI();
        }
        AssignValue();
    }
    public void UnequipSkill(int index)
    {
        switch (index)
        {
            case 0:
                battleCardInfo.bcm.data.Skill1StatusType = (byte)1;
                break;
            case 1:
                battleCardInfo.bcm.data.Skill2StatusType = (byte)1;
                break;
            case 2:
                battleCardInfo.bcm.data.Skill3StatusType = (byte)1;
                break;
            case 3:
                battleCardInfo.bcm.data.Skill4StatusType = (byte)1;
                break;
            case 4:
                battleCardInfo.bcm.data.Skill5StatusType = (byte)1;
                break;
            case 5:
                battleCardInfo.bcm.data.Skill6StatusType = (byte)1;
                break;
        }
        for(int i=0;i<skillSwitchItems.Count;i++)
        {
            skillSwitchItems[i].UpdateUI();
        }
        AssignValue();
    }
    public void OnSkillComfirmClick()
    {
        battleCardInfo.RefreshSkill();
        Leader[] leaders = GameObject.FindObjectsOfType<Leader>();
        for(int i=0;i<leaders.Length;i++)
        {
            if(leaders[i].UICard!=null && leaders[i].UICard==battleCardInfo.bcm.gameObject)
            {
                leaders[i].RegisterAttributes(battleCardInfo.bcm.data);
            }
        }
    }
}
