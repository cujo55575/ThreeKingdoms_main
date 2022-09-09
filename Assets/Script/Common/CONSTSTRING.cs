using UnityEngine;
using System.Collections;

public static class CONSTSTRING
{
    public const string APPLICATION_NAME = "富豪街";

    //public const string UI_LOGIN_STARTGAME = "开始游戏";

    public const string UI_LOADING_PERCENT = "{0:d}%";   

    public const int UI_MESSAGEBOX_TITLE = 10;
    public const int UI_MESSAGEBOX_BUTTON_SURE = 13;      
    public const int UI_MESSAGEBOX_BUTTON_CANCEL = 14;

    public static string[,] LOADING_TIPS = new string[,]
    {
        {"Game loading, no traffic consumption.", "游戏加载中，不消耗流量", "游戏加载中，不消耗流量"},
        {"Nirvana videos can be skipped by clicking on the screen.", "必杀技影片可点击画面跳过", "必杀技影片可点击画面跳过"}
    };

    public static string[] EXTRACTING = new string[]
    {
        "Extracting Resources ..............",
        "解開資源包……",
        "解开资源包……"
    };

    public const string BANNER_LINK = "https://www.google.com";
  

}
