using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpirit : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    private Animator _anim;
    private Animator _thisAnim;
    private Player _script;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = _player.GetComponent<Animator>();
        _thisAnim = GetComponent<Animator>();
        _script = _player.GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_anim.GetBool("isRunning"))
        {
            _thisAnim.SetBool("isRunning", true);
        }

        if(!_anim.GetBool("isRunning"))
        {
            _thisAnim.SetBool("isRunning", false);
        }

        if (Input.GetKey(KeyCode.Space) && _script._isGrounded && !_script._isJumping)
        {
            
            _thisAnim.SetTrigger("jumping");
        }

    }

}

