using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveGOLook : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.x <= transform.position.x)
        {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }

        if (_player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }

    }
}
