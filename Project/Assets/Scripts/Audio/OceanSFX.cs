using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSFX : MonoBehaviour
{
    public AudioManager audioManager;
    bool isInMainMenu;
    bool isPlaying;

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
            FindObjectOfType<AudioManager>()?.Play("Ocean Waves");
            isPlaying = true;
        }
        yield return new WaitForSeconds(audioManager.sounds[15].clip.length);
        isPlaying = false;
        FindObjectOfType<AudioManager>()?.Play("Ocean Waves");
        
    }

    public void Starting()
    {
        isInMainMenu = false;
        audioManager.sounds[15].source.Stop();
    }
}
