using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSFX : MonoBehaviour
{
    public AudioManager audioManager;
    bool isInMainMenu;
    bool isPlaying;
    bool isStop;

    void Awake()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Start()
    {
        isInMainMenu = true;

        //FindObjectOfType<AudioManager>()?.Play("Ocean Waves");

    }


    void Update()
    {
        if (isInMainMenu)
        {
            StartCoroutine(LoopAudio());
        }
        else StopCoroutine(LoopAudio());
    }

    IEnumerator LoopAudio()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            FindObjectOfType<AudioManager>()?.Play("Ocean Waves");
            Debug.Log("WEH");

        }
        else if (!isStop)
        {
            isStop = true;
            yield return new WaitForSeconds(70f);
            isStop = false;
            isPlaying = false;
        }


    }

    public void Starting()
    {
        isInMainMenu = false;
        audioManager.sounds[15].source.Stop();
    }
}
