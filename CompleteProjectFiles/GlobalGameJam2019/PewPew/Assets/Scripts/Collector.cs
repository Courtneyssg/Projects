using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{

    public float zinc = 0;
    public float tormaline = 0;
    public float quartz = 0;
    public float mineralD = 0;
    public float mineralE = 0;

    public bool shipRepaired = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Cargo")
        {
            switch (collision.gameObject.tag)
            {
                case "mineralA":
                    zinc++;
                    break;
                case "mineralB":
                    tormaline++;
                    break;
                case "mineralC":
                    quartz++;
                    break;
                case "mineralD":
                    mineralD++;
                    break;
                case "mineralE":
                    mineralE++;
                    break;
                default:
                    return;
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<RopeSystems>().ResetRope();
            Destroy(collision.gameObject);
        }
    }
}
