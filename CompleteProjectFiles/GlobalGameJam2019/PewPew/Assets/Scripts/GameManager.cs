using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public bool gameWon = false;
    private bool wonLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Application.Quit();
        }
        if (gameWon && !wonLoaded)
        {
            wonLoaded = true;
            GetComponent<AnyManager>().gameWon();
        }
    }
}
