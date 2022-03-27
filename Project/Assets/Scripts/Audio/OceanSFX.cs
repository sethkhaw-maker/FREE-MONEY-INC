using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSFX : MonoBehaviour
{
    public AudioManager audioManager;
    bool isInMainMenu;
    void Start()
    {
        isInMainMenu = true;
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        FindObjectOfType<AudioManager>()?.Play("Ocean Waves");

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
        yield return new WaitForSeconds(audioManager.sounds[15].clip.length);
        FindObjectOfType<AudioManager>()?.Play("Ocean Waves");
    }

    public void Starting()
    {
        isInMainMenu = false;
        audioManager.sounds[15].source.Stop();
    }
}
