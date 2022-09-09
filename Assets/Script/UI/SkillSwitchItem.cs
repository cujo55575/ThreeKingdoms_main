using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSwitchItem : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public RawImage SkillIcon;
    public int Index=0;
    private SkillSwitchPanel skillSwitchPanel;
    private SkillData m_skill;
    private PlayerHeroData m_heroData;

    public GameObject unselectedFrame;
    public GameObject selectedFrame;
    public Image SkillNameBar;
    public Text txtSkillName;

    private bool holding = false;
    void Awake()
    {
        skillSwitchPanel = GetComponentInParent<SkillSwitchPanel>();
        GetComponent<Button>().onClick.AddListener(OnClickFunction);
    }
    public void RefreshData(SkillData data)
    {
        if (data == null)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            m_skill = data;
            m_heroData = skillSwitchPanel.battleCardInfo.bcm.data;
            UpdateUI();
        }
    }
    public void UpdateUI()
    {
        if(!gameObject.activeSelf)
        {
            return;
        }
        SkillIcon.texture = m_skill.GetSkillTexture();
        txtSkillName.text = m_skill.GetSkillName();
        bool IsEquiped = m_heroData.IsEquiped(Index);

        unselectedFrame.SetActive(!IsEquiped);
        selectedFrame.SetActive(IsEquiped);
        if(IsEquiped)
        {
            SkillNameBar.color = Color.red;
            txtSkillName.color = Color.yellow;
        }
        else
        {
            SkillNameBar.color = Color.white;
            txtSkillName.color = Color.black;
        }
    }
    void OnClickFunction()
    {
        if(timer>0.5f)
        {
            return;
        }
        bool IsEquiped = m_heroData.IsEquiped(Index);
        if (IsEquiped)
        {
            skillSwitchPanel.UnequipSkill(Index);
        }
        else
        {
            Dictionary<byte, SkillData> skills = m_heroData.GetEquippedSkillsWithIndex();
            if (skills.Count < 3)
            {
                SkillData skill1;
                skills.TryGetValue((byte)E_EquipableStatus.EquippedAt1, out skill1);
                if (skill1 == null)
                {
                    skillSwitchPanel.EquipSkill(E_EquipableStatus.EquippedAt1, Index);
                    return;
                }
                SkillData skill2;
                skills.TryGetValue((byte)E_EquipableStatus.EquippedAt2, out skill2);
                if (skill2 == null)
                {
                    skillSwitchPanel.EquipSkill(E_EquipableStatus.EquippedAt2, Index);
                    return;
                }
                SkillData skill3;
                skills.TryGetValue((byte)E_EquipableStatus.EquippedAt3, out skill3);
                if (skill3 == null)
                {
                    skillSwitchPanel.EquipSkill(E_EquipableStatus.EquippedAt3, Index);
                    return;
                }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        timer = 0;
        holding = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        skillSwitchPanel.SkillDesPanel.SetActive(false);
    }
    private float timer;
    void Update()
    {
        if(holding)
        {
            timer += Time.deltaTime;
            if(timer>0.5f && !skillSwitchPanel.SkillDesPanel.activeSelf)
            {
                skillSwitchPanel.RefreshSkillDesPanel(m_skill);
                skillSwitchPanel.SkillDesPanel.SetActive(true);
            }
        }
    }
}
