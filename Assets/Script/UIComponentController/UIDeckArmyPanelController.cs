using UnityEngine;

public class UIDeckArmyPanelController : MonoBehaviour
{

    //public RawImage ImgArmyNameBG;
    //public RawImage ImgArmyType;
    //public Text ArmyName;
    //public Text ArmyLevel;
    //public Text txtCombatPower;
    public Transform ModelParent;
    private GameObject m_ModelObject;
    private HeroArmyData m_HeroArmyData;
    public HeroArmyData HeroArmyData
    {
        get => m_HeroArmyData;
        set => m_HeroArmyData = value;
    }

    //  public void RefreshData(PlayerHeroData _playerHeroData, HeroArmyData heroArmyData, NpcData npcData = null)
    //  {
    //      for (int i = 0; i < ModelParent.childCount; i++)
    //      {
    //          Destroy(ModelParent.GetChild(i).gameObject);
    //      }
    //ArmyData armyData;
    //if (npcData == null)
    //{
    //	HeroArmyData = heroArmyData;
    //	//ArmyLevel.text = string.Format("Lv {0}",HeroArmyData.ArmyLevel.ToString());
    //	//txtCombatPower.text = _playerHeroData.GetCombatPowerSelf().ToString();
    //	armyData = HeroArmyData.GetArmyData();
    //}
    //else
    //{
    //	armyData = npcData.GetArmyData();
    //	//ArmyLevel.text = string.Format("Lv {0}",npcData.ArmyLevel.ToString());
    //}
    //      //ArmyName.text = armyData.GetArmyName();
    //      GameObject armyModel = armyData.GetModelObject();
    //      m_ModelObject = Instantiate(armyModel, ModelParent);

    //      Vector3 position;
    //      Vector3 rotation;
    //      Vector3 scale;

    //      switch ((E_ArmyType)armyData.ArmyType)
    //      {
    //          case E_ArmyType.Footman:
    //              position = new Vector3(0, 0, 20);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          case E_ArmyType.Archer:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          case E_ArmyType.Horseman:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 65f;
    //              break;
    //          case E_ArmyType.Jiman:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          case E_ArmyType.HeavyFootman:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          case E_ArmyType.HeavyBowman:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          //horse man with hoodie;
    //          case E_ArmyType.Xianbeibowman:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 65f;
    //              break;
    //          case E_ArmyType.Jrider:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 55f;
    //              break;
    //          case E_ArmyType.HeavyRider:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 55f;
    //              break;
    //          case E_ArmyType.YulinArcher:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          case E_ArmyType.BasicFootman:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          case E_ArmyType.Xilianrider:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //          default:
    //              position = new Vector3(0, 0, 0);
    //              rotation = Vector3.up * 140f;
    //              scale = Vector3.one * 80f;
    //              break;
    //      }
    //      //ImgArmyType.texture = armyData.GetArmyJobTypeTexture();
    //      //ImgArmyNameBG.texture = armyData.GetArmyJobTypeTitleTexture();



    //      m_ModelObject.transform.localPosition = position;
    //      m_ModelObject.transform.rotation = Quaternion.Euler(rotation);
    //      m_ModelObject.transform.localScale = scale;

    //      SetLayerRecursively(m_ModelObject, 5);
    //  }
    public void RefreshData(HeroArmyData heroArmyData)
    {
        for (int i = 0; i < ModelParent.childCount; i++)
        {
            Destroy(ModelParent.GetChild(i).gameObject);
        }
        ArmyData armyData;
            HeroArmyData = heroArmyData;
            armyData = HeroArmyData.GetArmyData();
        GameObject armyModel = armyData.GetModelObject();
        m_ModelObject = Instantiate(armyModel, ModelParent);

        Vector3 position;
        Vector3 rotation;
        Vector3 scale;

        switch ((E_ArmyType)armyData.ArmyType)
        {
            case E_ArmyType.Footman:
                position = new Vector3(0, 0, 20);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.Archer:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.Horseman:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 65f;
                break;
            case E_ArmyType.Jiman:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.HeavyFootman:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.HeavyBowman:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            //horse man with hoodie;
            case E_ArmyType.Xianbeibowman:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 65f;
                break;
            case E_ArmyType.Jrider:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 55f;
                break;
            case E_ArmyType.HeavyRider:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 55f;
                break;
            case E_ArmyType.YulinArcher:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.BasicFootman:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.Xilianrider:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
            case E_ArmyType.EleRider:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 30f;
                break;

            default:
                position = new Vector3(0, 0, 0);
                rotation = Vector3.up * 140f;
                scale = Vector3.one * 80f;
                break;
        }
        //ImgArmyType.texture = armyData.GetArmyJobTypeTexture();
        //ImgArmyNameBG.texture = armyData.GetArmyJobTypeTitleTexture();



        m_ModelObject.transform.localPosition = position;
        m_ModelObject.transform.rotation = Quaternion.Euler(rotation);
        m_ModelObject.transform.localScale = scale;

        SetLayerRecursively(m_ModelObject, 8);
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
