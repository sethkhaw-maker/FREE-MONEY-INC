using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AnimSceneEffects : MonoBehaviour
{
    public int id;
    private Animator anim;
    private CinemachineImpulseSource source;
    void Start()
    {
        anim = GetComponent<Animator>();
        //Subscription to Event Manager (ID 1)
        //GameEvents.current.onGameEndTrigger += EndGameSequence;
        source = GetComponent<CinemachineImpulseSource>();
    }

    IEnumerator WaitEndSequence()
    {
        //Wait for rats
        source.GenerateImpulse();
        yield return new WaitForSeconds(7f);
        anim.SetTrigger("EndSequence");
    }
    public void EndGameSequence(int id)
    {
            StartCoroutine(WaitEndSequence());
            
        
    }
}
