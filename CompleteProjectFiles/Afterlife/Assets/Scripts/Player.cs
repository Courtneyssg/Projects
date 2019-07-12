using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector2 _speed;
    public int health;
    public Text _healthText;
    public Vector3 moveDirection;

    private Rigidbody2D _rb;

    private bool canDash;
    public bool _isGrounded, _isJumping, facingRight, _isDashing;
    [SerializeField]
    private Transform _feets;

    [SerializeField]
    private float _checkRadius, _dashSpeed, _startDashTime, _dashTime, _animationInput, _sizeX;

    [SerializeField]
    private LayerMask _ground;
    
    private int _direction;

    [SerializeField]
    private GameObject _dash;

    [SerializeField]
    private CameraShake _cam;

    private Animator _anim;

    private Dialog _dialog;

    public Boundaries _boundaries;

    public GameObject _Gameover;


    // Start is called before the first frame update
    void Start()
    {

        _healthText.text = "Player Health: " + health;
        moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        moveDirection.z = 0;
        moveDirection.Normalize();

        transform.position = new Vector2(-3.22f, -0.39f);
        _rb = GetComponent<Rigidbody2D>();
        _dashTime = _startDashTime;
        StartCoroutine(Dashing());
        _anim = GetComponent<Animator>();
        facingRight = true;
        _isDashing = false;
       
        
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            _Gameover.SetActive(true);
            Time.timeScale = 0.0f;

        }
        _healthText.text = "Player Health: " + health;
        HorizontalMovement();
        Dash();
        Animation();



    }

    private void HorizontalMovement()
    {

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * _speed.x * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * _speed.x * Time.deltaTime;
        }
    }

    private void Dash()
    {
        if(canDash)
        {
            if (_direction == 0)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {

                    
                    _direction = 1;
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    
                    _direction = 2;
                }
            }
            else
            {

                if (_dashTime <= 0)
                {
                    _direction = 0;
                    _dash.SetActive(false);
                    canDash = false;
                    _isDashing = false;
                    StartCoroutine(Dashing());
                    _dashTime = _startDashTime;
                }
                else
                {
                    _dashTime -= Time.deltaTime;

                    if (_direction == 1 && Input.GetKey(KeyCode.A))
                    {
                        _cam.canShake = true;
                        _isDashing = true;
                        
                        transform.position -= transform.right * _dashSpeed * Time.deltaTime;
                        _dash.SetActive(true);
                    }

                    else if (_direction == 2 && Input.GetKey(KeyCode.D))
                    {
                        _cam.canShake = true;
                        _isDashing = true;
                        transform.position += transform.right * _dashSpeed * Time.deltaTime;
                        _dash.SetActive(true);
                    }

                }

            }
        }
    }

    public IEnumerator Dashing()
    {
        yield return new WaitForSeconds(1);
        canDash = true;
    }

    private void Animation()
    {
        _animationInput = Input.GetAxisRaw("Horizontal");

        if (_animationInput == 0)
        {
            _anim.SetBool("isRunning", false);
        }
        else if (_animationInput != 0 && _isGrounded)
        {
            _anim.SetBool("isRunning", true);
        }


        if (_animationInput > 0 && !facingRight)
            Flip();
        else if (_animationInput < 0 && facingRight)
            Flip();

    }
    void Flip()
    {
        facingRight = !facingRight;
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
