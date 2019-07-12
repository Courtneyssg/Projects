using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject _mainMenuPanel;
    [SerializeField]
    private GameObject _tutorialPanel;

    void Start()
    {
        _mainMenuPanel.SetActive(true);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToTutorial()
    {
        _mainMenuPanel.SetActive(false);
        _tutorialPanel.SetActive(true);
    }

}
