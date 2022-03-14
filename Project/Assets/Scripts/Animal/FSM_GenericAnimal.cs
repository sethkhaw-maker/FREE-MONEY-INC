using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_GenericAnimal : SYS_FSM
{
    protected override void CheckForStateSwitch()
    {
        if (!(currState is STATE_JoiningParty || currState is STATE_FollowNoah) && self.isInParty) SwitchToState(states.Find(x => x is STATE_JoiningParty));
        if (currState is STATE_JoiningParty && currState.progress) SwitchToState(states.Find(x => x is STATE_FollowNoah));
        if (currState is STATE_Idle && currState.progress) SwitchToState(states.Find(x => x is STATE_Wander));
        if (currState is STATE_Wander && currState.progress) SwitchToState(states.Find(x => x is STATE_Idle));
    }

    protected override void AddStates()
    {
        states.Add(new STATE_Idle());
        states.Add(new STATE_Wander());
        states.Add(new STATE_FollowNoah());
        states.Add(new STATE_JoiningParty());
    }
}
