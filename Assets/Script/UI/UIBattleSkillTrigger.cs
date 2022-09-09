using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSkillTrigger : UIBase
{
    public RawImage Icon;
    public Text PopupTextA;
	public Image Line1;
	public Image Line2;
	public Sprite TeamOneFrame;
	public Sprite TeamTwoFrame;
	public RectTransform PopUp;
	public Transform Frame;
	public Transform BlackLines;
    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        if(Objects[0]!=null)
        {
            UIBattleSkillTrigger.UIBatttleSkillTriggerParams para = (UIBattleSkillTrigger.UIBatttleSkillTriggerParams)Objects[0];
            Icon.texture = para.texture;
            PopupTextA.text = para.SkillName;
			if (para.teamType == Team.TeamType.TeamOne)
			{
				PopUp.anchoredPosition = new Vector2(-450,-290);
				Line1.sprite = TeamOneFrame;
				Line2.sprite = TeamOneFrame;
				Frame.localScale = new Vector3(1,1,1);
				BlackLines.localScale = new Vector3(1,1,1);
			}
			else
			{
				PopUp.anchoredPosition = new Vector2(450,-290);
				Line1.sprite = TeamTwoFrame;
				Line2.sprite = TeamTwoFrame;
				Frame.localScale = new Vector3(-1,1,1);
				BlackLines.localScale = new Vector3(-1,1,1);
			}
        }
		StopCoroutine("LineOne");
		StopCoroutine("LineTwo");

		Line1.fillAmount = 0;
		Line2.fillAmount = 0;

		StartCoroutine("LineOne");
		StartCoroutine("LineTwo");
    }
	IEnumerator LineOne()
	{
		float timer = 0.0f;
		while (timer < 1)
		{
			timer += Time.deltaTime * 4;
			Line1.fillAmount = timer;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		Line1.fillAmount = 1;
		yield return new WaitForSeconds(0.0f);
	}
	IEnumerator LineTwo()
	{
		yield return new WaitForSeconds(0.2f);
		float timer = 0.0f;
		while (timer < 1)
		{
			timer += Time.deltaTime * 4;
			Line2.fillAmount = timer;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		Line2.fillAmount = 1;
	}
	public class UIBatttleSkillTriggerParams
    {
        public Texture texture;
        public string SkillName;
		public Team.TeamType teamType;
    }
}
