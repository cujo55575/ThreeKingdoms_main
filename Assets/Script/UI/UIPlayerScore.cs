using UnityEngine;
using UnityEngine.UI;

public class UIPlayerScore : MonoBehaviour
{
    public RawImage PlayerIcon;
    public Text HeroNameText;
    public Text RoadScoreText;
    public Text CastleScoreText;
    public Text TowerScoreText;
    public Text TotalScoreText;

    public void SetPlayerScore(PlayerScore score)
    {
        //PlayerIcon = playerIcon;
        HeroNameText.text = score.PlayerName;
        RoadScoreText.text = score.RoadScore.ToString ();
        CastleScoreText.text = score.CastleScore.ToString ();
        TowerScoreText.text = score.TowerScore.ToString ();
        TotalScoreText.text = score.TotalScore.ToString();
    }

}
