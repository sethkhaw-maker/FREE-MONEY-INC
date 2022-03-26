using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_MovementTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Noah" && TUT_TutorialStateManager.instance.tutorialState == 1)
        {
            TUT_TutorialStateManager.instance.ProgressTutorialState();
            gameObject.SetActive(false);
        }
    }
}
