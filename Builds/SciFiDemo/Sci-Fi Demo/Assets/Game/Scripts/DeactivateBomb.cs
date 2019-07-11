using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateBomb : MonoBehaviour {

    private UI_Manager uiManager;
    private bool active = false;

    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(other.tag == "Player")
        {
            if(player.hasCode && !active)
            {
                uiManager.bombWithCode.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    uiManager.inputField.SetActive(true);
                    active = true;
                }
            }

            if(!player.hasCode)
            {
                uiManager.bombWithoutCode.SetActive(true);
            }

            if(active)
            {
                uiManager.bombWithCode.SetActive(false);
            }

        }


    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(other.tag == "Player")
        {
            uiManager.bombWithCode.SetActive(false);
            uiManager.bombWithoutCode.SetActive(false);
        }
    }
}
