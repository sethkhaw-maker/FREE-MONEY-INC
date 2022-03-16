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

    private static int dayCount = 0;
    private bool dayIsEnding = false;

    //Minigame prefabs
    private GameObject minigameInstance;
    public GameObject minigamePrefab;

    //Game over prefabs
    private GameObject gameOverInstance;
    public GameObject gameOverPrefab;

    //Particle Prefabs
    public GameObject RainParticles;
    private ParticleSystem rainParticleSystem;
    private SpriteRenderer rainOverlay;
    public GameObject firefliesParticles;

    //Post Processing Objects
    public GameObject globalLight;
    public GameObject nightEffect;

    //In game variables
    [HideInInspector] public int animalsCollected = 0;                //Number of animals collected in this session

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

    void Start()
    {
        instance = this;
        rainChance = rainChanceBase;
        globalLight = GameObject.Find("Global Light 2D");
        rainParticleSystem = RainParticles.GetComponent<ParticleSystem>();
        rainOverlay = RainParticles.GetComponentInChildren<SpriteRenderer>();
        DisableNightVFX();
        oneDayRevolution = 10;
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

    //Update in-game timer
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
    }

    //Update in-game weather
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
    }

    //Chance of rain happening
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
    }

    //Call this to activate minigame
    public void InitMinigame()
    {
        gameState = GameState.MINIGAME;

        if (minigameInstance == null)
        {
            minigameInstance = Instantiate(minigamePrefab);
        }
    }

    //Call this to activate end of minigame
    public void EndMinigame(bool win)
    {
        StartCoroutine(DelayEndMinigame(win));
    }

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

    //Send the animals into the ark
    public void SendAnimalsIntoArk(GameObject ark)
    {
        //Send all into the ark (can probably do it to the 1st in party and recursive this)
        for (int i = 0; i < PlayerController.party.Count; i++)
        {
            PlayerController.party[i].target = ark;
            PlayerController.party[i].followOffset = 0.1f;
        }
    }

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
        globalLight.GetComponent<Light2D>().intensity = 1f;
        //nightEffect.SetActive(false);
        nightEffect.GetComponent<Animator>().SetBool("isNight", false);
        firefliesParticles.SetActive(false);
    }

    private void EnableNightVFX()
    {
        FindObjectOfType<AudioManager>()?.Play("Night SFX");
        globalLight.GetComponent<Light2D>().intensity = 0.8f;
        //nightEffect.SetActive(true);
        nightEffect.GetComponent<Animator>().SetBool("isNight", true);
        firefliesParticles.SetActive(true);
    }

    //////////////////////////////////////////////
    //Check win con
    private void CheckWin()
    {
        if (animalsCollected >= 10 && gameOverInstance == null && dayIsEnding == false)
        {
            //Day is progressing to next
            if (dayCount < 2)
            {
                dayIsEnding = true;
                dayCount++;
                
                fadeCanvas.SetInteger("fadeState", 1);

                Invoke("DelayReload", 1.5f);
            }
            else
            {
                //Replace with end cutscene
                gameOverInstance = Instantiate(gameOverPrefab);
                dayCount = 0;
            }
        }
    }

    private void DelayReload()
    {
        FindObjectOfType<SceneLoader>().ReloadScene();
    }
}
