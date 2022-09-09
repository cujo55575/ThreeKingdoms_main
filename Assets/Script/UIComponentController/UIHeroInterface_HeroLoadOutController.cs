using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MagneticScrollView;

public class UIHeroInterface_HeroLoadOutController : MonoBehaviour
{
    public GameObject objTitlebar;
    public RawImage RwImgHero;
    public RawImage ImgKingdomType;
    public RawImage[] Stars;
    public Sprite[] KingdomTypeImages;
    public Text TxtHeroName;
    public Text TxtAtk;
    public Text TxtDef;
    public Text txtCombatPower;
    public Button BtnRight, BtnLeft, btnHero, btnCardScrollView, btnArmy, btnBack;
    public List<PlayerHeroData> _OwnedHeroCards;
    private enum E_Direction
    {
        Left, Right
    }
    public TweenPosition _heroLoadOutTweenPos;
    public TweenPosition _heroLoadOutButtons;
    public Transform _leftTransforHeroLoadOut;
    public Transform rightTransForHeroLoadOut;
    public Transform leftTransforHeroLoadOutBtns;
    public Transform rightTransForHeroLoadOutBtns;
    public MagneticScrollRect magneticScrollRect;
    public static UIHeroInterface_HeroLoadOutController _heroLoadOutController;
    public bool isMainUIHeroDeck = false;
    public int uiIndex = 0;
    public int heroIndex = 0;
    private string[] ButtonArmyName = new string[2];
    private void Awake()
    {
        _heroLoadOutController = this;
    }
    private void Start()
    {
        ButtonArmyName = new string[] { "btn_soldier", "btn_soldier(d)" };

        btnHero.gameObject.SetActive(true);
        btnCardScrollView.gameObject.SetActive(false);
        btnArmy.gameObject.SetActive(false);
        BtnLeft.gameObject.SetActive(false);
        BtnRight.gameObject.SetActive(false);
        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        if (BtnLeft != null || BtnRight != null)
        {
            BtnLeft.onClick.AddListener(delegate
            {
                OnClickChangeSelectedHero(E_Direction.Left);
            });
            BtnRight.onClick.AddListener(delegate
            {
                OnClickChangeSelectedHero(E_Direction.Right);
            });
        }
        btnBack.onClick.AddListener(CloseUI);
        btnHero.onClick.AddListener(ShowHero);
        btnArmy.onClick.AddListener(ShowArmy);
        btnCardScrollView.onClick.AddListener(ShowUICardScrollView);

        RefreshHeroLoadOutData(_OwnedHeroCards[heroIndex]);
    }
    private void CloseUI()
    {
        UIManager.UIInstance<UIHeroInterface>().refreshPlayerHerdDataList();
        moveUICardBacktoRight(true);
        UIManager.Instance.CloseUI(GLOBALCONST.UI_HeroInterface);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
        PlayerDataManager.Instance.SavePlayerData();
    }
    private void ShowUICardScrollView()
    {
        btnTextureChange(0);
        if (uiIndex != 0 && uiIndex == 3)
        {
            StartCoroutine(delayButtons(1f));
        }
        if (uiIndex != 0 && uiIndex == 2)
        {

            StartCoroutine(delayButtons(0.4f));
        }
        uiIndex = 1;
        magneticScrollRect.ScrollRight(0);
        moveUICardBacktoRight(isMainUIHeroDeck);
    }
    public void cardScrollViewPanel()
    {
        objTitlebar.SetActive(true);
        btnHero.gameObject.SetActive(true);
        btnArmy.gameObject.SetActive(false);
        btnCardScrollView.gameObject.SetActive(false);
    }
    public void HeroPanel()
    {
        objTitlebar.SetActive(false);
        btnHero.gameObject.SetActive(false);
        btnArmy.gameObject.SetActive(true);
        btnCardScrollView.gameObject.SetActive(true);
    }
    private void ShowHero()
    {
        uiIndex = 2;
        btnTextureChange(1);
        StartCoroutine(delayButtons(0.5f));
        magneticScrollRect.ScrollLeft(1);
        moveUICard(isMainUIHeroDeck);
    }
    private void ShowArmy()
    {
        btnTextureChange(2);
        if (uiIndex == 1 || uiIndex == 0)
        {
            StartCoroutine(delayButtons(1.1f));
        }
        else if (uiIndex == 2)
        {
            StartCoroutine(delayButtons(0.4f));

        }
        uiIndex = 3;
        magneticScrollRect.ScrollLeft(2);
        moveUICard(isMainUIHeroDeck);
    }
    public void btnTextureChange(int _index)
    {
        if (_index == 0)
        {
            BtnRight.gameObject.SetActive(false);
            BtnLeft.gameObject.SetActive(false);
            btnArmy.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(ButtonArmyName[0]);
        }
        else if (_index == 1)
        {
            BtnRight.gameObject.SetActive(true);
            BtnLeft.gameObject.SetActive(true);
            btnArmy.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(ButtonArmyName[0]);
        }
        else if (_index == 2)
        {
            BtnRight.gameObject.SetActive(true);
            BtnLeft.gameObject.SetActive(true);
            btnArmy.GetComponent<RawImage>().texture = UIManager.UIInstance<UIHeroInterface>().GetTextureFromResource(ButtonArmyName[1]);
        }
    }
    public void moveUICard(bool _isMainUIHeroDeck)
    {
        if (_isMainUIHeroDeck == false)
        {
            HeroPanel();
            _heroLoadOutTweenPos.From = rightTransForHeroLoadOut.localPosition;
            _heroLoadOutTweenPos.To = _leftTransforHeroLoadOut.localPosition;
            _heroLoadOutTweenPos.Duration = 0.3f;
            _heroLoadOutTweenPos.TweenStart();

            _heroLoadOutButtons.From = rightTransForHeroLoadOutBtns.localPosition;
            _heroLoadOutButtons.To = leftTransforHeroLoadOutBtns.localPosition;
            _heroLoadOutButtons.Duration = 0.3f;
            _heroLoadOutButtons.TweenStart();
            isMainUIHeroDeck = true;
        }
    }
    public void moveUICardBacktoRight(bool _isMainUIHeroDeck)
    {
        if (_isMainUIHeroDeck == true)
        {
            cardScrollViewPanel();
            _heroLoadOutTweenPos.From = _leftTransforHeroLoadOut.localPosition;
            _heroLoadOutTweenPos.To = rightTransForHeroLoadOut.localPosition;
            _heroLoadOutTweenPos.Duration = 0.3f;
            _heroLoadOutTweenPos.TweenStart();

            _heroLoadOutButtons.From = leftTransforHeroLoadOutBtns.localPosition;
            _heroLoadOutButtons.To = rightTransForHeroLoadOutBtns.localPosition;
            _heroLoadOutButtons.Duration = 0.3f;
            _heroLoadOutButtons.TweenStart();
            isMainUIHeroDeck = false;
        }
    }
    public void RefreshHeroLoadOutData(PlayerHeroData playerHeroData)
    {
        heroIndex = playerHeroData.ID - 1;
        TxtHeroName.text = playerHeroData.GetHeroName();
        HeroData heroData = playerHeroData.GetHeroData();
        RwImgHero.texture = heroData.GetHeroTexture();
        AttributeData attributeData = playerHeroData.GetHeroAttribute();
        TxtAtk.text = attributeData.Atk.ToString();
        TxtDef.text = attributeData.Def.ToString();
        ImgKingdomType.texture = KingdomTypeImages[(int)heroData.KingdomID].texture;//need to fix some code
        txtCombatPower.text = playerHeroData.GetCombatPowerSelf().ToString();

        //StarBars refresh
        for (int i = 0; i < Stars.Length; i++)
        {
            if (i < playerHeroData.HeroLevel - 1)
            {
                Stars[i].gameObject.SetActive(true);
            }
            else
            {
                Stars[i].gameObject.SetActive(false);
            }
        }

    }
    private void OnClickChangeSelectedHero(E_Direction direction)
    {
        switch (direction)
        {
            case E_Direction.Right:
                heroIndex++;
                break;
            case E_Direction.Left:
                heroIndex--;
                break;
        }
        if (heroIndex >= _OwnedHeroCards.Count)
        {
            heroIndex = 0;
        }
        else if (heroIndex < 0)
        {
            heroIndex = _OwnedHeroCards.Count - 1;
        }
        UIManager.UIInstance<UIHeroInterface>()._heroIndex = heroIndex;
        RefreshHeroLoadOutData(_OwnedHeroCards[heroIndex]);
        UIHeroInterface_SkillController._SkillController.RefreshSkillData(_OwnedHeroCards[heroIndex], _OwnedHeroCards[heroIndex].GetHeroData().GetSkills()[0]);
        List<HeroArmyData> _heroArmies = new List<HeroArmyData>();
        _heroArmies = _OwnedHeroCards[heroIndex].GetAllArmiesSelf();
        for (int i = 0; i < _heroArmies.Count; i++)
        {
            if (E_EquipableStatus.Equipped == (E_EquipableStatus)_heroArmies[i].HeroArmyStatusType)
            {
                UIManager.UIInstance<UIHeroInterface>()._heroArmyIndex = i;
                break;
            }
        }
        UIHeroInterface_ArmyController._ArmyController.RefreshArmyData(_OwnedHeroCards[heroIndex]);
    }
    IEnumerator delayButtons(float delayTime)
    {
        btnBack.interactable = false;
        BtnLeft.interactable = false;
        BtnRight.interactable = false;
        btnArmy.interactable = false;
        btnHero.interactable = false;
        btnCardScrollView.interactable = false;
        yield return new WaitForSeconds(delayTime);
        BtnLeft.interactable = true;
        BtnRight.interactable = true;
        btnArmy.interactable = true;
        btnHero.interactable = true;
        btnBack.interactable = true;
        btnCardScrollView.interactable = true;
    }
}
