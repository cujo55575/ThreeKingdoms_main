using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEffect2 : MonoBehaviour
{

    public GameObject uiUpdateMessage;

    void Start()
    {
        Debug.Log("XXXXXXXXXX");
        GameObject go = Instantiate(uiUpdateMessage, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.SetParent(gameObject.transform, true);
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0);
    }


    void Update()
    {

    }
}
