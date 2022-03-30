using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericAnimEvent : MonoBehaviour
{
    public UnityEvent loadEvent;
    //public Animator fade;

    //Animation Event for generic
    public void LoadGame()
    {
        //objectiveCanvas.SetActive(true);
        loadEvent.Invoke();
    }

    //public void SkipObjectives()
    //{
    //    fade.SetInteger("fadeState", 1);
    //}
}
