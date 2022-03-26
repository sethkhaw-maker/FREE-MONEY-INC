using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_TutorialStateManager : MonoBehaviour
{
    [Header("Dialogue Text")]
    public List<GameObject> dialogueText = new List<GameObject>();

    [Header("Obj References")]
    public Animator cloudCanvas;

    [Header("READONLY")]
    public int tutorialState = 0;

    [HideInInspector] public static TUT_TutorialStateManager instance;
    [HideInInspector] public static bool tutorialRunning = false;

    private void Start() => instance = this;

    private void Update()
    {
        if (!tutorialRunning && tutorialState == 0) { ReadAnimator(); }
    }

    public void ProgressTutorial()
    {
        switch (tutorialState)
        {
            case 0: ShowTutorialText(); break;
        }
    }

    public void ShowTutorialText()
    {
        StartTutorial();
        switch (tutorialState)
        {
            case 0: dialogueText[0].SetActive(true); break;
            case 5: dialogueText[1].SetActive(true); break;
            case 10: dialogueText[2].SetActive(true); break;
        }
    }

    public void SwitchArrow()
    {

    }

    public void ProgressTutorialState() { tutorialState++; ProgressTutorial(); }
    public void CloseTutorialText()
    {
        switch (tutorialState)
        {
            case 0: dialogueText[0].SetActive(false); break;
            case 5: dialogueText[1].SetActive(false); break;
            case 10: dialogueText[2].SetActive(false); break;
        }
    }
    public static void StartTutorial() => tutorialRunning = true;
    public static void EndTutorial() => tutorialRunning = false;

    void ReadAnimator() { if (cloudCanvas.GetCurrentAnimatorStateInfo(0).normalizedTime > cloudCanvas.GetCurrentAnimatorClipInfo(0).Length) ProgressTutorial(); }
}
