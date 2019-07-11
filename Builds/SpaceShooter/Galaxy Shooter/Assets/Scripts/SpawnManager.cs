using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField]
    private GameObject _enemyShipPrefab; // Reference to enemy gameobject 
    [SerializeField]
    private GameObject[] powerups; // Reference to array holding powerup gameobjects
    private GameManager _gameManager; // Reference to GameManager
    private float _max = 7.78f; // Maximum x position on screen
    private float _min = -7.78f;// Minimum x position on screen
    private float _topOfScreen = 6.44f; // top of screen where enemies and powerups spawn

    // Use this for initialization
    void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // get access to gamemanager
        StartCoroutine(EnemySpawnRoutine()); // Call coroutine method
        StartCoroutine(PowerUpSpawnRoutine()); // Call coroutine method
	}
    // Method that starts spawning
    public void StartSpawnRoutines()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }
	
    IEnumerator EnemySpawnRoutine(){
        while(!(_gameManager.gameOver)) //while gameover is false
        {
            //Instantiate the enemy prefab at a random x position at the top of the window, wait 5 seconds, and spawn another
            Instantiate(_enemyShipPrefab, new Vector3(Random.Range(_min, _max), _topOfScreen, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator PowerUpSpawnRoutine()
    {
        while(!(_gameManager.gameOver)) // while gameover is false
        {
            int randomPowerUp = Random.Range(0, 3); // Randomly pick a powerup from the array of powerup game objects
            // Instantite that powerup at a random x position at the top of the window, wait 5 seconds, and spawn another.
            Instantiate(powerups[randomPowerUp], new Vector3(Random.Range(_min, _max), _topOfScreen, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
        
    }
}
