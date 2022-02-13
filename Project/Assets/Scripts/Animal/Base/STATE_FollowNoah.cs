using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_FollowNoah : SYS_FSMState
{
    float offset = 1.5f;

    public override void Init(Animal _s)
    {
        self = _s;
    }

    public override void OnEnter()
    {
        GameObject followTarget = PlayerController.GameObject;

        int index = PlayerController.party.IndexOf(self);
        if (index != 0) followTarget = PlayerController.party[index - 1].gameObject;

        self.target = followTarget;
    }

    public override void OnExit() { }

    public override void Running()
    {
        float dist = Vector3.Distance(self.transform.position, self.target.transform.position);

        if (dist > offset)
        {
            //Debug.Log("dist: " + dist + " | offset: " + offset + " | self: " + self.transform.position + " | target: " + self.target.transform.position + " | targetName: " + self.target.name);
            self.transform.position = Vector3.MoveTowards(self.transform.position, self.target.transform.position, self.wanderSpeed * Time.deltaTime);
        }
            
    }
}
