using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_LookToChase : SYS_FSMState
{

    public override void OnEnter() { }
    public override void OnExit()
    {
        progress = false;
    }

    public override void Running()
    {
        foreach (Animal animal in Animal.allAnimals)
            if (IsATarget(animal.name, self.chaseTargets) && IsWithinDistance(animal.transform))
                AssignTarget(animal);
        progress = true;
    }

    bool IsATarget(string name, string listOfTargets)
    {
        if (string.IsNullOrEmpty(listOfTargets) || string.IsNullOrWhiteSpace(listOfTargets))
            return false;
        return listOfTargets.Contains(name);
    }
    bool IsWithinDistance(Transform other) => Vector3.Distance(self.transform.position, other.position) <= self.runRange ? true : false;
    void AssignTarget(Animal a) => self.targetAsAnimal = a;
}
