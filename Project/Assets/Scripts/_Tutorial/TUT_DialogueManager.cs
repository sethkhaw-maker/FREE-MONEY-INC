using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_DialogueManager : MonoBehaviour
{
    public GameObject tutorialSection;
    public GameObject dialogue;
    int dialogueNumber = 1;

    public void DialogueProgress()
    {
        //turn off the current text
        dialogue.SetActive(false);
        //Get the name for the next dialogue
        ++dialogueNumber;

        Transform t = tutorialSection.transform.Find("Dialogue_" + dialogueNumber);

        if (t == null)
        {
            TUT_TutorialStateManager.instance.ProgressTutorialState();
            TUT_TutorialStateManager.EndTutorial();
            tutorialSection.SetActive(false);
        }
        else
        {
            dialogue = tutorialSection.transform.Find("Dialogue_" + dialogueNumber).gameObject;
            dialogue.SetActive(true);
            //Debug.Log(dialogue);
        }
    }
}
