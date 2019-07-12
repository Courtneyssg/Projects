using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [SerializeField]
    private float _speed = 3.0f; // Speed of powerups
    public float _bottomOfScreen = -6.44f; // Position on y where powerups get destroyed
    [SerializeField]
    private int _powerupID; // 0 - triple shot, 1 = speed boost, 2 = shields
    [SerializeField]
    private AudioClip _audioClip; // Reference to audioclip

    void Update () {
        // move downward
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= _bottomOfScreen) // If the position of the powerup is below the window view
        {
            Destroy(this.gameObject); //destroy the powerup
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") // If the powerup collides with the player
        {
            Player player = other.GetComponent<Player>(); //reference to player

            if(player != null) // use null checking when working with getComponent
            {
                if(_powerupID == 0)
                {
                    player.TripleShotPowerUpOn(); //If powerup instantiated has id 0, call tripleshotpowerup method in player script
                }
                else if(_powerupID == 1)
                {
                    player.SpeedBoostOn(); // If powerup instantiated has id 1, call speedboost method in player script
                }
                else if(_powerupID == 2)
                {
                    player.EnableShields(); // If powerup instantiated has id 2, call enableshields method in player script
                }
            }
            // Play powerup audioclip when player collects it
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
            // Destroy the powerup
            Destroy(this.gameObject);
        } 
    }
}
