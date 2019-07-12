﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    public Movement1 _collision;

    private float shapeSpeed;
    private float shapeScale;
    private float rotateRate;
    private Vector3 directionn;
    private SpriteRenderer rend;


    private void OnEnable()
    {
        shapeSpeed = Random.Range(1, 5);
        shapeScale = Random.Range(0.01f, 1);
        rotateRate = Random.Range(20, 500);
        directionn = Random.onUnitSphere;

        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;
        StartCoroutine(FadeOut());

    }
    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 0, 1 * rotateRate * Time.deltaTime);
        transform.localScale = new Vector3(shapeScale, shapeScale, shapeScale);
        GetComponent<Rigidbody2D>().velocity = directionn * shapeSpeed;

    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.01f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
            if(c.a <= -0.04)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
