using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroHPStatus : MonoBehaviour
{
    public UIBattleHPBar controller;
    public Leader leader;
    public RawImage Icon;

    public Text txtArmyName;
    public Text txtHeroName;

    public Mask mask;

    public RectTransform Background;
    public Material GrayscaleMat;

    public GameObject StatusObject;
    public Image FillImage;
    public GameObject DeadIcon;

    public PlayerHeroData HeroData;
    public Shader GrayScaleShader;

    private void Start()
    {
        Material mat = new Material(GrayScaleShader);
        GrayscaleMat = mat;
        Background.GetComponent<RawImage>().material=GrayscaleMat;
        Icon.material = GrayscaleMat;


    }
    public void InitData(Leader _leader,UIBattleHPBar _controller)
    {
        leader = _leader;
        Icon.texture = leader.FaceSprite;
        controller = _controller;
        txtHeroName.text = leader.HeroName;
        txtArmyName.text = leader.ArmyName;

        Background.GetComponent<RawImage>().texture=_leader.m_PlayerHeroData.GetEquippedArmySelf().GetArmyData().GetArmyJobTypeBGTexture();
    }
    private void Update()
    {
        if(controller==null)
        {
            return;
        }
        if (leader.HP>0)
        {   if(GrayscaleMat.GetFloat("_EffectAmount")!=0)
            {
                GrayscaleMat.SetFloat("_EffectAmount", 0);
                mask.enabled = false;
                mask.enabled = true;
            }
            
        }
        else
        {
            if (GrayscaleMat.GetFloat("_EffectAmount") != 1)
            {
                GrayscaleMat.SetFloat("_EffectAmount", 1);
                mask.enabled = false;
                mask.enabled = true;
            }
        }
        if(leader.HP<=0)
        {
            DeadIcon.SetActive(true);
        }
        else
        {
            DeadIcon.SetActive(false);
        }
        
        FillImage.fillAmount = leader.HP / leader.TotalHP;
    }
}
