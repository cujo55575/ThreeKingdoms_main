using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRareUpConfirmPanelController : MonoBehaviour
{
	public UIRareUp RareUpReference;
	public Text ConfirmTitle;
	public Text Content;
	public Text BtnOkayDescription;
	public Text BtnCancelDescription;

	public Button BtnOkay;
	public Button BtnCancel;
	public Button BtnClose;

	private const int CONFIRM_TITLE_STRING_ID = 60002;
	private const int CONFIRM_CONTENT_STRING_ID = 60003;

    void Start()
    {
		BtnOkay.onClick.AddListener(() => OnClickConfirm());
		BtnCancel.onClick.AddListener(() => OnClickCancel());
		BtnClose.onClick.AddListener(() => OnClickCancel());

		ConfirmTitle.text = TableManager.Instance.LocaleStringDataTable.GetString(CONFIRM_TITLE_STRING_ID);
		Content.text = TableManager.Instance.LocaleStringDataTable.GetString(CONFIRM_CONTENT_STRING_ID);
		BtnOkayDescription.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CONFIRM_STRING_ID);
		BtnCancelDescription.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.CANECL_STRING_ID);
    }
	
	private void OnClickConfirm()
	{
		RareUpReference.UpgradeConfirm();
		OnClickCancel();
	}

	private void OnClickCancel()
	{
		gameObject.SetActive(false);
	}

	public void ShowConfrimPanel()
	{
		gameObject.SetActive(true);
	}
}
