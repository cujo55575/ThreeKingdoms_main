using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardDetailPanel : MonoBehaviour
{
    public UICard UICardObject;
    public Text txtArmy;
    public Button BtnSkill;
    public Button BtnRareUp;
    public Button BtnArmy;
    public Button BtnHero;
    public Text TxtLevel;
    private PlayerHeroData m_PlayerHeroData;
    public List<PlayerHeroData> _OwnedHeroCards;

    private void Awake()
    {
        txtArmy.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIDECK_ARMY);
        BtnSkill.onClick.AddListener(ShowSkill);
        BtnRareUp.onClick.AddListener(ShowRareUp);
        BtnArmy.onClick.AddListener(ShowArmy);
        BtnHero.onClick.AddListener(ShowHero);
    }

    public void RefreshData(PlayerHeroData cardData)
    {
        m_PlayerHeroData = cardData;
        UICardObject.RefreshData(cardData);
    }

    private void ShowSkill()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_DECK);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_SKILL, m_PlayerHeroData);
    }
    private void ShowRareUp()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_DECK);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_RAREUP, m_PlayerHeroData);
    }
    private void ShowArmy()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_DECK);
        if (m_PlayerHeroData != null)
        {
            UIManager.Instance.ShowUI(GLOBALCONST.UI_CHANGE_ARMY, m_PlayerHeroData);
        }
    }
    private void ShowHero()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_DECK);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_HeroConfig, m_PlayerHeroData, _OwnedHeroCards);
    }
}
