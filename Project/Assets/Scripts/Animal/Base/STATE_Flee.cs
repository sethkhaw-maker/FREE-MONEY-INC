using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Flee : SYS_FSMState
{
    Vector3 endPos;
    float leniency = 0.1f;
    bool fleeing;

    public override void OnEnter() { }
    public override void OnExit()
    {
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
        // OLD: ah jeez. T_T)a i need to figure out the following for endPos:
        /* a. get target flip position
         * b. do self position + (self.runRange * flip)
         * c. if any offsets are wanted, should be modified to the position here???
         */

        endPos = -SYS_AnimalTools.MoveTowards(self.target.transform.position, self, self.runRange);
        Debug.Log("self.target: " + self.target.name + " | startPos: " + self.transform.position + " | endPos: " + endPos);
        fleeing = true;
    }
    void FleeFromTarget() => self.rb.velocity = SYS_AnimalTools.MoveTowards(endPos, self, self.runSpeed);
    bool OutOfRange() => Vector3.Distance(self.transform.position, endPos) <= leniency ? true : false;
    void LoseTarget() { self.target = null; self.shouldFlee = false; }
}
