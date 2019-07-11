using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] 
    private float _speed = 5.0f; // Player speed
    [SerializeField]
    private int _lives = 3; // Total player lives
    [SerializeField]
    private GameObject _laserPrefab; // Reference to laser prefab
    [SerializeField]
    private GameObject _tripleShotPrefab; // Reference to tripleShot prefab
    [SerializeField]
    private GameObject _explosion; // Reference to explosion prefab
    [SerializeField]
    private GameObject _Shield; // Reference to shield prefab
    [SerializeField]
    private bool hasShields = false; // shields inilized to false
    [SerializeField]
    private float _fireRate = 0.25f; // The rate at which we let player fire laser
    private float _canFire = 0.0f; // Added to time.time 
    public bool canTripleShot = false; // Can't do tripleshot
    public bool speedUp = false; // Can't speedup
    private UIManager _uiManager; // Reference to UIManager
    private GameManager _gameManager; // Reference to GameManager
    private SpawnManager _spawnManager; // Reference to SpawnManager
    private AudioSource _audioSource; // reference to AudioSource
    [SerializeField]
    private GameObject[] _engines; // Reference to all the different engine gameobjects
    private int _hitCount = 0; // Keeps track of how many times the player was hit to instantiate engines

    void Start () {
        // Player starts at this position
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>(); // Get access to UIManager

        if(_uiManager != null)
        {
            _uiManager.UpdateLives(_lives); //call UpdateLives method in uimanager with current lives
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get access to gamemanager
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // Get access to spawnmanager
         
        if(_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines(); // Call method in spawnmanager to start spawning
        }

        _audioSource = GetComponent<AudioSource>(); // Get access to audiosource
        _hitCount = 0; // initialize hitcount to 0 
	}
	
	// Update is called once per frame
	void Update () {

        Movement(); // Call movement method
        Shoot();    // Call Shoot method
	}

    private void Movement()
    {
        // Movement if player gets speed boost
        if (speedUp)
        {
            transform.Translate(Vector3.right * (_speed * 1.5f) * Input.GetAxis("Horizontal") * Time.deltaTime);
            transform.Translate(Vector3.up * (_speed * 1.5f) * Input.GetAxis("Vertical") * Time.deltaTime);
        }

        // Otherwise normal movement
        transform.Translate(Vector3.right * _speed * Input.GetAxis("Horizontal") * Time.deltaTime);
        transform.Translate(Vector3.up * _speed * Input.GetAxis("Vertical") * Time.deltaTime);

        if (transform.position.y > 0)
        {
            // Prevents player from moving above 0 on the y axis
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        else if (transform.position.y < -4.2f)
        {
            // Prevents player from moving off the screen on the bottom
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        if (transform.position.x > 9.5f)
        {
            // Loops player to the other side of the window if go too far right
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }

        else if (transform.position.x < -9.5f)
        {
            // Loops player to the other side of the window if go too far left
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // If space pressed or mouse clicked
        {
            
            if (Time.time > _canFire)// If the time that the game has been running is greater than canfire
            {
                _audioSource.Play(); // Play laser audio clip
                if (canTripleShot) // If tripleshot is enabled
                {
                    Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity); // Instantiate tripleshot
                }
                else
                {
                    //else instantiate the regular laser
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity); 
                }

                _canFire = Time.time + _fireRate; //time game has been running + fireRate adding firerate allows the game to make sure there is a delay
            }
        }
    }
    // Makes tripleshot true and calls the tripleshot power down routine method
    public void TripleShotPowerUpOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    // Makes speedup true and calls the speedup power down routine method
    public void SpeedBoostOn()
    {
        speedUp = true;
        StartCoroutine(SpeedUpPowerDownRoutine());
    }
    // Enables shields and sets hasShields to true
    public void EnableShields()
    {
        _Shield.SetActive(true);
        hasShields = true;
    }
    // Creates a time delay for the player to use speed up and then makes speedup false
    public IEnumerator SpeedUpPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        speedUp = false;
    }
    // Creates a time delay for the player to use triple shot and then makes tripleshot false
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    public void Damage()
    {
        if(hasShields) // If the player has shields
        {
            hasShields = false; // Player doesn't have shields
            _Shield.SetActive(false); // Shields are not active
            return;
        }
        // Each time player takes damage hitcount increments
        _hitCount += 1;
        if (_hitCount == 1)
        {
            _engines[1].SetActive(true); // If hitcount is 1 then first engine failure is set to active
        }

        if (_hitCount == 2)
        {
            _engines[0].SetActive(true); // If hitcount is 2 then second engine failure is set to active
        }

        _lives -= 1; // Each hit decreases life count by 1
        _uiManager.UpdateLives(_lives); // Call Update lives method in uimanager
        if(_lives == 0)
        {   
            // If player lives = 0 instantiate explosion, gameover = true, the titlescreen is displayed, and the player is destroyed
            Instantiate(_explosion, transform.position, Quaternion.identity);
           _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }
}
