using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files

public class DataController : MonoBehaviour
{
    private Highscore _highscore;
    private UIManager _uiManager;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        LoadHighscore();
        _uiManager.highScoreDisplay.text = "High Score: " + GetHighestScore().ToString();
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        if(newScore > _highscore.highestScore)
        {
            _highscore.highestScore = newScore;
            SaveHighscore();
        }
    }

    public int GetHighestScore()
    {
        return _highscore.highestScore;
    }

    private void LoadHighscore()
    {
        _highscore = new Highscore();
        if(PlayerPrefs.HasKey("highestScore"))//if we already stored highest score
        {
            //we are going to load that score
            //key is highestScore, value we get back if there is one is the score stored
            _highscore.highestScore = PlayerPrefs.GetInt("highestScore");
            // 
        }

    }

    private void SaveHighscore()
    {
        PlayerPrefs.SetInt("highestScore", _highscore.highestScore);
        //using setint to store the highest score in the player prefs
    }

    public void ResetHighScore()
    {

        PlayerPrefs.SetInt("highestScore", 0);
        _highscore.highestScore = 0;
        LoadHighscore();
        _uiManager.HighScoreOnGameOver.text = "High Score: " + GetHighestScore().ToString();
        _uiManager.highScoreDisplay.text = "High Score: " + GetHighestScore().ToString();

    }
}
