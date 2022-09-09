using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void MessageCallback(bool sure, params object[] param);

public class UIMessageBox : UIBase
{
    public GameObject YesNoView;
    public GameObject YesView;

    public Text txtContent;
    // public Text txtComingSoon;
    public Text txtConfirm;

	public Text txtYesButtom;
	public Text txtNoButtom;

    private static MessageCallback m_BtnYesEvent;
    private static object[] m_Params;

    protected override void OnShow(params object[] Objects)
    {

        CloseAllViews();

        base.OnShow(Objects);

        string content = (string)Objects[0];
        if(content==string.Empty)
            txtContent.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.COMINGSOON);
        else
            txtContent.text = content;
        txtConfirm.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);

		txtYesButtom.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_BUTTON);
		txtNoButtom.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIMatchMaking_CANCELBTN);

        E_MessageBox type = (E_MessageBox)Objects[1];
        switch (type)
        {
            case E_MessageBox.YesNo:
				txtContent.text = content;
                YesNoView.SetActive(true);
                break;

            case E_MessageBox.Yes:
                YesView.SetActive(true);
                break;
        }
    }
    void CloseAllViews()
    {
        YesNoView.SetActive(false);
        YesView.SetActive(false);
    }
    public static void ShowMessageBox(string Content, E_MessageBox enumMessageBox, MessageCallback yesCallback = null, params object[] param)
    {
        m_BtnYesEvent = yesCallback;
        m_Params = param;
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MESSAGEBOX, Content, enumMessageBox);
    }
    public void CallYesFunction()
    {
        if (m_BtnYesEvent != null)
        {
            m_BtnYesEvent(true, m_Params);
        }
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MESSAGEBOX);
    }
    public void CallNoFunction()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_MESSAGEBOX);
    }
}
