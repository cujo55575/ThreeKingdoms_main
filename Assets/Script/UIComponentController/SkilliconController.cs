using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkilliconController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public SkillInfoPanelController _SkillInfoPanelControler;
    public Image _skillBgImage;
    public RawImage ImgSkillIcon;
    public GameObject Mask, Mask2, Mask3, selectCircle, unSelectCircle;
    public Text text;
    private SkillData Skill;
    private PlayerHeroData CardData;
    public int SkillIndex, skillCountIndex;
    public UIHeroConfig _uIHeroConfig;
    private bool holding = false;
    private float timer;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(onClickFunction);
        //EquipFirstThreeSkill();
    }
    void Update()
    {
        if (holding)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f && !_SkillInfoPanelControler.skillDescriptionPanelParent.activeSelf)
            {
                _SkillInfoPanelControler.StartPressOnSkillIcon(SkillIndex, Skill, CardData);
                _SkillInfoPanelControler.skillDescriptionPanelParent.SetActive(true);
            }
        }
    }
    public void EquipFirstThreeSkill()
    {
            if (CardData.GetEquippedSkillsWithIndex().Count < 3)
            {
                int skill1=(byte)CardData.Skill1StatusType;
                int skill2= (byte)CardData.Skill2StatusType;
                int skill3=(byte)CardData.Skill3StatusType;
            if (skill1 == 1 && !CardData.IsEquiped(SkillIndex))
            {
                EquipSkill(E_EquipableStatus.EquippedAt1);
                return;
            }
            else if (CardData.IsEquiped(SkillIndex))
             {
                    CardData.Skill1StatusType = (byte)1;
             }
        }
    }
    public void UpdateData(SkillData data, PlayerHeroData HeroData, int _SkillIndex)
    {
        Skill = data;
        CardData = HeroData;
        SkillIndex = _SkillIndex;
        ImgSkillIcon.texture = Skill.GetSkillTexture();
        text.text = data.GetSkillName();
        bool IsEquiped = CardData.IsEquiped(SkillIndex);
        if (CardData.HeroLevel >= Skill.UnlockLevel)
        {
            Mask.SetActive(false);
            if (IsEquiped)
            {
                text.color = new Color(255, 255, 0, 255);
                selectCircle.SetActive(true);
                unSelectCircle.SetActive(false);
            }
            else
            {
                text.color = new Color(0, 0, 0, 255);
                selectCircle.SetActive(false);
                unSelectCircle.SetActive(true);
            }
        }
        else
        {
            text.color = new Color(0, 0, 0, 255);
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
        // _SkillInfoPanelControler.StartPressOnSkillIcon(SkillIndex, Skill, CardData);
        UpdateData(Skill, CardData, SkillIndex);
        _uIHeroConfig.UiCard.RefreshData(CardData);
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
        // _SkillInfoPanelControler.StartPressOnSkillIcon(SkillIndex, Skill, CardData);
        UpdateData(Skill, CardData, SkillIndex);
        _uIHeroConfig.UiCard.RefreshData(CardData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        _SkillInfoPanelControler.ReleasePressOnSkillIcon();
    }

}

