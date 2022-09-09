using UnityEngine;
using UnityEngine.UI;

public class ModePanelController : MonoBehaviour
{
    public GameObject _armyIcon;
    public GameObject objPVP_Panel;
    public GameObject objPVE_Panel;
    public RawImage ImgArmyIcon;
    public RawImage ImgBackgroundForMode;
    public RawImage ImgMode;
    public Text txtHeroArmyName;
    public Text txtChapterNumber;
    public Text txtChapterName;
    public Text txtPVPSeasonValue;
    public Transform ArmyIconPanel;
    public int index;
    void Start()
    {
        _armyIcon.SetActive(false);
    }
    void Update()
    {
        refershArmyIcon();
    }
    public void refershArmyIcon()
    {
        foreach (Transform child in ArmyIconPanel)
        {
            Destroy(child.gameObject);
        }
        if (PlayerDataManager.Instance.PlayerData.SavedArmyFormation != null)
        {
            for (int i = 0; i < PlayerDataManager.Instance.PlayerData.SavedArmyFormation.Count; i++)
            {
                for (int j = 0; j < PlayerDataManager.Instance.PlayerData.OwnedHeros.Count; j++)
                {
                    if (PlayerDataManager.Instance.PlayerData.OwnedHeros[j].ID == PlayerDataManager.Instance.PlayerData.SavedArmyFormation[i].PlayerHeroID)
                    {
                        PlayerHeroData _playerHeroData = PlayerDataManager.Instance.PlayerData.OwnedHeros[j];
                        txtHeroArmyName.text = _playerHeroData.GetHeroName();
                        ImgArmyIcon.texture = _playerHeroData.GetHeroData().GetHeroTexture();
                        GameObject go = Instantiate(_armyIcon);
                        go.transform.SetParent(ArmyIconPanel.transform, false);
                        go.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
