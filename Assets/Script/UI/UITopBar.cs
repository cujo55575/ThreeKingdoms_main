using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopBar : UIBase
{
    public Text txtBambooRoll;
    public Text txtBambooFrag;
    public Text txtBattlePoints;
    public Button bntBambooRollIncrease;
    public Button btnBambooFragIncrease;
    public Button btnBambooPointsIncrease;
    protected override void OnInit()
    {
        base.OnInit();
        RefreshData();
    }
    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
    }


    public void RefreshData()
    {
        if (txtBambooRoll != null)
        {
            txtBambooRoll.text = PlayerDataManager.Instance.PlayerData.BambooRoll.ToString();
        }
        if (txtBambooFrag != null)
        {
            txtBambooFrag.text = PlayerDataManager.Instance.PlayerData.BambooFragment.ToString();
        }
        if (txtBattlePoints != null)
        {
            txtBattlePoints.text = PlayerDataManager.Instance.PlayerData.BattlePoint.ToString();
        }

    }

    public void ShowOption()
    {
        UIManager.Instance.ShowUI(GLOBALCONST.UI_OPTION);
    }
}
