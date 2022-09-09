using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMessage : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform defaultPosition;
    void Start()
    {
        defaultPosition = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 5);
        GetComponent<SpriteRenderer>().color -= new Color (0, 0, 0, (Time.deltaTime * 3/2));
    }

    
}
