using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStoryBookCover : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void StartGameSequence()
    { 
        anim.SetTrigger("StartSequence");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
