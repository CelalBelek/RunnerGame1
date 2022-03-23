using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControll : MonoBehaviour
{
    GameController gameController;
    CharacterController characterController;

    public Animator CharacterAnimator;

    private Transform localTrans;

    public GameObject finishLine;
    public GameObject target;

    public float speed = 0.4f;
    private float nspeed = 0.4f;
    Vector3 dirNormalized;

    public bool move;

    public Color32[] skin;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        characterController = FindObjectOfType<CharacterController>();
        localTrans = GetComponent<Transform>();
        gameObject.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].color = skin[Random.Range(0, skin.Length)];

        CharacterAnimator = GetComponent<Animator>();
        CharacterAnimator.SetBool("Game", false);

        move = true;

        target = finishLine;
        dirNormalized = (target.transform.position - transform.position).normalized;
    }

    private void Update()
    {
        if (move == true && gameController.StartTime <= 0)
        {
            // Süre sıfırlanınca ve hareket true olunca Agent koşmaya başlar ve animasyon oynar 
            transform.position = transform.position + dirNormalized * speed * Time.deltaTime;
            CharacterAnimator.SetBool("Game", true);
        }

        if (target.transform.parent.tag == "Obstacle" || target.transform.parent.tag == "StaticTrigger" || target.transform.parent.tag == "HorizontalTrigger")
        {
            // Agent Yukarıdaki Tag lara değmişse (aşağıda kontrol ettik OnTrigger'da) hareket direkt true olacak
            move = true;
        }
        else if (target.transform.parent.tag == "RotatorTrigger")
        {
            // Agent Yukarıdaki Taga değmişse (aşağıda kontrol ettik OnTrigger'da) hareket değdiğdiği objenin pozisyonuna göre şekilenecek
            if (target.transform.position.x > 0.0f)
            {
                if (target.transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y <= 180.0f || target.transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y >= 300.0f)
                {
                    if(transform.position.x > 0)
                    {
                        CharacterAnimator = GetComponent<Animator>();
                        CharacterAnimator.SetBool("Game", true);

                        speed = nspeed;

                        move = true;
                    }
                    else
                    {
                        CharacterAnimator = GetComponent<Animator>();
                        CharacterAnimator.SetBool("Game", false);

                        speed = 0.0f;

                        move = false;
                    }
                }
                else
                {
                    CharacterAnimator = GetComponent<Animator>();
                    CharacterAnimator.SetBool("Game", true);

                    speed = nspeed;

                    move = true;
                }
            }
            else if (target.transform.position.x < 0.0f)
            {
                if (target.transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y <= 290.0f && target.transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y >= 160.0f)
                {
                    if (transform.position.x < 0)
                    {
                        CharacterAnimator = GetComponent<Animator>();
                        CharacterAnimator.SetBool("Game", true);

                        speed = nspeed;

                        move = true;
                    }
                    else
                    {
                        CharacterAnimator = GetComponent<Animator>();
                        CharacterAnimator.SetBool("Game", false);

                        speed = 0.0f;

                        move = false;
                    }
                }
                else
                {
                    CharacterAnimator = GetComponent<Animator>();
                    CharacterAnimator.SetBool("Game", true);

                    speed = nspeed;

                    move = true;
                }
            }
        }
        else if (target.transform.parent.tag == "HalfTrigger")
        {
            // Agent Yukarıdaki Taga değmişse (aşağıda kontrol ettik OnTrigger'da) hareket değdiğdiği objenin pozisyonuna göre şekilenecek
            if (target.transform.parent.GetChild(2).gameObject.transform.position.x > 0.27f)
            {
                CharacterAnimator = GetComponent<Animator>();
                CharacterAnimator.SetBool("Game", true);

                speed = nspeed;

                move = true;
            }
            else
            {
                CharacterAnimator = GetComponent<Animator>();
                CharacterAnimator.SetBool("Game", false);

                speed = 0.0f;

                move = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Agent Herhangi bir collider a çarptıysa ona göre karar verilip devam edilecek.
        if (other.tag == "Obstacle")
        {
            target = finishLine;
            dirNormalized = (target.transform.position - transform.position).normalized;
        }
        else if (other.tag == "StaticTrigger" || other.tag == "HalfTrigger")
        {
            if (Random.Range(1, 50) < 26)
                target = other.transform.parent.gameObject.transform.GetChild(0).gameObject;
            else
                target = other.transform.parent.gameObject.transform.GetChild(1).gameObject;

            dirNormalized = (target.transform.position - transform.position).normalized;
        }
        else if (other.tag == "RotatorTrigger")
        {
            if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y < 60.0f)
                target = other.transform.parent.gameObject.transform.GetChild(1).gameObject;
            else if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y < 150.0f)
                target = other.transform.parent.gameObject.transform.GetChild(0).gameObject;
            else if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y < 270.0f)
                target = other.transform.parent.gameObject.transform.GetChild(1).gameObject;
            else if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.localRotation.eulerAngles.y < 360.0f)
                target = other.transform.parent.gameObject.transform.GetChild(0).gameObject;

            dirNormalized = (target.transform.position - transform.position).normalized;
        }
        else if (other.tag == "HorizontalTrigger")
        {
            bool a = other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.GetComponent<HorizontalObstacleControll>().directionBool; 
            
            if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.position.x < 0.1f && a)
                target = other.transform.parent.gameObject.transform.GetChild(1).gameObject;
            else if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.position.x < 0.1f && !a)
                target = other.transform.parent.gameObject.transform.GetChild(1).gameObject;
            else if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.position.x > 0.1f && a)
                target = other.transform.parent.gameObject.transform.GetChild(0).gameObject;
            else if (other.transform.parent.gameObject.transform.GetChild(0).transform.parent.GetChild(2).gameObject.transform.position.x > 0.1f && !a)
                target = other.transform.parent.gameObject.transform.GetChild(1).gameObject;

            dirNormalized = (target.transform.position - transform.position).normalized;
        }
        else if (other.tag == "RotateGround")
        {
            target = other.transform.parent.gameObject.transform.GetChild(2).gameObject;
            dirNormalized = (target.transform.position - transform.position).normalized;
        }
        else if (other.tag == "StaticObstacle" || other.tag == "HalfObstacle")
        {
            // Karakterimiz veya Agentımız bu iki taga değerse restartlandırılıyor aşağıdaki diğer sorgulamalarda benzer
            CharacterAnimator.SetBool("Death", true);
            StartCoroutine(RestartGame());
            speed = 0.0f;
        }
        else if (other.tag == "RotatorObstacle")
        {
            CharacterAnimator.SetBool("Death", true);
            StartCoroutine(RestartGame());
            speed = 0.0f;
        }
        else if (other.tag == "HorizontalObstacle")
        {
            CharacterAnimator.SetBool("Death", true);
            StartCoroutine(RestartGame());
            speed = 0.0f;
        }
        else if (other.tag == "RotateObstacle")
        {
            CharacterAnimator.SetBool("Death", true);
            StartCoroutine(RestartGame());
            speed = 0.0f;
        }
        else if (other.tag == "Negative")
        {
            speed = 0.0f;
            gameObject.transform.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(RestartGame());
        }
        else if (other.tag == "Finish")
        {
            FinishController();
        }
    }

    IEnumerator RestartGame()
    {
        // Karakter yeniden doğacağından ayarları sıfırlanıyor
        yield return new WaitForSeconds(2.0f);
        localTrans.position = gameController.playerStart.position;
        CharacterAnimator.SetBool("Death", false);
        gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
        speed = 0.4f;
    }

    public void FinishController()
    {
        //Agentımız oyunu bitirirse ona göre davranış seçiliyor. KArakterin de durumu belirleniyor.
        if (gameController.finishCount == 0)
        {
            characterController.speed = 0;
            characterController.move = false;
            gameController.CharacterAnimator.SetBool("Over", true);

            for (int i = 0; i < gameController.agentList.Length; i++)
            {
                if (gameController.agentList[i].name != gameObject.name)
                {
                    gameController.agentList[i].transform.GetComponent<AgentControll>().speed = 0;
                    gameController.agentList[i].transform.GetComponent<AgentControll>().move = false;
                    gameController.agentList[i].transform.GetComponent<AgentControll>().CharacterAnimator.SetBool("Over", true);
                }
            }

            CharacterAnimator.SetBool("Dance", true);
            speed = 0.0f;
            move = false;
        }

        gameController.finishCount++;

        gameController.Lose();
    }
}
