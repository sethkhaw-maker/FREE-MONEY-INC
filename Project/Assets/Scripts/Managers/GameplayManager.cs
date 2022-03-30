using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

//This script runs the world logic for day/night cycle and weather system
public class GameplayManager : MonoBehaviour
{
    //Enums
    public enum GameState { PLAYING, MINIGAME, SCOPING, PAUSED }
    public enum ClockState { DAY, NIGHT }
    public enum WeatherState { CLEAR, RAINY }

    //Variables
    public static GameplayManager instance;
    public static GameState gameState;

    public static int dayCount = 0;
    private bool dayIsEnding = false;

    //Minigame prefabs
    private GameObject minigameInstance;
    public GameObject minigamePrefab;

    //Game over prefabs
    //private GameObject gameOverInstance;
    //public GameObject gameOverPrefab;

    //Particle Prefabs
    public GameObject RainParticles;
    private ParticleSystem rainParticleSystem;
    private SpriteRenderer rainOverlay;
    public GameObject firefliesParticles;

    //Post Processing Objects
    public GameObject globalLight;
    public GameObject nightEffect;

    //In game variables
    //[HideInInspector] public int animalsCollected = 0;                //Number of animals collected in this session
    [HideInInspector] public string[] animalsToCollect = new string[3];
    [HideInInspector] public int[] animalsToCollect_Required = new int[3];
    [HideInInspector] public int[] animalsToCollect_Current = new int[3];

    //Game timers
    [HideInInspector] public float clockTimer;
    private ClockState clockState = ClockState.DAY;
    [HideInInspector] public float oneDayRevolution = 180f;          //Seconds before a full day is over

    //Weather stuffs
    [Header("Rain Variables")]
    [HideInInspector] public WeatherState weatherState = WeatherState.CLEAR;

    private float weatherTimer;
    [SerializeField] private float rainCheckInterval = 15f;     //Intervals for rain check during clear weather
    private int rainChance;                                     //Out of 100
    [SerializeField] private int rainChanceBase = 40;           //Base chance of rain
    [SerializeField] private int rainChanceIncrement = 15;      //Increased chance per failed rain
    [SerializeField] private float rainDuration = 30f;          //Duration of rain

    //Fade transition
    public Animator fadeCanvas;
    public Animator cloudCanvas;

    //Environment prefab
    public GameObject[] daysEnvironmentVariants;

    private void Awake()
    {
        Animal.ResetStaticObjs();
    }

    void Start()
    {
        instance = this;
        rainChance = rainChanceBase;
        SetAnimalQuota();
        globalLight = GameObject.Find("Global Light 2D");
        rainParticleSystem = RainParticles.GetComponent<ParticleSystem>();
        rainOverlay = RainParticles.GetComponentInChildren<SpriteRenderer>();
        DisableNightVFX();
        oneDayRevolution = 90f;

        daysEnvironmentVariants[dayCount].SetActive(true);
    }

    private void OnEnable()
    {
        Animal.UpdateAnimalCount += UpdateAnimalCount;
    }

    private void OnDisable()
    {
        Animal.UpdateAnimalCount -= UpdateAnimalCount;
    }

    void Update()
    {
        if (gameState == GameState.PLAYING)
        {
            UpdateGameTime();
            UpdateWeatherTime();
        }

        CheckWin();
    }


