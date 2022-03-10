using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    //Internal timers
    private float spawnTimer;

    //Variables
    public int spawnCap = 20;
    private int spawnedQuantity = 0;

    public float spawnRadius = 20;
    public float congestionRadius = 3;

    public float spawnInterval = 2f;

    //Increase in spawn chance, lower is rarer
    private float preyIncrement = 80;
    private float predatorIncrement = 40;
    private float mediatorIncrement = 10;

    //List of animal prefabs that can be generated
    public GameObject[] preys;
    public GameObject[] predators;
    public GameObject[] mediators;

    public LayerMask animalLayer;
    public Vector3 spawnPoint;

    public int preySpawned;
    public int predatorSpawned;
    public int mediatorSpawned;

    private enum AnimalType
    {
        PREY,
        PREDATOR,
        MEDIATOR
    }
    private AnimalType animalType;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        //Decrease spawn timer over time
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && spawnedQuantity < spawnCap)
        {
            //Randomly select a spawn point
            spawnPoint = SelectSpawnPoint();

            //Check if spawn point is valid
            if (spawnPoint == Vector3.zero)
            {
                //Not valid, try again
                spawnTimer = 0;
            }
            else
            {
                //Valid, spawn and reset timer
                SelectAnimalType();
                spawnTimer = spawnInterval;
            }
            
        }
    }

    //Randomly select a spawn point from within a circle range
    private Vector3 SelectSpawnPoint()
    {
        //Select a random point in circle
        Vector3 randomPoint = Random.insideUnitCircle * spawnRadius;

        //Cast a circle cast to check for area congestion
        RaycastHit2D hit = Physics2D.CircleCast(randomPoint, congestionRadius, Vector2.up, 0, animalLayer);

        //Area is congested
        if (hit.collider != null)
        {
            return Vector3.zero;
        }
        //Area is not congested
        return randomPoint;
    }

    //Select a type of animal
    private void SelectAnimalType()
    {
        float rnd = Random.Range(0, AnimalSpawnRates.GetTotalProbability());

        //Prey selected
        if (rnd < AnimalSpawnRates.probabilityPrey)
        {
            //Spawn a prey
            SpawnAnimal(animalType = AnimalType.PREY);

            //Modify spawn rates
            ModifySpawnRates(AnimalType.PREY);
        }
        //Mediator selected
        else if (rnd > AnimalSpawnRates.probabilityPrey + AnimalSpawnRates.probabilityPredator)
        {
            //Spawn a mediator
            SpawnAnimal(animalType = AnimalType.MEDIATOR);

            //Modify spawn rates
            ModifySpawnRates(AnimalType.MEDIATOR);
        }
        //Predator selected
        else
        {
            //Spawn a predator
            SpawnAnimal(animalType = AnimalType.PREDATOR);

            //Modify spawn rates
            ModifySpawnRates(AnimalType.PREDATOR);
        }

    }

    //Spawn the chosen animal at selected spawn point
    private void SpawnAnimal(AnimalType typeOfAnimal)
    {
        int randomAnimal = 0;

        switch (typeOfAnimal)
        {
            case AnimalType.PREY:
                randomAnimal = Random.Range(0, preys.Length);
                Instantiate(preys[randomAnimal], gameObject.transform).transform.position = spawnPoint;

                //Chance of being a leader
                bool isLeader = Random.Range(0, 10) <= 2 ? true : false;

                if (isLeader)
                {
                    //Set this animal as the leader

                }

                break;
            case AnimalType.PREDATOR:
                randomAnimal = Random.Range(0, predators.Length);
                Instantiate(predators[randomAnimal], gameObject.transform).transform.position = spawnPoint;
                break;
            case AnimalType.MEDIATOR:
                randomAnimal = Random.Range(0, mediators.Length);
                Instantiate(mediators[randomAnimal], gameObject.transform).transform.position = spawnPoint;
                break;
            default:
                break;
        }
        
        spawnedQuantity++;
    }

    //Modify the probability of animal spawn rates
    private void ModifySpawnRates(AnimalType spawnType)
    {
        switch (spawnType)
        {
            case AnimalType.PREY:
                AnimalSpawnRates.probabilityPredator += predatorIncrement;
                AnimalSpawnRates.probabilityMediator += mediatorIncrement;
                preySpawned++;
                break;
            case AnimalType.PREDATOR:
                AnimalSpawnRates.probabilityPrey += preyIncrement;
                AnimalSpawnRates.probabilityMediator += mediatorIncrement;
                predatorSpawned++;
                break;
            case AnimalType.MEDIATOR:
                AnimalSpawnRates.probabilityPrey += preyIncrement;
                AnimalSpawnRates.probabilityPredator += predatorIncrement;
                mediatorSpawned++;
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
