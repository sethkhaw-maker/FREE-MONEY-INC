using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinScript : MonoBehaviour
{
    public Animator fadeCanvas;

    public void EndGame()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();

        if (sceneLoader != null)
        {
            sceneLoader.LoadScene(6);
            sceneLoader.ChangeBGM(0);
        }
    }
    public void RetryDay() => GameplayManager.instance.RetryDay();
}
