using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public static List<Animal> allAnimals = new List<Animal>();
    public static List<Animal> allAnimalLeaders = new List<Animal>();
    public static List<Animal> allAnimalPrey = new List<Animal>();

    [Header("Animal Details")]
    public string animalName;
    public ANIMALTYPE animalType;
    public float wanderRange, wanderSpeed;
    public float runRange, runSpeed;
    public float idleTime = 2.5f;

    [Header("Spawn Conditions")]
    public SPAWNTIME spawnTime;
    public SPAWNWEATHER spawnWeather;
    public EMOTE state;

    [Header("Animal Behaviour")]
    public eSTATE firstState = eSTATE.WANDER;
    public string chaseTargets, fleeTargets;
    public GameObject target;
    public Animal targetAsAnimal;
    public float followOffset = 1.5f;
    public bool isLeader, isHungry;
    public int herdNum;

    [Header("Class References")]
    [HideInInspector] public SYS_FSM animalFSM;
    [HideInInspector] public SYS_Emote animalEmote = new SYS_Emote();

    [HideInInspector] public SpriteRenderer animalSprite;
    [HideInInspector] public Animator animalAnimator;
    [HideInInspector] public FlipAnimal flipAnimal;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool isDespawning, isInParty, shouldFlee;
    [HideInInspector] public int preyPredatorInteraction = 0;
    [HideInInspector] public int maxHerdSize = 5;

    public int difficultyLevel = 0;

    public GameObject thoughtBubble;

    private void Start()
    {
        if (animalType == ANIMALTYPE.PREY) firstState = eSTATE.FOLLOWLEADER;
        flipAnimal.Init(this);
        animalFSM.Init(this);
        animalFSM.SetupStates();
        animalFSM.SwitchToState(firstState);
        //RegisterAnimalToParty();
    }

    private void OnEnable()
    {
        if (isLeader) allAnimalLeaders.Add(this);
        allAnimals.Add(this);

        flipAnimal = GetComponent<FlipAnimal>();
        rb = GetComponent<Rigidbody2D>();
        animalFSM = GetComponent(typeof(SYS_FSM)) as SYS_FSM;
    }

    public void RegisterAnimalToParty()
    {
        PlayerController.party.Add(this);
        isInParty = true;

        if (isLeader)
            RemoveLeader();

        StartCoroutine(animalEmote.EmoteShowBubble(thoughtBubble, 1.5f, EMOTE.HAPPY));
    }

    public void RegisterAnimalToArk()
    {
        GameplayManager.instance.animalsCollected++;

        PlayerController.party.Remove(this);
        isInParty = false;

        Destroy(gameObject);
    }

    public void RegisterAnimalAsLeader()
    {
        isLeader = true;
        allAnimalLeaders.Add(this);
        //int index = allAnimalLeaders.FindIndex(x => x.animalName == animalName);
        //allAnimalLeaders[index] = this;
        animalFSM.SetupStates();
    }

    public void MinigameIsStarting()
    {
        if (!animalFSM.currState.IsInteractable) return;
        animalFSM.active = false;
        rb.velocity = Vector2.zero;
    }

    public Animal SetNewLeader()
    {
        List<Animal> animalsOfSameType = GetAllSameAnimals();

        foreach (Animal a in animalsOfSameType)
            if (!a.isInParty && !a.isLeader)
            {
                a.isLeader = true;
                allAnimalLeaders.Add(a);
                return a;
            }
        return null;
    }

    public void MarkAnimalAsPrey(Animal a) => allAnimalPrey.Add(a);
    public void Eaten(Animal a) => allAnimalPrey.Remove(a);
    public bool AlreadyBeingStalked(Animal a) => allAnimalPrey.Contains(a);

    public List<Animal> GetAllSameAnimals() => allAnimals.FindAll(x => x.animalName == animalName);
    public List<Animal> GetAllSameAnimals(string givenName) => allAnimals.FindAll(x => x.animalName == givenName);
    public List<Animal> GetAllSameHerd() => allAnimals.FindAll(x => x.animalName == animalName && x.herdNum == herdNum);
    public Animal GetLeader() => allAnimalLeaders.Find(x => x.animalName == animalName && x.herdNum == herdNum);
    public int CountLeaders() => allAnimalLeaders.FindAll(x => x.animalName == animalName).Count;
    public List<Animal> GetLeaders() => allAnimalLeaders.FindAll(x => x.animalName == animalName);
    public int GetHerdSize(int herdIndex) => allAnimalLeaders.FindAll(x => x.animalName == animalName && x.herdNum == herdIndex).Count;
    public void SetTargetAs(Animal a) { target = a.gameObject; targetAsAnimal = a; }
    public void RemoveLeader() { allAnimalLeaders.Remove(this); isLeader = false; }
    public void ClearTarget() { target = null; targetAsAnimal = null; }
}
