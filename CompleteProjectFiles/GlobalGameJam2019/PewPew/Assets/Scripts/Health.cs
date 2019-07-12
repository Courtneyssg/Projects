using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float previousHealth;

    // Start is called before the first frame update
    void Start()
    {
        previousHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void doDamage(float damage)
    {
        //dealing damage to the shields if any
        if(transform.Find("Shields") != null && transform.Find("Shields").gameObject.activeSelf)
        {
            transform.Find("Shields").GetComponent<Health>().doDamage(damage);
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            switch (gameObject.name)
            {
                case "ship":
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().gameOver = true;
                    break;
                case "Shields":
                    gameObject.transform.parent = null;
                    break;
            }
            Destroy(gameObject);
        }
    }


}
