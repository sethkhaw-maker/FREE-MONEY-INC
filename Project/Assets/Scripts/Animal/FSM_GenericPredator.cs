using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericPredator : SYS_FSM
{
    protected override void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_FollowNoah());
        states.Add(new STATE_CheckForPrey());
        states.Add(new STATE_Sneak());
        states.Add(new STATE_CheckEatPrey());
        states.Add(new STATE_Chase());
        states.Add(new STATE_InteractionPredator());
    }

    public override void SetupStates() => base.SetupStates();

    protected override void CheckForStateSwitch()
    {
        if (currState == null) return;
        if (!(currState is STATE_FollowNoah) && self.isInParty) SwitchToState(states.Find(x => x is STATE_JoiningParty));
        if (currState is STATE_JoiningParty && currState.progress) SwitchToState(states.Find(x => x is STATE_FollowNoah));

        if (InDefaultBehaviour() && currState.progress) SwitchToState(states.Find(x => x is STATE_CheckForPrey));
        if (currState is STATE_CheckForPrey && currState.progress) { if (HuntPrey()) SwitchToState(states.Find(x => x is STATE_Sneak)); else RandomizeState(defaultBehaviour); };
        if (currState is STATE_Sneak && currState.progress) SwitchToState(states.Find(x => x is STATE_CheckEatPrey));
        if (currState is STATE_CheckEatPrey && currState.progress) { if (StartInteraction()) SwitchToState(states.Find(x => x is STATE_InteractionPredator)); else SwitchToState(states.Find(x => x is STATE_CheckForPrey)); }
        if (currState is STATE_InteractionPredator && currState.progress) RandomizeState(defaultBehaviour);

        if (currState is STATE_Idle && currState.progress) SwitchToState(states.Find(x => x is STATE_Wander));
        if (currState is STATE_Wander && currState.progress) SwitchToState(states.Find(x => x is STATE_Idle));
    }

    void RandomizeState(List<eSTATE> eStates) => SwitchToState(eStates[Mathf.RoundToInt(Random.Range(0, eStates.Count))]);
    bool HuntPrey() => self.targetAsAnimal != null ? true : false;
    bool StartInteraction() => self.preyPredatorInteraction > 0 ? true : false;
    bool InDefaultBehaviour() => currState is STATE_Wander || currState is STATE_Idle;
}
