using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBattleStatus : MonoBehaviour
{
	public RawImage imgIcon;
	public Image FillImage;
	public Image FrameImage;
	public Text text;
	public GameObject Dialogue;

	public string[] texts;

	public Color TeamOneColor;
	public Color TeamTwoColor;

	public Sprite TeamOneFrame;
	public Sprite TeamTwoFrame;

	public Sprite TeamOneDialogueBG;
	public Sprite TeamTwoDialogueBG;

	private void Start()
	{
		//text.text = texts[Random.RandomRange(0,texts.Length)];
	}
	void Update()
    {
		
		transform.forward = Camera.main.transform.forward * -1;
		if (Dialogue.activeSelf)
		{
			Dialogue.GetComponent<RectTransform>().sizeDelta = text.GetComponent<RectTransform>().sizeDelta + new Vector2(150,150);
		}
    }
	public void ShowDialogue(string skillText)
	{
        text.text = skillText;
		Dialogue.SetActive(true);
		GetComponent<Canvas>().sortingOrder=1;
		Invoke("CloseDialogue",1f); 
	}
	void CloseDialogue()
	{
		Dialogue.SetActive(false);
	}
	public void UpdateHP(float fillamount)
	{
		FillImage.fillAmount = fillamount;
	}
	public void ChangeColor()
	{
		FrameImage.sprite = TeamTwoFrame;
		GetComponent<LineRenderer>().material.color=TeamTwoColor;
		Dialogue.GetComponent<Image>().sprite=TeamTwoDialogueBG;
	}
    public void ChangeIcon(Texture _Image)
    {
        imgIcon.texture = _Image;
    }
}