    public void UpdateGameTime()
    {
        //Increase timer by clockspeed, 360 for 1 revolution
        clockTimer += Time.deltaTime;


        //Reset to day
        if (clockTimer >= oneDayRevolution)
        {
            //Reset to 0
            clockTimer = 0;
            clockState = ClockState.DAY;
            DisableNightVFX();
        }
        //Set to night
        else if (clockTimer >= (oneDayRevolution / 2))
        {
            clockState = ClockState.NIGHT;
            EnableNightVFX();

        }
    }         //Update in-game timer
    public void UpdateWeatherTime()
    {
        if (weatherState == WeatherState.CLEAR)
        {
            //Increase weather timer by time during clear weather
            weatherTimer += Time.deltaTime;

            //Check chance to rain every 10s starting from 30s
            if (weatherTimer >= 15 + rainCheckInterval)
            {
                if (Rain())
                {
                    //Rain occurred, set weather to rain
                    weatherState = WeatherState.RAINY;
                    weatherTimer = rainDuration;
                    rainParticleSystem.Play();
                    FindObjectOfType<AudioManager>()?.Play("Rain SFX");
                    StartCoroutine(RainOverlayFadeOut());
                }
                else
                {
                    //Failed to rain, soft reset timer to interval check
                    weatherTimer = 15;
                }
            }

        }
        else
        {
            //Decrease weather time by time during rainy weather
            weatherTimer -= Time.deltaTime;

            //Change weather back to clear after crossing rain duration
            if (weatherTimer <= 0)
            {
                weatherState = WeatherState.CLEAR;
                weatherTimer = 0;
                rainParticleSystem.Stop();
                StartCoroutine(RainOverlayFadeIn());
            }
        }
    }      //Update in-game weather
    public bool Rain()
    {
        int rnd = Random.Range(0, 100);
        //Succeeded in raining
        if (rnd <= rainChance)
        {
            rainChance = rainChanceBase;
            return true;
        }
        //Failed to rain
        else
        {
            //Increase the chance of raining on next check
            rainChance += rainChanceIncrement;
            return false;
        }
    }                   //Chance of rain happening
    public void InitMinigame()
    {
        gameState = GameState.MINIGAME;

        if (minigameInstance == null)
        {
            minigameInstance = Instantiate(minigamePrefab);
        }
    }           //Call this to activate minigame




    private IEnumerator DelayEndMinigame(bool win)
    {
        //Add a delay for atas
        yield return new WaitForSeconds(0.5f);

        //Destroy minigame instance
        if (minigameInstance != null)
        {
            Destroy(minigameInstance);
        }

        //Resume game
        gameState = GameState.PLAYING;

        //Add animal to party if minigame won
        if (win)
        {
            PlayerController.instance.targetAnimal.RegisterAnimalToParty();
        }
        //Otherwise just let it go
        else
        {
            PlayerController.instance.targetAnimal.MinigameFailed();
        }

        //Reset target animal from player
        PlayerController.instance.targetAnimal = null;
    }

    
    public void SendAnimalsIntoArk(GameObject ark)
    {
        //Send all into the ark (can probably do it to the 1st in party and recursive this)
        for (int i = 0; i < PlayerController.instance.party.Count; i++)
        {
            PlayerController.instance.party[i].target = ark;
            PlayerController.instance.party[i].followOffset = 0.1f;
        }
    } //Send the animals into the ark

    //Fade In and Fade Out Effects for rain overlay
    private IEnumerator RainOverlayFadeIn()
    {
        float alphaValue = rainOverlay.color.a;
        Color temp = rainOverlay.color;

        //When it isn't transparent -
        while (rainOverlay.color.a > 0)
        {
            alphaValue -= 0.01f;
            temp.a = alphaValue;
            rainOverlay.color = temp;

            yield return new WaitForSeconds(0.05f);
        }
    }
    private IEnumerator RainOverlayFadeOut()
    {
        float alphaValue = rainOverlay.color.a;
        Color temp = rainOverlay.color;

        //When it isn't transparent -
        while (rainOverlay.color.a < 0.35)
        {
            alphaValue += 0.01f;
            temp.a = alphaValue;
            rainOverlay.color = temp;

            yield return new WaitForSeconds(0.05f);
        }

    }

