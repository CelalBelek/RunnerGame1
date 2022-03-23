using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFast : MonoBehaviour
{
    CharacterController characterController;
    GameController gameController;

    public int val;
    float speedN;
    float speedF;
    float speedS;
    float time;

    private void Start()
    {
        characterController = FindObjectOfType<CharacterController>();
        gameController = FindObjectOfType<GameController>();

        speedN = 0.4f;
        speedF = 0.6f;
        speedS = 0.25f;
        time = 1.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Karakter ve Agentın hızı değdiği slow veya fast objesine göre ayarlanıyor

        if (other.tag == "Player")
        {
            if(val == 0)
            {
                characterController.speed = speedF;
            }
            else if (val == 1)
            {
                characterController.speed = speedS;
            }

            StartCoroutine(NormalSpeed(other));
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (other.tag == "NPC")
        {
            if (val == 0)
            {
                other.gameObject.transform.GetComponent<AgentControll>().speed = speedF;
            }
            else if (val == 1)
            {
                other.gameObject.transform.GetComponent<AgentControll>().speed = speedS;
            }

            StartCoroutine(NormalSpeed(other));
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator NormalSpeed(Collider other)
    {
        // Karakter ve Agent normal hızına dönüyor
        yield return new WaitForSeconds(time);

        if (other.tag == "Player")
        {
            characterController.speed = speedN;
        }
        else
        {
            other.gameObject.transform.GetComponent<AgentControll>().speed = speedN;
        }

        Destroy(gameObject);
    }
}
