using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] agentList;

    public GameObject win;
    public GameObject lose;

    public GameObject[] winList;
    public TextMeshProUGUI winText;

    public GameObject characterPrefab;
    public GameObject sprayCount;
    public GameObject pie;

    public Animator CharacterAnimator;

    public Transform playerStart;

    public int finishCount;
    
    public float SprayPoint;
    public float SprayPointR;
    public float StartTime = 4.0f;

    public TextMeshProUGUI timeText;

    public AudioSource music;
    public Sprite[] musicSprite;
    public Button musicButton;

    void Start()
    {
        playerStart.transform.position = new Vector3(0, 0, UnityEngine.Random.Range(0.0f, 0.7f));
        characterPrefab.transform.position = playerStart.position;
        sprayCount.SetActive(false);

        win.SetActive(false);
        lose.SetActive(false);

        music.volume = 0.25f;
        musicButton.GetComponent<Image>().sprite = musicSprite[0];
    }

    private void Update()
    {
        // Oyunun başlangıcındaki geri sayma mekaniği kontrol ediliyor
        if (StartTime > 0)
        {
            StartTime -= Time.deltaTime;

            timeText.text = Convert.ToInt32(StartTime).ToString();
        }
        else
        {
            timeText.text = "GO";
            StartCoroutine(Start2());
        }
    }

    public void RestartGame()
    {
        // Oyun bitince restart butonu tetikliyor
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator Start2()
    {
        // GO yazısı kapansın
        yield return new WaitForSeconds(1.0f);
        timeText.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void Win(float starVal)
    {
        //Oyun sonunda yıldızların ve puanın durumu hesaplanıyor

        win.SetActive(true);

        float b = starVal * 100;

        if (b >= 85)
        {
            winList[0].SetActive(true);
            winList[1].SetActive(true);
            winList[2].SetActive(true);
        }
        else if (b >= 70)
        {
            winList[0].SetActive(true);
            winList[1].SetActive(true);
            winList[2].SetActive(false);
        }
        else if (b >= 50)
        {
            winList[0].SetActive(true);
            winList[1].SetActive(false);
            winList[2].SetActive(false);
        }
        else
        {
            winList[0].SetActive(false);
            winList[1].SetActive(false);
            winList[2].SetActive(false);
        }

        int a = (int)b;
        winText.text = a.ToString();
    }

    public void Lose()
    {
        lose.SetActive(true);
    }

    public void MusicYN()
    {
        // Müzik açılıp kapanıyor

        if(music.volume == 0)
        {
            music.volume = 0.25f;

            musicButton.GetComponent<Image>().sprite = musicSprite[0];
        }
        else
        {
            music.volume = 0;

            musicButton.GetComponent<Image>().sprite = musicSprite[1];
        }
    }
}
