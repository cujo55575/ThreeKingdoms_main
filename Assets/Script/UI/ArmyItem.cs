using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyItem : MonoBehaviour
{
    public Text txtCurrentArmyTitle;
    public Text txtCurrentArmyLevel;
    public RawImage imgArmyTexture;
    public RawImage imgJobIcon;
    public RawImage imgTitleIcon;

    public List<Sprite> jobIconSprites;
    public List<Sprite> jobTitleSprites;

    private Material GrayscaleMat;
    public Shader GrayScaleShader;

    public GameObject mask;

    private ArmySwitchPanel armySwitchPanel;
    private void CreateMat()
    {
        Material mat = new Material(GrayScaleShader);
        GrayscaleMat = mat;

        imgJobIcon.material = GrayscaleMat;
        imgArmyTexture.material = GrayscaleMat;
        imgTitleIcon.material = GrayscaleMat;

    }
    private void Start()
    {
        armySwitchPanel = GetComponentInParent<ArmySwitchPanel>();
    }
    private HeroArmyData temp_data;
    private PlayerHeroData temp_PlayerheroData;
    public void RefreshData(PlayerHeroData data,int index)
    {
        if(GrayscaleMat==null)
        {
            CreateMat();
        }
        

        imgArmyTexture.texture =data.GetAllArmiesSelf()[index].GetArmyData().GetArmyTexture();
        txtCurrentArmyTitle.text = data.GetAllArmiesSelf()[index].GetArmyData().GetArmyName();
        txtCurrentArmyLevel.text = string.Format("Lv.{0}", data.GetAllArmiesSelf()[index].ArmyLevel);

        temp_data = data.GetAllArmiesSelf()[index];
        temp_PlayerheroData = data;

        imgJobIcon.texture = temp_data.GetArmyData().GetArmyJobTypeTexture();
        imgTitleIcon.texture = temp_data.GetArmyData().GetArmyJobTypeTitleTexture();
        if (data.GetAllArmiesSelf()[index].HeroArmyStatusType==(byte)E_EquipableStatus.Equipped)
        {
            GrayscaleMat.SetFloat("_EffectAmount", 1);
            mask.SetActive(true);
        }
        else
        {
            GrayscaleMat.SetFloat("_EffectAmount", 0);
            mask.SetActive(false);
        }
    }
    public void OnClick()
    {
        if(temp_data.HeroArmyStatusType==(byte)E_EquipableStatus.Equipped)
        {
            return;
        }
        temp_PlayerheroData.GetEquippedArmySelf().HeroArmyStatusType = (byte)E_EquipableStatus.Unlocked;
        temp_data.HeroArmyStatusType = (byte)E_EquipableStatus.Equipped;
        armySwitchPanel.battleCardInfo.SwitchModel(temp_data);
    }
}
