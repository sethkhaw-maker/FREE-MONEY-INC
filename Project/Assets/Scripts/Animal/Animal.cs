using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public static List<Animal> allAnimals = new List<Animal>();

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

    [Header("Class References")]
    public SYS_FSM animalFSM;
    [HideInInspector] public SYS_Emote animalEmote = new SYS_Emote();

    [HideInInspector] public SpriteRenderer animalSprite;
    [HideInInspector] public Animator animalAnimator;
    [HideInInspector] public bool isDespawning, inParty;

    public int difficultyLevel = 0;

    public GameObject thoughtBubble;

    private void Start()
    {
        allAnimals.Add(this);
        animalFSM.Init(this);
        animalFSM.SetupStates();
        animalFSM.SwitchToState(firstState);
        //RegisterAnimalToParty();
    }

    public void RegisterAnimalToParty()
    {
        PlayerController.party.Add(this);
        inParty = true;

        StartCoroutine(animalEmote.EmoteShowBubble(thoughtBubble, 1.5f, EMOTE.HAPPY));
    }

    public void RegisterAnimalToArk()
    {
        PlayerController.party.Remove(this);
        inParty = false;

        Destroy(gameObject);
    }
}
