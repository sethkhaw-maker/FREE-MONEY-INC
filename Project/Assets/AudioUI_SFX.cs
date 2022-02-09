using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUI_SFX : MonoBehaviour
{
    public AudioSource MainMenu;
    
    
    public void PlayButtonHoverAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[0];
        MainMenu.Play();
    }
    public void PlayButtonClickAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[1];
        MainMenu.Play();
    }
    public void PlayButtonPlayGameAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[2];
        MainMenu.Play();
    }

    public void PlayButtonFlipCardAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[3];
        MainMenu.Play();
    }
}
