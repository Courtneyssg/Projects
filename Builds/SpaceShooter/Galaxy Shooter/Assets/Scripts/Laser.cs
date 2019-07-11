using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField]
    private float _speed = 10.0f; // laser speed
	
	void Update () {
        // Move laser upwards when shot
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y >= 6) // if y position >= 6 then destroy the laser object and parent in case of triple shot
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
	}
}
