using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBombCode : MonoBehaviour {

    private UI_Manager uiManager;

    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        
    }

    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (other.tag == "Player")
        {
            if(uiManager != null)
            {
                if(!player.hasCode)
                {
                    uiManager._pickPocket.SetActive(true);
                }
                
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                if (player != null)
                {
                    player.hasCode = true;
                }

                    if (uiManager != null)
                    {
                        uiManager.CollectedCode();
                    }
                    
            }
            if(player.hasCode)
            {
                uiManager._pickPocket.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if(uiManager != null)
            {
                uiManager._pickPocket.SetActive(false);
            }
        }
    }
}
