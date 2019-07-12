using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        if(transform.localScale.x < 0.1f | transform.localScale.y < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
