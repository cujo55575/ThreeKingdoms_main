using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : UIBase
{
    public Button BtnBack;
	public UITopBar TopBar;

    protected override void OnInit()
    {
        base.OnInit();
        BtnBack.onClick.AddListener(CloseUI);
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
		TopBar.RefreshData();
		UIMessageBox.ShowMessageBox("Coming Soon!",E_MessageBox.Yes,CloseMessageCallback);

	}

	public void CloseMessageCallback(bool sure,object[] param)
	{
		Debug.Log("YesMessageCallbackCalled.");
		CloseUI();
	}

	private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_LEVEL);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
    }
}
