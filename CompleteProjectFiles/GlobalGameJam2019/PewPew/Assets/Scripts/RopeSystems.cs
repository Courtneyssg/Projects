using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystems : MonoBehaviour
{

    public bool ropeAttached;
    private Vector2 playerPosition;
    public AudioSource _audio;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 20f;

    public GameObject ropePosition;
    public float minRopeLength;
    //public float speed = 100;
    //private float rotateSpeed = 100f;
    //private float radiusSpeed = 1f;

    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    private float _angle;

    //public Transform crosshair;
    //public SpriteRenderer crosshairSprite;

    void Awake()
    {
        playerPosition = transform.position;
    }

    void Update()
    {
        // updating aim angle
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        // updating aimdirection
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        playerPosition = transform.position;



        HandleInput();
       // SetCrosshairPosition(aimAngle);
        UpdateRopePositions();
    }

    // handling input
    private void HandleInput()
    {
        if (!ropeAttached && Input.GetKey(KeyCode.E))
        {
            var hit = Physics2D.Raycast(playerPosition, Vector2.down, ropeMaxCastDistance, ropeLayerMask);
            
            // checking if a hit with the rope
            if (hit.collider != null && (ropePosition == null || (Vector2)ropePosition.transform.position != hit.point))
            {
                ropeAttached = true;

                ropeRenderer.enabled = true;

                ropePosition = hit.transform.gameObject;

                if (ropePosition.GetComponent<Rigidbody2D>().IsSleeping())
                {
                    ropePosition.GetComponent<Rigidbody2D>().WakeUp();
                }
                if(ropePosition.tag == "FuelPlatform")
                {
                    if (ropePosition.GetComponent<FuelPlatform>().hasBeenMoved)
                    {
                        ResetRope();
                        return;
                    }
                    ropePosition.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    ropePosition.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                }

                _audio.Play();
                ropePosition.GetComponent<DistanceJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                ropePosition.GetComponent<DistanceJoint2D>().distance = Vector2.Distance(transform.position, ropePosition.transform.position);
                ropePosition.GetComponent<DistanceJoint2D>().enabled = true;
            }
            // no hit, removing
            else
            {
                if (!ropeAttached)
                {
                    ropeRenderer.enabled = false;
                    ropeAttached = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            ResetRope();
        }
    }

    //private void SetCrosshairPosition(float aimAngle)
    //{
    //    if (!crosshairSprite.enabled)
    //    {
    //        crosshairSprite.enabled = true;
    //    }

    //    var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
    //    var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

    //    var crossHairPosition = new Vector3(x, y, 0);
    //    crosshair.transform.position = crossHairPosition;
    //}


    // resetting the rope stuff
    public void ResetRope()
    {
        ropeAttached = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropeRenderer.enabled = false;

        if (ropePosition != null)
        {
            _audio.Play();
            if (ropePosition.tag == "FuelPlatform")
            {
                
                ropePosition.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                ropePosition.GetComponent<FuelPlatform>().hasBeenMoved = true;
                StartCoroutine(delayedKinematic(ropePosition));
            }
            ropePosition.GetComponent<DistanceJoint2D>().enabled = false;
            ropePosition.GetComponent<DistanceJoint2D>().connectedBody = null;
            ropePosition = null;
        }
    }

    private void UpdateRopePositions()
    {
        
        if (!ropeAttached)
        {
            return;
        }

        //transform.RotateAround(ropePosition.transform.position, axis, rotateSpeed * Time.deltaTime);
        //desiredPosition = (transform.position - ropePosition.transform.position).normalized * ropeJoint.distance + ropePosition.transform.position;
        //transform.position = Vector3.MoveTowards(transform.position, desiredPosition, radiusSpeed * Time.deltaTime);


        //setting the minimum distance
        DistanceJoint2D joint = ropePosition.GetComponent<DistanceJoint2D>();
        if(joint.distance < minRopeLength)
        {
            joint.distance = minRopeLength;
        }

        //rendering the rope correctly
        ropeRenderer.positionCount = 2;

        ropeRenderer.SetPosition(1, ropePosition.transform.position);
        ropeRenderer.SetPosition(0, transform.position);
    }


    IEnumerator delayedKinematic(GameObject obj)
    {
        yield return 0.5f;
        obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
}
