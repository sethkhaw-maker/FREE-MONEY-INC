using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_TutorialStateManager : MonoBehaviour
{
    public int tutorialState = 0;
    public TUT_TutorialStateManager instance;
    public static bool tutorialRunning = false;

    private void Start() => instance = this;

    public void ProgressTutorial()
    {
        switch (tutorialState)
        {
            case 0: PlayTutorial(); break;
        }
    }

    public void PlayTutorial()
    {

    }

    public void SwitchArrow()
    {

    }

    public void ProgressTutorialState() { tutorialState++; ProgressTutorial(); }
    public static void StartTutorial() => tutorialRunning = true;
    public static void EndTutorial() => tutorialRunning = false;
}
