using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconObjController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RawImage SkillIcon;
    public Text skillName;
    public GameObject LockedMask;
    public GameObject selectCircle;
    public GameObject unSelectCircle;
    public GameObject skillIconDescriptionPanel;
    public Image imgSkillName;
    public ArmySkillPanelController armySkillPanelController;
    public UISkillIconDescription _uiSkillIconDescripton;
    public SkillData Skill;
    public HeroArmyData _myHeroArmyData;
    private int m_Index;
    private bool holding = false;
    private float timer;
   
    void Update()
    {
        if (holding)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f && !_uiSkillIconDescripton.objskillDescription.activeSelf)
            {
                StartPressOnSkillIcon(m_Index, Skill, _myHeroArmyData);
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
        skillIconDescriptionPanel.SetActive(false);
        holding = false;
        _uiSkillIconDescripton.ReleasePressOnSkillIcon();
    }

    public void RefreshData(SkillData _skillData, Texture iconTexture, bool isLocked, int index,HeroArmyData _heroArmyData)
    {
        Skill = _skillData;
        _myHeroArmyData = _heroArmyData;
        skillName.text = _skillData.GetSkillName();
        SkillIcon.texture = iconTexture;
        if (isLocked)
        {
            imgSkillName.color = new Color(0, 0, 0, 255);
            unSelectCircle.SetActive(true);
            selectCircle.SetActive(false);
            LockedMask.SetActive(true);
        }
        else if (!isLocked)
        {
            skillName.color = new Color(255, 255, 0, 255);
            imgSkillName.color = new Color(255, 0, 0, 255);
            unSelectCircle.SetActive(false);
            selectCircle.SetActive(true);
            LockedMask.SetActive(false);
        }
        m_Index = index;
    }
    public void StartPressOnSkillIcon(int index, SkillData data, HeroArmyData _heroArmyData)
    {
        skillIconDescriptionPanel.SetActive(true);
        _uiSkillIconDescripton.StartPressOnSkillIconForUIArmy(index, data, _heroArmyData);
    }
}
