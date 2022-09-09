using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueMessageData : TableDataBase
{
	public int ID;
	public int MessageID;
	public string HeroImageName;
	public int HeroNameID;
	public int DialogueUIType; ///<see cref="E_DialogueUIType"/>
	public int HeroNameColor;///<see cref="E_Color"/>
	public override int key()
	{
		return ID;
	}

	public string GetLocalizedMessage()
	{
		return TableManager.Instance.LocaleStringDataTable.GetString(MessageID);
	}
	public Texture GetHeroTexture()
	{
		Texture texture = Resources.Load<Texture>("Image/Sprite/" + HeroImageName);
		return texture;
	}

	public string GetHeroName()
	{
		return TableManager.Instance.LocaleStringDataTable.GetString(HeroNameID);
	}

	public E_DialogueUIType GetDialogueType()
	{
		return (E_DialogueUIType)DialogueUIType;
	}

	public E_Color GetHeroNameColor()
	{
		return (E_Color)HeroNameColor;
	}
	

}


[Serializable]
public class DialogueMessageDataTable : TableBase<DialogueMessageData>
{
}