using UnityEngine;
using XEResources;

public class SoundManager : Singleton<SoundManager>
{
	private AudioSource mMainAudioSource;
	private AudioSource MainAudioSource
	{
		get
		{
			if (mMainAudioSource != null)
				return mMainAudioSource;
			if (MainCamera.AudioSource != null)
				mMainAudioSource = MainCamera.AudioSource;
            if(mMainAudioSource==null)
            {
                mMainAudioSource = Camera.main.GetComponent<AudioSource>();
            }
			return mMainAudioSource;
		}
	}

	private bool ChangeBGM = false;
	private bool NoResource = false;
	private float mFadeoutTime = 0f;
	private string NowBGM = string.Empty;
	private string NextBGM = string.Empty;

	public bool IsBGMPlay
	{
		get
		{
			return !(string.IsNullOrEmpty(NowBGM) && string.IsNullOrEmpty(NextBGM));
		}
	}
	public bool MuteBGM;
	public bool MuteSound;

	public void OnUpdate()
	{
		if (!ChangeBGM)
			return;
		mFadeoutTime += Time.deltaTime;
		if (mFadeoutTime < 1)
		{
			MainAudioSource.volume = Mathf.Lerp(1,0,mFadeoutTime);
			return;
		}
		AudioClip NextBGMClip = ResourceManager.Instance.Load<AudioClip>(string.Format(GLOBALCONST.FORMAT_SOUNDPATH,NextBGM));
		if (NextBGMClip == null)
			return;
        MainAudioSource.volume = 1;
		MainAudioSource.loop = true;
		//MainAudioSource.volume = PlayerDataManager.Instance.SoundVolume;
		MainAudioSource.clip = NextBGMClip;
		MainAudioSource.Play();
		//mFadeoutTime = 0.0F;
		ChangeBGM = false;
		NoResource = false;
		NowBGM = NextBGM;
		NextBGM = string.Empty;
	}

	public void SetSoundVolume(float volume)
	{
		//PlayerDataManager.Instance.SoundVolume = Mathf.Clamp01(volume);
		//MainAudioSource.volume = PlayerDataManager.Instance.SoundVolume;
	}
	public float GetSoundVolume()
	{
		//return PlayerDataManager.Instance.SoundVolume;
		return 1f;
	}

	public void PlayBGM(string BGMName,bool instant=false)
	{
		if (MuteBGM)
			return;
		if (MainAudioSource.isPlaying)
			if (NowBGM.Equals(BGMName))
				return;
		if (string.IsNullOrEmpty(BGMName))
			return;
		NextBGM = BGMName;
		if (MainAudioSource.isPlaying)
		{
				ChangeBGM = true;
                if(instant)
                {
                mFadeoutTime = 0;
                }
                else
                {   
                mFadeoutTime = 1;
                }
				return;
		}

		AudioClip audioclip = ResourceManager.Instance.Load<AudioClip>(string.Format(GLOBALCONST.FORMAT_SOUNDPATH,BGMName));
		if (audioclip == null)
		{
			ChangeBGM = false;
			NextBGM = string.Empty;
			mFadeoutTime = 0;
			return;
		}
		NowBGM = BGMName;
		NextBGM = string.Empty;
        //MainAudioSource.volume = PlayerDataManager.Instance.SoundVolume;
        mFadeoutTime = 1.1f;
        MainAudioSource.volume = 1;
        MainAudioSource.loop = true;
		MainAudioSource.clip = audioclip;
		MainAudioSource.Play();
	}
	public void StopBGM()
	{
        MainAudioSource.Stop();
		NowBGM = string.Empty;
		NextBGM = string.Empty;
	}

	public void PlaySound(string SoundName)
	{
		if (MuteSound)
			return;
		AudioClip audioclip = ResourceManager.Instance.Load<AudioClip>(string.Format(GLOBALCONST.FORMAT_SOUNDPATH,SoundName));
		if (audioclip == null)
			return;
        //MainAudioSource.volume = PlayerDataManager.Instance.SoundVolume;
        MainAudioSource.volume = 1;
        mFadeoutTime = 1.1f;
        MainAudioSource.PlayOneShot(audioclip,1);
	}

}
