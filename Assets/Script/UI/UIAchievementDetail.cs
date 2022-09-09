
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievementDetail : MonoBehaviour
{
	public Text AchName;
	public Text AchDetail;
	public Image ProgressImage;
	public Image RewardImage;
	public Text ProgressText;
	public Text RewardAmount;
	public Button BtnFinished;
    public Button BtnUnFinished;

	public void AssignData(AchievementData data)
	{
		AchName.text = data.AchName;
		AchDetail.text = data.AchDetail;
		ProgressImage.fillAmount = data.Progress;

		ProgressText .text= data.Value1 + "/" + data.Value2;
		RewardAmount.text = "x"+ data.RewardAmount;
        
        BtnFinished.gameObject.SetActive(data.Rewardable);
        BtnUnFinished.gameObject.SetActive(!data.Rewardable);
	}
}
