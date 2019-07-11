using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    [SerializeField]
    private Text _ammoText; //Text that tells you how much ammo is left
    [SerializeField]
    private GameObject _coin; // Notifies the player to press E when near coin
    [SerializeField]
    private GameObject _Intro; // Pop-up that gives player some context to scene
    [SerializeField]
    private GameObject _introText; // Text to display on intro sign
    public GameObject _coinPickup; // Prompts user to click E to pick up coin
    private bool _countDown = false; // allows countdown to start when player closes menu
    private float _count = 30.0f; // amount of time player has to disarm the bomb
    [SerializeField]
    private Text _timeLeft; // Text display of timer on Canvas
    [SerializeField]
    private GameObject _code; // The code gameobject 
    public GameObject _buyGun; // Prompts user to press E to buy gun
    public GameObject _pickPocket; //Prompts user to press E to get the code
    public GameObject bombWithCode; // Notifies the player to click E to interact with bomb
    public GameObject bombWithoutCode; // Notifies the player they need a bomb code
    public GameObject inputField; // takes the user input and a code entry for bomb
    public GameObject Explosion; // Explosion animation with bomb detonates
    public GameObject youLose; // You Lose text which is displayed if time runs out
    private int createExplostion = 1; // extra helper to deactivate a text notification


    void Start()
    {
        StartCoroutine(IntroMessage()); // calls coroutine to display sign
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _Intro.SetActive(false);
            _introText.SetActive(false);
            _countDown = true;
        }

        if(_countDown)
        {
            _count -= Time.deltaTime;
            _timeLeft.text = "TIME LEFT: " + _count.ToString("F0");
            

            if(_count <= 10)
            {
                _timeLeft.color = new Color(255f, 0f, 0f);
                _timeLeft.fontSize = 60;
            }
            if(_count <= 0)
            {
                _timeLeft.text = "TIMES UP!";
                if(createExplostion > 0)
                {
                    Instantiate(Explosion, new Vector3(8.62f, 1.282606f, 3f), Quaternion.identity);
                    GameObject bomb = GameObject.Find("Bomb");
                    AudioSource playbomb = bomb.GetComponent<AudioSource>();
                    playbomb.Play(); // play shoot audio when mouse button is held down
                    createExplostion = 0;
                }

                inputField.SetActive(false);
                bombWithCode.SetActive(false);

                youLose.SetActive(true);

            }
        }
    }


    public void UpdateAmmo(int count)
    {
        _ammoText.text = "Ammo: " + count;
    }

    public void CollectedCoin()
    {
        _coin.SetActive(true);
    }

    public void CollectedCode()
    {
        _code.SetActive(true);
       
    }

    public void RemoveCoin()
    {
        _coin.SetActive(false);
    }


    IEnumerator IntroMessage()
    {
        yield return new WaitForSeconds(3);
        _Intro.SetActive(true);
        _introText.SetActive(true);
    }
}
