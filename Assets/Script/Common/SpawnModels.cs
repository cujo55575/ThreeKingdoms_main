using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnModels : MonoBehaviour
{
    public GameObject spawnMod;
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void spawnModels()
    {
        GameObject obj = Instantiate(spawnMod, transform.position, Quaternion.identity);
        obj.transform.SetParent(parent, false);
    }
}
