using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public static List<Animal> allAnimals = new List<Animal>();
    public static List<Animal> allAnimalLeaders = new List<Animal>();

    [Header("Animal Details")]
    public string animalName;
    public float wanderRange, wanderSpeed;
    public float runRange, runSpeed;
    public float idleTime = 2.5f;
    public bool isLeader;

    [Header("Spawn Conditions")]
    public SPAWNTIME spawnTime;
    public SPAWNWEATHER spawnWeather;
    public EMOTE state;

    [Header("Animal Behaviour")]
    public eSTATE firstState = eSTATE.WANDER;
    public string chaseTargets, fleeTargets;
    public GameObject target;
    public Animal targetAsAnimal;

    [Header("Class References")]
    public SYS_FSM animalFSM;
    [HideInInspector] public SYS_Emote animalEmote;

    [HideInInspector] public SpriteRenderer animalSprite;
    [HideInInspector] public Animator animalAnimator;
    [HideInInspector] public bool isDespawning, isInParty;

    private void OnEnable()
    {
        if (isLeader) allAnimalLeaders.Add(this);
        allAnimals.Add(this);
    }

    private void Start()
    {
        animalFSM.Init(this);
        animalFSM.SetupStates();
        animalFSM.SwitchToState(firstState);
        //RegisterAnimalToParty();
    }

    public void RegisterAnimalToParty()
    {
        PlayerController.party.Add(this);
        isInParty = true;
    }

    public void RegisterAnimalAsLeader()
    {
        isLeader = true;
        int index = allAnimalLeaders.FindIndex(x => x.animalName == animalName);
        allAnimalLeaders[index] = this;
        animalFSM.SetupStates();
    }

    public List<Animal> GetAllSameAnimals() => allAnimals.FindAll(x => x.animalName == animalName);
    public Animal GetLeader() => allAnimalLeaders.Find(x => x.animalName == animalName);
}
