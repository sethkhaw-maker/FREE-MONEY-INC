using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_HungerManager : MonoBehaviour
{
    public bool active = true;
    float timer = 20f;

    private void Start()
    {
        StartCoroutine(CheckForHunger());
    }

    IEnumerator CheckForHunger()
    {
        while (active)
        {
            yield return new WaitForSeconds(timer);
            MakeAnimalsHungry();
        }
    }

    void MakeAnimalsHungry()
    {
        int quota = Mathf.RoundToInt(Animal.allAnimals.Count * 0.05f);

        for (int i = 0; i < quota; i++) // pick random animal from all animals.
        {
            Animal a = null;
            bool aCanBeHungry = false;
            while (!aCanBeHungry)       // keep getting new animals until an animal can be marked as hungry
            {
                int j = Random.Range(0, Animal.allAnimals.Count - 1);
                a = Animal.allAnimals[j];

                // if their states invalid (aka is hungry, is in party, is despawning) don't make them eligible for hunger.
                if (!(a.isHungry || a.isInParty || a.isDespawning)) aCanBeHungry = true;
            }

            a.isHungry = true;
            //Debug.Log("Animal " + a.gameObject.name + " is hungry!");
        }
    }

    void ProvokeLeadersForHerdHunger()
    {

    }
}
