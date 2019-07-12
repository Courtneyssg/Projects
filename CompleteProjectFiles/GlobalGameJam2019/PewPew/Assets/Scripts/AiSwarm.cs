using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSwarm : MonoBehaviour
{

    public Vector2 speed;
    public LayerMask mask;
    public float sigthDistance;
    public float distanceKeeping;

    private bool needToMove = true;
    private Vector2 home;
    private GameObject player;
    private Vector3 desiredPosition;
    public Vector3 axis = Vector3.up;

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

        //if the enemy is too close to the player it will rotate around the player
        if (Vector2.Distance(transform.position, player.transform.position) <= distanceKeeping)
        {
            needToMove = false;
            transform.RotateAround(player.transform.position, axis, speed.x * Time.deltaTime * 40);
        }

        //checking if the enemy needs to go to its home
        if (hit.collider == null || hit.collider.gameObject.tag != "Player")
        {
            aimDirection = getAimDirection(home);
            if (Vector2.Distance(transform.position, home) <= 1)
            {
                needToMove = false;
            }
        }
        //moving in a direction
        if (needToMove)
        {
            transform.rotation = Quaternion.identity;
            transform.Translate(aimDirection * speed * Time.deltaTime);
        }

    }


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
