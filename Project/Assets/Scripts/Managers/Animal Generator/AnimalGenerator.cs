using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    public GameObject animalPool;

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

    int animalCount = 0;

    private ANIMALTYPE animalType;

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

    
    private void SelectAnimalType() //Select a type of animal
    {
        float rnd = Random.Range(0, AnimalSpawnRates.GetTotalProbability());

        if (rnd < AnimalSpawnRates.probabilityPrey) //Prey selected
        {
            SpawnAnimal(animalType = ANIMALTYPE.PREY);  //Spawn a prey
            ModifySpawnRates(ANIMALTYPE.PREY);          //Modify spawn rates
        }
        else if (rnd > AnimalSpawnRates.probabilityPrey + AnimalSpawnRates.probabilityPredator) //Mediator selected
        {
            SpawnAnimal(animalType = ANIMALTYPE.MEDIATOR);  //Spawn a mediator
            ModifySpawnRates(ANIMALTYPE.MEDIATOR);          //Modify spawn rates
        }
        else //Predator selected
        {
            SpawnAnimal(animalType = ANIMALTYPE.PREDATOR);  //Spawn a predator
            ModifySpawnRates(ANIMALTYPE.PREDATOR);          //Modify spawn rates
        }
    }

    private void SpawnAnimal(ANIMALTYPE typeOfAnimal) //Spawn the chosen animal at selected spawn point
    {
        int randomAnimal = 0;
        GameObject animalGO = null;

        switch (typeOfAnimal)
        {
            case ANIMALTYPE.PREY:
                randomAnimal = Random.Range(0, preys.Length);
                animalGO = Instantiate(preys[randomAnimal], spawnPoint, Quaternion.identity);
                LeaderOrPrey(animalGO);
                break;
            case ANIMALTYPE.PREDATOR:
                randomAnimal = Random.Range(0, predators.Length);
                animalGO = Instantiate(predators[randomAnimal], spawnPoint, Quaternion.identity);
                animalGO.name = "#" + animalCount++ + " | "+ animalGO.GetComponent<Animal>().animalName;
                break;
            case ANIMALTYPE.MEDIATOR:
                randomAnimal = Random.Range(0, mediators.Length);
                animalGO = Instantiate(mediators[randomAnimal], spawnPoint, Quaternion.identity);
                animalGO.name = "#" + animalCount++ + " | " + animalGO.GetComponent<Animal>().animalName;
                break;
            default:
                break;
        }
        animalGO.transform.parent = animalPool.transform;
        spawnedQuantity++;
    }

    //Modify the probability of animal spawn rates
    private void ModifySpawnRates(ANIMALTYPE spawnType)
    {
        switch (spawnType)
        {
            case ANIMALTYPE.PREY:
                AnimalSpawnRates.probabilityPredator += predatorIncrement;
                AnimalSpawnRates.probabilityMediator += mediatorIncrement;
                preySpawned++;
                break;
            case ANIMALTYPE.PREDATOR:
                AnimalSpawnRates.probabilityPrey += preyIncrement;
                AnimalSpawnRates.probabilityMediator += mediatorIncrement;
                predatorSpawned++;
                break;
            case ANIMALTYPE.MEDIATOR:
                AnimalSpawnRates.probabilityPrey += preyIncrement;
                AnimalSpawnRates.probabilityPredator += predatorIncrement;
                mediatorSpawned++;
                break;
            default:
                break;
        }
    }

    void LeaderOrPrey(GameObject go)
    {
        Animal prey = go.GetComponent<Animal>();

        // ensure that there is always 1 leader
        if (prey.GetLeaders().Count == 0) { prey.isLeader = true; }
        else prey.isLeader = Random.Range(0, 15) <= 2 ? true : false;

        // for generic prey, find closest leader & add to herd if there is space in herd.
        if (!prey.isLeader) 
        {
            List<Animal> allLeadersOfPreyType = prey.GetLeaders();
            float closestDist = 99999f;
            Animal closestLeader = null;
            foreach (Animal leader in allLeadersOfPreyType)
            {
                float currDist = Vector3.Distance(leader.transform.position, prey.transform.position);
                //Debug.Log("herdNum: " + leader.herdNum + " | getHerdSize: " + prey.GetHerdSize(leader.herdNum));
                if (currDist < closestDist && prey.GetHerdSize(leader.herdNum) < prey.maxHerdSize)
                {
                    closestDist = currDist;
                    closestLeader = leader;
                }
            }

            if (closestLeader != null) { prey.herdNum = closestLeader.herdNum; prey.SetTargetAs(closestLeader); }
            else prey.isLeader = true; // assuming all leaders checked + no space in herd, make new herd
        }

        // placed after generic prey check to reuse code if prey is made leader due to full herds.
        if (prey.isLeader)  
        {
            prey.RegisterAnimalAsLeader();
            prey.herdNum = prey.CountLeaders();
        }

        go.name = "#" + animalCount++ + " | " + prey.animalName + " | herdNum: " + prey.herdNum + (prey.isLeader ? " | Leader" : "");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
