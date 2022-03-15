using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_FindOasis : SYS_FSMState
{
    public override bool IsInteractable => true;

    static GameObject oasis = null;
    float leniency = 0.5f;
    float eatingTime = 5f;
    int miniState = 0;
    bool checkForFlipped = false;

    float timer = 0;

    public override void OnEnter() 
    {
        if (oasis != null) return; 
        
        oasis = GameObject.Find("Oasis");
        leniency += oasis.GetComponent<CircleCollider2D>().radius;
    }
    public override void OnExit() 
    {
        progress = false;
        miniState = 0;
        timer = 0f;
        checkForFlipped = false;
    }

    public override void Running()
    {
        switch (miniState)
        {
            case 0: MoveTowardsOasis(); break;
            case 1: Eat(); break;
            default: EnterNextState(); break;
        }
    }
    void MoveTowardsOasis()
    {
        self.rb.velocity = SYS_AnimalTools.MoveTowards(oasis.transform.position, self, self.wanderSpeed);
        if (Vector3.Distance(oasis.transform.position, self.transform.position) < leniency) AdvanceMiniState();
    }
    void Eat()
    {
        timer += Time.deltaTime;
        FlipForFunsies();
        if (timer > eatingTime)
        {
            self.isHungry = false;
            EnsureEveryoneHasEaten();
            AdvanceMiniState();
        }
    }
    void FlipForFunsies()
    {
        if (checkForFlipped) return;

        if (timer > (eatingTime / 2) && Random.Range(0, 2) > 0) self.flipAnimal.Flip();
        checkForFlipped = true;
    }
    void EnsureEveryoneHasEaten()
    {
        foreach (Animal a in self.GetAllSameHerd())
            a.isHungry = false;
    }

    void AdvanceMiniState()
    {
        self.rb.velocity = Vector2.zero;
        miniState++;
    }
}
