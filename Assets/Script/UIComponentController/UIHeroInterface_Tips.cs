using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIHeroInterface_Tips : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject objHeroCard_Tips;
    private void Start()
    {
        objHeroCard_Tips.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        objHeroCard_Tips.SetActive(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        objHeroCard_Tips.SetActive(false);
    }
}
