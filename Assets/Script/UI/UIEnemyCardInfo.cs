using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyCardInfo : UIBase
{
    public UICard card;
    public Text txtCombatPower;

    private PlayerHeroData m_PlayerHeroData;
    public UIDeckArmyPanelController armyPanel;

	private NpcData m_NpcData;

    protected override void OnInit()
    {
        base.OnInit();
    }
	protected override void OnShow(params object[] Objects)
	{
		base.OnShow(Objects);
		m_PlayerHeroData = null;
		if (GLOBALVALUE.CURRENT_MATCH_MODE == E_MatchMode.FriendlyMatch || GLOBALVALUE.CURRENT_MATCH_MODE==E_MatchMode.RankedMatch)
		{
			ShowData((PlayerHeroData)Objects[0],(HeroArmyData)Objects[1]);
		}
		else
		{
			ShowData((NpcData)Objects[0]);
		}
    }

	public void ShowData(NpcData npcData)
	{
		//if (m_NpcData == npcData)
		//{
		//	Debug.Log("This called. XXXXXXXXXXX");
		//	UIManager.Instance.CloseUI(GLOBALCONST.UI_ENEMYCARDINFO);
		//	return;
		//}
		m_NpcData = npcData;
		card.RefreshData(m_NpcData);
		txtCombatPower.text = m_NpcData.GetNPCCombatPower().ToString();
		armyPanel.RefreshData(null);//Super Out Tan Sar Code
	}
    public void ShowData(PlayerHeroData data,HeroArmyData heroArmyData=null)
    {
        if(m_PlayerHeroData==data)
        {
            UIManager.Instance.CloseUI(GLOBALCONST.UI_ENEMYCARDINFO);
            return;
        }
        m_PlayerHeroData = data;
        card.RefreshData(m_PlayerHeroData);
        txtCombatPower.text = data.GetCombatPower().ToString();
		if (heroArmyData != null)
		{
			armyPanel.RefreshData(heroArmyData);
		}
		else
		{
			armyPanel.RefreshData(m_PlayerHeroData.GetEquippedArmy());
		}
        
    }

}

