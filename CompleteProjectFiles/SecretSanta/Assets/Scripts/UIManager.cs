using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public int score; // current score
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Sprite[] lives;
    [SerializeField]
    private Image livesImageDisplay;   // Holds life sprite as updated
    [SerializeField]
    private GameObject _pausedText;
    public GameObject _gameOverPanel;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private Player _player;
    private int counter = 0;
    [SerializeField]
    private GameObject _levelUp;
    public Text highScoreDisplay;
    public Text currentScoreOnGameOver;
    public Text HighScoreOnGameOver;
    public Text BonusScoreText;
    public Text newScoreTotal;
    public Text giftTotal;
    [SerializeField]
    private DataController _dataController;
    public int giftCount;
    private int bonusScore;
    private int totalScore;
    [SerializeField]
    AudioClip _clip;


	// Use this for initialization
	void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        if (counter > 0)
        {
           
            _levelUp.SetActive(true);

            if(Input.GetKeyDown(KeyCode.L))
            {
                //if L is pressed
                Enemy._speed += 0.5f; // enemy speed is increased again
                score += 100;
                counter -= 1;
            }
        }


        if (counter <= 0)
        {
            _levelUp.SetActive(false);
        }

        

    }


    public void UpdateLives(int currentLives)
    {
        if(_player._playerLives <= 3 && _player._playerLives >= 0)
        {
            livesImageDisplay.sprite = lives[currentLives];
        }
        
    }

    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "" + score;
        LevelUp();
        _dataController.SubmitNewPlayerScore(score);
        highScoreDisplay.text = "High Score: " + _dataController.GetHighestScore().ToString();
        
    }

    public void ShowPauseScreen()
    {
        _gameManager.paused = true;
        Time.timeScale = 0f;
        _pausedText.SetActive(true);
    }

    public void HidePauseScreen()
    {
        _gameManager.paused = false;
        Time.timeScale = 1f;
        _pausedText.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        currentScoreOnGameOver.text = "Score: " + score;
        BonusScoreText.text = "Gift Bonus: " + bonusScore;
        totalScore = score + bonusScore;
        newScoreTotal.text = "Total Score: " + totalScore;
        _dataController.SubmitNewPlayerScore(totalScore);
        HighScoreOnGameOver.text = "High Score: " + _dataController.GetHighestScore().ToString();
        float high = _dataController.GetHighestScore();
        if(score > high)
        {

        }
        while (giftCount != 0)
        {
            bonusScore += 100;
            BonusScoreText.text = "Gift Bonus: " + bonusScore;
            totalScore = score + bonusScore;
            newScoreTotal.text = "Total Score: " + totalScore;
            _dataController.SubmitNewPlayerScore(totalScore); // submit score + bonus in this part because score is already checked
            HighScoreOnGameOver.text = "High Score: " + _dataController.GetHighestScore().ToString();
            giftCount -= 1;
        }
        if(giftCount == 0)
        {
            giftTotal.text = "= " + giftCount;
        }
        _gameManager.gameOver = true;
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        //submit the new player score and ask the data controller for the new highscore so we can display it
    }

    public void HideGameOverScreen()
    {
        highScoreDisplay.text = "High Score: " + _dataController.GetHighestScore().ToString();
        _player._playerLives = 3;
        UpdateLives(_player._playerLives);
        Enemy._speed = 1;
        score = 0;
        scoreText.text = "" + score;
        Time.timeScale = 1f;
        _gameManager.gameOver = false;
        _gameOverPanel.SetActive(false);
        _spawnManager._numberOfGiftsSpawned = 0;
        _player.transform.position = new Vector3(0, 0, 0);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LevelUp()
    {
             
        for (int i = 100; i <= 1300; i = i + 200)
        {
            //for each score 100 300 500 700 
            if(score == i)
            {
                Enemy._speed += 0.5f;
            }
        } 
        
        if(score >= 1500)
        {
            //if score >1500 add 200 until get to 4900 for score, for each time score = that interation
            for(int i = 1500; i<= 4500; i = i + 500)
            {
                
                if(score == i)
                {
                    AudioSource.PlayClipAtPoint(_clip, _player.transform.position);
                    counter += 1;
                }
            }
            

        }

    }

}
