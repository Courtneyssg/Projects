using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGhost : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private float _hits;
    [SerializeField]
    private CameraShake _cam;


    public AudioClip _dashAttack;
    public AudioSource _source;
    // Start is called before the first frame update
    void Start()
    {
        
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, _target.position) <= 5)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }

        if(Vector2.Distance(transform.position, _target.position) <= 1 && !_player._isDashing)
        {
            Debug.Log("Triggered" + _player.health);
            _player.health -= 1;
            if (transform.position.x > _target.position.x)
            {
                transform.position = new Vector2(transform.position.x + 3, transform.position.y);
                _target.position = new Vector2(_target.position.x - 3, _target.position.y);
                _cam.canShake = true;
                _source.clip = _dashAttack;
                _hits -= 1;
                if(!_source.isPlaying)
                {
                    _source.Play();
                }
                

            }
        }
        
        if(Vector2.Distance(transform.position, _target.position) <= 1 && _player._isDashing)
        {
            if (_player.facingRight)
            {
                transform.position = new Vector2(_target.position.x + 10, transform.position.y);
                _target.position = new Vector2(_target.position.x, _target.position.y);
                _source.clip = _dashAttack;
                if (!_source.isPlaying)
                {
                    _source.Play();
                }
                _hits -= 1;
            }

            if(!_player.facingRight)
            {
                transform.position = new Vector2(_target.position.x - 10, transform.position.y);
                _target.position = new Vector2(_target.position.x, _target.position.y);
                _source.clip = _dashAttack;
                if (!_source.isPlaying)
                {
                    _source.Play();
                }
                _hits -= 1;
            }
        }

        if(_hits <= 0)
        {
            Destroy(gameObject);
        }
    }
}
