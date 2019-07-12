using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawn : MonoBehaviour
{
    private SpriteRenderer rend;
    public float duration;

    private void OnEnable()
    {
        rend = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
