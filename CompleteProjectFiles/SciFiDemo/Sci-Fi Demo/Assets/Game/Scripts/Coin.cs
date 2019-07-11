using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField]
    private AudioClip _coinPickup;
    private UI_Manager uiManager;

    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            UI_Manager uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
            if(uiManager != null)
            {
                uiManager._coinPickup.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player player = other.GetComponent<Player>();
                if(player != null)
                {
                    player.hasCoin = true;
                    AudioSource.PlayClipAtPoint(_coinPickup, transform.position, 1f);
                    
                    if(uiManager != null)
                    {
                        uiManager.CollectedCoin();
                    }
                    Destroy(this.gameObject);
                    uiManager._coinPickup.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (uiManager != null)
            {
                uiManager._coinPickup.SetActive(false);
            }
        }
    }

}
