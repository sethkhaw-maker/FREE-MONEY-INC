using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
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
}
