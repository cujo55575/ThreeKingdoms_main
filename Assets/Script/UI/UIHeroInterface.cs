using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHeroInterface : UIBase
{
    public Toggle[] choosePanel_toggle;
    public Button btnAttribute;
    public Button btnHighToLow;
    public Button btnChoosePanel_Cancel;
    public Button btnChoosePanel_Ok;
    public Button[] btnChoosePanel;
    public Text txtAttribute;
    public Text txtHighToLow;
    public GameObject PrefCardItem;
    public GameObject objBackgournd2;
    public GameObject objChoosePanel;
    public Transform CardScrollView;
    public List<PlayerHeroData> _OwnedHeroCards = new List<PlayerHeroData>();
    public int _heroIndex;
    public int _heroArmyIndex = 0;
    private string[] choose_unchoose = new string[2];
    protected override void OnInit()
    {
        choose_unchoose = new string[] { "btn_choose_atb", "btn_choose_atb(c)" };
        base.OnInit();
        PrefCardItem.SetActive(false);
        objBackgournd2.SetActive(false);
        objChoosePanel.SetActive(false);
        TextFiller();
        btnAttribute.onClick.AddListener(delegate
        {
            objChoosePanel.SetActive(true);
        });
        btnChoosePanel[0].onClick.AddListener(delegate
        {
            ChoosePanel_Buttons(0);
        });
        btnChoosePanel[1].onClick.AddListener(delegate
        {
            ChoosePanel_Buttons(1);
        });
        btnChoosePanel[2].onClick.AddListener(delegate
        {
            ChoosePanel_Buttons(2);
        });
        btnChoosePanel_Cancel.onClick.AddListener(delegate
        {
            objChoosePanel.SetActive(false);
        });
        btnChoosePanel_Ok.onClick.AddListener(delegate
        {
            objChoosePanel.SetActive(false);
        });
        choosePanel_toggle[0].onValueChanged.AddListener(delegate
        {
            if (choosePanel_toggle[0].isOn)
            {
                SwitchCards(E_KingdomType.Shu);
                choosePanel_toggle[1].isOn = false;
                choosePanel_toggle[2].isOn = false;
                choosePanel_toggle[3].isOn = false;
                choosePanel_toggle[4].isOn = false;
            }
        });
        choosePanel_toggle[1].onValueChanged.AddListener(delegate
      {
          if (choosePanel_toggle[1].isOn)
          {
              SwitchCards(E_KingdomType.Wei);
              choosePanel_toggle[0].isOn = false;
              choosePanel_toggle[2].isOn = false;
              choosePanel_toggle[3].isOn = false;
              choosePanel_toggle[4].isOn = false;
          }
      });
        choosePanel_toggle[2].onValueChanged.AddListener(delegate
        {
            if (choosePanel_toggle[2].isOn)
            {
                SwitchCards(E_KingdomType.Wu);
                choosePanel_toggle[0].isOn = false;
                choosePanel_toggle[1].isOn = false;
                choosePanel_toggle[3].isOn = false;
                choosePanel_toggle[4].isOn = false;
            }
        });
        choosePanel_toggle[3].onValueChanged.AddListener(delegate
      {
          if (choosePanel_toggle[3].isOn)
          {
              SwitchCards(E_KingdomType.Other);
              choosePanel_toggle[0].isOn = false;
              choosePanel_toggle[1].isOn = false;
              choosePanel_toggle[2].isOn = false;
              choosePanel_toggle[4].isOn = false;
          }
      });
        choosePanel_toggle[4].onValueChanged.AddListener(delegate
        {
            if (choosePanel_toggle[4].isOn)
            {
                refreshPlayerHerdDataList();
                RefreshCard(_OwnedHeroCards);
                choosePanel_toggle[0].isOn = false;
                choosePanel_toggle[1].isOn = false;
                choosePanel_toggle[2].isOn = false;
                choosePanel_toggle[3].isOn = false;
            }
        });
    }
    protected override void OnShow(params object[] Objects)
    {
        choosePanel_toggle[0].isOn = false;
        choosePanel_toggle[1].isOn = false;
        choosePanel_toggle[2].isOn = false;
        choosePanel_toggle[3].isOn = false;
        choosePanel_toggle[4].isOn = true;


        base.OnShow(Objects);
        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        RefreshCard(_OwnedHeroCards);
        SoundManager.Instance.PlayBGM("BGM00002");


    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        for (int i = 0; i < choosePanel_toggle.Length; i++)
        {
            if (choosePanel_toggle[i].isOn == true)
            {
                choosePanel_toggle[i].interactable = false;
            }
            else
            {
                choosePanel_toggle[i].interactable = true;
            }
        }
    }
    void SwitchCards(E_KingdomType _kingdomType)
    {
        _OwnedHeroCards = new List<PlayerHeroData>();
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; i++)
        {
            PlayerHeroData data = PlayerDataManager.Instance.PlayerData.OwnedHeros[i];
            if ((E_KingdomType)data.GetHeroData().KingdomID == _kingdomType)
            {
                _OwnedHeroCards.Add(data);
            }
        }
        for (int j = 0; j < _OwnedHeroCards.Count; j++)
        {
            _OwnedHeroCards[j].ID = j + 1;
        }
        RefreshCard(_OwnedHeroCards);

    }
    public void refreshPlayerHerdDataList()
    {
        _OwnedHeroCards = new List<PlayerHeroData>();
        _OwnedHeroCards = PlayerDataManager.Instance.PlayerData.OwnedHeros;
        for (int i = 0; i < _OwnedHeroCards.Count; i++)
        {
            _OwnedHeroCards[i].ID = i + 1;
        }
    }
    public void RefreshCard(List<PlayerHeroData> tempPlayerHeroDataList)
    {
        foreach (Transform child in CardScrollView)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < tempPlayerHeroDataList.Count; i++)
        {
            GameObject go = Instantiate(PrefCardItem);
            go.SetActive(true);
            go.transform.SetParent(CardScrollView, false);
            UIHeroInterface_HeroCard _heroCard = go.GetComponent<UIHeroInterface_HeroCard>();
            _heroCard.UpdateData(tempPlayerHeroDataList[i]);
        }
        UIHeroInterface_ArmyController._ArmyController._OwnedHeroCards = new List<PlayerHeroData>();
        UIHeroInterface_ArmyController._ArmyController._OwnedHeroCards = tempPlayerHeroDataList;
        UIHeroInterface_HeroLoadOutController._heroLoadOutController._OwnedHeroCards = new List<PlayerHeroData>();
        UIHeroInterface_HeroLoadOutController._heroLoadOutController._OwnedHeroCards = tempPlayerHeroDataList;
        UIHeroInterface_HeroLoadOutController._heroLoadOutController.RefreshHeroLoadOutData(UIHeroInterface_HeroLoadOutController._heroLoadOutController._OwnedHeroCards[0]);
        UIHeroInterface_ArmyController._ArmyController.RefreshArmyData(UIHeroInterface_ArmyController._ArmyController._OwnedHeroCards[0]);
        UIHeroInterface_SkillController._SkillController.RefreshSkillData(_OwnedHeroCards[0], _OwnedHeroCards[0].GetHeroData().GetSkills()[0]);
    }

    void TextFiller()
    {
        txtAttribute.text = "Attribute";
        txtHighToLow.text = "High To Low";
    }
    private void ChoosePanel_Buttons(int _index)
    {
        for (int i = 0; i < btnChoosePanel.Length; i++)
        {
            if (i == _index)
            {
                btnChoosePanel[i].GetComponent<RawImage>().texture = GetTextureFromResource(choose_unchoose[1]);
            }
            else
            {
                btnChoosePanel[i].GetComponent<RawImage>().texture = GetTextureFromResource(choose_unchoose[0]);
            }
        }
    }
    public Texture GetTextureFromResource(string playerImageName)
    {
        Texture texture = Resources.Load<Texture>("Image/New Images/" + playerImageName);
        return texture;
    }
}
