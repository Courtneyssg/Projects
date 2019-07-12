using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera _cam;
    private Vector2 _point;
    private Vector2 _mousePos;
    private float step = 0.01f;
    private float timedstep;
    [SerializeField]
    private GameObject _mask;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        timedstep = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos.x = Input.mousePosition.x;
        _mousePos.y = Input.mousePosition.y;
  
        _point = _cam.ScreenToWorldPoint(new Vector3(_mousePos.x, _mousePos.y, 6.7f));

        transform.position = new Vector2(_point.x, _point.y);

        if(Time.time > timedstep)
        {
            Instantiate(_mask, transform.position, Quaternion.identity);
            timedstep = Time.time + step;
        }

    }
}
