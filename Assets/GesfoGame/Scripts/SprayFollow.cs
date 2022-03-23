using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprayFollow : MonoBehaviour
{
    CharacterController characterController;
    GameController gameController;

    private void Start()
    {
        characterController = FindObjectOfType<CharacterController>();
        gameController = FindObjectOfType<GameController>();

        gameController.sprayCount.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
    }

    void Update()
    {
        // Karakter duvarı boyarken hedef alabilsin diye raycast kullanıldı ve arkaya gmesh i kapalı bir hedef konularak uclara değmesi sağlandı
        gameController.SprayPoint -= Time.deltaTime;
        
        if (gameController.SprayPoint > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                characterController.sprayParticle.GetComponent<ParticleSystem>().Play();
                gameController.CharacterAnimator.SetBool("Spray", true);
            }
            else if (Input.GetMouseButton(0))
            {
                if (characterController.sprayParticle.GetComponent<ParticleSystem>() == true)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    Physics.Raycast(ray, out hit);
                    transform.LookAt(hit.point);

                    gameController.sprayCount.transform.GetChild(0).GetComponent<Image>().fillAmount -= Time.deltaTime / gameController.SprayPointR;
                }
            }
        }
        else
        {
            characterController.sprayParticle.GetComponent<ParticleSystem>().Stop();
            characterController.spray.SetActive(true);

            float a = gameController.pie.GetComponent<Image>().fillAmount;

            gameController.Win(a);
        }
    }
}
