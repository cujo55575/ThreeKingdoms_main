using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroInterface_ArmyController : MonoBehaviour
{
    public Button BtnLeft;
    public Button BtnRight;
    public Button btnSoldierGo;
    public Button btnUpgrade;
    public Button[] btnArmySkill = new Button[4];
    public Button[] btnSoldierMenu = new Button[2];
    public GameObject[] armySkillMask = new GameObject[4];
    public GameObject ArmyPlaceHolderPrefab;
    public GameObject SpotLightObject;
    public GameObject armyPlateParent;
    public GameObject unpgradeEffect;
    public GameObject objWarningMessage;
    public GameObject objSoldierAbailityPanel;
    public GameObject objNormalSkillPanel;
    public GameObject objSkillFrameMask;
    public GameObject objArmLock;
    private GameObject go;
    public RawImage imgSoldierQuality;
    public Text txtArmyName;
    public List<PlayerHeroData> _OwnedHeroCards;
    private List<HeroArmyData> armies;
    public List<ClickOn> clickOnList = new List<ClickOn>();
    private PlayerHeroData m_PlayerHeroData;
    private HeroArmyData _heroArmyDataRef;
    public ArmyData _armyData;
    public TweenRotation ArmyParentRotator;
    public Transform ArmyParent;
    public Transform upgradeEffectTrans;
    private Animator m_AttackingAnimator;
    public int m_SelectedArmyIndex = 0;
    public int m_heroIndex = 0;
    private float m_Timer;
    private bool m_Rotating = false;
    private string[] buttonSoldier = new string[2];
    private string[] soldierQuality = new string[4];
    private string[] soldierInfoTab = new string[2];
    private string[] buttonTextureName = new string[3];
    public static UIHeroInterface_ArmyController _ArmyController;
    private enum E_Direction
    {
        Left, Right
    }
    void Awake()
    {
        _ArmyController = this;
        //effectCircularWall.SetActive(false);
        if (armyPlateParent != null)
        {
            if ((float)Screen.width / (float)Screen.height >= 1.8f)
            {
                armyPlateParent.transform.localScale = Vector3.one * 1.68f;
                Vector3 temp = new Vector3(0, -40, 0);
            }
            else
            {
                armyPlateParent.transform.localScale = Vector3.one * 2.1f;
            }
        }
    }
    void Start()
    {
        buttonSoldier = new string[] { "btn_soldierGo", "ui_soldierGoing" };
        soldierQuality = new string[] { "item_soldierAtb_atk", "item_soldierAtb_def", "item_soldierAtb_raid", "item_soldierAtb_sup" };
        soldierInfoTab = new string[] { "btn_soldierMenu(d)", "btn_cancel" };
        buttonTextureName = new string[] { "btn_skill", "btn_skill(c)", "btn_skill_mask(c)" };

        objWarningMessage.SetActive(false);
        armySkillMask[0].SetActive(false);
        armySkillMask[1].SetActive(true);
        armySkillMask[2].SetActive(true);
        armySkillMask[3].SetActive(true);
        objSoldierAbailityPanel.SetActive(true);
        objNormalSkillPanel.SetActive(false);
        objSkillFrameMask.SetActive(false);
        objArmLock.SetActive(false);


        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;

        armies = _OwnedHeroCards[m_heroIndex].GetAllArmies();
        _heroArmyDataRef = armies[m_SelectedArmyIndex];
        RefreshArmyData(_OwnedHeroCards[m_heroIndex]);

        btnSoldierMenu[0].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(soldierInfoTab[0]);
        btnSoldierMenu[1].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(soldierInfoTab[1]);

        BtnRight.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Left);
        });
        BtnLeft.onClick.AddListener(delegate
        {
            OnClickChangeSelectedHero(E_Direction.Right);
        });
        ArmyParentRotator.FinishMethod = delegate
        {
            RotateFinish();
        };
        btnSoldierGo.onClick.AddListener(delegate
        {
            SendSoldierToBattle();
            showUpgradeEffect();
        });
        btnSoldierMenu[0].onClick.AddListener(delegate
        {
            ChangeSoldierMenuTabTexture(0);
            objSoldierAbailityPanel.SetActive(true);
            objNormalSkillPanel.SetActive(false);
        });
        btnSoldierMenu[1].onClick.AddListener(delegate
       {
           ChangeSoldierMenuTabTexture(1);
           objSoldierAbailityPanel.SetActive(false);
           objNormalSkillPanel.SetActive(true);
       });
        btnUpgrade.onClick.AddListener(delegate
        {
            StartCoroutine(ShowWarningMsg());
        });
        btnArmySkill[0].onClick.AddListener(delegate
        {
            ShowArmySkill(0);
            objSkillFrameMask.SetActive(false);
        });
        btnArmySkill[1].onClick.AddListener(delegate
     {
         ShowArmySkill(1);
         objSkillFrameMask.SetActive(true);
     });
        btnArmySkill[2].onClick.AddListener(delegate
        {
            ShowArmySkill(2);
            objSkillFrameMask.SetActive(true);
        });
        btnArmySkill[3].onClick.AddListener(delegate
     {
         ShowArmySkill(3);
         objSkillFrameMask.SetActive(true);
     });
    }
    private void Update()
    {
        if (m_AttackingAnimator != null && m_AttackingAnimator.GetBool("IsAttacking"))
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_AttackingAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                m_AttackingAnimator.SetBool("IsAttacking", false);
                m_AttackingAnimator = null;
            }
        }
    }
    private void OnClickChangeSelectedHero(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Left:
                m_SelectedArmyIndex--;
                break;
            case E_Direction.Right:
                m_SelectedArmyIndex++;
                break;
            default:
                m_SelectedArmyIndex--;
                break;
        }
        if (m_SelectedArmyIndex >= m_PlayerHeroData.GetAllArmiesSelf().Count)
        {
            m_SelectedArmyIndex = 0;
        }
        else if (m_SelectedArmyIndex < 0)
        {
            m_SelectedArmyIndex = m_PlayerHeroData.GetAllArmiesSelf().Count - 1;
        }
        Destroy(go);
        UIManager.UIInstance<UIHeroInterface>()._heroArmyIndex = m_SelectedArmyIndex;
        _heroArmyDataRef = armies[m_SelectedArmyIndex];
        RotateArmyParent(direction);
        RefreshUI(m_PlayerHeroData.GetAllArmiesSelf()[m_SelectedArmyIndex]);
        ChangeScaleSelected();
    }
    private void SendSoldierToBattle()
    {
        btnSoldierGo.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonSoldier[1]);
        List<HeroArmyData> _armiesRef = m_PlayerHeroData.GetAllArmiesSelf();
        for (int i = 0; i < _armiesRef.Count; i++)
        {
            if (i == m_SelectedArmyIndex)
            {
                HeroArmyData equippedArmy = m_PlayerHeroData.GetEquippedArmySelf();
                if (equippedArmy != null)
                {
                    equippedArmy.HeroArmyStatusType = (byte)E_EquipableStatus.Unlocked;
                }
                _armiesRef[i].HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;

                break;
            }
        }
        // m_AttackingAnimator = ArmyParent.transform.GetChild(m_SelectedArmyIndex).gameObject.GetComponentInChildren<Animator>();
        // m_AttackingAnimator.SetBool("IsAttacking", true);
        // m_Timer = 0;
    }
    public void RefreshArmyData(PlayerHeroData playerHeroData)
    {
        clickOnList = new List<ClickOn>();
        for (int i = 0; i < ArmyParent.childCount; i++)
        {
            Destroy(ArmyParent.GetChild(i).gameObject);
        }
        m_heroIndex = playerHeroData.ID - 1;

        m_PlayerHeroData = playerHeroData;
        armies = m_PlayerHeroData.GetAllArmiesSelf();
        _heroArmyDataRef = m_PlayerHeroData.GetEquippedArmySelf();
        float constRot = 360f / armies.Count;
        for (int i = 0; i < armies.Count; i++)
        {
            GameObject ins = Instantiate(ArmyPlaceHolderPrefab, transform.position, Quaternion.identity);
            ins.transform.SetParent(ArmyParent, false);
            ins.transform.localRotation = Quaternion.Euler(0, i * constRot + 180, 0);
            ins.transform.localPosition = Vector3.zero;
            ins.transform.GetComponentInChildren<ClickOn>().InstantiateArmyObject(armies[i]);
            clickOnList.Add(ins.transform.GetComponentInChildren<ClickOn>());
        }
        float armyParentYRotation = 0;
        m_SelectedArmyIndex = 0;
        for (int i = 0; i < armies.Count; i++)
        {
            if (E_EquipableStatus.Equipped == (E_EquipableStatus)armies[i].HeroArmyStatusType)
            {
                armyParentYRotation = i * constRot;
                m_SelectedArmyIndex = i;
                break;
            }
        }
        RefreshUI(_heroArmyDataRef);
        ChangeScaleSelected();
        ArmyParentRotator.gameObject.transform.localEulerAngles = Vector3.up * -armyParentYRotation;
    }
    private void RefreshUI(HeroArmyData _hData)
    {
        _armyData = _hData.GetArmyData();
        // txtArmyName.text = _armyData.GetArmyName();
        bool isArmyLocked = (E_EquipableStatus.Locked == (E_EquipableStatus)_hData.HeroArmyStatusType);
        bool isArmyEquip = (E_EquipableStatus.Equipped == (E_EquipableStatus)_hData.HeroArmyStatusType);
        btnSoldierGo.gameObject.SetActive(!isArmyLocked);
        objArmLock.SetActive(isArmyLocked);
        if (isArmyEquip)
        {
            btnSoldierGo.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonSoldier[1]);
        }
        else
        {
            btnSoldierGo.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonSoldier[0]);
        }
        // SpotLightObject.SetActive(!isArmyLocked);

    }
    private void ChangeSoldierMenuTabTexture(int _index)
    {
        for (int i = 0; i < btnSoldierMenu.Length; i++)
        {
            if (i == _index)
            {
                btnSoldierMenu[i].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(soldierInfoTab[0]);
            }
            else
            {
                btnSoldierMenu[i].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(soldierInfoTab[1]);
            }
        }
    }
    private void ChangeScaleSelected()
    {
        for (int i = 0; i < clickOnList.Count; i++)
        {
            if (i == m_SelectedArmyIndex)
            {
                // clickOnList[i].IncreaseModelSize();
                clickOnList[i].armyUnlockLevel.SetActive(true);
                if (_armyData.ArmyMoveType == (byte)E_ArmyMoveType.Cavalry)
                {
                    clickOnList[i].armyUnlockLevel.transform.localPosition = new Vector3(0, 3.22f, 0);
                }
                else
                {
                    clickOnList[i].armyUnlockLevel.transform.localPosition = new Vector3(0, 2.3f, 0);
                }
            }
            else
            {
                // clickOnList[i].DecreaseModelSizeToNormal();
                clickOnList[i].armyUnlockLevel.SetActive(false);
            }
        }
    }
    private void ShowArmySkill(int _index)
    {
        for (int i = 0; i < btnArmySkill.Length; i++)
        {
            if (i == _index)
            {
                btnArmySkill[i].GetComponent<RectTransform>().sizeDelta = new Vector2(160, 77);
                btnArmySkill[i].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonTextureName[1]);
            }
            else
            {
                btnArmySkill[i].GetComponent<RectTransform>().sizeDelta = new Vector2(160, 55);
                btnArmySkill[i].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonTextureName[0]);
            }
        }
    }
    public void showUpgradeEffect()
    {
        go = Resources.Load("Effects/UIHeroDeckEffect/UPgradeEffect2") as GameObject;
        if (_armyData.ArmyMoveType == (byte)E_ArmyMoveType.Cavalry)
        {
            go.transform.localScale = Vector3.one * 1.45f;
        }
        else
        {
            go.transform.localScale = Vector3.one * 0.75f;
        }
        go = Instantiate(go);
        go.transform.SetParent(upgradeEffectTrans, false);
    }
    private void RotateArmyParent(E_Direction direction)
    {
        if (m_Rotating)
        {
            return;
        }
        float degrePerTurn = 360f / m_PlayerHeroData.GetAllArmiesSelf().Count;
        ArmyParentRotator.From = ArmyParentRotator.transform.localEulerAngles;
        switch (direction)
        {
            case E_Direction.Left:
                ArmyParentRotator.To = ArmyParentRotator.transform.localEulerAngles + (Vector3.up * degrePerTurn);
                break;

            case E_Direction.Right:
                ArmyParentRotator.To = ArmyParentRotator.transform.localEulerAngles - (Vector3.up * degrePerTurn);
                break;

            default:
                ArmyParentRotator.To = ArmyParentRotator.transform.localEulerAngles + (Vector3.up * degrePerTurn);
                break;

        }
        ArmyParentRotator.Duration = 0.5f;

        Debug.Log(string.Format("From {0} To {1}", ArmyParentRotator.From.ToString(), ArmyParentRotator.To.ToString()));
        ArmyParentRotator.TweenStart();
        BtnRight.interactable = false;
        BtnLeft.interactable = false;
        m_Rotating = true;
    }
    private void RotateFinish()
    {
        Debug.Log("RotateFinish");
        BtnRight.interactable = true;
        BtnLeft.interactable = true;
        m_Rotating = false;
    }
    private IEnumerator ShowWarningMsg()
    {
        objWarningMessage.SetActive(true);
        yield return new WaitForSeconds(1);
        objWarningMessage.SetActive(false);
    }
}

