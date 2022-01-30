using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Chase : SYS_FSMState
{
    float leniency = 0.1f;
    Vector3 startPos, endPos;
    bool chasing;

    public override void Init(Animal _s)
    {
        self = _s;
    }
    public override void OnEnter() { }
    public override void OnExit()
    {
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        chasing = false;
        progress = false;
    }

    public override void Running()
    {
        if (!chasing)
            GetPositions();
        ChaseTarget();
        if (AtChasePosition())
        {
            LoseTarget();
            EnterNextState();
        }
    }

    void GetPositions()
    {
        startPos = self.transform.position;
        endPos = self.target.transform.position;
        chasing = true;
    }
    void ChaseTarget() => self.transform.position = Vector3.MoveTowards(startPos, endPos, self.runSpeed * Time.deltaTime);
    bool AtChasePosition() => Vector3.Distance(self.transform.position, endPos) < leniency ? true : false;
    void LoseTarget() => self.target = null;
}
