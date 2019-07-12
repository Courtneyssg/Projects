using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool gameOver = true; // Reference to gameover variable
    public GameObject player; // Reference to player
    private UIManager _uiManager; // Reference to UIManager
    public GameObject enemy; // Reference to enemy gameobject 
    public GameObject enemyExplosion; // Reference to enemyExplosion gameobject

	void Start () {
        // Get access to UIManager
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
	}
	
	void Update () {
        // If Gameover is true
        if(gameOver)
        {
            // Reset enemy size and enemyExplosion size back to the original size 
            enemy.transform.localScale = new Vector3(1, 1, 1);
            enemyExplosion.transform.localScale = new Vector3(1, 1, 1);
            // If user presses space bar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //respawn player
                Instantiate(player, Vector3.zero, Quaternion.identity);
                gameOver = false; // gameover is false
                _uiManager.HideTitleScreen(); //hide the title screen
            }
        }
	}
}
