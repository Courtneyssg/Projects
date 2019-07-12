using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    public AudioClip _clip;
    public AudioClip _clip2;
    public Boundaries _player;
    public AudioSource _source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_player._basement)
        {
            _source.clip = _clip2;
            if (!_source.isPlaying)
            {
                _source.Play();
            }
        }

        if(!_player._basement)
        {
            _source.clip = _clip;
            if (!_source.isPlaying)
            {
                _source.Play();
            }
        }
    }
}
