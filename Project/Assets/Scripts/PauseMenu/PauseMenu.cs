using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    //public GameObject optionsMenuUI;
    private AudioSource audiosourceref;

    public static PauseMenu instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }

    private void Start()
    {
        audiosourceref = MusicTransition.instance.GetComponent<AudioSource>();
        audiosourceref.volume = 5f;

        if (GameIsPaused)
        {
            Resume();
            Debug.Log("is resumed");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!Application.isFocused) 
        { 
            if (pauseMenuUI != null)
            {
                Pause(); 
            } 
        }

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
            audiosourceref.volume = 0f;
        }

    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        //optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        audiosourceref.volume = 1f;
        GameIsPaused = false;
    }
    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        audiosourceref.volume = 0f;
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

    public void LoadMenu ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
