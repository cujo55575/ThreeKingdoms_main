using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroGraphCanvas : MonoBehaviour
{
	public RectTransform TextBg;
	public RectTransform Text;

    public RawImage ImgFace;
    public Text SkillText;

	public bool oppositeDirection;
    private void Awake()
    {
        SkillText = Text.GetComponent<Text>();
    }
    public void RefreshUI(Texture FaceSprite,SkillData data)
	{
        ImgFace.texture = FaceSprite;
        SkillText.text = data.GetSkillName();
	}
	public void Update()
	{
		TextBg.sizeDelta = new Vector2(Text.sizeDelta.x + 125,175f);
		if (oppositeDirection)
		{
			TextBg.anchoredPosition = new Vector2(Text.sizeDelta.x / 2 + 150,Text.sizeDelta.y / 2 * -1);
		}
		else
		{
			TextBg.anchoredPosition = new Vector2(Text.sizeDelta.x / 2 * -1 - 150,Text.sizeDelta.y / 2 * -1);
		}
	}
}
