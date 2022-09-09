using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHeroInterface_HeroCard : MonoBehaviour
{
    public RawImage ImgCard;
    public Sprite[] KingdomImages;
    public RawImage[] Stars;
    public RawImage ImgCampType;
    private PlayerHeroData PlayerHeroData;
    private CardCountData m_CardCountData;
    private E_KingdomType KingdomType;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            if (PlayerHeroData != null)
            {
                Refresh_Skill_Army_Panel(PlayerHeroData);
            }
        });
    }
    public void UpdateData(PlayerHeroData playerHeroData)
    {
        PlayerHeroData = playerHeroData;
        KingdomType = (E_KingdomType)PlayerHeroData.GetHeroData().KingdomID;
        HeroData heroData = PlayerHeroData.GetHeroData();
        ImgCampType.texture = KingdomImages[(int)heroData.KingdomID].texture;
        ImgCard.texture = heroData.GetHeroTexture();
        HeroArmyData equippedArmy = playerHeroData.GetEquippedArmySelf();
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
        if (equippedArmy == null)
        {
            Common.Player.PlayerData data = PlayerDataManager.Instance.PlayerData;
            Debug.LogError(string.Format("EquippedArmy Null for PlayerHeroDataID = {0}", playerHeroData.key()));
        }
        else
        {
        }
        m_CardCountData = TableManager.Instance.CardCountDataTable.GetData(GLOBALCONST.FRAGMENT_COST_TABLEID);
    }
    public void Refresh_Skill_Army_Panel(PlayerHeroData data)
    {
        List<HeroArmyData> _heroArmies = data.GetAllArmiesSelf();
        UIManager.UIInstance<UIHeroInterface>()._heroIndex = data.ID - 1;
        UIHeroInterface_HeroLoadOutController._heroLoadOutController.RefreshHeroLoadOutData(data);
        UIHeroInterface_SkillController._SkillController.RefreshSkillData(data, data.GetHeroData().GetSkills()[0]);
        for (int i = 0; i < _heroArmies.Count; i++)
        {
            if (E_EquipableStatus.Equipped == (E_EquipableStatus)_heroArmies[i].HeroArmyStatusType)
            {
                UIManager.UIInstance<UIHeroInterface>()._heroArmyIndex = i;
                break;
            }
        }
        UIHeroInterface_ArmyController._ArmyController.RefreshArmyData(data);
    }
}
