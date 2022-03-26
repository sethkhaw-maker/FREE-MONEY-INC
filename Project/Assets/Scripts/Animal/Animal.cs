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
    public bool isTutorialAnimal = false;

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
    [HideInInspector] public GameObject wanderPoint, jail, tutorialArrow;

    public int difficultyLevel = 0;
    public static event MESSAGING_string UpdateAnimalCount;

    public static void ResetStaticObjs()
    {
        allAnimals.Clear();
        allAnimalPrey.Clear();
        allAnimalLeaders.Clear();
    }

    private void Start()
    {
        if (animalType == ANIMALTYPE.PREY && !isTutorialAnimal) firstState = eSTATE.FOLLOWLEADER;
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
        animalSprite = GetComponent<SpriteRenderer>();
        if (isTutorialAnimal) { wanderPoint = GameObject.Find("WanderPoint_" + animalName); jail = GameObject.Find("ANIMALJAIL_" + animalName); tutorialArrow = transform.Find("Tutorial Arrow").gameObject; }
    }

    public void RegisterAnimalToParty()
    {
        PlayerController.instance.party.Add(this);
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
        UpdateAnimalCount?.Invoke(animalName);
        //if (animalType == ANIMALTYPE.PREY) GameplayManager.instance.preysCollected++;
        //else if (animalType == ANIMALTYPE.PREDATOR) GameplayManager.instance.predatorsCollected++;
        //else if (animalType == ANIMALTYPE.MEDIATOR) GameplayManager.instance.mediatorsCollected++;

        PlayerController.instance.party.Remove(this);
        allAnimals.Remove(this);
        isInParty = false;

        AnimalGenerator aniGen = FindObjectOfType<AnimalGenerator>();

        if (aniGen != null) aniGen.spawnedQuantity--;

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
        List<Animal> animalList = GetAllSameHerd();

        if (animalList.Count != 0)
        foreach (Animal a in animalList)
        {
            if (!a.animalFSM.currState.IsInteractable) continue;
            a.targetAsAnimal = GetLeader();
            a.target = a.targetAsAnimal.gameObject;
        }
    }

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

    public void Despawn()
    {
        StartCoroutine(DespawnAnimal());

        IEnumerator DespawnAnimal()
        {
            float despawnTime = 1f;
            float timer = 0f;

            Color oCol = animalSprite.color;
            Color transp = oCol;
            oCol.a = 0;

            while (timer <= despawnTime)
            {
                timer += Time.deltaTime;
                animalSprite.color = Color.Lerp(oCol, transp, (despawnTime - timer)/despawnTime);
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }

    public void MarkAnimalAsPrey(Animal a) => allAnimalPrey.Add(a);
    public bool AlreadyBeingStalked(Animal a) => allAnimalPrey.Contains(a);

    public void RemoveLeader() { allAnimalLeaders.Remove(this); isLeader = false; }
    public void SetTargetAs(Animal a) { target = a.gameObject; targetAsAnimal = a; }
    public void ClearTarget() { target = null; targetAsAnimal = null; }
    public void Eaten(Animal a) { allAnimalPrey.Remove(a); Despawn(); }

    public List<Animal> GetAllAnimalsOfType(ANIMALTYPE type) => allAnimals.FindAll(x => x.animalType == type);
    public List<Animal> GetAllSameAnimals() => allAnimals.FindAll(x => x.animalName == animalName);
    public List<Animal> GetAllSameAnimals(string givenName) => allAnimals.FindAll(x => x.animalName == givenName);
    public List<Animal> GetAllSameHerd() => allAnimals.FindAll(x => x.animalName == animalName && x.herdNum == herdNum);
    public List<Animal> GetLeaders() => allAnimalLeaders.FindAll(x => x.animalName == animalName);
    public Animal GetLeader() => allAnimalLeaders.Find(x => x.animalName == animalName && x.herdNum == herdNum);
    public int CountLeaders() => allAnimalLeaders.FindAll(x => x.animalName == animalName).Count;
    public int GetHerdSize(int herdIndex) => allAnimals.FindAll(x => x.animalName == animalName && x.herdNum == herdIndex).Count;

    public void PlayShakeEmote() { if (!animalEmote.isShaking) StartCoroutine(animalEmote.ShakeCoroutine()); }

}
