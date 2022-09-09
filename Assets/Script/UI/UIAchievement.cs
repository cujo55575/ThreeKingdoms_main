using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievement : UIBase
{
    public Transform Content;
    public UIAchievementDetail AchDetail;
    public Button BtnBack;

    protected override void OnShow(params object[] Objects)
    {
        List<AchievementData> data = new List<AchievementData>();
        for (int i = 0; i < 2; i++)
        {
            data.Add(new AchievementData());
        }
        data[0].Value1 = 20;
        data[0].Value2 = 20;
        data[0].Progress = 1f;
        data[0].Rewardable = true;
        UpdateData(data);
        base.OnShow(Objects);
		UIMessageBox.ShowMessageBox("Coming Soon!",E_MessageBox.Yes,CloseMessageCallback);
	}

	public void CloseMessageCallback(bool sure,object[] param)
	{
		Debug.Log("YesMessageCallbackCalled.");
		Close();
	}

	public void UpdateData(List<AchievementData> data)
    {
        //close all
        for (int x = 0; x < Content.childCount; x++)
        {
            Content.GetChild(x).gameObject.SetActive(false);
        }
        for (int i = 0; i < data.Count; i++)
        {
            if (Content.childCount <= i)
            {
                GameObject ins = Instantiate(AchDetail.gameObject);
                ins.transform.SetParent(Content, false);
                ins.GetComponent<UIAchievementDetail>().AssignData(data[i]);
                ins.SetActive(true);
            }
            else
            {
                UIAchievementDetail ins = Content.GetChild(i).GetComponent<UIAchievementDetail>();
                ins.AssignData(data[i]);
                ins.gameObject.SetActive(true);
            }
        }
    }
    public void Close()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_ACHIEVEMENT);
        UIManager.Instance.ShowUI(GLOBALCONST.UI_MAIN_MENU);
    }

    protected override void OnInit()
    {
        base.OnInit();
        BtnBack.onClick.AddListener(Close);
    }
}
public class AchievementData
{
    public string AchName;
    public string AchDetail;
    public int Value1;
    public int Value2;
    public float Progress;
    public E_RewardType rewardType;
    public int RewardAmount;
    public bool Rewardable;

    public AchievementData()
    {
        AchName = "AchName";
        AchDetail = "AchDetail";
        Value1 = 10;
        Value2 = 20;
        Progress = 0.5f;
        rewardType = E_RewardType.Type1;
        RewardAmount = 1000;
        Rewardable = false;
    }
    public AchievementData(string _AchName, string _AchDetail, int _Value1, int _Value2, float _Progress, E_RewardType _Type, int _RewardAmount, bool _Rewardable)
    {
        AchName = _AchName;
        AchDetail = _AchDetail;
        Value1 = _Value1;
        Value2 = _Value2;
        Progress = _Progress;
        rewardType = _Type;
        RewardAmount = _RewardAmount;
        Rewardable = _Rewardable;
    }
}
