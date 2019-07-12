using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 3f;        // Player move speed
    [SerializeField]
    private float _attackTime;        // Attack Time
    public int _playerLives = 3;
    [SerializeField]
    private float _shootRate;
    private float _canFire = 0.0f;
    private float _attackTimeCounter; // Counting time between attacks pauses
    private Animator _animator;        // Reference to animator
    private bool _playerMoving;       // Tells us if the player is currently moving or idle
    public bool playerAttacking;      // Whether the player is currently attacking or not
    private Vector2 _lastMove;        // Used for proper idle direction
    public Vector2 _moveDirection;    // Used for proper movement direction
    private bool canPressW = true;    // If true can move up, set to false if another key is pressed
    private bool canPressA = true;    // If true can move left, set to false if another key is pressed
    private bool canPressS = true;    // If true can move down, set to false if another key is pressed
    private bool canPressD = true;    // If true can move right, set to false if another key is pressed
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private UIManager _uiManager;
    private AudioSource _music;
    void Start()
    {
        _music = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        if(!_music.isPlaying)
        {
            _music.Play();
        }
        transform.position = new Vector3(0, 0, 0);
        _animator = GetComponent<Animator>(); // Reference to animator
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

    }
    void Update()
    {
        _playerMoving = false; // Set _playerMoving to false

        SetBoundaries();

        PlayerMovement();

        AttackAnimation();



    }

    private void SpawnBullet()
    {
        _playerMoving = true;
        playerAttacking = true;                                           // The player is able to attack
        if (Time.time > _canFire)
        {
            if(playerAttacking && _moveDirection.x == 1.0 && _moveDirection.y == 0.0)
            {
                //moving right
                Instantiate(_bullet, new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 270));
            }

            else if(playerAttacking && _moveDirection.x == 0 && _moveDirection.y == -1)
            {
                //moving down
                Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180));
            }

            else if(playerAttacking && _moveDirection.x == -1 && _moveDirection.y == 0)
            {
                //move left
                Instantiate(_bullet, new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90));
            }

            else if(playerAttacking && _moveDirection.x == 0 && _moveDirection.y == 1)
            {
                Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 360));
            }
            _canFire = Time.time + _shootRate;

        }
        _attackTimeCounter = _attackTime;
        _animator.SetBool("PlayerAttacking", true);                        // Play attacking animation
    }

    public void PlayerDamage()
    {
        _playerLives -= 1; // Each hit decreases life count by 1
        _uiManager.UpdateLives(_playerLives);
        // Call Update lives method in uimanager
        if(_playerLives <= 0)
        {
            _uiManager.ShowGameOverScreen();
            
        }
    }

    public void SetBoundaries()
    {
        if (transform.position.x >= 20)
        {
            transform.position = new Vector3(20, transform.position.y, transform.position.z);
        }
        if (transform.position.y >= 20)
        {
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        }
        if (transform.position.x <= -20)
        {
            transform.position = new Vector3(-20, transform.position.y, transform.position.z);
        }
        if (transform.position.y <= -20)
        {
            transform.position = new Vector3(transform.position.x, -20, transform.position.z);
        }
    }

    private void PlayerMovement()
    {
        bool W = Input.GetKey(KeyCode.W); // Reference to W keypress
        bool A = Input.GetKey(KeyCode.A); // Reference to A keypress
        bool S = Input.GetKey(KeyCode.S); // Reference to S keypress
        bool D = Input.GetKey(KeyCode.D); // Reference to D keypress

        if (Input.GetKeyDown(KeyCode.Space))
        {                                                                      // If Space key is pressed
            SpawnBullet();
        }

        if (W && canPressW)
        {                                                                      // If W key is pressed and no other key is pressed
            _playerMoving = true;                                              // Player is moving when a key is pressed
            _lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));         // Updates animation so player faces right direction when standing
            _moveDirection.y = 1;                                              // Updates the direction being moved in
            _moveDirection.x = 0;                                              // Prevent movement on x axis
            canPressA = false;                                                 // Disallow A to be pressed
            canPressD = false;                                                 // Disallow D to be pressed
            canPressS = false;                                                 // Disallow S to be pressed
            transform.position += new Vector3(0, _speed, 0) * Time.deltaTime;  // Move the player
        }

        if (Input.GetKeyUp(KeyCode.W))                                         // When the W key is lifted
        {                                                                      // Allow other keys to be pressed
            canPressA = true;
            canPressS = true;
            canPressD = true;
        }

        if (A && canPressA)
        {                                                                      // If A key is pressed and no other key is pressed
            _playerMoving = true;                                              // Player is moving when a key is pressed
            _lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);       // Updates animation so player faces right direction when standing
            _moveDirection.x = -1;                                             // Updates the direction being moved in
            _moveDirection.y = 0;                                              // Prevent movement on y axis
            canPressS = false;                                                 // Disallow S to be pressed
            canPressD = false;                                                 // Disallow D to be pressed
            canPressW = false;                                                 // Disallow W to be pressed
            transform.position += new Vector3(-_speed, 0, 0) * Time.deltaTime; // Move the player
        }

        if (Input.GetKeyUp(KeyCode.A))                                         // When the A key is lifted 
        {                                                                      // Allow other keys to be pressed
            canPressW = true;
            canPressS = true;
            canPressD = true;
        }

        if (S && canPressS)
        {                                                                      // If S key is pressed and no other key is pressed
            _playerMoving = true;                                              // Player is moving when a key is pressed
            _lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));         // Updates the animation so player faces right direction when standing
            _moveDirection.y = -1;                                             // Updates the direction being moved in
            _moveDirection.x = 0;                                              // Prevent movement on the x axis
            canPressD = false;                                                 // Disallow D to be pressed
            canPressA = false;                                                 // Disallow A to be pressed
            canPressW = false;                                                 // Disallow W to be pressed
            transform.position += new Vector3(0, -_speed, 0) * Time.deltaTime; // Move the player
        }

        if (Input.GetKeyUp(KeyCode.S))                                         // When the S key is lifted
        {                                                                      // Allow other keys to be pressed
            canPressW = true;
            canPressD = true;
            canPressA = true;
        }

        if (D && canPressD)
        {                                                                      // If D key is pressed and no other key is pressed
            _playerMoving = true;                                              // Player is moving when a key is pressed
            _lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);       // Updates the animation so player faces right direction when standing
            _moveDirection.x = 1;                                              // Updates the direction being moved in
            _moveDirection.y = 0;                                              // Prevent movement on y axis
            canPressW = false;                                                 // Disallow W to be pressed
            canPressA = false;                                                 // Disallow A to be pressed
            canPressS = false;                                                 // Disallow S to be pressed
            transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;  // Move the player
        }

        if (Input.GetKeyUp(KeyCode.D))                                         // When the D key is lifted
        {                                                                      // Allow other keys to be pressed
            canPressW = true;
            canPressA = true;
            canPressS = true;
        }

        // Setting the MoveX and MoveY for the animator to read X and Y axis to accurately change the moving direction of the player
        _animator.SetFloat("MoveX", _moveDirection.x);
        _animator.SetFloat("MoveY", _moveDirection.y);
        // When the player is moving update the moving animation
        _animator.SetBool("PlayerMoving", _playerMoving);
        // Setting the lastMoveX and lastMoveY for the animator to read X and Y axis to accurately change the standing direction of player
        _animator.SetFloat("LastMoveX", _lastMove.x);
        _animator.SetFloat("LastMoveY", _lastMove.y);
    }

    private void AttackAnimation()
    {
        if (_attackTimeCounter > 0)
        {
            _attackTimeCounter -= Time.deltaTime;                                  // If attack counter is greater than 0, subtract time
        }
        if (_attackTimeCounter <= 0)
        {
            playerAttacking = false;                                              // If time runs out, attacking is false and attacking animation is false
            _animator.SetBool("PlayerAttacking", false);
        }
    }
}
