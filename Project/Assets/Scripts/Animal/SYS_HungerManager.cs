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
        ProvokeLeadersForHerdHunger();  
    }
    void ProvokeLeadersForHerdHunger() // if enough herd members are hungry, leader will go find food (become hungry as well).
    {
        foreach (Animal a in Animal.allAnimalLeaders)
        {
            if (a.isHungry) continue;

            float herdSize = a.GetHerdSize(a.herdNum);
            if (herdSize >= 2 && CheckToInvokeHunger(a.GetAllSameHerd())) a.isHungry = true;
            if (herdSize < 2 && CheckToInvokeHunger(a.GetAllSameHerd(), 1)) a.isHungry = true;

            if (a.isHungry) Debug.Log("leader " + a.name + " is getting hungry. | herdcount: " + herdSize);
        }
    }
    bool CheckToInvokeHunger(List<Animal> animals, int min = 2)
    {
        int i = 0;

        foreach (Animal a in animals)
        {
            if (a.isHungry) i++;
            if (i >= min) return true;
        }
        return false;
    }
}
