using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Flee : SYS_FSMState
{
    public override bool IsInteractable => false;
    Vector3 endPos;
    bool fleeing;
    float timer;
    float timerRun = 2f;
    float timerWalk = 3.5f;

    public override void OnEnter() { }
    public override void OnExit()
    {
        //Debug.Log("leaving state");
        endPos = Vector3.zero;
        fleeing = false;
        progress = false;
        self.rb.velocity = Vector2.zero;
        timer = 0f;
        self.preyPredatorInteraction = 0;
    }

    public override void Running()
    {
        if (!fleeing)
            GetEndPosition();

        if (PredatorChasing()) timer = 0;
        if (!StopRunning())
            RunFromTarget();
        else if (!StopWalking())
            WalkFromTarget();
        else
        {
            // Debug.Log(self.gameObject.name + " | outOfRange");
            LoseTarget();
            EnterNextState();
        }
    }

    void GetEndPosition()
    {
        endPos = -SYS_AnimalTools.MoveTowards(self.target.transform.position, self, 100f);
        // Debug.Log(self.gameObject.name + " | self.target: " + self.target.name + " | startPos: " + self.transform.position + " | endPos: " + endPos); 
        fleeing = true;
    }
    void RunFromTarget() { self.rb.velocity = SYS_AnimalTools.MoveTowards(endPos, self, self.runSpeed); timer += Time.deltaTime; }
    void WalkFromTarget() { self.rb.velocity = SYS_AnimalTools.MoveTowards(endPos, self, self.wanderSpeed); timer += Time.deltaTime; }
    void LoseTarget() { self.target = null; self.shouldFlee = false; }
    bool StopRunning() => timer >= timerRun;
    bool StopWalking() => timer >= timerWalk;
    bool PredatorChasing() => self.preyPredatorInteraction > 0 && Vector3.Distance(self.target.transform.position, self.transform.position) < self.runRange;
    //bool OutOfRange() => Vector3.Distance(self.transform.position, endPos) <= leniency ? true : false;
}
