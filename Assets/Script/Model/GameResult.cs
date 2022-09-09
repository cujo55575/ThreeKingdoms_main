using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult 
{
    public PlayerScore PlayerOneResult;
    public PlayerScore PlayerTwoResult;

    public GameResult(PlayerScore playerOneResult, PlayerScore playerTwoResult)
    {
        PlayerOneResult = playerOneResult;
        PlayerTwoResult = playerTwoResult;
    }
}
public class PlayerScore
{
    private string iconName;
    private string playerName;
    private int roadScore;
    private int towerScore;
    private int castleScore;
    private int totalScore;

    public PlayerScore(string iconName, string playerName, int roadScore, int towerScore, int castleScore)
    {
        IconName = iconName;
        PlayerName = playerName;
        RoadScore = roadScore;
        TowerScore = towerScore;
        CastleScore = castleScore;
        totalScore = roadScore + towerScore + castleScore;
    }

    public string IconName { get => iconName; set => iconName = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int RoadScore { get => roadScore; set => roadScore = value; }
    public int TowerScore { get => towerScore; set => towerScore = value; }
    public int CastleScore { get => castleScore; set => castleScore = value; }
    public int TotalScore { get => totalScore; set => totalScore = value; }
}
