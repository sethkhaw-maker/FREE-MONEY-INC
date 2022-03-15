using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_CheckForPrey : SYS_FSMState
{
    public override bool IsInteractable => true;
    List<Animal> allPrey = new List<Animal>();
    Animal closestPrey;

    public override void OnEnter() { }
    public override void OnExit() 
    {
        closestPrey = null;
        progress = false;
    }

    public override void Running()
    {
        if (allPrey.Count <= 0)
            GetAllPrey();

        if (self.isHungry)
        {
            GetNearestPrey();
            AssignPreyAsTarget();
        }

        EnterNextState();
    }

    void GetNearestPrey()
    {
        float shortestDist = 99999f;

        foreach(Animal a in allPrey)
        {
            if (a.isLeader || a.isInParty || a.isDespawning) continue;
            if (self.AlreadyBeingStalked(a)) continue;
            float distToPrey = Vector3.Distance(self.transform.position, a.transform.position);
            if (distToPrey <= shortestDist)
            {
                closestPrey = a;
                shortestDist = distToPrey;
                Debug.Log("closestPrey: " + a.gameObject.name + " | shortestDist: " + shortestDist);
            }
        }
    }

    void AssignPreyAsTarget()
    {
        if (closestPrey == null) return;
        self.targetAsAnimal = closestPrey;
        self.target = closestPrey.gameObject;
        self.MarkAnimalAsPrey(self.targetAsAnimal);
    }

    void GetAllPrey()
    {
        string[] prey = self.chaseTargets.Split(',');

        foreach (string animal in prey)
            allPrey.AddRange(self.GetAllSameAnimals(animal));
    }
}
