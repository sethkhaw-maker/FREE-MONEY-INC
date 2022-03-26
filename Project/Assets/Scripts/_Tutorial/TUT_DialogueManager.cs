using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_DialogueManager : MonoBehaviour
{
    public GameObject tutorialSection;
    public GameObject dialogue;
    int dialogueNumber = 1;
    int maxDialogue = 0;

    private void Start()
    {
        maxDialogue = transform.parent.childCount - 3;
    }

    public void DialogueProgress()
    {
        //turn off the current text
        dialogue.SetActive(false);
        //Get the name for the next dialogue
        dialogueNumber += 1;
        Transform t = tutorialSection.transform.Find("Dialogue_" + dialogueNumber);

        if (dialogueNumber <= maxDialogue)
        {
            Debug.Log("dialogueNum: " + dialogueNumber + " | maxDialogue: " + maxDialogue);
            dialogue = t.gameObject;
            dialogue.SetActive(true);
        }
        else
        {
            TUT_TutorialStateManager.EndTutorial();
            TUT_TutorialStateManager.instance.CloseTutorialText();
        }
    }
}
