using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericAnimal : SYS_FSM
{
    protected override void CheckForStateSwitch()
    {
        if (currState.progress && self.inParty) SwitchToState(states.Find(x => x is STATE_FollowNoah));
        if (currState is STATE_Idle && currState.progress) SwitchToState(states.Find(x => x is STATE_Wander));
        if (currState is STATE_Wander && currState.progress) SwitchToState(states.Find(x => x is STATE_Idle));
    }

    public override void SetupStates()
    {
        AddStates();
        InitStates();
    }

    void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_FollowNoah());
    }

    void InitStates() { foreach (SYS_FSMState state in states) state.Init(self); }
}
