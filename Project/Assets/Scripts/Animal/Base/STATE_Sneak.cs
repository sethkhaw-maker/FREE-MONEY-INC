using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Sneak : SYS_FSMState
{
    public override bool IsInteractable => true;

    float range = 12f;
    float wanderTimer = 1.5f;
    float idleTimer = 1f;
    float timer;
    bool moving = true;

    public override void OnEnter() { }
    public override void OnExit() 
    {
        self.rb.velocity = Vector2.zero;
        progress = false;
    }

    public override void Running()
    {
        timer += Time.deltaTime;

        if (moving)
            StalkPrey();
        else
            Wait();

        if (CloseToTarget())
            EnterNextState();
    }

    void StalkPrey()
    {
        if (timer > wanderTimer) { ResetTimer(); return; }
        self.rb.velocity = SYS_AnimalTools.MoveTowards(self.target.transform.position, self, self.wanderRange);
    }

    void Wait() 
    { 
        if (timer > idleTimer) { ResetTimer(); return; }
        self.rb.velocity = Vector2.zero;
    }

    void ResetTimer() { timer = 0; moving = !moving; }
    bool CloseToTarget() => Vector3.Distance(self.target.transform.position, self.transform.position) <= range ? true : false;
}
