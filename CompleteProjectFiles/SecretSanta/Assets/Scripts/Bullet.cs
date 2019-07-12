using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private float _speed = 5.0f; // bullet speed
    private int _bulletDirection = 0;


    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (_player._moveDirection.x == 0 && _player._moveDirection.y == 1)
        {
            //moving up
            _bulletDirection = 1;
        }

        if (_player._moveDirection.x == -1 && _player._moveDirection.y == 0)
        {
            //moving left
            _bulletDirection = 2;
        }

        if (_player._moveDirection.x == 0 && _player._moveDirection.y == -1)
        {
            //moving down
            _bulletDirection = 3;
        }

        if (_player._moveDirection.x == 1 && _player._moveDirection.y == 0)
        {
            //moving right
            _bulletDirection = 4;
        }
    }

    void Update()
    {
        if (_bulletDirection == 1)
        {
            //up
            transform.position += new Vector3(0, _speed, 0) * Time.deltaTime;
        }

        if (_bulletDirection == 2)
        {
            //left
            transform.position += new Vector3(-_speed, 0, 0) * Time.deltaTime;
        }

        if (_bulletDirection == 3)
        {
            //down
            transform.position += new Vector3(0, -_speed, 0) * Time.deltaTime;
        }

        if (_bulletDirection == 4)
        {
            //right
            transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;

        }

        StartCoroutine(DestroyBullet());

    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }
}