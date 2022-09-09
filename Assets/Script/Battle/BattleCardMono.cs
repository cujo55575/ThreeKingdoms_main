using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleCardMono : MonoBehaviour, IPointerDownHandler
{
    public bool IsInBattle = false;
    public PlayerHeroData data;
    public HeroPlacementManager Manager;
    public RawImage Icon;
    public Image ColorMask;
    public GameObject SelectedImage;
    public Text txtHeroName;
    void Start()
    {
        Icon.texture = data.GetHeroData().GetHeroTexture();
        txtHeroName.text = data.GetHeroName();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(IsInBattle)
        {
            return;
        }
        Manager.ChooseCard(this.gameObject);
    }
    void Update()
    {
        ColorMask.gameObject.SetActive(IsInBattle);
    }
    public void OnClick()
    {
        UIBattleCardInfo infoUI = UIManager.UIInstance<UIBattleCardInfo>();
        if(infoUI!=null && infoUI.gameObject.activeSelf)
        {
            infoUI.ShowData(this);
        }
        else
        {
            UIManager.Instance.ShowUI(GLOBALCONST.UI_BATTLECARDINFO,this);
        }
    }
}
