using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUserInput : MonoBehaviour {

    [SerializeField]
    private GameObject _youWin;
    public GameObject Explosion;
    [SerializeField]
    private GameObject _youLose;


    public void GetInput(string code)
    {
        UI_Manager uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        Player player = GetComponent<Player>();
        if (code == "238")
        {
            _youWin.SetActive(true);
            uiManager.inputField.SetActive(false);
            uiManager.bombWithCode.SetActive(false);

        }

        if(code != "238")
        {
            
            Instantiate(Explosion, new Vector3(8.62f, 1.282606f, 3f), Quaternion.identity);
            GameObject bomb = GameObject.Find("Bomb");
            AudioSource playbomb = bomb.GetComponent<AudioSource>();
            playbomb.Play();
            uiManager.inputField.SetActive(false);
            uiManager.bombWithCode.SetActive(false);

            _youLose.SetActive(true);


            


        }
    }
}
