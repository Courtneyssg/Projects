using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAi : MonoBehaviour
{

    public float maxDistance;
    public LayerMask mask;
    public GameObject shootEffect;
    public float bulletSpeed;
    public float cooldown;

    private float timeStamp = 0;
    private GameObject player;
    private bool flipped = true;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        var facingDirection = player.transform.position - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        // finding thingy
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        var hit = Physics2D.Raycast(transform.position, aimDirection, maxDistance, mask);

        if (aimDirection.x > 0 && !flipped)
            Flip();
        else if (aimDirection.x < 0 && flipped)
            Flip();

        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            if (!GetComponent<Animator>().enabled)
            {
                GetComponent<Animator>().enabled = true;
            }

            //do shooty shoot
            if (timeStamp <= Time.time)
            {
                GameObject bullet = Instantiate(shootEffect, transform.position + aimDirection * 2, Quaternion.identity);
                BulletMovement movement = bullet.GetComponent<BulletMovement>();
                movement.direction  = aimDirection;
                movement.speed = bulletSpeed;
                movement.ownerLayer = gameObject.layer;

                timeStamp = Time.time + cooldown;
            }

        }
        else
        {
            if (GetComponent<Animator>().enabled)
            {
                GetComponent<Animator>().enabled = false;
            }
        }
    }

    void Flip()
    {
        flipped = !flipped;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
