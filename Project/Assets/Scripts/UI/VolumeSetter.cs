using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    AudioManager audioManager;
    public Slider sfxSlider;
    public Slider bgmSlider;

    void Start()
    {
        audioManager = AudioManager.instance;
        sfxSlider.value = audioManager.sfxVolume;
        bgmSlider.value = audioManager.bgmVolume;
    }

    public void SetBGMVolume()
    {
        audioManager.bgmVolume = bgmSlider.value;
    }

    public void SetSFXVolume()
    {
        audioManager.sfxVolume = sfxSlider.value;
    }
}
