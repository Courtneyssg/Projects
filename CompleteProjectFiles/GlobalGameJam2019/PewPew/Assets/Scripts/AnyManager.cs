using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyManager : MonoBehaviour
{
    public static AnyManager anyManager;

    private float timeStamp;
    private Animator anim;

    bool gameStart;

    private void Awake()
    {
        if (!gameStart)
        {
            anyManager = this;
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            anim = GameObject.Find("LevelChanger").transform.Find("Canvas").transform.Find("BlackFade").GetComponent<Animator>();
            gameStart = true;
        }
    }

    private void Start()
    {
        anim.SetInteger("Type", 0);
    }

    public void attempSceneChange(int currentScene, int nextScene)
    {
        if (timeStamp <= Time.time)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.GetComponent<RopeSystems>().ropeAttached)
            {
                player.GetComponent<RopeSystems>().ropePosition.GetComponent<SceneOwner>().sceneOwner = nextScene;
                player.GetComponent<RopeSystems>().ropePosition.transform.position = player.transform.position;
            }
            if (nextScene != 2)
            {
                UnLoadScene(currentScene);
            }
            loadScene(nextScene);
            timeStamp = Time.time + 1;
        }
    }

    public void gameWon()
    {
        StartCoroutine(Unload(2, false));
        GameObject.FindGameObjectWithTag("Finish").SetActive(false);
        StartCoroutine(Unload(1, false));
        loadScene(7);
        StartCoroutine(Unload(0, false));
        
    }

    public bool ownerLoaded(int scene)
    {
     //   Debug.Log(SceneManager.GetActiveScene().buildIndex);
        return false;
    }

    public void loadScene(int scene)
    {
        
        sceneLoadManagement(scene);
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }



    public void UnLoadScene(int scene)
    {
        
        StartCoroutine(Unload(scene, scene!=2));
    }


    private void sceneLoadManagement(int scene)
    {
        
        switch (scene)
        {
            case 2:
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingOrder = 10;
                GameObject.FindGameObjectWithTag("Player").transform.Find("flames").GetComponent<SpriteRenderer>().sortingOrder = 10;
                GameObject.FindGameObjectWithTag("Player").transform.Find("flames2").GetComponent<SpriteRenderer>().sortingOrder = 10;
                break;
            default:
                
                break;
        }
    }

    private void sceneUnloadManagement(int scene)
    {
        switch (scene)
        {
            case 2:
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingOrder = 5;
                GameObject.FindGameObjectWithTag("Player").transform.Find("flames").GetComponent<SpriteRenderer>().sortingOrder = 5;
                GameObject.FindGameObjectWithTag("Player").transform.Find("flames2").GetComponent<SpriteRenderer>().sortingOrder = 5;
                break;
            default:
                anim.SetInteger("Type", 0);
                break;

        }
    }


    IEnumerator Unload(int scene, bool unload)
    {
        sceneUnloadManagement(scene);
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
        if(unload)
        anim.SetInteger("Type", 1);
    }
}
