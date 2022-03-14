using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_AlertFollowers : SYS_FSMState
{
    public override bool IsInteractable => true;
    public override void OnEnter() { }
    public override void OnExit() { progress = false; }

    public override void Running()
    {
        // if there's an anim anywhere, put it here
        AlertAllFollowers();
        self.shouldFlee = true;
        EnterNextState();
    }

    void AlertAllFollowers() 
    {
        List<Animal> allFollowers = self.GetAllSameAnimals();
        foreach (Animal a in allFollowers)
        {
            a.SetTargetAs(self.targetAsAnimal);
            a.shouldFlee = true;
        }
    }
}
