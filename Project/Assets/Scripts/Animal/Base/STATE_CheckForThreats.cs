using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_CheckForThreats : SYS_FSMState
{
    public override bool IsInteractable => true;
    public float range = 10f;   // can be set on fsm setupstates if needed to be changed. default value for testing = 10f.
    public bool threatFound = false;
    List<Animal> listOfPredators = new List<Animal>();

    public override void OnEnter() { }
    public override void OnExit()
    {
        threatFound = false;
        progress = false;
    }

    public override void Running() 
    {
        GetAllPredators();
        CheckToSpotPredator();
        EnterNextState();
    }

    void GetAllPredators()
    {
        if (listOfPredators.Count > 0) return;

        string[] split = self.chaseTargets.Split(',');
        foreach (string animal in split)
            listOfPredators.AddRange(self.GetAllSameAnimals(animal));
    }

    void CheckToSpotPredator()
    {
        foreach (Animal predator in listOfPredators)
            if (DistCheck(predator))    // ensure within range + self is facing dir of predator
            {                           // if so, ready to alert other members.
                self.SetTargetAs(predator);
                threatFound = true;
                break;
            }
    }

    bool DistCheck(Animal predator)
    {
        Vector3 predatorPos = predator.transform.position;
        Vector3 selfPos = self.transform.position;

        if (Vector3.Distance(predatorPos, selfPos) <= range)
            return true;
        // TODO: fix animal flip before enabling below.
        //if (self.flipAnimal.facingRight)
        //    {
        //        if (predatorPos.x < selfPos.x)
        //            return true;
        //    }
        //    else
        //    {
        //        if (predatorPos.x > selfPos.x)
        //            return true;
        //    }
        return false;
    }
}
