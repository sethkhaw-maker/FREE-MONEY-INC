using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericPrey : SYS_FSM
{
    List<eSTATE> defaultBehaviour = new List<eSTATE> { eSTATE.IDLE, eSTATE.WANDER };

    protected override void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_FollowNoah());
        states.Add(new STATE_FollowLeader());
        states.Add(new STATE_AlertFollowers());
        states.Add(new STATE_CheckForThreats());
        states.Add(new STATE_Flee());
        states.Add(new STATE_FindLeader());
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
            if (NeedToCheckForThreats() && currState.progress) SwitchToState(states.Find(x => x is STATE_CheckForThreats));
            if (currState is STATE_CheckForThreats && currState.progress) { if (ThreatFound()) SwitchToState(states.Find(x => x is STATE_AlertFollowers)); else RandomizeState(defaultBehaviour); }
            if (currState is STATE_Flee && currState.progress) SwitchToState(states.Find(x => x is STATE_CheckForThreats));  
            // re:line34 | need to polish this, likely; ensure that leader 'wanders' away from predator in a way that calls herd back to leader.
        }
        else
        {
            if (self.targetAsAnimal.isLeader)
            {
                if (self.targetAsAnimal.animalFSM.currState is STATE_Wander) SwitchToState(states.Find(x => x is STATE_FollowLeader));
                if (currState is STATE_FollowLeader && currState.progress) RandomizeState(defaultBehaviour);
            }
            else
            {
                if (currState is STATE_Flee && currState.progress) SwitchToState(states.Find(x => x is STATE_FindLeader));
            }
        }
        if (self.shouldFlee) SwitchToState(states.Find(x => x is STATE_Flee));
        if (currState is STATE_Idle && currState.progress) SwitchToState(states.Find(x => x is STATE_Wander));
        if (currState is STATE_Wander && currState.progress) SwitchToState(states.Find(x => x is STATE_Idle));
        if (currState is STATE_FindLeader && currState.progress) RandomizeState(defaultBehaviour);
    }

    void RandomizeState(List<eSTATE> eStates) => SwitchToState(eStates[Mathf.RoundToInt(Random.Range(0, eStates.Count))]);
    bool ThreatFound() { STATE_CheckForThreats state = currState as STATE_CheckForThreats; return state.threatFound; }
    bool NeedToCheckForThreats() => currState is STATE_Wander || currState is STATE_Idle;
}
