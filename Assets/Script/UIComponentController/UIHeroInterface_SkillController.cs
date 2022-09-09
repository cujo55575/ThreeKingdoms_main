using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroInterface_SkillController : MonoBehaviour
{
    public Button[] btnNormalSkill = new Button[3];
    public Button btnSkillLevelUp;
    public Button btnCancel;
    public Button btnOk;
    public GameObject[] objskillNameMask = new GameObject[3];
    public GameObject objHeroCard_skillPanel;
    public GameObject objPanelMask;
    public GameObject objSkillUnlockTips;
    public GameObject objWindowTips2;
    public GameObject objWindowTips3;
    public GameObject objHideWhenLevelUpPanel;
    public GameObject objJungling_Levelup;
    public GameObject objCardtips_LevelUp;
    public RawImage imgCardIconNormal;
    public RawImage imgCardIconLevelUp;
    public Text txtSkillUnlockTips;
    public Text txtSkillDescription;
    public Text txtCardNumber;
    public Text txtJunglingNormal;
    public Text txtJungling_LevelUP;
    public Text txtTipsTitle;
    public static UIHeroInterface_SkillController _SkillController;
    private PlayerHeroData _playerHeroDataRef;
    private string[] buttonTextureName = new string[3];
    private string[] buttonLevelUp = new string[2];
    private void Awake()
    {
        _SkillController = this;
    }
    private void Start()
    {
        buttonTextureName = new string[] { "btn_skill", "btn_skill(c)", "btn_skill_mask(c)" };
        buttonLevelUp = new string[] { "btn_cardup", "btn_cardup(d)" };

        objWindowTips2.SetActive(false);
        objWindowTips3.SetActive(false);
        objCardtips_LevelUp.SetActive(false);
        objJungling_Levelup.SetActive(false);
        objHideWhenLevelUpPanel.SetActive(true);
        btnOk.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(false);
        objskillNameMask[0].SetActive(false);
        objskillNameMask[1].SetActive(false);
        objskillNameMask[2].SetActive(false);

        ShowNormalSkill(0);

        btnNormalSkill[0].onClick.AddListener(delegate
        {
            ShowNormalSkill(0);
        });
        btnNormalSkill[1].onClick.AddListener(delegate
        {
            ShowNormalSkill(1);
        });
        btnNormalSkill[2].onClick.AddListener(delegate
        {
            ShowNormalSkill(2);
        });
        btnSkillLevelUp.onClick.AddListener(delegate
        {
            if (btnSkillLevelUp.GetComponent<RawImage>().texture == UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonLevelUp[0]))
            {
                btnSkillLevelUp.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonLevelUp[1]);
            }
            else
            {
                btnSkillLevelUp.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonLevelUp[0]);
            }
        });
        btnSkillLevelUp.onClick.AddListener(delegate
        {
            objHideWhenLevelUpPanel.SetActive(false);
            StartCoroutine(showWindowTips2());
            btnOk.gameObject.SetActive(true);
            btnCancel.gameObject.SetActive(true);
            objJungling_Levelup.SetActive(true);
            objCardtips_LevelUp.SetActive(true);
            objCardtips_LevelUp.GetComponentInChildren<Text>().text = "使否提升至武魂四";
        });
        btnOk.onClick.AddListener(delegate
     {
         objHideWhenLevelUpPanel.SetActive(true);
         StartCoroutine(showWindowTips3());
         btnOk.gameObject.SetActive(false);
         btnCancel.gameObject.SetActive(false);
         objJungling_Levelup.SetActive(false);
         objCardtips_LevelUp.SetActive(false);
     });
        btnCancel.onClick.AddListener(delegate
   {
       objHideWhenLevelUpPanel.SetActive(true);
       btnOk.gameObject.SetActive(false);
       btnCancel.gameObject.SetActive(false);
       objJungling_Levelup.SetActive(false);
       objCardtips_LevelUp.SetActive(false);
   });
    }
    private void ShowNormalSkill(int _index)
    {
        for (int i = 0; i < btnNormalSkill.Length; i++)
        {
            if (i == _index)
            {
                btnNormalSkill[i].GetComponent<RectTransform>().sizeDelta = new Vector2(178, 72);
                RefreshSkillData(_playerHeroDataRef, _playerHeroDataRef.GetHeroData().GetSkills()[i]);
                btnNormalSkill[i].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonTextureName[1]);
            }
            else
            {
                btnNormalSkill[i].GetComponent<RectTransform>().sizeDelta = new Vector2(170, 46);
                btnNormalSkill[i].GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(buttonTextureName[0]);
            }
        }
    }
    public void RefreshSkillData(PlayerHeroData _playerHeroData, SkillData _skillData)
    {
        _playerHeroDataRef = _playerHeroData;
        imgCardIconLevelUp.texture = _playerHeroData.GetHeroData().GetHeroTexture();
        imgCardIconNormal.texture = _playerHeroData.GetHeroData().GetHeroTexture();
        List<SkillData> _skillDataList = _playerHeroData.GetHeroData().GetSkills();
        for (int i = 0; i < btnNormalSkill.Length; i++)
        {
            btnNormalSkill[i].GetComponentInChildren<Text>().text = _skillDataList[i].GetSkillName();
            if (_playerHeroDataRef.HeroLevel >= _skillDataList[i].UnlockLevel)
            {
                objskillNameMask[i].SetActive(false);
            }
            else
            {
                objskillNameMask[i].SetActive(true);
            }
            // txtSkillDescription.text = _skillDataList[i].
        }
        if (_playerHeroData.HeroLevel >= _skillData.UnlockLevel)
        {
            objPanelMask.SetActive(false);
            objSkillUnlockTips.SetActive(false);
        }
        else
        {
            objPanelMask.SetActive(true);
            objSkillUnlockTips.SetActive(true);
            string _skillLevelText = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIHEROCONFIG_UNLOCKSKILL_LEVEL);
            txtSkillUnlockTips.text = string.Format(_skillLevelText, _skillData.UnlockLevel);
        }
    }
    private IEnumerator showWindowTips2()
    {
        objWindowTips3.SetActive(true);
        objWindowTips3.GetComponentInChildren<Text>().text = "武魂已達最高階數!";
        yield return new WaitForSeconds(1);
        objWindowTips3.SetActive(false);
    }
    private IEnumerator showWindowTips3()
    {
        objWindowTips2.SetActive(true);
        objHideWhenLevelUpPanel.SetActive(false);
        btnOk.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(false);
        objSkillUnlockTips.gameObject.SetActive(false);
        objJungling_Levelup.SetActive(false);
        objHeroCard_skillPanel.SetActive(false);
        objWindowTips2.GetComponentInChildren<Text>().text = "武魂已達最高階數!";
        yield return new WaitForSeconds(2);
        objHideWhenLevelUpPanel.SetActive(true);
        objHeroCard_skillPanel.SetActive(true);
        objWindowTips2.SetActive(false);
    }
}
