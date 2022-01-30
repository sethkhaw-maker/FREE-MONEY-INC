using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Wander : SYS_FSMState
{
    float leniency = 0.1f;
    Vector3 startPos, endPos;
    bool wandering;

    public override void Init(Animal _s)
    {
        self = _s;
    }

    public override void OnEnter() { }
    public override void OnExit()
    {
        wandering = false;
        progress = false;
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
        startPos = self.transform.position;
        endPos = startPos + wanderPoint;
        wandering = true;
    }
    void MoveTowardsWanderPosition() => self.transform.position = Vector3.MoveTowards(startPos, endPos, self.wanderSpeed * Time.deltaTime);
    bool AtWanderPosition() => Vector3.Distance(self.transform.position, endPos) < leniency ? true : false;
}
