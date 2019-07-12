using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMovement : MonoBehaviour
{
    public Dialog _dialog;
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
    public AudioSource _source;
    public AudioClip  _dashAttack;
    public AudioClip _lasers;
    public Text _bossHealth;
    public GameObject _laser;
    public Boundaries _boundaries;
    // Start is called before the first frame update
    void Start()
    {
        _bossHealth.text = "Boss Health: " + _hits;
    }

    // Update is called once per frame
    void Update()

    {
        if(_boundaries._basement && _dialog._evilTrigger ==2)
        {
            StartCoroutine(ShootLaser());
        }
     
        _bossHealth.text = "Boss Health: " + _hits;
        if (_dialog._evilTrigger == 2)
        {

            if (Vector2.Distance(transform.position, _target.position) <= 10)
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            }

            if (Vector2.Distance(transform.position, _target.position) <= 1 && !_player._isDashing)
            {
                if (transform.position.x > _target.position.x)
                {
                    transform.position = new Vector2(transform.position.x + 3, transform.position.y);
                    _target.position = new Vector2(_target.position.x - 3, _target.position.y);
                    _cam.canShake = true;
                    _source.clip = _dashAttack;
                    _player.health -= 1;
                    _player._healthText.text = "Player Health: " + _player.health;
                    if (!_source.isPlaying)
                    {
                        _source.Play();
                    }


                }
            }

            if (Vector2.Distance(transform.position, _target.position) <= 1 && _player._isDashing)
            {
                if (_player.facingRight)
                {
                    transform.position = new Vector2(_target.position.x + 10, transform.position.y);
                    _target.position = new Vector2(_target.position.x, _target.position.y);
                    _hits -= 1;
                    _source.clip = _dashAttack;
                    if (!_source.isPlaying)
                    {
                        _source.Play();
                    }
                    
                }

                if (!_player.facingRight)
                {
                    transform.position = new Vector2(_target.position.x - 10, transform.position.y);
                    _target.position = new Vector2(_target.position.x, _target.position.y);
                    _source.clip = _dashAttack;
                    _hits -= 1;
                    if (!_source.isPlaying)
                    {
                        _source.Play();
                    }
                    
                }
            }

            if (_hits <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(5);
        Instantiate(_laser, transform.position, Quaternion.identity);
        _source.clip = _lasers;
        if (!_source.isPlaying)
        {
            _source.Play();
        }

    }
}
