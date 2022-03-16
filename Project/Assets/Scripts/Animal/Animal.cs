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

    private void Start()
    {
        if (animalType == ANIMALTYPE.PREY) firstState = eSTATE.FOLLOWLEADER;
        flipAnimal.Init(this);
        animalEmote.Init(this);
        animalFSM.Init(this);
        animalFSM.SetupStates();
        animalFSM.SwitchToState(firstState);
    }

    private void OnEnable()
    {
        if (isLeader) allAnimalLeaders.Add(this);
        allAnimals.Add(this);

        flipAnimal = GetComponent<FlipAnimal>();
        rb = GetComponent<Rigidbody2D>();
        animalFSM = GetComponent(typeof(SYS_FSM)) as SYS_FSM;
        animalEmote.thoughtBubble = transform.GetChild(0).gameObject;
    }

    public void RegisterAnimalToParty()
    {
        PlayerController.party.Add(this);
        isInParty = true;

        StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.HAPPY));
        animalFSM.active = true;
    }

    public void MinigameFailed()
    {
        switch (animalType)
        {
            case ANIMALTYPE.PREY:
                StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.SCARED, isPartyInteraction: true));
                PlayShakeEmote();
                break;
            case ANIMALTYPE.PREDATOR:
                StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.ANGRY, isPartyInteraction: true));
                break;
            case ANIMALTYPE.MEDIATOR:
                StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.CONFUSED, isPartyInteraction: true));
                break;
        }
    }

    public void RegisterAnimalToArk()
    {
        GameplayManager.instance.animalsCollected++;

        PlayerController.party.Remove(this);
        allAnimals.Remove(this);
        isInParty = false;

        FindObjectOfType<AnimalGenerator>().spawnedQuantity--;
        Destroy(gameObject);
    }

    public void RegisterAnimalAsLeader()
    {
        isLeader = true;
        allAnimalLeaders.Add(this);
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
            if (a.herdNum == herdNum && !a.isInParty && !a.isLeader)
            {
                //Debug.Log(a.name + " | is becoming a leader. herdnum: " + herdNum);
                a.isLeader = true;
                allAnimalLeaders.Add(a);
                return a;
            }
        return null;
    }

    public void SetHerdFollowNewLeader()
    {
        foreach (Animal a in GetAllSameHerd())
        {
            if (!a.animalFSM.currState.IsInteractable) continue;
            a.targetAsAnimal = GetLeader();
            a.target = a.targetAsAnimal.gameObject;
        }
    }

    public void MarkAnimalAsPrey(Animal a) => allAnimalPrey.Add(a);
    public void Eaten(Animal a) => allAnimalPrey.Remove(a);
    public bool AlreadyBeingStalked(Animal a) => allAnimalPrey.Contains(a);

    public void RemoveLeader() { allAnimalLeaders.Remove(this); isLeader = false; }
    public void SetTargetAs(Animal a) { target = a.gameObject; targetAsAnimal = a; }
    public void ClearTarget() { target = null; targetAsAnimal = null; }

    public List<Animal> GetAllAnimalsOfType(ANIMALTYPE type) => allAnimals.FindAll(x => x.animalType == type);
    public List<Animal> GetAllSameAnimals() => allAnimals.FindAll(x => x.animalName == animalName);
    public List<Animal> GetAllSameAnimals(string givenName) => allAnimals.FindAll(x => x.animalName == givenName);
    public List<Animal> GetAllSameHerd() => allAnimals.FindAll(x => x.animalName == animalName && x.herdNum == herdNum);
    public List<Animal> GetLeaders() => allAnimalLeaders.FindAll(x => x.animalName == animalName);
    public Animal GetLeader() => allAnimalLeaders.Find(x => x.animalName == animalName && x.herdNum == herdNum);
    public int CountLeaders() => allAnimalLeaders.FindAll(x => x.animalName == animalName).Count;
    public int GetHerdSize(int herdIndex) => allAnimals.FindAll(x => x.animalName == animalName && x.herdNum == herdIndex).Count;

    public void PlayShakeEmote() { if (!animalEmote.isShaking) StartCoroutine(animalEmote.ShakeCoroutine()); }
    public void PlayPartyInteraction()
    {
        switch (animalType)
        {
            case ANIMALTYPE.PREY:
                StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.SCARED, isPartyInteraction: true));
                PlayShakeEmote();
                break;
            case ANIMALTYPE.PREDATOR:
                StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.HUNGRY_PREDATOR, isPartyInteraction: true));
                break;
            case ANIMALTYPE.MEDIATOR:
                StartCoroutine(animalEmote.EmoteShowBubble(EMOTE.CONFUSED, isPartyInteraction: true));
                break;
        }
    }
}
