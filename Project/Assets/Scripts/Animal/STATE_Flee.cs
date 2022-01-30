using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Flee : SYS_FSMState
{
    Vector3 startPos, endPos;
    bool fleeing;

    public override void Init(Animal _s)
    {
        self = _s;
    }

    public override void OnEnter() { }
    public override void OnExit()
    {
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        fleeing = false;
        progress = false;
    }

    public override void Running()
    {
        if (!fleeing)
            GetEndPosition();
        FleeFromTarget();
        if (OutOfRange())
        {
            LoseTarget();
            EnterNextState();
        }
    }

    void GetEndPosition()
    {
        startPos = self.transform.position;
        // ah jeez. T_T)a i need to figure out the following for endPos:
        /* a. get target flip position
         * b. do self position + (self.runRange * flip)
         * c. if any offsets are wanted, should be modified to the position here???
         */
        endPos = self.target.transform.position; 
        fleeing = true;
    }
    void FleeFromTarget() => self.transform.position = Vector3.MoveTowards(startPos, endPos, self.runSpeed * Time.deltaTime);
    bool OutOfRange() => Vector3.Distance(self.transform.position, endPos) >= self.target.runRange ? true : false;
    void LoseTarget() => self.target = null;
}
