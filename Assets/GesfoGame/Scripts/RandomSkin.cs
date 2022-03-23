using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkin : MonoBehaviour
{
    public Material[] skin;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material = skin[Random.Range(0, skin.Length)]; 
    }
}
