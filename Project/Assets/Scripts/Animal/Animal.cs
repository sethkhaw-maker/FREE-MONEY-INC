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
    public bool isLeader;

    [Header("Class References")]
    public SYS_FSM animalFSM;
    [HideInInspector] public SYS_Emote animalEmote = new SYS_Emote();

    [HideInInspector] public SpriteRenderer animalSprite;
    [HideInInspector] public Animator animalAnimator;
    [HideInInspector] public FlipAnimal flipAnimal;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool isDespawning, isInParty, shouldFlee;

    public int difficultyLevel = 0;

    public GameObject thoughtBubble;

    private void Start()
    {
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
    }

    public void RegisterAnimalToParty()
    {
        PlayerController.party.Add(this);
        isInParty = true;

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
        int index = allAnimalLeaders.FindIndex(x => x.animalName == animalName);
        allAnimalLeaders[index] = this;
        animalFSM.SetupStates();
    }

    public void MinigameIsStarting()
    {
        if (!animalFSM.currState.isInteractable) return;
        animalFSM.active = false;
        rb.velocity = Vector2.zero;
    }

    public List<Animal> GetAllSameAnimals() => allAnimals.FindAll(x => x.animalName == animalName);
    public List<Animal> GetAllSameAnimals(string givenName) => allAnimals.FindAll(x => x.animalName == givenName);
    public Animal GetLeader() => allAnimalLeaders.Find(x => x.animalName == animalName);
    public void SetTargetAs(Animal a) { target = a.gameObject; targetAsAnimal = a; }

}
