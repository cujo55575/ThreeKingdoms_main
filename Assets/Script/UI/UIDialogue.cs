using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : UIBase
{
    public GameObject HeroImageParent;
    public Transform HeroImageLeftTransform;
    public Transform HeroImageRightTransform;

    public GameObject HeroNameParent;
    public Transform HeroNameLeftTransform;
    public Transform HeroNameRightTransform;

    public RectTransform TextDialogueRightTransform;
    public RectTransform TextDialogueLeftTransform;
    public RectTransform TextDialogueMidTransform;

    public Transform DownbtnLeftTransform;
    public Transform DownbtnRightTransform;

    public Text txtHeroname;
    public Text txtDialogue;
    public RectTransform txtDialogueRect;
    public Text txtbgDialogue;
    public Button BtnQuitGame;
    public Button BtnForawrd;
    public Button btnDown;
    public GameObject objBgDialogue;
    public GameObject objDialoguePanel;

    public RawImage imgHeroFace;

    public float MessageInterval = 3f;
    private float AutoSkipWaitInterval = 5f;
    private DialogueData m_Data;
    private List<DialogueMessageData> m_Messages;
    private int m_MessageIndex;
    private float m_Timer;

    private AspectRatioFitter aspectFitter;

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void OnInit()
    {
        base.OnInit();
        //HeroImageParent.GetComponent<Button>().onClick.AddListener(OnBtnDownClick);
        aspectFitter = imgHeroFace.GetComponent<AspectRatioFitter>();
    }
    private float LastTimeScale;
    protected override void OnShow(params object[] Objects)
    {
        m_Data = (DialogueData)Objects[0];
        m_Messages = m_Data.GetAllDialogueMessages();
        Debug.Log(string.Format("DialogueData ID = {0}. m_MessagesCount = {1}", m_Data.key(), m_Messages.Count));
        for (int i = 0; i < m_Messages.Count; i++)
        {
            Debug.Log(string.Format("MessageIndex = {0}. Message = {1}", i, m_Messages[i].GetLocalizedMessage()));
        }
        m_MessageIndex = 0;
        m_Timer = Time.unscaledTime - MessageInterval;


        LastTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        StartCoroutine(SkipAfterTime());
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log("Update");
        if (Time.unscaledTime - m_Timer > MessageInterval)
        {
            m_Timer = Time.unscaledTime;
            if (m_MessageIndex < m_Messages.Count)
            {
                UpdateUI(m_Messages[m_MessageIndex]);
                m_MessageIndex++;
            }
            else
            {
                Skip();
            }
        }
    }
    public void Skip()
    {
        StopAllCoroutines();
        UIManager.Instance.CloseUI(GLOBALCONST.UI_DIALOGUE);
        Time.timeScale = LastTimeScale;
        HeroPlacementManager heroplaceScript = GameObject.FindObjectOfType<HeroPlacementManager>();
        if (heroplaceScript != null)
        {
            heroplaceScript.cardsCanvas.enabled = true;
            heroplaceScript.m_Canvas.enabled = true;
        }
    }
    public IEnumerator SkipAfterTime()
    {
        yield return new WaitForSecondsRealtime(AutoSkipWaitInterval + (m_Messages.Count * MessageInterval));
        Skip();
    }

    private void UpdateUI(DialogueMessageData messageData)
    {
        txtDialogue.text = messageData.GetLocalizedMessage();
        Texture tex = messageData.GetHeroTexture();
        imgHeroFace.texture = tex;
        if (tex != null)
        {
            aspectFitter.aspectRatio = (float)tex.width / (float)tex.height;
            imgHeroFace.color = Color.white;
        }
        else
        {
            imgHeroFace.color = Color.black;
        }
        txtHeroname.text = TableManager.Instance.LocaleStringDataTable.GetString(messageData.HeroNameID);
        Vector2 size = Vector2.zero;
        switch (messageData.GetDialogueType())
        {
            case E_DialogueUIType.ImageEmpty:
                HeroNameParent.SetActive(false);


                HeroImageParent.SetActive(false);
                btnDown.transform.position = DownbtnLeftTransform.transform.position;
                txtDialogue.transform.position = TextDialogueMidTransform.transform.position;
                txtDialogue.alignment = TextAnchor.UpperCenter;
                size = TextDialogueMidTransform.rect.size;
                txtDialogueRect.sizeDelta = size;
                break;

            case E_DialogueUIType.ImageLeft:
                HeroNameParent.SetActive(true);
                HeroNameParent.transform.position = HeroNameLeftTransform.position;
                HeroNameParent.transform.localScale = Vector3.one;
                txtHeroname.transform.localScale = Vector3.one;
                txtHeroname.alignment = TextAnchor.MiddleCenter;

                HeroImageParent.SetActive(true);
                HeroImageParent.transform.position = HeroImageLeftTransform.position;

                btnDown.transform.position = DownbtnLeftTransform.transform.position;

                txtDialogue.transform.position = TextDialogueRightTransform.position;
                txtDialogue.alignment = TextAnchor.UpperLeft;

                size = TextDialogueLeftTransform.rect.size;
                txtDialogueRect.sizeDelta = size;
                break;

            case E_DialogueUIType.ImageRight:
                HeroNameParent.SetActive(true);
                HeroNameParent.transform.position = HeroNameRightTransform.position;
                HeroNameParent.transform.localScale = new Vector3(-1f, 1f, 1f);
                txtHeroname.transform.localScale = new Vector3(-1f, 1f, 1f);
                txtHeroname.alignment = TextAnchor.MiddleCenter;


                HeroImageParent.SetActive(true);
                HeroImageParent.transform.position = HeroImageRightTransform.position;

                btnDown.transform.position = DownbtnRightTransform.transform.position;

                txtDialogue.transform.position = TextDialogueLeftTransform.position;
                txtDialogue.alignment = TextAnchor.UpperLeft;
                size = TextDialogueLeftTransform.rect.size;
                txtDialogueRect.sizeDelta = size;
                break;

            default: break;
        }
    }

    public void OnBtnDownClick()
    {
        m_Timer = Time.unscaledTime;
        m_MessageIndex++;
        if (m_MessageIndex < m_Messages.Count)
        {
            UpdateUI(m_Messages[m_MessageIndex]);
        }
        else
        {
            Skip();
        }
    }


}
