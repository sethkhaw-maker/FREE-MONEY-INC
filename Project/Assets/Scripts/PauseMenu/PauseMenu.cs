using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUICanvas;
    public GameObject confirmationUICanvas;
    public GameObject optionsUICanvas;
    public GameObject htpUICanvas;

    //public GameObject pauseButton;

    private AudioSource audiosourceref;
    public static PauseMenu instance;

    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(this.gameObject);
    //        return;
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //}

    private void Start()
    {
        //audiosourceref = MusicTransition.instance.GetComponent<AudioSource>();
        //audiosourceref.volume = 5f;

        if (GameIsPaused)
        {
            Resume();
            Debug.Log("is resumed");
        }

        confirmationUICanvas.SetActive(false);
        pauseMenuUICanvas.SetActive(false);
        optionsUICanvas.SetActive(false);
        htpUICanvas.SetActive(false);
    }


    void Update()
    {
        //This gets in the way of debugging!!
        //if (!Application.isFocused)
        //{
        //    if (pauseMenuUICanvas != null)
        //    {
        //        Pause();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                Debug.Log("is resumed");
            }
            else
            {
                Pause();
                Debug.Log("is paused");
            }
        }

        if (GameIsPaused)
        {
            //audiosourceref.volume = 0f;
        }

    }

    public void Resume()
    {
        //pauseButton.SetActive(true);
        pauseMenuUICanvas.SetActive(false);
        confirmationUICanvas.SetActive(false);
        pauseMenuUICanvas.SetActive(false);
        optionsUICanvas.SetActive(false);
        htpUICanvas.SetActive(false);

        Time.timeScale = 1f;
        //audiosourceref.volume = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUICanvas.SetActive(true);
        // pauseButton.SetActive(false);


        Time.timeScale = 0f;
       // audiosourceref.volume = 0f;
        GameIsPaused = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
