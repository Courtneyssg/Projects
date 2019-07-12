using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject player;

    public GameObject craft;
    public GameObject crafting;
    public Text numZinc;
    public Text numTormaline;
    public Text numQuartz;

    public GameObject collector;
    public GameObject shields;
    public GameObject fuelPlatform;

    private float zincCollected;
    private float tormalineCollected;
    private float quartzCollected;

    private Rigidbody2D rb;

    private bool switchOn;


    //private GameObject collector;
    // Start is called before the first frame update
    void Start()
    {
        craft.SetActive(true);
        crafting.SetActive(false);
        
        collector = GameObject.FindGameObjectWithTag("CollectorBucket");
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            craft.SetActive(false);
            crafting.SetActive(true);
            switchOn = false;
            player.GetComponent<Movement1>().enabled = false;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (craft.activeSelf && !switchOn) {
            player.GetComponent<Movement1>().enabled = true;
            switchOn = true;
            rb.constraints = RigidbodyConstraints2D.None;
        }

        zincCollected = collector.GetComponent<Collector>().zinc;
        tormalineCollected = collector.GetComponent<Collector>().tormaline;
        quartzCollected = collector.GetComponent<Collector>().quartz;

        numZinc.text = zincCollected.ToString() + "/1";
        numTormaline.text = tormalineCollected.ToString() + "/1";
        numQuartz.text = quartzCollected.ToString() + "/1";


    }
    

    public void BuildShields()
    {
        if (zincCollected >= 1)
        {
            GameObject shield = Instantiate(shields, player.transform);
            shield.SetActive(false);
            collector.GetComponent<Collector>().zinc--;
        }

    }

    public void BuildFuel()
    {
        if (tormalineCollected >= 1)
        {
            GameObject platform = Instantiate(fuelPlatform, new Vector2(142, 3), Quaternion.identity);
            platform.GetComponent<SceneOwner>().sceneOwner = 2;
            collector.GetComponent<Collector>().tormaline--;
        }
    }

    public void BuildShipPart()
    {
        if (quartzCollected >= 1)
        {
            collector.GetComponent<Collector>().shipRepaired = true;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().gameWon = true;
            collector.GetComponent<Collector>().quartz--;
        }
    }

}
