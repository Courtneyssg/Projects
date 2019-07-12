using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public int ownerLayer;
    public float damage;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(direction * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checking for friendly fire
        if (collision.tag != "Bullet" && ownerLayer != collision.gameObject.layer)
        {
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                collision.gameObject.GetComponent<Health>().doDamage(damage);
            }
            //adding force to the hit object and destroying the bullet
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * speed * 50);
            Destroy(gameObject);
        }
    }
}
