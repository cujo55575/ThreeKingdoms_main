using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class GachaAnimationPlayer : MonoBehaviour
{
	public RawImage rawImage;
	public VideoPlayer videoPlayer;


    //public AudioSource audioSource;
    public GameObject GachaRewardMask;
	private void OnEnable()
	{
        //StartCoroutine(PlayVideo());
        GachaRewardMask.SetActive(true);
		videoPlayer.loopPointReached += EndReached;
	}
	IEnumerator PlayVideo()
	{
		videoPlayer.Prepare();
		WaitForSeconds waitForSeconds = new WaitForSeconds(1);
		while (!videoPlayer.isPrepared)
		{
			yield return waitForSeconds;
			break;
		}
		rawImage.texture = videoPlayer.texture;
		videoPlayer.Play();
		//audioSource.Play();
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
        GachaRewardMask.SetActive(false);
        gameObject.SetActive(false);
        GetComponentInParent<UIGacha>().Mask.color = Color.white;
	}
}
