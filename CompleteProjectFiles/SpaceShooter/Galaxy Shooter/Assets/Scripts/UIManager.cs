using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite[] lives;            // Holds an array of life sprites
    public Image livesImageDisplay;   // Holds life sprite as updated
    public Text scoreText;            // Score text
    public Text levelText;            // Level text
    public int level;            // Current level
    public int score;            // Current Score
    public GameObject titleScreen;    // Main Screen
    private AudioSource _audio;       // Reference to AudioSource
    public GameObject enemy;          // Enemy Ship
    public GameObject enemyExplosion; // Enemy Explosion

    public void Start()
    {
        // Get access to AudioScoure component attached to Main Camera
        _audio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    // Method updates the image displaying life total
    public void UpdateLives(int currentLives)
    {
        livesImageDisplay.sprite = lives[currentLives];
    }

    // Method updates the current level the player is on (called when score >= 100)
    public void UpdateLevel()
    {
        if(score % 10 == 0) // If the score is a multiple of 10 
        {
            level += 1;                         // Increment level
            levelText.text = "Level: " + level; // Update level text to show current level
            _audio.pitch += 0.01f;              // Increase the pitch of the background music
            EnemyAI._enemySpeed += 0.2f;        // The enemies speed is incremented by 0.2 with each level increase
           
            if(enemy.transform.localScale.x >= 0.3 & enemy.transform.localScale.y > 0.3 & enemy.transform.localScale.z > 0.3)
            {
                // Decrease enemy size by 0.05 with each level increase
                enemy.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                // Decrease enemy explosion size by 0.025 with each level increase
                enemyExplosion.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
        }  
    }
    // Updates the score
    public void UpdateScore()
    {
        score += 10;                        // 10 points per enemy killed
        scoreText.text = "Score: " + score; // Updates score text to display current score
        if(score >= 100)                    // If the score is >= 100 call the UpdateLevel() method
        {
            UpdateLevel();
        }
    }

    // Sets the title screen to active when the player dies
    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
    }

    // Sets the title screen to not active when the player presses space to start the game
    public void HideTitleScreen()
    {
        score = 0;
        level = 0;
        scoreText.text = "Score: " + score; // Update score text so on start score = 0
        levelText.text = "Level: " + level; // Update level text so on start level = 1
        titleScreen.SetActive(false);
        
    }

}
