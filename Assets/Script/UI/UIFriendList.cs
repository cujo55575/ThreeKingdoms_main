using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendList : UIBase
{
    public Button BtnClose;
    public GameObject PanelFriendList;
    public GameObject PanelRequest;
    public GameObject Popup;

    protected override void OnInit()
    {
        base.OnInit();
        BtnClose.onClick.AddListener(CloseUI);
        
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
		UIMessageBox.ShowMessageBox("Coming Soon!",E_MessageBox.Yes,CloseMessageCallback);
	}
	public void CloseMessageCallback(bool sure,object[] param)
	{
		Debug.Log("YesMessageCallbackCalled.");
		CloseUI();
	}

	public void TabChanged(Toggle m_toggle)
     {
        switch (m_toggle.name)
        {
            case "TabFriend":
                PanelFriendList.SetActive(true);
                PanelRequest.SetActive(false);
                break;
            case "TabRequest":
                PanelFriendList.SetActive(false);
                PanelRequest.SetActive(true);
                break;
        }
    }

    private void CloseUI()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_FRIEND_LIST);
    }

}
