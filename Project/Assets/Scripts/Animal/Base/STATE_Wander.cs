using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Wander : SYS_FSMState
{
    public override bool IsInteractable => true;

    //float leniency = 0.1f;
    Vector3 endPos;
    bool wandering;
    float timer = 0f;

    public override void OnEnter() { }
    public override void OnExit()
    {
        wandering = false;
        progress = false;
        endPos = Vector3.zero;
        self.rb.velocity = Vector2.zero;
        timer = 0f;
    }

    public override void Running()
    {
        if (!wandering)
            GetWanderPosition();
        MoveTowardsWanderPosition();
        timer += Time.deltaTime;
        if (TimeHasPassed())
            EnterNextState();
    }

    void GetWanderPosition()
    {
        float x = Random.Range(-self.wanderRange, self.wanderRange);
        float y = Random.Range(-self.wanderRange, self.wanderRange);

        Vector3 wanderPoint = new Vector3(x, y) * 30f;
        endPos = self.transform.position + wanderPoint;
        wandering = true;
    }
    void MoveTowardsWanderPosition() => self.rb.velocity = SYS_AnimalTools.MoveTowards(endPos, self, self.wanderSpeed);
    bool TimeHasPassed() => timer >= 1.5f;
    //bool AtWanderPosition() => Vector3.Distance(self.transform.position, endPos) < leniency ? true : false;
}
