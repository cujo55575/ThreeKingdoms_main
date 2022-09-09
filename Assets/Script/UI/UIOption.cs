using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase
{
	public Button BtnClose;
	public Slider SliderBGM;
	public Slider SliderSound;
	public Text TxtBGM;
	public Text TxtSound;
	public Text TxtChangeLanguage;
	public Dropdown DpdChangeLanguage;

	private const int SWITCH_LANGUAGE_STRING_ID = 59026;

	protected override void OnInit()
	{
		base.OnInit();

	
		SliderBGM.value = 0.5f;
		SliderSound.value = 0.1f;

		SliderBGM.onValueChanged.AddListener(OnBGMValueChanged);
		SliderSound.onValueChanged.AddListener(OnSoundValueChanged);

		BtnClose.onClick.AddListener(CloseUI);
		DpdChangeLanguage.onValueChanged.AddListener(DropDownValueChanged);
		DpdChangeLanguage.ClearOptions();
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		int selectedValue = 0;
		foreach (E_SystemLanguage language in System.Enum.GetValues(typeof(E_SystemLanguage)))
		{
			string localizedLanguageName = TableManager.Instance.LocaleStringDataTable.GetString((int)language);
			if (string.IsNullOrEmpty(localizedLanguageName))
			{
				continue;
			}
			Dropdown.OptionData oData = new Dropdown.OptionData(localizedLanguageName);
			options.Add(oData);
			if (language == GLOBALVALUE.SYSTEM_LANGUAGE)
			{
				selectedValue = options.Count - 1;
			}
		}
		DpdChangeLanguage.AddOptions(options);
		DpdChangeLanguage.value = selectedValue;

	}
	private void DropDownValueChanged(int value)
	{
		if (value == 0)
		{
			GLOBALVALUE.SYSTEM_LANGUAGE = E_SystemLanguage.Simplified_Chinese;
		}
		else if (value == 1)
		{
			GLOBALVALUE.SYSTEM_LANGUAGE = E_SystemLanguage.Traditional_Chinese;
		}
		else if (value == 2)
		{
			GLOBALVALUE.SYSTEM_LANGUAGE = E_SystemLanguage.Japanese;
		}
		else if(value==3)
        {
			GLOBALVALUE.SYSTEM_LANGUAGE = E_SystemLanguage.English;
		}
        //foreach (E_SystemLanguage language in System.Enum.GetValues(typeof(E_SystemLanguage)))
        //{
        //	string localizedLanguageName = TableManager.Instance.LocaleStringDataTable.GetString((int)language);
        //	if (string.Equals(localizedLanguageName, DpdChangeLanguage.options[value].text.ToString()))
        //	{
        //		GLOBALVALUE.SYSTEM_LANGUAGE = language;
        //              //UIMainMenu mainMenu = UIManager.UIInstance<UIMainMenu>();
        //              //if (mainMenu != null)
        //              //{
        //              //    PlayerDataManager.Instance.SavePlayerData();
        //              //    UIManager.Instance.CloseUI(GLOBALCONST.UI_MAIN_MENU);
        //              //    UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
        //              //}
        //              return;
        //          }
        //}
    }
    private void OnEnable()
    {
		TxtBGM.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIOPTION_TXTBGM);
		TxtSound.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIOPTION_TXTSOUND);
		TxtChangeLanguage.text = TableManager.Instance.LocaleStringDataTable.GetString(SWITCH_LANGUAGE_STRING_ID);
	}
    private void OnBGMValueChanged(float value)
	{

	}

	private void OnSoundValueChanged(float value)
	{

	}

	private void CloseUI()
	{
		PlayerDataManager.Instance.SavePlayerData();
		UIManager.Instance.CloseUI(GLOBALCONST.UI_MAIN_MENU);
		UIManager.Instance.CloseUI(GLOBALCONST.UI_OPTION);
		UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
	}
}
