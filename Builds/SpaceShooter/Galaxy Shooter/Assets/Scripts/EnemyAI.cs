using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public static float _enemySpeed = 5.0f; // Current enemy starting speed
    private float _topOfScreen = 6.44f;     // Y position to move enemy position to
    private float _endOfScreen = -6.44f;    // Y position when enemy leaves screen view
    private float _min = -7.78f;            // Minimum x value enemies can spawn at
    private float _max = 7.78f;             // Maximum Y value enemies can spawn at
    [SerializeField]
    private GameObject _enemyExplosion;     // Enemy explosion prefab
    private UIManager _uiManager;           // Reference to UIManager
    private GameManager _gameManager;       // Reference to GameManager
    [SerializeField]
    private AudioClip _audioClip;           // Reference to AudioClip

    void Start()
    {
        // Initialize starting position at a random x location and y = 6.44
        transform.position = new Vector3(Random.Range(_min, _max), _topOfScreen, 0);
        // Get access to the UIManager
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // Get access to the GameManager
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Downward movement
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        // if the y position of the enemy goes below -6.44 and gameover is false
        //move the position of the enemy to 6.44 (top) at a randomized x
        if (transform.position.y <= _endOfScreen & !(_gameManager.gameOver))
        {
            transform.position = new Vector3(Random.Range(_min, _max), _topOfScreen, 0);
        }
        if (_gameManager.gameOver)
        {
            Destroy(this.gameObject, 4f); // If player dies and gameover is true, destroy the remaining enemy game objects after 4 seconds
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")  // If Enemy collides with laser
        {
            if (other.transform.parent != null) // Check to make sure object exists
            {
                Destroy(other.transform.parent.gameObject); // Destroy parent in case of triple shot
            }
            Destroy(other.gameObject); // Destroy laser
            Instantiate(_enemyExplosion, transform.position, Quaternion.identity); // Instantiate the enemy explosion animation
            _uiManager.UpdateScore(); // Update the players score
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f); // Play  enemy explosion audioclip
            Destroy(this.gameObject); // Destroy the enemy
        }

        if (other.tag == "Player") // If Enemy collides with Player
        {
            Player player = other.GetComponent<Player>(); // Get access to player
            if (player != null) // If player exists
            {
                player.Damage(); // call the damage method on player
            }
            Instantiate(_enemyExplosion, transform.position, Quaternion.identity); // Instantiate enemy explosion animation
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f); // Play explosion sound clip
            Destroy(this.gameObject); // Destroy enemy
        }
    }
}