using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericPrey : SYS_FSM
{
    protected override void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_JoiningParty());
        states.Add(new STATE_FollowNoah());
        states.Add(new STATE_FollowLeader());
        states.Add(new STATE_AlertFollowers());
        states.Add(new STATE_CheckForThreats());
        states.Add(new STATE_Flee());
        states.Add(new STATE_FindLeader());
        states.Add(new STATE_InteractionPrey());
        states.Add(new STATE_FindOasis());
    }

    protected override void CheckForStateSwitch()
    {
        if (currState == null) return;
        if (!active) return;
        if (self.isDespawning) { DeactivateFSM(); return; }
        if (self.isLeader)
        {
            if (NeedToCheckForThreats() && currState.progress) SwitchToState(states.Find(x => x is STATE_CheckForThreats));
            if (currState is STATE_FollowLeader) SwitchToState(states.Find(x => x is STATE_Wander));
            if (self.isHungry && currState.progress && !(currState is STATE_FindOasis)) SwitchToState(states.Find(x => x is STATE_FindOasis));
            if (currState is STATE_CheckForThreats && currState.progress) { if (ThreatFound()) SwitchToState(states.Find(x => x is STATE_AlertFollowers)); else RandomizeState(defaultBehaviour); }
            if (currState is STATE_AlertFollowers && currState.progress) SwitchToState(states.Find(x => x is STATE_Flee));
            if (currState is STATE_Flee && currState.progress) SwitchToState(states.Find(x => x is STATE_CheckForThreats));
            if (currState is STATE_FindOasis && currState.progress) RandomizeState(defaultBehaviour);
            // re:line34 | need to polish this, likely; ensure that leader 'wanders' away from predator in a way that calls herd back to leader.
        }
        else
        {
            if (self.targetAsAnimal != null && self.targetAsAnimal.isLeader)
            {
                if (currState is STATE_FollowLeader && self.targetAsAnimal.isInParty) SwitchToState(eSTATE.IDLE);
                if (currState is STATE_FollowLeader && currState.progress) RandomizeState(defaultBehaviour);
                if (self.targetAsAnimal.animalFSM.currState is STATE_FindOasis && self.targetAsAnimal.rb.velocity == Vector2.zero) RandomizeState(defaultBehaviour);
                if (self.targetAsAnimal.animalFSM.currState is STATE_Wander || self.targetAsAnimal.animalFSM.currState is STATE_FindOasis) SwitchToState(states.Find(x => x is STATE_FollowLeader));
            }
            else
            {
                if (currState is STATE_Flee && currState.progress) SwitchToState(states.Find(x => x is STATE_FindLeader));
            }
        }
        if (!(currState is STATE_JoiningParty || currState is STATE_FollowNoah) && self.isInParty) SwitchToState(states.Find(x => x is STATE_JoiningParty));
        if (currState is STATE_JoiningParty && currState.progress) SwitchToState(states.Find(x => x is STATE_FollowNoah));
        if (!(currState is STATE_Flee) && self.shouldFlee) SwitchToState(states.Find(x => x is STATE_Flee));
        if (currState is STATE_Idle && currState.progress) SwitchToState(states.Find(x => x is STATE_Wander));
        if (currState is STATE_Wander && currState.progress) SwitchToState(states.Find(x => x is STATE_Idle));
        if (currState is STATE_FindLeader && currState.progress) RandomizeState(defaultBehaviour);
        if (currState is STATE_InteractionPrey && currState.progress) { if (!self.shouldFlee) RandomizeState(defaultBehaviour); else SwitchToState(states.Find(x => x is STATE_Flee)); }
        if (self.preyPredatorInteraction > 0 && !(currState is STATE_InteractionPrey || currState is STATE_Flee)) SwitchToState(states.Find(x => x is STATE_InteractionPrey));
    }

    void RandomizeState(List<eSTATE> eStates) => SwitchToState(eStates[Mathf.RoundToInt(Random.Range(0, eStates.Count))]);
    bool ThreatFound() { STATE_CheckForThreats state = currState as STATE_CheckForThreats; return state.threatFound; }
    bool NeedToCheckForThreats() => currState is STATE_Wander || currState is STATE_Idle;
    void DeactivateFSM()
    {
        self.rb.velocity = Vector2.zero;
        active = false;
    }
}
