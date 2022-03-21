using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericAnimEvent : MonoBehaviour
{
    public UnityEvent loadEvent;

    //Animation Event for generic
    public void LoadGame()
    {
        loadEvent.Invoke();
    }
}
