using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _magnitude, _duration, _decreaseAmount;
    [SerializeField]
    private Transform _cam;
    public bool canShake;
    private Vector3 _startPosition;
    [SerializeField]
    private float initialDuration;
    

    void Start()
    {
        _cam = Camera.main.transform;
        _startPosition = _cam.localPosition;


    }

    private void Update()
    {
        if(canShake)
        {
            if(_duration > 0)
            {
                _cam.localPosition = _startPosition + Random.insideUnitSphere * _magnitude;
                _duration -= Time.deltaTime * _decreaseAmount;

            }

            else
            {
                canShake = false;
                _duration = initialDuration;
                _cam.localPosition = _startPosition;
            }
        }
    }




}
