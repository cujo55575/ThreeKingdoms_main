using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkill : UIBase
{
    public Button BtnClose;

    public GameObject PrefSkillItem;
    public Transform ItemScrollView;

    public UICard UiCard;
    private PlayerHeroData CardData;

    public Text txtCardName;

    public RectTransform DragSkill;
    public RawImage SkillImage;

    private Camera UICamera;

    protected override void OnInit()
    {
        base.OnInit();
        BtnClose.onClick.AddListener(CloseUI);
        UICamera = GetComponentInParent<Canvas>().worldCamera;
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);

        ItemScrollView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        CardData = (PlayerHeroData)Objects[0];
        UiCard.RefreshData(CardData);

        txtCardName.text = CardData.GetHeroName();

        RefreshSkillItem();
        RefreshData();
    }

    private void RefreshSkillItem()
    {
        foreach (Transform child in ItemScrollView)
        {
            Destroy(child.gameObject);
        }
        List<SkillData> skills = CardData.GetHeroData().GetSkills();
        for (int i = 0; i < skills.Count; i++)
        {
            GameObject go = Instantiate(PrefSkillItem);
            go.SetActive(true);
            go.transform.SetParent(ItemScrollView, false);
            ItemSkill itemDeckCard = go.GetComponent<ItemSkill>();
            itemDeckCard.UpdateData(skills[i], CardData, i);
        }
    }

    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_SKILL);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK, CardData);
        //PlayerDataManager.Instance.SavePlayerData();
    }
    private ItemSkill DraggingItemSkill;
    public void StartDrag(ItemSkill itemSkill)
    {
        if (DraggingItemSkill != null)
        {
            return;
        }
        DraggingItemSkill = itemSkill;
        SkillImage.texture = DraggingItemSkill.ImgSkillIcon.texture;
    }
    public RawImage FirstSlot;
    public RawImage SecondSlot;
    public RawImage ThirdSlot;
    public void EndDrag(ItemSkill itemSkill)
    {
        if (DraggingItemSkill != itemSkill)
        {
            return;
        }
        RawImage closestSlot = FirstSlot;

        float closestDistance = Vector3.Distance(DragSkill.transform.position, FirstSlot.transform.position);

        float d2 = Vector3.Distance(DragSkill.transform.position, SecondSlot.transform.position);
        if (closestDistance > d2)
        {
            closestDistance = d2;
            closestSlot = SecondSlot;
        }

        float d3 = Vector3.Distance(DragSkill.transform.position, ThirdSlot.transform.position);
        if (closestDistance > d3)
        {
            closestDistance = d3;
            closestSlot = ThirdSlot;
        }
        E_EquipableStatus status = E_EquipableStatus.EquippedAt1;
        if (closestSlot == SecondSlot)
        {
            status = E_EquipableStatus.EquippedAt2;
        }
        else if (closestSlot == ThirdSlot)
        {
            status = E_EquipableStatus.EquippedAt3;
        }
        if (closestDistance < 100)
        {
            closestSlot.GetComponent<Button>().onClick.Invoke();
            switch (itemSkill.SkillIndex)
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
        }
        DraggingItemSkill = null;
        UiCard.RefreshData(CardData);
        RefreshData();
    }
    private int GetSkillIndex(SkillData data)
    {
        List<SkillData> skills = CardData.GetHeroData().GetSkills();
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i] == data)
            {
                return i;
            }
        }
        return -1;
    }
    public void RemoveSkill(int index)
    {
        Debug.Log(index);
        switch (index)
        {
            case 0:
                CardData.Skill1StatusType = 1;
                break;
            case 1:
                CardData.Skill2StatusType = 1;
                break;
            case 2:
                CardData.Skill3StatusType = 1;
                break;
            case 3:
                CardData.Skill4StatusType = 1;
                break;
            case 4:
                CardData.Skill5StatusType = 1;
                break;
            case 5:
                CardData.Skill6StatusType = 1;
                break;
        }

        UiCard.RefreshData(CardData);
    }
    public void RefreshData()
    {
        FirstSlot.GetComponent<Button>().onClick.RemoveAllListeners();
        SecondSlot.GetComponent<Button>().onClick.RemoveAllListeners();
        ThirdSlot.GetComponent<Button>().onClick.RemoveAllListeners();

        Dictionary<byte, SkillData> skills = CardData.GetEquippedSkillsWithIndex();

        SkillData skill1;
        skills.TryGetValue((byte)E_EquipableStatus.EquippedAt1, out skill1);
        if (skill1 != null)
        {
            FirstSlot.GetComponent<Button>().onClick.AddListener(() => RemoveSkill(GetSkillIndex(skill1)));
        }
        SkillData skill2;
        skills.TryGetValue((byte)E_EquipableStatus.EquippedAt2, out skill2);
        if (skill2 != null)
        {
            SecondSlot.GetComponent<Button>().onClick.AddListener(() => RemoveSkill(GetSkillIndex(skill2)));
        }
        SkillData skill3;
        skills.TryGetValue((byte)E_EquipableStatus.EquippedAt3, out skill3);
        if (skill3 != null)
        {
            ThirdSlot.GetComponent<Button>().onClick.AddListener(() => RemoveSkill(GetSkillIndex(skill3)));
        }
    }
    protected override void OnUpdate()
    {
        if (DraggingItemSkill != null)
        {
            DragSkill.gameObject.SetActive(true);
        }
        else
        {
            DragSkill.gameObject.SetActive(false);
        }
        Vector2 cursorPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        DragSkill.transform.position = cursorPos;

        base.OnUpdate();
    }
}
