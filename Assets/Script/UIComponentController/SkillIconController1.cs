using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillIconController1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image _skillBgImage;
    public Image _skillBgImageFroSkillDescriptionPanel;
    public RawImage ImgSkillIcon;
    public GameObject Mask;
    public GameObject Mask2;
    public GameObject Mask3;
    public GameObject selectCircle;
    public GameObject unSelectCircle;
    public GameObject skillIconDescriptionPanel;
    public Text txtSkillName;
    public SkillData Skill;
    private PlayerHeroData CardData;
    private int SkillIndex;
    public UIHeroDeck _uiHeroDeck;
    public UISkillIconDescription _uiSkillIconDescripton;
    private bool holding = false;
    private float timer;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(onClickFunction);
    }

    void Update()
    {
        if (holding)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f && !_uiSkillIconDescripton.objskillDescription.activeSelf)
            {
                StartPressOnSkillIcon(SkillIndex, Skill, CardData);
            }
        }
    }
    public void UpdateData(SkillData data, PlayerHeroData HeroData, int _SkillIndex)
    {
        Skill = data;
        CardData = HeroData;
        SkillIndex = _SkillIndex;
        ImgSkillIcon.texture = Skill.GetSkillTexture();
        txtSkillName.text = data.GetSkillName();
        bool IsEquiped = _uiSkillIconDescripton.IsEquiped = CardData.IsEquiped(SkillIndex);
        if (CardData.HeroLevel >= Skill.UnlockLevel)
        {
            Mask.SetActive(false);
            if (IsEquiped)
            {
                txtSkillName.color = new Color(255, 255, 0, 255);
                selectCircle.SetActive(true);
                unSelectCircle.SetActive(false);
            }
            else
            {
                txtSkillName.color = new Color(96, 96, 96, 200);
                selectCircle.SetActive(false);
                unSelectCircle.SetActive(true);
            }
        }
        else
        {
            Mask.SetActive(true);
        }
        Mask3.SetActive(!IsEquiped);
        Mask2.SetActive(IsEquiped);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        timer = 0;
        holding = true;
    }
    public void EquipFirstThreeSkill()
    {
        if (CardData.HeroLevel >= Skill.UnlockLevel)
        {
            Dictionary<byte, SkillData> skills = CardData.GetEquippedSkillsWithIndex();
            SkillData skill1;
            skills.TryGetValue((byte)E_EquipableStatus.EquippedAt1, out skill1);
            if (CardData.Skill1StatusType != (byte)1 && skill1 == null)
            {
                EquipSkill(E_EquipableStatus.EquippedAt1);
                return;
            }
            else if (CardData.Skill1StatusType == (byte)1)
            {
                UnEquipSkill();
                return;
            }
            SkillData skill2;
            skills.TryGetValue((byte)E_EquipableStatus.EquippedAt2, out skill2);
            if (CardData.Skill2StatusType != (byte)1 && skill2 == null)
            {
                EquipSkill(E_EquipableStatus.EquippedAt2);
                return;
            }
            else if (CardData.Skill2StatusType == (byte)1)
            {
                CardData.Skill2StatusType = (byte)1;
                UpdateData(Skill, CardData, SkillIndex);
                _uiHeroDeck._uiCardRef.RefreshData(CardData);
                return;
            }
        }
    }
    void onClickFunction()
    {
        if (timer > 0.6f)
        {
            return;
        }
        if (!CardData.IsEquiped(SkillIndex) && CardData.HeroLevel >= Skill.UnlockLevel)
        {
            Dictionary<byte, SkillData> skills = CardData.GetEquippedSkillsWithIndex();
            if (skills.Count < 3)
            {
                SkillData skill1;
                skills.TryGetValue((byte)E_EquipableStatus.EquippedAt1, out skill1);
                if (skill1 == null)
                {
                    EquipSkill(E_EquipableStatus.EquippedAt1);
                    return;
                }
                SkillData skill2;
                skills.TryGetValue((byte)E_EquipableStatus.EquippedAt2, out skill2);
                if (skill2 == null)
                {
                    EquipSkill(E_EquipableStatus.EquippedAt2);
                    return;
                }
                SkillData skill3;
                skills.TryGetValue((byte)E_EquipableStatus.EquippedAt3, out skill3);
                if (skill3 == null)
                {
                    EquipSkill(E_EquipableStatus.EquippedAt3);
                    return;
                }
            }
        }
        else if (CardData.IsEquiped(SkillIndex))
        {
            UnEquipSkill();
        }
    }

    public void EquipSkill(E_EquipableStatus status)
    {
        switch (SkillIndex)
        {
            case 0:
                CardData.Skill1StatusType = (byte)status;
                break;
            case 1:
                CardData.Skill2StatusType = (byte)status;
                break;
            case 2:
                CardData.Skill3StatusType = (byte)status;
                break;
            case 3:
                CardData.Skill4StatusType = (byte)status;
                break;
            case 4:
                CardData.Skill5StatusType = (byte)status;
                break;
            case 5:
                CardData.Skill6StatusType = (byte)status;
                break;
        }
        UpdateData(Skill, CardData, SkillIndex);
        _uiHeroDeck._uiCardRef.RefreshData(CardData);
    }
    public void UnEquipSkill()
    {
        switch (SkillIndex)
        {
            case 0:
                CardData.Skill1StatusType = (byte)1;
                break;
            case 1:
                CardData.Skill2StatusType = (byte)1;
                break;
            case 2:
                CardData.Skill3StatusType = (byte)1;
                break;
            case 3:
                CardData.Skill4StatusType = (byte)1;
                break;
            case 4:
                CardData.Skill5StatusType = (byte)1;
                break;
            case 5:
                CardData.Skill6StatusType = (byte)1;
                break;
        }
        UpdateData(Skill, CardData, SkillIndex);
        _uiHeroDeck._uiCardRef.RefreshData(CardData);
    }
    public void StartPressOnSkillIcon(int index, SkillData data, PlayerHeroData HeroData)
    {
        skillIconDescriptionPanel.SetActive(true);
        _uiSkillIconDescripton.StartPressOnSkillIcon(index, data, HeroData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        skillIconDescriptionPanel.SetActive(false);
        holding = false;
        _uiSkillIconDescripton.ReleasePressOnSkillIcon();
    }
  
}
