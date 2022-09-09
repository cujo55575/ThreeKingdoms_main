using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEResources;

public class UIBattleStartAnimation : UIBase
{
    public Text PlayerOneText;
    public Text PlayerTwoText;

    public RawImage PlayerOneImage;
    public RawImage PlayerTwoImage;
    protected override void OnShow(params object[] Objects)
    {
        PlayerOneText.text = (string)Objects[0];
        PlayerTwoText.text = (string)Objects[1];
        PlayerOneImage.texture = (Texture)Objects[2];
        PlayerTwoImage.texture = (Texture)Objects[3];

        Invoke("CloseSelf", 4.0f);

        base.OnShow(Objects);
    }
    public void PlaySFX()
    {
        SoundManager.Instance.PlaySound("MS00018");
    }
    public void CloseSelf()
    {
        UIManager.Instance.CloseUI(GLOBALCONST.UI_BATTLE_START_ANIMATION);
    }
}
