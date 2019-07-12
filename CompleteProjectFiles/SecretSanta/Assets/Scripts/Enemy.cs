using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {
    private GameObject _player;
    public static float _speed = 1f;
    private Animator _animator;       // Reference to animator
    private Vector3 _enemyMoveDirection;
    private Vector3 _playerPosition;
    private GameManager _gameManager;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _glassBreak;
    private bool _destroyEnemy;

    // Use this for initialization
    void Start () {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

    }
	
	// Update is called once per frame
	void Update () {
        _playerPosition = _player.transform.position;
        _enemyMoveDirection = transform.position;

        float step = _speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);
        float yPos = Math.Abs(_enemyMoveDirection.y);
        float xPos = Math.Abs(_enemyMoveDirection.x);
        if ((_playerPosition.y > _enemyMoveDirection.y) && (yPos > xPos)) //moving up
        {
            _animator.SetFloat("MoveOnY", 1);
            _animator.SetFloat("MoveOnX", 0);

        }
        if ((_playerPosition.y <= _enemyMoveDirection.y) && (yPos > xPos)) // moving down
        {
            _animator.SetFloat("MoveOnY", -1);
            _animator.SetFloat("MoveOnX", 0);
        }

        if ((_playerPosition.x > _enemyMoveDirection.x) && (yPos <= xPos)) // moving right
        {
            _animator.SetFloat("MoveOnX", 1);
            _animator.SetFloat("MoveOnY", 0);
        }

        if ((_playerPosition.x <= _enemyMoveDirection.x) && (yPos <= xPos)) // moving left
        {
            _animator.SetFloat("MoveOnX", -1);
            _animator.SetFloat("MoveOnY", 0);
        }

        if (_gameManager.gameOver)
        {
            Destroy(gameObject);
        }

        if (_destroyEnemy)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            if (transform.localScale.x <= 0 && transform.localScale.y <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_glassBreak, other.transform.position);
            Destroy(other.gameObject);
            _destroyEnemy = true;
            
            

        }

        if (other.gameObject.tag == "Player")
        {
            Player _playerscript = other.GetComponent<Player>();
            _playerscript.PlayerDamage();
            Destroy(gameObject);
        }

    }


}
