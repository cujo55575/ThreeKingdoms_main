using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class LocaleStringData: TableDataBase
{
	public override int key()
	{
		return StringID;
	}

	public int StringID;
	public string English;    
	public string Chinese;
    public string SimplifiedChinese;
	public string Japanese;
}

[Serializable]
public class LocaleStringDataTable : TableBase<LocaleStringData>
{
	public string GetString(int id)
	{
		LocaleStringData Data = GetData (id);
		if (Data == null)
			return string.Empty;

		switch(GLOBALVALUE.SYSTEM_LANGUAGE)
        {            
            case E_SystemLanguage.Simplified_Chinese:
                return Data.SimplifiedChinese.Replace("\\n", "\n");
            case E_SystemLanguage.Traditional_Chinese:
                return Data.Chinese.Replace("\\n", "\n");
			case E_SystemLanguage.Japanese:
				return Data.Japanese.Replace("\\n","\n");
			case E_SystemLanguage.English:
            default:
                return Data.English.Replace("\\n", "\n");
        }
	}
}
