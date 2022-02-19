using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script holds the rates of spawning each type of animal
public static class AnimalSpawnRates
{
    public static float probabilityPrey = 10;
    public static float probabilityPredator = 10;
    public static float probabilityMediator = 10;

    //Get the probability total of all animals
    public static float GetTotalProbability()
    {
        return probabilityPrey + probabilityPredator + probabilityMediator;
    }

}
