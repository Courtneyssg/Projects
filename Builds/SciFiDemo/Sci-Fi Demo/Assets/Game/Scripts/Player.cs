using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 3.0f; // character move speed
    [SerializeField]
    private float _gravity = 9.81f; // effects of gravity on player
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _hitMarkerPrefab;
    [SerializeField]
    private AudioSource _weaponAudio;
    [SerializeField]
    private int currentAmmo; // ammo player has
    private int maxAmmo = 50; // most ammo player can have
    private bool _isReloading = false;
    private UI_Manager _uiManager;
    public bool hasCoin = false;
    public bool hasCode = false;
    [SerializeField]
    private GameObject _weapon;

	void Start () {
        _controller = GetComponent<CharacterController>();
        // set cursor invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentAmmo = maxAmmo; // player has full ammo on game start
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
	}
	
	void Update () {
        if(Input.GetMouseButton(0) && currentAmmo > 0) // if mouse button is down and the current ammo isn't 0
        {
            Shoot();
        }

        else
        {
            _muzzleFlash.SetActive(false); // when mouse button isn't held down set flash to false so not shooting
            _weaponAudio.Stop(); // Stop playing shoot audio when mouse button isn't held down
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)// if R is pressed call coroutine to reload weapon
        {
            _isReloading = true; // reloading weapon
            StartCoroutine(Reload());
        }
        // Use escape key to bring back mouse cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        //player movement method call
        CalculateMovement();
	}

    // moves player along vertical and horizontal axis, and applies gravity to player
    void Shoot()
    {
        currentAmmo--; // each time we shoot ammo is used
        _uiManager.UpdateAmmo(currentAmmo);
        _muzzleFlash.SetActive(true); // set shooting active when mouse button is held down
        if (!_weaponAudio.isPlaying) // Check that weapon audio isn't already playing
        {
            _weaponAudio.Play(); // play shoot audio when mouse button is held down
        }

        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //cast a ray at centre of screen
        RaycastHit hitInfo; //Get info about object being hit

        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            Debug.Log("Hit" + hitInfo.transform.name);
            //Instatiate the hitmarker when mouse held down and destroy the game object after 1 second
            GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            Destroy(hitMarker, 1f);

            Destructable crate = hitInfo.transform.GetComponent<Destructable>();
            if(crate != null)
            {
                crate.DestroyCrate();
            }
        }
    }
    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _speed;
        velocity.y -= _gravity;
        velocity = transform.transform.TransformDirection(velocity);
        _controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        currentAmmo = maxAmmo;
        _uiManager.UpdateAmmo(currentAmmo);
        _isReloading = false; // not currently reloading weapon
    }

    public void EnableWeapons()
    {
        _weapon.SetActive(true);
    }

}
