using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Idle : SYS_FSMState
{
    float timer;

    public override void Init(Animal _s)
    {
        self = _s;
    }

    public override void OnEnter() { }
    public override void OnExit()
    {
        timer = 0f;
    }

    public override void Running()
    {
        timer += Time.deltaTime;
        if (timer > self.idleTime)
            progress = true;
    }
}
