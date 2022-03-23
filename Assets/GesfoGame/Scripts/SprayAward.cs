using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayAward : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Karakter ve Agent yerdeki spraylari topluyor
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<GameController>().SprayPoint += 0.5f;
            FindObjectOfType<GameController>().SprayPointR += 0.5f;
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "NPC")
        {
            Destroy(gameObject);
        }
    }
}
