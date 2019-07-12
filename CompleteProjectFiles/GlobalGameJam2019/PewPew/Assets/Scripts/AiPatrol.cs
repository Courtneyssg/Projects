using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public Vector2 speed;
    private int p = 0;
    

    // Update is called once per frame
    void Update()
    {
        var aimdirection = getAimDirection(waypoints[p].position);
        transform.Translate(aimdirection * speed * Time.deltaTime, relativeTo:Space.World);
        if(Vector2.Distance(transform.position, waypoints[p].position) <= 1)
        {
            p++;
            if (p == waypoints.Length)
            {
                p = 0;
            }
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
