using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public void EndGame()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();

        if (sceneLoader != null)
        {
            sceneLoader.LoadScene(0);
            sceneLoader.ChangeBGM(0);
        }
    }
}
