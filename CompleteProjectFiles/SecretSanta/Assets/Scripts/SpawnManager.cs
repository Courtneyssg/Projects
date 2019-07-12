using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _gift;
    public int _numberOfGiftsSpawned = 0;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private Transform _player;

    void Start () {

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(GiftSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        float time = 5f;
        while (!_gameManager.gameOver)
        {
            bool canSpawnEnemy = true;
            for(int i = 200; i<= 1600; i = i + 200)
            {
                if(_uiManager.score == i)
                {
                    time -= 0.2f;
                }
            }
            int randomEnemyX = Random.Range(-20, 21);
            int randomEnemyY = Random.Range(-20, 21);
            Vector3 checkEnemyPos = new Vector3(randomEnemyX, randomEnemyY, 0);
            if (checkEnemyPos.x > _player.transform.position.x - 10 && checkEnemyPos.x < _player.transform.position.x + 10 && checkEnemyPos.y > _player.transform.position.y - 10 && checkEnemyPos.y < _player.transform.position.y + 10)
            {
                canSpawnEnemy = false;
            }

            if(canSpawnEnemy)
            {
                Instantiate(_enemyPrefab, checkEnemyPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator GiftSpawnRoutine()
    {
        while(!_gameManager.gameOver)
        {
            //generate a random number for x pos
            //generate a random number for y pos
            //spawn at the randomized coords
            bool canSpawn = true;
            int randomX = Random.Range(-18, 18);
            int randomY = Random.Range(-18, 18);
            yield return new WaitForSeconds(10);
            Vector3 checkPosition = new Vector3(randomX, randomY, 0);
            if(checkPosition.x < 3 && checkPosition.x > -3 && checkPosition.y < -14 && checkPosition.y > -20) //make sure gift doesn't spawn in sleigh
            {
                canSpawn = false;
            }

            if(_numberOfGiftsSpawned == 0 && canSpawn)
            {
                Instantiate(_gift, checkPosition, Quaternion.identity);
                _numberOfGiftsSpawned = 1;
            }
        }
    }
}
