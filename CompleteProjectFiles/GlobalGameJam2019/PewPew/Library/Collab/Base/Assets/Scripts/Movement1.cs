using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement1 : MonoBehaviour {
    public float rotations = 200.0f;
    public float speed = 0;
    public float maxSpeed = 7;
    Rigidbody2D rb;
    public GameObject flames;
    public GameObject flames2;
    public GameObject fuel;
    public GameObject collisionEffect;
    public GameObject shootEffect;
    public GameManager gameOver;
    public float bulletSpeed;
    public float fuelLimit;
    public float fuelLevel;
    public float wallDamage;

    public AudioSource _collisionAudio;
    public AudioSource _engineAudio;

    private Vector2 velocity;
    private float timeStamp;

    public GameObject shields;


	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        
        fuelLevel = fuelLimit;
        

    }
	
	// Update is called once per frame
	void Update () {

        if(!(gameOver.gameOver))
        {
            fuel.transform.localScale = new Vector3(fuelLevel / 100, fuelLevel / 100, fuelLevel / 100);
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, 0, -1.0f * rotations * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, 0, 1.0f * rotations * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                shields.SetActive(true);  
            }

            if (Input.GetKey(KeyCode.Space))
            {
                //do shooty shoot
                if (timeStamp <= Time.time)
                {
                    GameObject bullet = Instantiate(shootEffect, transform.position + transform.up * 2, Quaternion.identity);

                    BulletMovement movement = bullet.GetComponent<BulletMovement>();
                    movement.direction = transform.up;
                    movement.speed = bulletSpeed;
                    movement.ownerLayer = gameObject.layer;
                    timeStamp = Time.time + 1;
                }
            }

            if ((Input.GetMouseButton(0)))
            {
                flames.SetActive(true);
                flames2.SetActive(true);
                if (!_engineAudio.isPlaying )
                {
                    _engineAudio.Play();
                }
                
                fuelLevel -= 1;
                if (fuelLevel < 0)
                {
                    fuelLevel = 0;
                    fuel.transform.localScale = new Vector3(0, 0, 0);
                }
                speed += 2f;
                // transform.Translate(Vector2.up * Time.deltaTime * speed);
                rb.AddForce(transform.up * speed);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
                if (speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
            }

            if (!(Input.GetMouseButton(0)))
            {
                flames.SetActive(false);
                flames2.SetActive(false);
                _engineAudio.Stop();

                if (rb.velocity.x > 0)
                {
                    rb.velocity -= new Vector2(0.01f, 0);
                    if (rb.velocity.x <= 0.5)
                    {
                        rb.velocity = new Vector2(0.5f, rb.velocity.y);
                    }
                }
                if (rb.velocity.x < 0)
                {
                    rb.velocity += new Vector2(0.01f, 0);
                    if (rb.velocity.x >= -0.5)
                    {
                        rb.velocity = new Vector2(-0.5f, rb.velocity.y);
                    }
                }
                if (speed > 0)
                {
                    speed -= 0.1f;
                    if (speed <= 0)
                    {
                        speed = 0;
                    }


                }


            }

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
            velocity = rb.velocity;

            /*if (fuel.transform.localScale.x <= 0.05f | fuel.transform.localScale.y <= 0.05f)
            {
                gameOver.gameOver = true;
                
            }*/ //COMMENTED OUT FOR NOW SO WE DON'T RUN OUT OF FUEL WHILE PLAY TESTING
        }

        if(gameOver.gameOver)
        {
            rb.velocity = new Vector3(rb.velocity.x, -5f, 0);
            flames.SetActive(false);
        }

        if(transform.position.y >= 25)
        {
            transform.position = new Vector3(transform.position.x, 25, transform.position.z);
        }
        if(transform.position.x <= -52)
        {
            transform.position = new Vector3(-52, transform.position.y, transform.position.z);
        }


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "FuelPlatform":
            case "CollectorBucket":
                rb.velocity = new Vector2(0, 0);
                return;
            default:
                Vector2 D = transform.position - other.transform.position;
                float angle = Mathf.Atan2(D.y, D.x);
                angle -= 180;

                rb.velocity = new Vector2(Mathf.Sin(angle) * velocity.x, Mathf.Cos(angle) * velocity.y);

                if(other.gameObject.name != "Checkpoint")
                {
                    GetComponent<Health>().doDamage(wallDamage);
                }

                break;
        }

        for (int i = 0; i < 80; i++)
        {
            Instantiate(collisionEffect, transform.position, Quaternion.identity);
        }

        _collisionAudio.Play();
    }

    

    /*private void ImpactShapes(GameObject shape) // created a trail renderer by accident
    {
        float shapeSpeed = Random.Range(1, 20);
        float shapeScale = Random.Range(0.1f, 1);
        float rotateRate = Random.Range(20, 200);
        float numberOfShapes = Random.Range(20, 50);
        for(int i = 0; i <= numberOfShapes; i++)
        {
            Instantiate(shape, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            shape.transform.Rotate(0, 0, 1 * rotateRate * Time.deltaTime);
            shape.transform.localScale = new Vector3(shapeScale, shapeScale, shapeScale);
            shape.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * speed;
        }

        
        

    }*/

}
