using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Wander : SYS_FSMState
{
    float leniency = 0.1f;
    Vector3 endPos;
    bool wandering;

    public override void OnEnter() { }
    public override void OnExit()
    {
        wandering = false;
        progress = false;
        endPos = Vector3.zero;
        self.rb.velocity = Vector2.zero;
    }

    public override void Running()
    {
        if (!wandering)
            GetWanderPosition();
        MoveTowardsWanderPosition();
        if (AtWanderPosition())
            EnterNextState();
    }

    void GetWanderPosition()
    {
        float x = Random.Range(-self.wanderRange, self.wanderRange);
        float y = Random.Range(-self.wanderRange, self.wanderRange);

        Vector3 wanderPoint = new Vector3(x, y);
        endPos = self.transform.position + wanderPoint;
        wandering = true;
    }
    void MoveTowardsWanderPosition() => self.rb.velocity = SYS_AnimalTools.MoveTowards(endPos, self, self.wanderSpeed);
    bool AtWanderPosition() => Vector3.Distance(self.transform.position, endPos) < leniency ? true : false;
}
