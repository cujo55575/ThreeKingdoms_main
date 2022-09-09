using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debugger
{
    public static void Log(string log, bool isShow = true, string color="black")
    {
        if (isShow)
            Debug.Log("<color=" + color +">" + log + "</color>");
    }
}
