using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public static List<Animal> allAnimals = new List<Animal>();

    [Header("Animal Details")]
    public string animalName;
    public float wanderRange, wanderPause, wanderSpeed;
    public float runRange, runSpeed;
    public float idleTime = 2.5f;

    [Header("Spawn Conditions")]
    public SPAWNTIME spawnTime;
    public SPAWNWEATHER spawnWeather;
    public EMOTE state;

    [Header("Animal Behaviour")]
    public eSTATE firstState = eSTATE.WANDER;
    public string chaseTargets, fleeTargets;
    public Animal target;

    [HideInInspector] public SYS_FSM animalFSM;
    [HideInInspector] public SYS_Emote animalEmote;

    [HideInInspector] public SpriteRenderer animalSprite;
    [HideInInspector] public bool isDespawning, inParty;

    private void Start()
    {
        allAnimals.Add(this);
    }
}
