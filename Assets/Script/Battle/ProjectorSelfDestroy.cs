using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorSelfDestroy : MonoBehaviour
{
    private Material mat;
    private float Alpha = 1f;
    private void Awake()
    {
        mat =GetComponent<Projector>().material;
        mat.SetFloat("_Alpha", 1f);
    }
    private void Start()
    { 
        Destroy(this.gameObject,2.0f);
    }
    private void Update()
    {
        Alpha = Mathf.Lerp(Alpha,0,Time.deltaTime*1.5f);
        mat.SetFloat("_Alpha",Alpha);
    }
}
