using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour {
    [SerializeField]
    private float _sensitivity = 1.0f; // Controls sensitiviy looking up and down

    void Start()
    {

    }

    void Update()
    {
        //rotates the field of view around the x axis when mouse is moved up and down
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x -= mouseY * _sensitivity;
        transform.localEulerAngles = newRotation;
    }
}