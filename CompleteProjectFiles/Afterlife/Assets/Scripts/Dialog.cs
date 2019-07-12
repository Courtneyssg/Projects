using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{

    public GameObject _DialogWindow;
    public Text _DialogText;
    public string[] _Rosesentences1;
    public string[] _Rosesentences2;
    public string[] _gotItRight;
    public string[] _gotItWrong;
    public string[] _Hubertsentences1;
    public string[] _goodish;
    public string[] _goodish2;
    public int _roseTrigger;
    public int _hubertTrigger;
    public int _goodishTrigger;
    public bool _isGoodish;
    public bool _isRose;
    public bool _isHubert;
    private int _index;
    public GameObject clickE;
    public GameObject _HubertPrompt;
    public GameObject _goodishPetName;
    public GameObject _findRichard;

    public InputField _beeInput;
    public GameObject _bee;
    public string _petname;

    public Dialog _roseDialog;
    public Dialog _goodishDialog;

    public GameObject _goGetKey;
    public GameObject _goToBasement;
    public bool _canGoToBasement;
    public bool _talkedToRose;

    public string[] evilText;
    public bool _isEvil;
    public int _evilTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _DialogWindow.SetActive(false);
        _DialogText.text = "";
        _petname = "";
        _canGoToBasement = false;
    }

    public void EnterInput()
    {
        _petname = _beeInput.text;
        if(_petname == "Bee" || _petname == "BEE" || _petname == "bee")
        {
            _roseTrigger = 4;
            Time.timeScale = 0.0f;
            _DialogWindow.SetActive(true);
            _DialogText.text = _gotItRight[_index];
            _bee.SetActive(false);
        }

        else
        {
            _roseTrigger = 5;
            Time.timeScale = 0.0f;
            _DialogWindow.SetActive(true);
            _DialogText.text = _gotItWrong[_index];
            _bee.SetActive(false);
        }
    }
    void Update()
    {
        
        if(_roseDialog._roseTrigger == 1 && _goodishDialog._goodishTrigger == 3)
        {
            _roseDialog._roseTrigger = 3;
        }
        if(_roseDialog._roseTrigger == 2 && _goodishDialog._goodishTrigger == 3)
        {
            _roseDialog._roseTrigger = 3;
        }

        if (clickE.activeInHierarchy)
        {
            if(_isEvil && _evilTrigger == 1)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Time.timeScale = 0.0f;
                    _DialogWindow.SetActive(true);
                    _DialogText.text = evilText[_index];
                    clickE.SetActive(false);
                }
            }
            if(_isRose && _roseTrigger == 1)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Time.timeScale = 0.0f;
                    _DialogWindow.SetActive(true);
                    _DialogText.text = _Rosesentences1[_index];
                    clickE.SetActive(false);  
                }
            }

            if(_isRose && _roseTrigger == 3)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    _DialogWindow.SetActive(true);
                    _DialogText.text = _Rosesentences2[_index];
                    clickE.SetActive(false);
                }
            }

            if(_isHubert && _hubertTrigger == 1)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Time.timeScale = 0.0f;
                    _DialogWindow.SetActive(true);
                    _DialogText.text = _Hubertsentences1[_index];
                    clickE.SetActive(false);
                }
            }

            if (_isGoodish && _goodishTrigger == 1)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Time.timeScale = 0.0f;
                    _DialogWindow.SetActive(true);
                    _DialogText.text = _goodish[_index];
                    clickE.SetActive(false);
                }
            }

            if (_isGoodish && _goodishTrigger == 2)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Time.timeScale = 0.0f;
                    _DialogWindow.SetActive(true);
                    _DialogText.text = _goodish2[_index];
                    clickE.SetActive(false);
                }
            }
        }
    }

    public void NextDiaglog()
    {
        if (_isRose && _roseTrigger == 1)
        {
            if (_index <= _Rosesentences1.Length - 1)
            {
                if (_index == _Rosesentences1.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _index = 0;
                    Time.timeScale = 1.0f;
                    _talkedToRose = true;
                    _roseTrigger = 2;
                }
                else
                {
                    _index++;
                    _DialogText.text = _Rosesentences1[_index];
                }
            }
        }

        if (_isRose && _roseTrigger == 3)
        {
            if (_index <= _Rosesentences2.Length - 1)
            {
                if (_index == _Rosesentences2.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _index = 0;
                    _bee.SetActive(true);
                    
                    
                }
                else
                {
                    _index++;
                    _DialogText.text = _Rosesentences2[_index];
                }
            }
        }

        if (_isRose && _roseTrigger == 4)
        {
            if (_index <= _gotItRight.Length - 1)
            {
                if (_index == _gotItRight.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _goodishDialog._goodishTrigger = 2;
                    _index = 0;
                    Time.timeScale = 1.0f;
                }
                else
                {
                    _index++;
                    _DialogText.text = _gotItRight[_index];
                }
            }
        }

        if (_isRose && _roseTrigger == 5)
        {
            _DialogWindow.SetActive(false);
            _index = 0;
            Time.timeScale = 1.0f;
            _roseTrigger = 3;
        }



        if (_isHubert && _hubertTrigger == 1)
        {
            if (_index <= _Hubertsentences1.Length - 1)
            {
                if (_index == _Hubertsentences1.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _index = 0;
                    Time.timeScale = 1.0f;
                    _hubertTrigger = 2;
                }

                else
                {
                    _index++;
                    _DialogText.text = _Hubertsentences1[_index];
                }
            }
        }

        if (_isGoodish && _goodishTrigger == 1)
        {
            if (_index <= _goodish.Length - 1)
            {
                if (_index == _goodish.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _index = 0;
                    Time.timeScale = 1.0f;
                    _goodishTrigger = 3;
                    if (_talkedToRose)
                    {
                        _roseDialog._roseTrigger = 3;
                    }
                    else
                    {
                        _roseDialog._roseTrigger = 1;
                    }
                    
                }

                else
                {
                    _index++;
                    _DialogText.text = _goodish[_index];
                }

            }
        }

        if (_isGoodish && _goodishTrigger == 2)
        {
            if (_index <= _goodish2.Length - 1)
            {
                if (_index == _goodish2.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _index = 0;
                    Time.timeScale = 1.0f;
                    _canGoToBasement = true;
                    _goodishTrigger = 4;
                }

                else
                {
                    _index++;
                    _DialogText.text = _goodish2[_index];
                }

            }
        }

        if(_isEvil && _evilTrigger == 1)
        {

            if (_index <= evilText.Length - 1)
            {
                if (_index == evilText.Length - 1)
                {
                    _DialogWindow.SetActive(false);
                    _index = 0;
                    _evilTrigger = 2;
                    Time.timeScale = 1.0f;

                }
                else
                {
                    _index++;
                    _DialogText.text = evilText[_index];
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && this.gameObject.tag == "Evil")
        {
            _isEvil = true;
            if(_evilTrigger == 1)
            {
                clickE.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);
                clickE.SetActive(true);
            }
        }
        if (other.tag == "Player" && this.gameObject.tag == "Rose")
        {
            _isRose = true;
            if (_roseTrigger == 1 || _roseTrigger == 3)
            {
                clickE.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                clickE.SetActive(true);
            }

            if (_roseTrigger == 2)
            {

                _findRichard.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                _findRichard.SetActive(true);
            }

            if(_roseTrigger == 4)
            {
                _goGetKey.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                _goGetKey.SetActive(true);
            }

            
        }

        if(other.tag == "Player" && this.gameObject.tag == "Hubert")
        {
            _isHubert = true;
            if(_hubertTrigger == 1)
            {
                clickE.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                clickE.SetActive(true);
            }

            if(_hubertTrigger == 2)
            {
                _HubertPrompt.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                _HubertPrompt.SetActive(true);
            }
            
        }

        if(other.tag == "Player" && this.gameObject.tag == "Goodish")
        {
            _isGoodish = true;
            if(_goodishTrigger == 1 || _goodishTrigger == 2)
            {
                clickE.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                clickE.SetActive(true);
            }

            if (_goodishTrigger == 3)
            {
                _goodishPetName.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                _goodishPetName.SetActive(true);
            }

            if(_goodishTrigger == 4)
            {
                _goToBasement.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                _goToBasement.SetActive(true);
                
            }

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && this.gameObject.tag == "Rose")
        {
            _isRose = false;
            if (_roseTrigger == 1 || _roseTrigger == 3)
            {
                clickE.SetActive(false);
            }
            if(_roseTrigger ==2)
            {
                _findRichard.SetActive(false);
            }


        }

        if (other.tag == "Player" && this.gameObject.tag == "Hubert")
        {
            _isHubert = false;
            if(_hubertTrigger == 1)
            {
                clickE.SetActive(false);
            }
            if(_hubertTrigger == 2)
            {
                _HubertPrompt.SetActive(false);
            }
            
        }

        if(other.tag == "Player" && this.gameObject.tag == "Goodish")
        {
            _isGoodish = false;
            if(_goodishTrigger ==1 || _goodishTrigger == 2)
            {
                clickE.SetActive(false);
            }

            if(_goodishTrigger == 3)
            {
                _goodishPetName.SetActive(false);
            }

            if(_goodishTrigger == 4)
            {
                _goToBasement.SetActive(false);
            }
        }

        if(other.tag == "Player" && this.gameObject.tag == "Evil")
        {
            _isEvil = false;
            if(_evilTrigger == 1)
            {
                clickE.SetActive(false);
            }
        }
    }

}
