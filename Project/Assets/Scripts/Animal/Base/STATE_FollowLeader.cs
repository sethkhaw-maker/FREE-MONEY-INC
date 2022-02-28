using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_FollowLeader : SYS_FSMState
{
    float offset = 3f;

    public override void OnEnter()
    {
        self.targetAsAnimal = self.GetLeader();
        self.target = self.targetAsAnimal.gameObject;
    }

    public override void OnExit() 
    { 
        progress = false;
    }

    public override void Running()
    {
        if (FarFromLeader())
            MoveTowardsLeader();
        else
            EnterNextState();
    }

    bool FarFromLeader() => Vector3.Distance(self.transform.position, self.target.transform.position) > offset ? true : false;
    void MoveTowardsLeader() => self.rb.velocity = SYS_AnimalTools.MoveTowards(self.target.transform.position, self, self.wanderSpeed);
}
