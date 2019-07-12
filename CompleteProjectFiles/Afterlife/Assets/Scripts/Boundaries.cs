using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    //Text prompts for travelling between floors
    [SerializeField]
    private GameObject _goUpstairs, _goDownstairs, _goBasement, _leaveBasement;
    //Bools that indicate level player is currently on
    public bool _upstairs, _downstairs, _basement;
    //For limiting player movement on right and left sides
    [SerializeField]
    private float _upstairsLimitLeft, _upstairsLimitRight, _downstairsLimitLeft, _downstairsLimitRight, _basementLimitLeft, _basementLimitRight;
    // Space from prompt player has to be to activate it
    [SerializeField]
    private float _activationDistance;
    [SerializeField]
    private Dialog _dialog;
    

    void Start()
    {
        _upstairs = false;
        _downstairs = true;
        _basement = false;
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT LIMITATIONS FOR UPSTAIRS

        if (_upstairs && transform.position.x <= _upstairsLimitLeft)
        {
            transform.position = new Vector2(_upstairsLimitLeft, transform.position.y);
        }

        if(_upstairs && transform.position.x >= _upstairsLimitRight)
        {
            transform.position = new Vector2(_upstairsLimitRight, transform.position.y);
        }

        //MOVEMENT LIMITATIONS FOR DOWNSTAIRS

        if (_downstairs && transform.position.x <= _downstairsLimitLeft)
        {
            transform.position = new Vector2(_downstairsLimitLeft, transform.position.y);
        }

        if (_downstairs && transform.position.x >= _downstairsLimitRight)
        {
            transform.position = new Vector2(_downstairsLimitRight, transform.position.y);
        }

        //MOVEMENT LIMITATIONS FOR BASMENT

        if (_basement && transform.position.x <= _basementLimitLeft)
        {
            transform.position = new Vector2(_basementLimitLeft, transform.position.y);
        }

        if (_basement && transform.position.x >= _basementLimitRight)
        {
            transform.position = new Vector2(_basementLimitRight, transform.position.y);
        }

        // ACTIVATE PROMPT TO GO UPSTAIRS
        Debug.Log(_goUpstairs.transform.position.x - _activationDistance + " " + _goUpstairs.transform.position.x + _activationDistance);
        if (_downstairs && transform.position.x >= _goUpstairs.transform.position.x - _activationDistance
        && transform.position.x <= _goUpstairs.transform.position.x + _activationDistance)
        {
            _goUpstairs.SetActive(true);
        }
        else
        {
            _goUpstairs.SetActive(false);
        }

        // ACTIVATE PROMPT TO GO DOWNSTAIRS

        if (_upstairs && transform.position.x >= _goDownstairs.transform.position.x - _activationDistance
        && transform.position.x <= _goDownstairs.transform.position.x + _activationDistance)
        {
            _goDownstairs.SetActive(true);
        }

        else
        {
            _goDownstairs.SetActive(false);
        }

        // ACTIVATE PROMPT TO GO TO BASEMENT

        if (_downstairs && transform.position.x >= _goBasement.transform.position.x - _activationDistance
        && transform.position.x <= _goBasement.transform.position.x + _activationDistance && _dialog._canGoToBasement)
        {
            _goBasement.SetActive(true);
        }

        else
        {
            _goBasement.SetActive(false);
        }

        // ACTIVATE PROMPT TO LEAVE BASEMENT

        if (_basement && transform.position.x >= _leaveBasement.transform.position.x - _activationDistance
        && transform.position.x <= _leaveBasement.transform.position.x + _activationDistance)
        {
            _leaveBasement.SetActive(true);
        }

        else
        {
            _leaveBasement.SetActive(false);
        }

        //CONTROLS TO TRAVEL BETWEEN LOCATIONS

        if(_goUpstairs.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                transform.position = _goDownstairs.transform.position;
                _upstairs = true;
                _downstairs = false;
                _basement = false;
            }
        }

        if(_goDownstairs.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                transform.position = _goUpstairs.transform.position;
                _downstairs = true;
                _upstairs = false;
                _basement = false;
            }
        }

        if(_goBasement.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                transform.position = _leaveBasement.transform.position;
                _downstairs = false;
                _upstairs = false;
                _basement = true;
            }
        }

        if(_leaveBasement.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                transform.position = _goBasement.transform.position;
                _downstairs = true;
                _basement = false;
                _upstairs = false;
            }
        }
    }
}
