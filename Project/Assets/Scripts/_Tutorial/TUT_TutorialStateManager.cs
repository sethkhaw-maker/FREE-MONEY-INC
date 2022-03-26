﻿using System.Collections;
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
    [HideInInspector] public static bool[] tutorialDisplayed = new bool[20];

    private void Start() => instance = this;

    private void Update()
    {
        CheckForFlagsInUpdate();
    }

    void CheckForFlagsInUpdate()
    {
        if (!tutorialRunning && tutorialState == 0 && !tutorialDisplayed[0]) { ReadAnimator(); }
    }

    public void ProgressTutorial()
    {
        switch (tutorialState)
        {
            case 0: ShowTutorialText(); break;
            case 1: SwitchArrow(); break;
            case 2: SwitchArrow(); ShowTutorialText(); break;
        }
    }

    public void ShowTutorialText()
    {
        StartTutorial();
        switch (tutorialState)
        {
            case 0: dialogueText[0].SetActive(true); break;
            case 2: dialogueText[1].SetActive(true); break;
            case 10: dialogueText[2].SetActive(true); break;
        }
    }

    public void SwitchArrow()
    {
        switch (tutorialState)
        {
            case 1: GameObject.Find("Zebra").GetComponent<Animal>().tutorialArrow.SetActive(true); break;
            case 2: GameObject.Find("Zebra").GetComponent<Animal>().tutorialArrow.SetActive(false); break;
        }
    }

    public void ProgressTutorialState() { SetTutorialFlag(); tutorialState++; ProgressTutorial(); }
    public void SetTutorialFlag() => tutorialDisplayed[tutorialState] = true;
    public static void StartTutorial() => tutorialRunning = true;
    public static void EndTutorial() { tutorialRunning = false; }

    void ReadAnimator() { if (cloudCanvas.GetCurrentAnimatorStateInfo(0).normalizedTime > cloudCanvas.GetCurrentAnimatorClipInfo(0).Length) ProgressTutorial(); }
}
