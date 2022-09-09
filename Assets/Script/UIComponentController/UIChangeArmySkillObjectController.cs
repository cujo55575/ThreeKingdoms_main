using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIChangeArmySkillObjectController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RawImage SkillIcon;
    public Text skillName;
    public GameObject LockedMask;
    public GameObject selectCircle;
    public GameObject unSelectCircle;
    public Image imgSkillName;
    public UIChangeArmySkillInfoPanelController SkillInfoPanelControl;
    private int m_Index;
    private bool holding = false;
    private float timer;
    void Update()
    {
        if (holding)
        {
            timer += Time.deltaTime;
            if (timer > 0.4f && !SkillInfoPanelControl.DescriptionPanelParent.activeSelf)
            {
                SkillInfoPanelControl.StartPressOnSkillIcon(m_Index, SkillIcon.texture);
                SkillInfoPanelControl.DescriptionPanelParent.SetActive(true);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        timer = 0;
        holding = true;
        // SkillInfoPanelControl.StartPressOnSkillIcon(m_Index, SkillIcon.texture);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        SkillInfoPanelControl.ReleasePressOnSkillIcon();
    }

    public void RefreshData(SkillData _skillData, Texture iconTexture, bool isLocked, int index)
    {
        skillName.text = _skillData.GetSkillName();
        skillName.color = new Color(0, 0, 0, 255);
        imgSkillName.color = new Color(240, 240, 240, 240);
        SkillIcon.texture = iconTexture;
        unSelectCircle.SetActive(isLocked);
        selectCircle.SetActive(!isLocked);
        LockedMask.SetActive(isLocked);
        if (!isLocked)
        {
            skillName.color = new Color(255, 255, 0, 255);
            imgSkillName.color = new Color(255, 0, 0, 255);
        }
        m_Index = index;
    }
}
