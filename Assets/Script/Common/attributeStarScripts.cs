using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class attributeStarScripts : MonoBehaviour
{
    public GameObject[] starsobj;
    public GameObject stars;

    public void SetStars(int _Count)
    {
        for (int i = 0; i < starsobj.Length; i++)
        {
            if (i < _Count)
            {
                starsobj[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                starsobj[i].GetComponent<Image>().enabled = false;
            }

        }
    }

}
