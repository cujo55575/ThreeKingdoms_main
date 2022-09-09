using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEResources;

public class UICardInfo : UIBase {

	public UICard CardControl;
	public Text CardName;   
    public Button BtnClose;

	public Button BtnEquip;

	private PlayerHeroData m_CardData;

    protected override void OnInit()
    {
        base.OnInit();
        BtnClose.onClick.AddListener(delegate { OnClickClose();});
		BtnEquip.onClick.AddListener(delegate {	OnClickEquip();});
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
		m_CardData = (PlayerHeroData)Objects[0];
		CardControl.RefreshData(m_CardData);
		CardName.text = m_CardData.GetHeroName();
	}

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

	private void OnClickEquip()
	{
		OnClickClose();
	}

	private void OnClickClose()
	{
		m_CardData = null;
		UIManager.Instance.CloseUI(GLOBALCONST.UI_CARD_INFO);
	}
}
