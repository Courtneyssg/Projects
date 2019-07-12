using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator _anim; // Reference to Animator

	void Start () {
        _anim = GetComponent<Animator>(); // Get access to animator
	}
	
	// Update is called once per frame
	void Update () {
        // If A is pressed or left arrow is pressed
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _anim.SetBool("Turn_Left", true);  // Turn left animation is true
            _anim.SetBool("Turn_Right", false);// Turn right animation is true 
        }
        // If a key is let go or left arrow is let go
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _anim.SetBool("Turn_Left", false);// turn left animation is false
            _anim.SetBool("Turn_Right", false);// turn right animation is false
        }
        // If key d or right arrow is pressed
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _anim.SetBool("Turn_Right", true); // Turn right is true
            _anim.SetBool("Turn_Left", false); // Turn left is false
        }
        // If key d is let go or right arrow is let go
        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _anim.SetBool("Turn_Right", false); // Turn right is false
            _anim.SetBool("Turn_Left", false); // Turn left is false
        }
	}
}
