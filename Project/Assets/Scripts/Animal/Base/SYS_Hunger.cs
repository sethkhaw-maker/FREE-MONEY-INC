using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Hunger : MonoBehaviour
{
    public float hunger = 100f;
    public float maxHunger = 100f;
    public float hungerIncrement = 5f;

    private void Update()
    {
        // logic for hunger here. i'm assuming it's like, every once in a while, deduct? or randomize deduction?
        // to be decided. but let's just say hungry.
    }

    public void ModifyHunger(float num) { hunger += num; }
}
