using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.UIElements;

public class Checkpoint : MonoBehaviour
{
    private float rate = 2f;
    private float rotateRate = 100f;
    private bool decrease;
    private bool increase;

    public int nextScene;
    public int currentScene;
    private bool loaded;
    public bool spinning;
  //  public AudioClip _clip;
    public AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        //if (spinning)
        //{
        //    transform.localScale = new Vector3(2, 2, 2);
        //    decrease = true;

        //}
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (spinning)
        //{
        //    transform.Rotate(0, 0, 1 * rotateRate * Time.deltaTime);
        //    if (decrease)
        //    {
        //        transform.localScale -= new Vector3(rate, rate, rate) * Time.deltaTime;

        //        if (transform.localScale.x <= 0 | transform.localScale.y <= 0)
        //        {
        //            increase = true;
        //            decrease = false;
        //        }
        //    }

        //    if (increase)
        //    {
        //        transform.localScale += new Vector3(rate, rate, rate) * Time.deltaTime;

        //        if (transform.localScale.x >= 2 | transform.localScale.y >= 2)
        //        {
        //            decrease = true;
        //            increase = false;
        //        }
        //    }
        //}
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AnyManager.anyManager.attempSceneChange(currentScene, nextScene);
        }
    }

}
