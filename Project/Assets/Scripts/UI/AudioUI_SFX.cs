using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUI_SFX : MonoBehaviour
{
    public AudioSource MainMenu;
    
    
    public void PlayButtonHoverAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[0];
        MainMenu.PlayOneShot(MainMenu.clip, AudioManager.instance.sfxVolume);
    }
    public void PlayButtonClickAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[1];
        MainMenu.PlayOneShot(MainMenu.clip, AudioManager.instance.sfxVolume);
    }
    public void PlayButtonPlayGameAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[2];
        MainMenu.PlayOneShot(MainMenu.clip, AudioManager.instance.sfxVolume);
    }

    public void PlayButtonFlipCardAudio()
    {
        MainMenu.clip = MainMenu.gameObject.GetComponent<MainMenu>().SFXClips[3];
        MainMenu.PlayOneShot(MainMenu.clip, AudioManager.instance.sfxVolume);
    }
}
