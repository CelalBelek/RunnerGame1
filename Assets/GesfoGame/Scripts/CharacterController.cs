using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private GameController gameController;

    private Transform localTrans;

    private Vector3 lastMousePos;
    private Vector3 mousePos;
    private Vector3 newPosForTrans;

    private float xDiff;

    public GameObject spray;
    public GameObject sprayParticle;

    public Camera mainCamera;

    public float speed;
    public float swipeSpeed;
    public float sprayPoint;

    private bool finishLine;
    public bool move;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        localTrans = GetComponent<Transform>();
        finishLine = false;
        move = true;
        spray.SetActive(false);

        speed = 0.4f;
        swipeSpeed = 0.5f;
    }

    void Update()
    {
        // Karakter fare basılı tutulunca ileri ve sağ sol hareketleri hesaplanıyor hep bir birim ilerisine gidiyor ve animasyonlar düzenleniyor.
        if (finishLine == false && gameController.StartTime <= 0 && move == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, swipeSpeed));
                lastMousePos = mousePos;
            }
            else if (Input.GetMouseButton(0))
            {
                if (gameController.CharacterAnimator.GetBool("Game") == false)
                    gameController.CharacterAnimator.SetBool("Game", true);

                mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, swipeSpeed));

                xDiff = mousePos.x - lastMousePos.x;
                newPosForTrans.x = localTrans.position.x + xDiff;
                newPosForTrans.y = localTrans.position.y;
                newPosForTrans.z = localTrans.position.z;

                localTrans.position = newPosForTrans + localTrans.forward * speed * Time.deltaTime;

                lastMousePos = mousePos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (gameController.CharacterAnimator.GetBool("Game") == true)
                    gameController.CharacterAnimator.SetBool("Game", false);
            }
        }
    }

    IEnumerator RestartGame()
    {
        // Karakter hata yapıp yanınca oyun sıfırlanıyor.
        yield return new WaitForSeconds(2.0f);
        localTrans.position = gameController.playerStart.position;
        gameController.CharacterAnimator.SetBool("Death", false);
        gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
        speed = 0.4f;
        move = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Çarptığı objeye göre yanma veya oyun bitmesi kararlaştırılıyor
        if (other.gameObject.tag == "StaticObstacle" || other.gameObject.tag == "HorizontalObstacle" || other.gameObject.tag == "Negative")
        {
            gameController.CharacterAnimator.SetBool("Death", true);
            StartCoroutine(RestartGame());
            speed = 0.0f;
            move = false;
            gameObject.transform.GetComponent<Rigidbody>().useGravity = false;
        }
        else if (other.gameObject.tag == "Finish")
        {
            finishLine = true;

            gameController.CharacterAnimator.SetBool("Dance", true);

            sprayParticle.GetComponent<ParticleSystem>().Stop();
            spray.SetActive(true);

            FinishController();
        }
    }

    public void FinishController()
    {
        // Karakter kazanırsa diğer karkterlerin davranışalrı ayarlandı
        if (gameController.finishCount != 0)
        {
            for (int i = 0; i < gameController.agentList.Length; i++)
            {
                if (gameController.agentList[i].name != gameObject.name)
                {
                    gameController.agentList[i].transform.GetComponent<AgentControll>().speed = 0;
                    gameController.agentList[i].transform.GetComponent<AgentControll>().move = false;
                    gameController.agentList[i].transform.GetComponent<AgentControll>().CharacterAnimator.SetBool("Over", true);
                }
            }

            speed = 0.0f;
            swipeSpeed = 0.0f;
        }

        gameController.finishCount++;
    }
}


