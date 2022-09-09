using UnityEngine;
using UnityEngine.UI;

public class UICardForLeveUpAndRankUpPanel : MonoBehaviour
{
    public Text TxtHeroName;
    public Text txtHeroLevel;
    public Text txtHeroLevelForUIHeroLevelUPInterface;
    public Text TxtAtk;
    public Text TxtDef;
    public Text txtCombatPower;
    public Image ImgKingdomType;
    public Image[] Stars;
    public RawImage RwImgHero;
    public Sprite[] KingdomTypeImages;
    

    public void RefreshData(PlayerHeroData playerHeroData)
    {
        TxtHeroName.text = playerHeroData.GetHeroName();
        HeroData heroData = playerHeroData.GetHeroData();
        txtHeroLevel.text = playerHeroData.HeroLevel.ToString();
        if (txtHeroLevelForUIHeroLevelUPInterface != null)
        {
            txtHeroLevelForUIHeroLevelUPInterface.text = playerHeroData.HeroLevel.ToString();
        }
        RwImgHero.texture = heroData.GetHeroTexture();
        AttributeData attributeData = playerHeroData.GetHeroAttribute();
        TxtAtk.text = attributeData.Atk.ToString();
        TxtDef.text = attributeData.Def.ToString();
        ImgKingdomType.sprite = KingdomTypeImages[(int)heroData.KingdomID];
        if (txtCombatPower != null)
        {
            txtCombatPower.text = playerHeroData.GetCombatPowerSelf().ToString();
        }

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
}
