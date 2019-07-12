using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gift : MonoBehaviour {

    private Transform _player;
    private Player _playermove;
    private bool isOutsideBounds;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;
    [SerializeField]
    private AudioClip _clip;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _playermove = GameObject.FindWithTag("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
    }

    void Update()
    {
        DestroyGift();

        PutGiftsInSleigh();

        PreventCollisionWithTrees();

        CreateBoundaries();

        MoveWithPlayer(); 
    }

    public void DestroyGift()
    {
        if(_gameManager.gameOver)
        {
            Destroy(gameObject);
        }  
    }

    private void PutGiftsInSleigh()
    {
        if (transform.position.y <= -14 && transform.position.y >= -19.9f && transform.position.x >= -3 && transform.position.x <= 3)
        {
            _spawnManager._numberOfGiftsSpawned = 0;

            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            if (transform.localScale.x <= 0 && transform.localScale.y <= 0)
            {
                _uiManager.giftCount += 1;
                _uiManager.giftTotal.text = "= " + _uiManager.giftCount;
                AudioSource.PlayClipAtPoint(_clip, transform.position);
                Destroy(gameObject);
            }
        }
    }

    private void PreventCollisionWithTrees()
    {
        float px = transform.position.x;
        float py = transform.position.y;
        //first tree to right, second tree to top, third tree to left, fourth tree to bototm, fifth, two trees together, sixth bottom right, seventh top right
        if((px > 8.1 && px < 12 && py > 1.5 && py < 3.5f) || (py > 12.5f && py < 14.5f && px < -0.1f && px > -3.8f) || (px < -13.3f && px > -16.8f && py > 2.5f && py < 4.5f)
            || (py < -11.4f && py > -13.4f && px < 1.8f && px > -1.8f) || (px > -18.3f && px < -13 && py < -16.4f && py > -19) 
            || (px > 13.1f && px < 16.6f && py < -18.1f) || (py > 17.5f && py < 19.9f && px > 17.1f))
        {
            if(_playermove._moveDirection.x == 1 && _playermove._moveDirection.y == 0)
            {
                //moving right
                transform.position -= new Vector3(1, 0, 0);
            }

            if(_playermove._moveDirection.x == -1 && _playermove._moveDirection.y == 0)
            {
                //moving left
                transform.position += new Vector3(1, 0, 0);
            }

            if (_playermove._moveDirection.x == 0 && _playermove._moveDirection.y == -1)
            {
                //moving down
                transform.position += new Vector3(0,1,0);
            }

            if(_playermove._moveDirection.x == 0 && _playermove._moveDirection.y == 1)
            {
                //moving up
                transform.position -= new Vector3(0, 1, 0);
            }

        }
    }

    private void CreateBoundaries()
    {
        if (transform.position.x >= 19.8f)
        {
            isOutsideBounds = true;
            transform.position = new Vector3(18, transform.position.y, transform.position.z);
        }

        if (transform.position.y >= 19.8f)
        {
            isOutsideBounds = true;
            transform.position = new Vector3(transform.position.x, 18, transform.position.z);

        }
        if (transform.position.x <= -19.8f)
        {
            isOutsideBounds = true;
            transform.position = new Vector3(-18, transform.position.y, transform.position.z);
        }
        if (transform.position.y <= -19.8f)
        {
            isOutsideBounds = true;
            transform.position = new Vector3(transform.position.x, -18, transform.position.z);
        }
    }

    private void MoveWithPlayer()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        Vector3 scale = transform.localScale;
        isOutsideBounds = false;

        if (!isOutsideBounds)
        {
            if (_player.position.x >= transform.position.x - (scale.x - 0.1f) && _player.position.x <= transform.position.x + (scale.x - 0.1f) && _player.position.y >= transform.position.y - (scale.y - 0.1f) && _player.position.y <= transform.position.y + (scale.y - 0.1f))
            {
                if (_player.position.y > transform.position.y) // at top
                {
                    if (_playermove._moveDirection.y == -1 && _playermove._moveDirection.x == 0)
                    {
                        render.sortingOrder = 5;
                        transform.position = new Vector3(transform.position.x, _player.position.y - 0.9f, _player.position.z);
                    }

                    else
                    {
                        transform.position = transform.position;
                    }

                }

                if (_player.position.y < transform.position.y) // player is below the gift position
                {
                    if (_playermove._moveDirection.y == 1 && _playermove._moveDirection.x == 0)
                    {

                        render.sortingOrder = 3;
                        transform.position = new Vector3(transform.position.x, _player.position.y + 0.9f, _player.position.z);
                    }

                    else
                    {
                        transform.position = transform.position;
                    }
                }


                if (_player.position.x > transform.position.x) // if player position > gift position (to the right)
                {
                    //player is on the right side of the gift
                    if (_playermove._moveDirection.x == -1 && _playermove._moveDirection.y == 0)
                    {
                        transform.position = new Vector3(_player.position.x - 0.85f, transform.position.y, _player.position.z);

                    }

                    else
                    {
                        transform.position = transform.position;
                    }
                }


                if (_player.position.x < transform.position.x) //if player position < gift position (to the left)
                {
                    //player is on the left side of the gift
                    if (_playermove._moveDirection.x == 1 && _playermove._moveDirection.y == 0)
                    {
                        transform.position = new Vector3(_player.position.x + 0.85f, transform.position.y, _player.position.z);
                    }

                    else
                    {
                        transform.position = transform.position;
                    }
                }

            }
        }
    }

}
