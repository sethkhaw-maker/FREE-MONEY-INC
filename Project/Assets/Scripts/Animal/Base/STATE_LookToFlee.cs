using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_LookToFlee : SYS_FSMState
{

    public override void OnEnter() { }
    public override void OnExit()
    {
        progress = false;
    }

    public override void Running()
    {
        foreach (Animal animal in Animal.allAnimals)
            if (IsATarget(animal.name, self.fleeTargets) && IsOutOfRange(animal))
                AssignTarget(animal);
        progress = true;
    }

    bool IsATarget(string name, string listOfTargets)
    {
        if (string.IsNullOrEmpty(listOfTargets) || string.IsNullOrWhiteSpace(listOfTargets))
            return false;
        return listOfTargets.Contains(name);
    }
    bool IsOutOfRange(Animal other) => Vector3.Distance(self.transform.position, other.transform.position) >= other.runRange ? true : false;
    void AssignTarget(Animal a) => self.targetAsAnimal = a;
}
