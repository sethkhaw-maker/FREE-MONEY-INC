using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_FindLeader : SYS_FSMState
{
    public override bool IsInteractable => true;

    public override void OnEnter() { self.rb.velocity = Vector2.zero; }
    public override void OnExit() { progress = false; }

    public override void Running() 
    {
        GetLeader();
        if (LeaderIsFleeing())
            FleeFromPredator();
        EnterNextState();
    }

    void GetLeader() => self.SetTargetAs(self.GetLeader());
    bool LeaderIsFleeing() => self.targetAsAnimal.shouldFlee;
    void FleeFromPredator() { self.shouldFlee = true; self.SetTargetAs(self.targetAsAnimal.targetAsAnimal); }
}
