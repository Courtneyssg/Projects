using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkShop : MonoBehaviour {

    private UI_Manager uiManager;

    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            
            Player player = other.GetComponent<Player>();
            if (uiManager != null)
            {
                if(player != null)
                {
                    if (player.hasCoin)
                    {
                        uiManager._buyGun.SetActive(true);
                    }
                }
                
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                if(player != null)
                {
                    if(player.hasCoin)
                    {
                        player.hasCoin = false;
                        
                        if(uiManager != null)
                        {
                            uiManager.RemoveCoin();

                        }
                        AudioSource audio = GetComponent<AudioSource>();
                        audio.Play();
                        player.EnableWeapons();
                        uiManager._buyGun.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Get out of here");
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            uiManager._buyGun.SetActive(false);
        }
    }
}
