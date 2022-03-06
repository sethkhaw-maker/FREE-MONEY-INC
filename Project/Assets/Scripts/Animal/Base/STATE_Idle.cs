using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Idle : SYS_FSMState
{
    public override bool IsInteractable => true;

    float timer;

    public override void OnEnter() { }
    public override void OnExit()
    {
        timer = 0f;
        progress = false;
    }

    public override void Running()
    {
        timer += Time.deltaTime;
        if (timer > self.idleTime)
            EnterNextState();
    }
}
