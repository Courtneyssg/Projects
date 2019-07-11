using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour {

	void Start () {
        Destroy(this.gameObject, 4f); //destoys explosion animation after 4 seconds so is doesn't stay in hierarcy 
	}
}
