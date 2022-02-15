using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_FollowNoah : SYS_FSMState
{
    //public float offset = 1.5f; // needs recalc next time.

    public override void Init(Animal _s)
    {
        self = _s;
    }

    public override void OnEnter()
    {
        // Assume follow target is Noah.
        GameObject followTarget = PlayerController.instance.gameObject;
        
        // The Animal should already be registered to Noah's party, so try and get its position in
        // the party List.
        int index = PlayerController.party.IndexOf(self);
        
        // If it isn't the first animal on the list (aka following Noah), get the animal
        // in front of it as the target to follow.
        if (index != 0) followTarget = PlayerController.party[index - 1].gameObject;

        // Set target to animal.
        self.target = followTarget;
    }

    public override void OnExit() { }

    public override void Running()
    {
        // Calculate distance.
        float dist = Vector3.Distance(self.transform.position, self.target.transform.position);

        // Ensure that there's an offset. Move animal until offset distance.
        if (dist > self.followOffset)
        {
            self.transform.position = Vector3.MoveTowards(self.transform.position, self.target.transform.position, self.wanderSpeed * Time.deltaTime);
        }
        else
        {
            if (self.target.CompareTag("Ark"))
            {
                self.RegisterAnimalToArk();
            }
        }

    }
}
