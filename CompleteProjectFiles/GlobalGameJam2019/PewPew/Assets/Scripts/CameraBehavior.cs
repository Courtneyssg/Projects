using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 offset;
    public GameObject player;
    private GameManager manager;

    void Start()
    {
        offset = transform.position - player.transform.position;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.gameOver)
        {
            float newX = player.transform.position.x + offset.x;
            float newY = player.transform.position.y + offset.y;

            if (player.transform.position.y == 18)
            {
                newY = 18;
            }
            else if (player.transform.position.y > 18)
            {
                newY = transform.position.y;
            }



            if (player.transform.position.x == 125)
            {
                newX = 125;
            }
            else if (player.transform.position.x > 125 || player.transform.position.x < -45)
            {
                newX = transform.position.x;
            }
            else if (player.transform.position.x == -45)
            {
                newX = -45f;
            }
            transform.position = new Vector3(newX, newY, transform.position.z);
        }


        // 125 on x, -45 on x
    }
}
