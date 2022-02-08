using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFade
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        //Add the below line into scripts to enable audio fade from 1 audio file to another...
        //StartCoroutine(AudioFade.StartFade(AudioSource audioSource, float duration, float targetVolume));

        float currentTime = 0;
        float start = audioSource.volume;

        if (currentTime < duration)
        {
            currentTime += Time.unscaledTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}