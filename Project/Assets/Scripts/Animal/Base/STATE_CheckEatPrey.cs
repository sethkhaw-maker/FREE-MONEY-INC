using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_CheckEatPrey : SYS_FSMState
{
    public override bool IsInteractable => false;
    bool fail = false;
    Animal prey;

    public override void OnEnter() { prey = self.targetAsAnimal; }
    public override void OnExit() { progress = false; fail = false; }

    public override void Running()
    {
        if (PreyChangedWhileStalking())
        {
            EnterNextState();
            return;
        }

        if (!CanCatchPrey()) fail = true;
        MarkBothTargetsForInteraction();
        EnterNextState();
    }

    bool PreyChangedWhileStalking()
    {
        if (prey.isLeader || prey.isInParty || !prey.animalFSM.active) 
            return true;
        return false;
    }

    bool CanCatchPrey()
    {
        if (prey.shouldFlee) return true;

        //float rnd = Random.Range(0,3);
        ////Debug.Log("is prey catchable?: " + (rnd > 0 ? "yes" : "no"));
        //if (rnd > 0) return true;
        return true;
        //else return false;
    }

    void MarkBothTargetsForInteraction()
    {
        prey.preyPredatorInteraction = fail ? 2 : 1;
        self.preyPredatorInteraction = fail ? 2 : 1;
        prey.target = self.gameObject;
        prey.targetAsAnimal = self;
    }
}
