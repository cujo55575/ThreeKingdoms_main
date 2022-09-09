using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEResources;
public class UIBattleCanvas : UIBase
{
    public Button BtnQuit;
    public Button BtnEnemyBattleField;
    public Button BtnOwnBattleField;
    public Button BtnSpeedUp;


    public Text txtBackToArmy;
    public Text txtEnemyInfo;
    public Text txtQuit;
    public Text txtSpeedUp;
    protected override void OnInit()
    {
        base.OnInit();
        BtnQuit.onClick.AddListener(ShowQuitGame);
      //  BtnEnemyBattleField.onClick.AddListener(ShowEnemyBattleField);
     //   BtnOwnBattleField.onClick.AddListener(ShowOwnBattleField);
        BtnSpeedUp.onClick.AddListener(HandleSpeedUp);
    }
    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
      
        //BtnEnemyBattleField.gameObject.SetActive(true);
        //BtnOwnBattleField.gameObject.SetActive(false);

        txtBackToArmy.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIBATTLECANVAS_BTN_BACK_TO_OUR_ARMY);
        txtEnemyInfo.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIBATTLECANVAS_BTN_ENEMY_INFO);
        txtQuit.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.UIBATTLECANVAS_BTN_QUIT);

		BtnQuit.gameObject.SetActive(true);

        BtnSpeedUp.gameObject.SetActive(false);

        Time.timeScale = 1;
		 txtSpeedUp.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ONE_X);

		GameObject.FindObjectOfType<EnemyPlacement>().AllowCheckingInfo = false;
	}
    public void ShowQuitGame()
    {
		string content = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.SURE_TO_EXIT);
        UIMessageBox.ShowMessageBox(content, E_MessageBox.YesNo, QuitGameCallBack);
    }
    public void OnGameStart()
    {
        BtnEnemyBattleField.gameObject.SetActive(false);
        BtnOwnBattleField.gameObject.SetActive(false);
		BtnQuit.gameObject.SetActive(false);
        BtnSpeedUp.gameObject.SetActive(true);
	}
    public void QuitGameCallBack(bool sure, object[] param)
    {
        if (sure)
        {
            UIManager.Instance.CloseUI(GLOBALCONST.UI_MESSAGEBOX);
            QuitGame();
        }
        else
        {
            UIManager.Instance.CloseUI(GLOBALCONST.UI_MESSAGEBOX);
        }
    }
    public void QuitGame()
    {
        SoundManager.Instance.StopBGM();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            UIManager.Instance.CloseAllUI();
            UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
        }
    }
    public static void ShowEnemyBattleField()
    {
        GameObject.FindObjectOfType<CameraController>().ChangeToEnemyBattleField();
        GameObject.FindObjectOfType<HeroPlacementManager>().CancelRemovableLeader();
		//BtnEnemyBattleField.gameObject.SetActive(false);
		//BtnOwnBattleField.gameObject.SetActive(true);

		GameObject.FindObjectOfType<EnemyPlacement>().AllowCheckingInfo = true;

		UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLECARDINFO);
    }
    public static void ShowOwnBattleField()
    {
        GameObject.FindObjectOfType<CameraController>().BackToOwnBattleField();
        GameObject.FindObjectOfType<HeroPlacementManager>().CancelRemovableLeader();
		// BtnEnemyBattleField.gameObject.SetActive(true);
		//  BtnOwnBattleField.gameObject.SetActive(false);

		GameObject.FindObjectOfType<EnemyPlacement>().AllowCheckingInfo = false;

		UIManager.Instance.CloseUI(GLOBALCONST.UI_ENEMYCARDINFO);
    }
    public void HandleSpeedUp()
    {
		if (Time.timeScale == 1)
		{
			Time.timeScale = 2;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			txtSpeedUp.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.TWO_X);
		}
		else if (Time.timeScale == 2)
		{
			Time.timeScale = 4;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			txtSpeedUp.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.FOUR_X);
		}
		else if (Time.timeScale == 4)
		{
			Time.timeScale = 1;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			txtSpeedUp.text = TableManager.Instance.LocaleStringDataTable.GetString(GLOBALCONST.ONE_X);
		}
    }
}
