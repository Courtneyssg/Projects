using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMotherShip : MonoBehaviour
{

    public Vector2 speed;
    public float sigthDistance;
    public float cooldown;
    public LayerMask mask;
    public float shootingRange;
    public GameObject swarm;
    public float distanceKeeping;

    private Vector2 home;
    private bool needToMove = false;
    private float timeStamp = 0;
    private GameObject player;


    private void OnEnable()
    {
        home = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        needToMove = true;

        var aimDirection = getAimDirection(player.transform.position);

        var hit = Physics2D.Raycast(transform.position, aimDirection, sigthDistance, mask);

        //if the enemy is too close to the player it wont move
        if (Vector2.Distance(transform.position, player.transform.position) <= distanceKeeping)
        {
            needToMove = false;
        }

        //checking if the enemy needs to move home
        if (hit.collider == null || hit.collider.gameObject.tag != "Player")
        {
            aimDirection = getAimDirection(home);
            if (Vector2.Distance(transform.position, home) <= 1)
            {
                needToMove = false;
            }
            
        }
        //checking if the enemy can create a new smaller ship
        else if (hit.distance <= shootingRange && timeStamp <= Time.time)
        {
            Instantiate(swarm, (Vector2)transform.position + aimDirection * 2, Quaternion.identity);
            timeStamp = Time.time + cooldown;
        }

        //moving
        if (needToMove)
        {
            transform.Translate(aimDirection * speed * Time.deltaTime, relativeTo: Space.World);
        }

    }

    //getting the aim direction
    private Vector2 getAimDirection(Vector3 target)
    {
        var facingDirection = target - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        return Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
    }
}
