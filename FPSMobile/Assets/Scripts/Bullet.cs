using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "AI":
                Debug.Log("HIT AI");
                Destroy(this.gameObject);
                break;
            case "Player":
                Debug.Log("HIT PL");
                Destroy(this.gameObject);
                break;
        }
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
