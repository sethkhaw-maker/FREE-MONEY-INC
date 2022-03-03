using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericPredator : SYS_FSM
{
    public override void SetupStates()
    {
        base.SetupStates();
        if (!self.isLeader) self.targetAsAnimal = self.GetLeader();
    }

    protected override void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_FollowNoah());
        // TODO: checkforhungy -> check prey
        // TODO: stalkprey
        // TODO: checkforchaseprey?
        // TODO: chase prey
        // TODO: fail chase prey
    }

    protected override void CheckForStateSwitch()
    {
        throw new System.NotImplementedException();
    }

}
