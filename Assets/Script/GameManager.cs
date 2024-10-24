using System.Collections;
using System.Collections.Generic;
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

    public bool isMusic = true;
    public static int curentLevel = 1;
    public bool isPause = false;
    public bool win = false;


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
        Debug.Log("1111");
        isPause = false;
        SetActiveHomeAndMusic(isPause);
        pausePanel.SetActive(isPause);
    }

    public void RetryButton()
    {
        isPause = false;
        SetActiveHomeAndMusic(false);
        losePanel.SetActive(false);
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
}