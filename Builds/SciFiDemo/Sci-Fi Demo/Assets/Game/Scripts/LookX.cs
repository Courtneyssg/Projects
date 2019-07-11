using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour {
    [SerializeField]
    private float _sensitivity = 1.0f; // controls sensitivty looking left and right

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //rotates the screen view around the y axis when mouse is moved left and right
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y += mouseX * _sensitivity;
        transform.localEulerAngles = newRotation;
	}
}
