using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSkill : MonoBehaviour
{
    public UISkill uiSkill;

    public Text TxtSkillName;
    public Text TxtSkillDesc;
    public RawImage ImgSkillIcon;
    public GameObject Mask;

    private SkillData Skill;
    private PlayerHeroData CardData;
    public int SkillIndex;

    private Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
    }
    public void UpdateData(SkillData data, PlayerHeroData HeroData, int _SkillIndex)
    {
        Skill = data;
        CardData = HeroData;
        SkillIndex = _SkillIndex;
        TxtSkillName.text = Skill.GetSkillName();
        TxtSkillDesc.text = Skill.GetSkillDescription();
        ImgSkillIcon.texture = Skill.GetSkillTexture();
        if (CardData.HeroLevel >= Skill.UnlockLevel)
        {
            Mask.SetActive(false);
            m_Toggle.interactable = true;
        }
        else
        {
            Mask.SetActive(true);
            m_Toggle.interactable = false;
        }

    }
    public void StartDrag()
    {
        uiSkill.StartDrag(this);
    }
    public void EndDrag()
    {
        uiSkill.EndDrag(this);
    }
}
