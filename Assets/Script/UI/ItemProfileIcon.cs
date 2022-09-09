using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemProfileIcon : MonoBehaviour
{
    public RawImage IconProfile;
    public Toggle Toggle;
    public string IconName;
    public int index;
    private void Start()
    {
        Toggle.onValueChanged.AddListener((value) => ClickToggle(value, gameObject.GetComponent<ItemProfileIcon>()));
    }
    public void ClickToggle(bool isCheck, ItemProfileIcon item)
    {
        if (isCheck)
        {
            Toggle.isOn = true;
            UIManager.UIInstance<UIChooseProfile>().itemProfileIndex = index;
        }
    }
}
