using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public bool gameOver = false;
    public bool paused = false;
    private UIManager _uiManager;

	// Use this for initialization
	void Start () {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
	}
	
	// Update is called once per frame
	void Update () {

        OnPause();
		
	}

    public void OnPause()
    {
        if(!gameOver)
        {
            if (!paused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _uiManager.ShowPauseScreen();
                }
            }

            else if (paused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _uiManager.HidePauseScreen();
                }
            }
        }
       

    }


}