    private void DisableNightVFX()
    {
        //globalLight.GetComponent<Light2D>().intensity = 1f;
        //nightEffect.SetActive(false);
        nightEffect.GetComponent<Animator>().SetBool("isNight", false);
        firefliesParticles.SetActive(false);
    }
    private void EnableNightVFX()
    {
        //FindObjectOfType<AudioManager>()?.Play("Night SFX");
        //print("break test");
        //globalLight.GetComponent<Light2D>().intensity = 0.8f;
        //nightEffect.SetActive(true);
        nightEffect.GetComponent<Animator>().SetBool("isNight", true);
        firefliesParticles.SetActive(true);
    }

    private void SetAnimalQuota()
    {
        DecideWhichAnimalsToBeRecruited();
        DecideHowManyAnimalsToGet();

        void DecideHowManyAnimalsToGet()
        {
            ResetAnimalCurrent();
            switch (dayCount)
            {
                case 0: SetToAnimalRequired(3, 2, 1); break;
                case 1: SetToAnimalRequired(5, 3, 1); break;
                case 2: SetToAnimalRequired(5, 5, 2); break;
            }

            void SetToAnimalRequired(int prey, int predator, int mediator)
            {
                animalsToCollect_Required[0] = prey;
                animalsToCollect_Required[1] = predator;
                animalsToCollect_Required[2] = mediator;
            }

            void ResetAnimalCurrent()
            {
                animalsToCollect_Current[0] = 0;
                animalsToCollect_Current[1] = 0;
                animalsToCollect_Current[2] = 0;
            }
        }
        void DecideWhichAnimalsToBeRecruited()
        {
            for (int i = 0; i < animalsToCollect.Length; i++)
            {
                if (i == 0)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                    {
                        animalsToCollect[i] = "Zebra";
                    }
                    else if (rnd == 1)
                    {
                        animalsToCollect[i] = "Giraffe";
                    }
                    else if (rnd == 2)
                    {
                        animalsToCollect[i] = "Buffalo";
                    }
                }
                else if (i == 1)
                {
                    int rnd = Random.Range(0, 3);
                    if (rnd == 0)
                    {
                        animalsToCollect[i] = "Lion";
                    }
                    else if (rnd == 1)
                    {
                        animalsToCollect[i] = "Tiger";
                    }
                    else if (rnd == 2)
                    {
                        animalsToCollect[i] = "Hyena";
                    }
                }
                else if (i == 2)
                {
                    int rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        animalsToCollect[i] = "Elephant";
                    }
                    else if (rnd == 1)
                    {
                        animalsToCollect[i] = "Rhino";
                    }
                }
            }
        }
    }

    bool CollectedRequiredAnimals()
    {
        for (int i = 0; i < animalsToCollect_Current.Length; i++)
            if (animalsToCollect_Required[i] > animalsToCollect_Current[i]) return false;
        return true;
    }
    void UpdateAnimalCount(string name)
    {
        int i = -1;
        switch (name)
        {
            case "Zebra": case "Giraffe": case "Buffalo": i = 0; break;
            case "Tiger": case "Lion": case "Hyena": i = 1; break;
            case "Rhino": case "Elephant": i = 2; break;
        }
        if (i == -1) return;
        if (name == animalsToCollect[i]) animalsToCollect_Current[i]++;
    }
    void EndDay()
    {
        dayIsEnding = true;

        //Day is progressing to next
        if (dayCount < 2)
        {
            dayCount++;

            cloudCanvas.SetBool("endDay", true);
            Invoke("FadeDay", 2.583f);
        }
        else
        {
            //Replace with end cutscene
            dayCount = 0;
            fadeCanvas.SetInteger("fadeState", 2);
            FindObjectOfType<SceneLoader>().LoadScene(5);
        }
    }
    private void FadeDay()
    {
        fadeCanvas.SetInteger("fadeState", 2);
        Invoke("DelayReload", 1.5f);
    }

    private void CheckWin() { if (!dayIsEnding && CollectedRequiredAnimals()) EndDay(); }   // check win condition
    private void DelayReload() => FindObjectOfType<SceneLoader>().ReloadScene();
    public void EndMinigame(bool win) => StartCoroutine(DelayEndMinigame(win));             //Call this to activate end of minigame
}
