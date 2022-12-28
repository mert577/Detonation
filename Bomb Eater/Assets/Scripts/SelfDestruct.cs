using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    public float selfDestructTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, selfDestructTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
