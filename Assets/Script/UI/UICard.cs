using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEResources;

public class UICard : MonoBehaviour
{
    public Text TxtHeroName;
    public RawImage RwImgHero;
    public Text TxtAtk;
    public Text TxtDef;
    public Image ImgKingdomType;
    public Sprite[] KingdomTypeImages;

    public RawImage ImgCardType;
    public Text TxtCardType;

    public Image[] Stars;
    public RawImage[] EquippedSkills;

    private Card m_CardData;

    public Card CardData { get => m_CardData; set => m_CardData = value; }

    private PlayerHeroData m_PlayerHeroData;
    public PlayerHeroData PlayerHeroData
    {
        get => m_PlayerHeroData; set => m_PlayerHeroData = value;
    }

	private NpcData m_NpcData;
	public NpcData NpcData
	{
		get => m_NpcData; set => m_NpcData = value;
	}

    public void RefreshData(PlayerHeroData playerHeroData)
    {
        PlayerHeroData = playerHeroData;
        TxtHeroName.text = PlayerHeroData.GetHeroName();
        HeroData heroData = PlayerHeroData.GetHeroData();
        RwImgHero.texture = heroData.GetHeroTexture();
        AttributeData attributeData = PlayerHeroData.GetHeroAttribute();
        TxtAtk.text = attributeData.Atk.ToString();
        TxtDef.text = attributeData.Def.ToString();
        ImgKingdomType.sprite = KingdomTypeImages[(int)heroData.KingdomID];

        //StarBars refresh
        for (int i = 0; i < Stars.Length; i++)
        {
            if (i < PlayerHeroData.HeroLevel - 1)
            {
                Stars[i].gameObject.SetActive(true);
            }
            else
            {
                Stars[i].gameObject.SetActive(false);
            }
        }

        /*//EquippedSkills refresh
		List<SkillData> equippedSkills = PlayerHeroData.GetEquippedSkills();
		//List<SkillData> equippedSkills = heroData.GetSkills();
		for (int i = 0; i < equippedSkills.Count; i++)
		{
			if(i<3)
			EquippedSkills[i].texture = equippedSkills[i].GetSkillTexture();
		}*/

        Dictionary<byte, SkillData> skills = PlayerHeroData.GetEquippedSkillsWithIndex();

        EquippedSkills[0].color = new Color32(255, 255, 255, 100);
        EquippedSkills[1].color = new Color32(255, 255, 255, 100);
        EquippedSkills[2].color = new Color32(255, 255, 255, 100);

        EquippedSkills[0].texture = null;
        EquippedSkills[1].texture = null;
        EquippedSkills[2].texture = null;

        SkillData skill1;
        skills.TryGetValue((byte)E_EquipableStatus.EquippedAt1, out skill1);
        if (skill1 != null)
        {
            EquippedSkills[0].texture = skill1.GetSkillTexture();
            EquippedSkills[0].color = Color.white;
        }
        SkillData skill2;
        skills.TryGetValue((byte)E_EquipableStatus.EquippedAt2, out skill2);
        if (skill2 != null)
        {
            EquippedSkills[1].texture = skill2.GetSkillTexture();
            EquippedSkills[1].color = Color.white;
        }
        SkillData skill3;
        skills.TryGetValue((byte)E_EquipableStatus.EquippedAt3, out skill3);
        if (skill3 != null)
        {
            EquippedSkills[2].texture = skill3.GetSkillTexture();
            EquippedSkills[2].color = Color.white;
        }

    }

	public void RefreshData(NpcData npcData)
	{
		NpcData = npcData;
		TxtHeroName.text = NpcData.GetNPCName();
		RwImgHero.texture = NpcData.GetNPCImage();
		AttributeData attributeData = NpcData.GetNPCAttribute();
		TxtAtk.text = attributeData.Atk.ToString();
		TxtDef.text = attributeData.Def.ToString();
		//ImgKingdomType.sprite = KingdomTypeImages[(int)heroData.KingdomID];

		//StarBars refresh
		for (int i = 0; i < Stars.Length; i++)
		{
			if (i < NpcData.NpcRare - 1)
			{
				Stars[i].gameObject.SetActive(true);
			}
			else
			{
				Stars[i].gameObject.SetActive(false);
			}
		}

		EquippedSkills[0].color = new Color32(255,255,255,100);
		EquippedSkills[1].color = new Color32(255,255,255,100);
		EquippedSkills[2].color = new Color32(255,255,255,100);

		EquippedSkills[0].texture = null;
		EquippedSkills[1].texture = null;
		EquippedSkills[2].texture = null;

		List<SkillData> skills = NpcData.GetSkills();
		for (int i = 0; i < skills.Count; i++)
		{
			EquippedSkills[i].texture = skills[i].GetSkillTexture();
			EquippedSkills[i].color = Color.white;
		}
	}
}
