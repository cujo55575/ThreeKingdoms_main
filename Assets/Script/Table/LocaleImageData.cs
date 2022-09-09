using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LocaleImageData : TableDataBase {
	public override int key ()
	{
		return ID;
	}
	public int ID;
	public string English;    
	public string Chinese;
	public string SimplifiedChinese;
	public string Japanese;

}

[Serializable]
public class LocaleImageDataTable : TableBase<LocaleImageData>{
	public Texture GetTexture(int id)
	{
		LocaleImageData Data = GetData(id);
		if (Data == null)
		{
			Debug.Log("Texture Null for ID = " + id);
			return null;
		}

		string textureName = string.Empty;
		switch (GLOBALVALUE.SYSTEM_LANGUAGE)
		{
			case E_SystemLanguage.Simplified_Chinese:
			textureName = Data.SimplifiedChinese;
			break;
			case E_SystemLanguage.Traditional_Chinese:
			textureName = Data.Chinese;
			break;
			case E_SystemLanguage.Japanese:
			textureName = Data.Japanese;
			break;
			case E_SystemLanguage.English:
			default:
			textureName = Data.English;
			break;
		}

		Texture result = Resources.Load<Texture>(string.Format(GLOBALCONST.FORMAT_IMAGE_WORD_PATH,textureName));
		if (result == null)
		{
			Debug.Log("Texture cant find with name " + textureName);
		}
		return result;
	}
}
