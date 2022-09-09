using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : UIBase
{

    public GameObject LoginGroup;
    public GameObject DownloadGroup;

    public string DownloadUrl;
    public float progress;

    public Image FillImage;
    public Text ProgressText;

    private void OnEnable()
    {
        if (progress == 1)
        {
            //UIManager.Instance.CloseUI(GLOBALCONST.UI_LOGIN);
            LoginPhase.Instance.UserLogin();
            //UIManager.Instance.ShowUI(GLOBALCONST.UI_DECK);
        }

        LoginGroup.SetActive(true);
        DownloadGroup.SetActive(false);
        progress = 0;
    }

    protected override void OnShow(params object[] Objects)
    {
        base.OnShow(Objects);
        TableManager.Instance.LoadTable();  //讀取表格
        PlayerDataManager.Instance.Initialize();
        SoundManager.Instance.PlayBGM("BGM00001");

        // UIManager.Instance.CloseUI(GLOBALCONST.UI_LOGIN);
        // UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
    }

    public void PressToLogin()
    {
        if (!GLOBALVALUE.IS_LOGINABLE)
        {
            Debug.Log("Login Clicked");
            //Debug.Log("Player Data Name is "+PlayerDataManager.Instance.PlayerData.Name);
            return;
        }
        Debug.Log("PressToLogin()");
        LoginGroup.SetActive(false);
        DownloadGroup.SetActive(true);
        StartCoroutine(DownloadSth());
    }
    protected override void Update()
    {
        ProgressText.text = (int)(progress * 100) + "%";
        FillImage.fillAmount = progress;
    }
    IEnumerator DownloadSth()
    {
        WWW file = new WWW(DownloadUrl);
        //Debug.Log("download file="+DownloadUrl);
        while (!file.isDone)
        {
            if (file.progress > progress)
            {
                progress = file.progress;
            }

            yield return null;
        }
        progress = file.progress;
        yield return null;
        if (progress >= 1)
        {
#if OFFLINE_DATA
            PlayerDataManager.Instance.LoadPlayerData();
            UIManager.Instance.CloseUI(GLOBALCONST.UI_LOGIN);
            UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
            UIManager.Instance.ShowUI(GLOBALCONST.UITopBar);
#else

            Main.Instance.ChangeGameStatus(E_GameStatus.Login);
            LoginPhase.Instance.UserLogin();
#endif
        }
    }
}
