using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public RawImage SkillIcon;
    public void RefreshData(SkillData data)
    {
        if (data == null)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            SkillIcon.texture = data.GetSkillTexture();
            this.gameObject.SetActive(true);
        }
    }
}
