using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
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
    public TMP_Text scoreTxt;
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
    Vector3 cameraOffset = Vector3.zero;

    public bool isMusic = true;
    public static int currentLevel = 1;
    public static int highScore;
    public bool isPause = false;
    public bool win = false;
    public int index;


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
        highScore = PlayerPrefs.GetInt("highscore", 0);
        if (scoreTxt != null)
        {
            scoreTxt.text = "Score: " + (currentLevel - 1);
        }
        if (music != null)
        {
            img = music.GetComponent<Image>();
            img.sprite = isMusic ? onMusic : offMusic;
        }

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            // bool s = false;
            // foreach (Level levelx in levels)
            // {
            //     if (levelx.gameObject.activeSelf)
            //     {
            //         level = levelx;
            //         basket = level.transform.Find("Basket");
            //         GetCheckPos();
            //         s = true;
            //     }
            // }
            // if (!s)
            // {
            //     index = Random.Range(0, 7);
            //     level = levels[index];
            //     level.gameObject.SetActive(true);
            //     basket = level.transform.Find("Basket");
            //     GetCheckPos();
            // }
            index = Random.Range(0, 7);
            level = levels[index];
            level.gameObject.SetActive(true);
            GetCheckPos();
        }
    }

    private void Update()
    {
        if (scoreTxt != null)
        {
            scoreTxt.text = "Score: " + (currentLevel - 1);
        }
        if (player != null)
        {
            AdjustCameraPosition();
        }
    }

    private void AdjustCameraPosition()
    {
        SetCamPos();
    }

    public void SetCamPos()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, player.transform.position.y, mainCamera.transform.position.z); ;
    }

    void GetCheckPos()
    {
        basket = level.transform.Find("Basket");
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
        currentLevel = 1;
        SetActiveHomeAndMusic(false);
        losePanel.SetActive(false);
        player.Reset();
        mainCamera.transform.position = new Vector3(0, 0, -10f);
        level.gameObject.SetActive(false);
        int x = Random.Range(0, 7);
        while (x == index)
        {
            x = Random.Range(0, 7);
        }
        index = x;
        level = levels[index];
        level.gameObject.SetActive(true);
        GetCheckPos();
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
        highScore = Mathf.Max(highScore, currentLevel - 1);
        PlayerPrefs.SetInt("highscore", highScore);
        PlayerPrefs.Save();
        isPause = true;
        SetActiveHomeAndMusic(isPause);
        losePanel.SetActive(isPause);
        losePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "SCORE: " + (currentLevel - 1);
        losePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "HIGH SCORE: " + PlayerPrefs.GetInt("highscore", 0);
    }

    public void HandleWin()
    {
        currentLevel++;
        highScore = Mathf.Max(highScore, currentLevel - 1);
        PlayerPrefs.SetInt("highscore", highScore);
        PlayerPrefs.Save();
        StartCoroutine(Win());
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
        level.gameObject.SetActive(true);
        GetCheckPos();
        player.transform.position = new Vector3(player.transform.position.x, spawnPos.position.y, player.transform.position.z);
        SetCamPos();
        win = false;
    }
}
