using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericPrey : SYS_FSM
{
    protected override void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_FollowNoah());
        states.Add(new STATE_FollowLeader());
    }

    public override void SetupStates()
    {
        base.SetupStates();
        if (!self.isLeader) self.targetAsAnimal = self.GetLeader();
    }

    protected override void CheckForStateSwitch()
    {
        if (self.isLeader)
        {
            if (self.isInParty) SwitchToState(states.Find(x => x is STATE_FollowNoah));
        }
        else
        {
            if (self.targetAsAnimal.isLeader)
            {
                if (self.targetAsAnimal.animalFSM.currState is STATE_Wander) SwitchToState(states.Find(x => x is STATE_FollowLeader));
                if (currState is STATE_FollowLeader && currState.progress) RandomizeState(new List<eSTATE> { eSTATE.IDLE, eSTATE.WANDER });
            } 
        }
        if (currState is STATE_Idle && currState.progress) SwitchToState(states.Find(x => x is STATE_Wander));
        if (currState is STATE_Wander && currState.progress) SwitchToState(states.Find(x => x is STATE_Idle));
        
    }

    void RandomizeState(List<eSTATE> eStates) => SwitchToState(eStates[Mathf.RoundToInt(Random.Range(0, eStates.Count))]);
}
