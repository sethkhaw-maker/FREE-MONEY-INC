using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Idle : SYS_FSMState
{
    float timer;

    public override void OnEnter() 
    {
        self.rb.velocity = Vector2.zero;
    }
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
