﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_JoiningParty : SYS_FSMState
{
    public override bool IsInteractable => false;

    public override void OnEnter() { }
    public override void OnExit()
    {
        progress = false;
    }

    public override void Running()
    {
        if (self.isLeader)
        {
            self.SetNewLeader();
            self.RemoveLeader();
        }
        self.shouldFlee = false;
        self.preyPredatorInteraction = 0;
        self.herdNum = 0;
        self.ClearTarget();
        EnterNextState();
    }
}