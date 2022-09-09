using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEResources;

public class ItemDeckCard : MonoBehaviour
{
    public RawImage ImgCard;
    //public Image[] _reddot;
    public Sprite[] KingdomImages;
    public Image ImgCampType;
    //public Image reddot;
    //public Text TxtAmount;
    public Text txtHeroName;
    public Text txtHeroLevel;
    // public Text txtCombatPower;
    //public Text txtCombatPowerForSpecificitem;
    //public Button BtnCard;
    public PlayerHeroData PlayerHeroData;
    public CardCountData m_CardCountData;
    public E_KingdomType KingdomType;
    //public GameObject selectedObj;
    void Start()
    {
        //BtnCard.onClick.AddListener(SelectedItem);
    }
    //public void SelectedItem()
    //{
    //    selectedObj.SetActive(true);
    //}
    //public void unSelectedItem()
    //{
    //    selectedObj.SetActive(false);
    //}
    public void UpdateData(PlayerHeroData playerHeroData)
    {
        if (playerHeroData == null)
        {
            Debug.LogError("PlayerHeroData Null in ItemDeckCard.cs Line 34");
        }
        PlayerHeroData = playerHeroData;
        KingdomType = (E_KingdomType)PlayerHeroData.GetHeroData().KingdomID;
        HeroData heroData = PlayerHeroData.GetHeroData();
        ImgCampType.sprite = KingdomImages[(int)heroData.KingdomID];
        ImgCard.texture = heroData.GetHeroTexture();
        //TxtAmount.text = "x " + playerHeroData.FragmentCount;
        txtHeroName.text = PlayerHeroData.GetHeroName();
        //txtCombatPowerForSpecificitem.text = playerHeroData.GetCombatPowerSelf().ToString();
        HeroArmyData equippedArmy = playerHeroData.GetEquippedArmySelf();
        if (equippedArmy == null)
        {
			Common.Player.PlayerData data = PlayerDataManager.Instance.PlayerData;
            Debug.LogError(string.Format("EquippedArmy Null for PlayerHeroDataID = {0}", playerHeroData.key()));
        }
        else
        {
            //txtHeroLevel.text = equippedArmy.ArmyLevel.ToString();
            txtHeroLevel.text = playerHeroData.HeroLevel.ToString();
        }
        // txtCombatPower.text = PlayerHeroData.GetEquippedArmy().ArmyLevel.ToString();
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
        //if (playerHeroData.FragmentCount >= m_CardCountData.GetCostByLevel(playerHeroData.HeroLevel) && !playerHeroData.isHeroAtMaxLevel())
        //{
        //    reddot.gameObject.SetActive(true);
        //}
        //else
        //{
        //    reddot.gameObject.SetActive(false);
        //}
    }

}
