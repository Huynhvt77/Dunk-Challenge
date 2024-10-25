using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject music;
    public Sprite onMusic;
    public Sprite offMusic;
    Image img;

    public GameObject pausePanel;
    public GameObject losePanel;
    public GameObject home;
    public GameObject gamePlay;
    public GameObject checkUpPos;
    public GameObject checkDownPos;
    public GameObject stopBar;
    public Transform spawnPos;
    public Camera mainCamera;
    public Player player;
    Level level;
    public Level[] levels = new Level[7];
    Transform basket;

    public bool isMusic = true;
    public static int curentLevel = 1;
    public bool isPause = false;
    public bool win = false;
    public int index;
    public float cameraOffsetY = 0.5f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (music != null)
        {
            img = music.GetComponent<Image>();
            img.sprite = isMusic ? onMusic : offMusic;
        }

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            bool s = false;
            foreach (Level levelx in levels)
            {
                if (levelx.gameObject.activeSelf)
                {
                    level = levelx;
                    basket = level.transform.Find("Basket");
                    GetCheckPos();
                    s = true;
                }
            }
            if (!s)
            {
                index = Random.Range(0, 7);
                level = levels[index];
                level.gameObject.SetActive(true);
                basket = level.transform.Find("Basket");
                GetCheckPos();
            }
            // index = Random.Range(0, 7);
            // level = levels[index];
            // level.gameObject.SetActive(true);
            // basket = level.transform.Find("Basket");
            // GetCheckPos();
        }
    }

    private void Update()
    {
        if (player != null)
        {
            AdjustCameraPosition();
        }
    }

    private void AdjustCameraPosition()
    {
        float screenHeight = Camera.main.orthographicSize * 2;

        float minY = screenHeight / 4;
        float maxY = screenHeight * (5f / 6f);

        Vector3 playerPos = player.transform.position;

        if (playerPos.y < minY)
        {
            Vector3 newCameraPos = new Vector3(mainCamera.transform.position.x, playerPos.y + cameraOffsetY, mainCamera.transform.position.z);
            mainCamera.transform.position = newCameraPos;
        }
        else if (playerPos.y > maxY)
        {
            Vector3 newCameraPos = new Vector3(mainCamera.transform.position.x, playerPos.y - cameraOffsetY, mainCamera.transform.position.z);
            mainCamera.transform.position = newCameraPos;
        }
    }

    void GetCheckPos()
    {
        checkUpPos = basket.GetChild(2).gameObject;
        checkDownPos = basket.GetChild(3).gameObject;
    }

    public void OnOffMusic()
    {
        isMusic = !isMusic;

        if (isMusic)
        {
            img.sprite = onMusic;
            Debug.Log("music on");
        }
        else
        {
            img.sprite = offMusic;
            Debug.Log("music off");
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void PauseButton()
    {
        isPause = true;
        SetActiveHomeAndMusic(isPause);
        pausePanel.SetActive(isMusic);
    }

    public void ResumeButton()
    {
        player.rb.gravityScale = 5f;
        player.rb.constraints = RigidbodyConstraints2D.None;
        isPause = false;
        SetActiveHomeAndMusic(isPause);
        pausePanel.SetActive(isPause);
    }

    public void RetryButton()
    {
        isPause = false;
        SetActiveHomeAndMusic(false);
        losePanel.SetActive(false);
        player.Reset();
    }

    public void HomeButton()
    {
        isPause = false;
        SceneManager.LoadScene("StartScene");
    }

    public void SetActiveHomeAndMusic(bool active)
    {
        home.SetActive(active);
        music.SetActive(active);
    }

    public void SetActiveLosePanel()
    {
        isPause = true;
        SetActiveHomeAndMusic(isPause);
        losePanel.SetActive(isPause);
    }

    public void HandleWin()
    {
        StartCoroutine(Win());
        //win
    }

    IEnumerator Win()
    {
        stopBar.SetActive(false);
        yield return new WaitForSeconds(1f);
        stopBar.SetActive(true);
        level.gameObject.SetActive(false);
        int x = Random.Range(0, 7);
        while (x == index)
        {
            x = Random.Range(0, 7);
        }
        index = x;
        level = levels[index];
        GetCheckPos();

        player.transform.position = spawnPos.position;
        // giu nguyen vi tri tuong doi cua camera voi player
    }
}
