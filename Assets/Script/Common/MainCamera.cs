using UnityEngine;

public class MainCamera
{
	private static Camera m_Main;
	private static Transform m_Trans;
	private static AudioSource m_AudioSource;

	private static void ResetCamera()
	{
		GameObject MainObject = GameObject.Find(GLOBALCONST.GAMEOBJECT_NAME_MAINCAMERA);
		if (MainObject == null)
			return;
		m_Main = MainObject.GetComponent<Camera>();
		m_Trans = MainObject.transform;

		m_AudioSource = MainObject.GetComponent<AudioSource>();
		if (m_AudioSource == null)
			m_AudioSource = MainObject.AddComponent<AudioSource>();
	}

	public static Camera Main
	{
		get
		{
			if (m_Main == null)
				ResetCamera();
			return m_Main;
		}
	}

	public static Transform Trans
	{
		get
		{
			if (m_Trans == null)
				ResetCamera();
			return m_Trans;
		}
	}

	public static AudioSource AudioSource
	{
		get
		{
			if (m_AudioSource == null)
				ResetCamera();
			return m_AudioSource;
		}
	}
}
