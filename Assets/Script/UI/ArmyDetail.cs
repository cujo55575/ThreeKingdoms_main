using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyDetail : MonoBehaviour
{
    public Text TxtArmyName;
    public Text TxtLevel;
    public Text TxtHp;
    public Text TxtHpValue;
    public Text TxtAtk;
    public Text TxtAtkValue;
    public Text TxtDef;
    public Text TxtDefValue;
    public Text TxtASpeed;
    public Text TxtASpeedValue;
    public Text TxtCritical;
    public Text TxtCriticalValue;
    public Text TxtCriticalDef;
    public Text TxtCriticalDefValue;
    public Text TxtPassiveSkill;
    public Text TxtMoveSpeed;
    public Text TxtMoveSpeedValue;
    public Text TxtAtkRange;
    public Text TxtAtkRangeValue;

    public RawImage ImgPassiveSkill;


    public void RefreshData(HeroArmyData data)
    {

        TxtArmyName.text = data.GetArmyData().GetArmyName();
        TxtLevel.text = string.Format("Lv {0}", data.ArmyLevel.ToString());
        AttributeData attributeData = data.GetArmyAttribute(data.ArmyLevel);
        TxtHpValue.text = attributeData.Hp.ToString();
        TxtAtkValue.text = attributeData.Atk.ToString();
        TxtDefValue.text = attributeData.Def.ToString();
        TxtASpeedValue.text = attributeData.AtkSpeed.ToString();
        TxtCriticalValue.text = attributeData.Crit.ToString();
        TxtCriticalDefValue.text = attributeData.CritDef.ToString();
        TxtMoveSpeedValue.text = attributeData.MoveSpeed.ToString();
        TxtAtkRangeValue.text = attributeData.AtkRange.ToString();

        TxtHp.text = "HP";
        TxtAtk.text = "ATK";
        TxtDef.text = "DEF";
        TxtASpeed.text = "A-Speed";
        TxtCritical.text = "Critical";
        TxtCriticalDef.text = "Ciritcal Def";
        TxtMoveSpeed.text = "Attack Speed";
        TxtAtkRange.text = "Attack Range";
    }
}
