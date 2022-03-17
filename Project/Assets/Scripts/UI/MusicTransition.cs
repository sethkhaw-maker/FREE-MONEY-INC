using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Obsolete, use Audio Manager
public class MusicTransition : MonoBehaviour
{
    public static MusicTransition instance;
    private AudioSource audiosourceref;
    public AudioClip[] audioTracks;
    private bool isChanged = false;
    private string sceneName;
    private Animator thisAnim;
    //int buildIndex = SceneManager.GetActiveScene().buildIndex;

    //private void Awake()
    //{
    //    thisAnim = GetComponent<Animator>();
    //    if (instance == null) 
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(instance);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public void SceneChange()
    {
        thisAnim.SetTrigger("fadeIn");
    }

    public void PlayButtonSceneChange()
    {
        //thisAnim.SetTrigger("fadeOut");
    }

    public void MainMenuMusic()
    {
        audiosourceref.Stop();
        audiosourceref.clip = audioTracks[0];
        audiosourceref.Play();
    }
    public void ChangeBGM()
    {
        if (sceneName != "Loading")
        {
            audiosourceref.Stop();

            if (sceneName == "MainMenu")
            {
                isChanged = false;
                audiosourceref.clip = audioTracks[0];
            }

            else if (sceneName != "Main Menu" && sceneName != "Loading" && isChanged == false)
            {
                isChanged = true;
                audiosourceref.clip = audioTracks[1];
            }

            //audiosourceref.Play();
        }
        else audiosourceref.Stop();



    }
    private void Start()
    {
        audiosourceref = GetComponent<AudioSource>();
        audiosourceref.volume = 1;
    }

    public void Update()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
       // Debug.Log("Current Scene Build Index is " + currentScene.name);
        // Retrieve the name of this scene.
        sceneName = currentScene.name;

        
        
    }
}
