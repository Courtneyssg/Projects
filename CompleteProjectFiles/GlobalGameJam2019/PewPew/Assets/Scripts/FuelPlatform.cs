using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPlatform : MonoBehaviour
{
    public float flowRate;
    public bool hasBeenMoved;


    private float timeStamp;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Called");
        if (collision.gameObject.tag == "Player" && timeStamp <= Time.time)
        {
            Movement1 mov = collision.gameObject.GetComponent<Movement1>();
            //refueling the shipd
            if (mov.fuelLevel < mov.fuelLimit)
            {
                float flow = flowRate / mov.fuelLimit;
                mov.fuelLevel += flowRate;
                timeStamp = Time.time + 1;
            }else if(mov.fuelLevel >= mov.fuelLimit)
            {
                mov.fuelLevel = mov.fuelLimit;
            }
        }
    }


}
