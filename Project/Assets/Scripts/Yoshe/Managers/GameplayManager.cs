using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script runs the world logic for day/night cycle and weather system
public class GameplayManager : MonoBehaviour
{
    //Enums
    public enum GameState { PLAYING, MINIGAME, PAUSED }
    public enum ClockState { DAY, NIGHT }
    public enum WeatherState { CLEAR, RAINY }


    //Variables
    public static GameplayManager instance;
    public static GameState gameState;

    //Minigame prefabs
    private GameObject minigameInstance;
    public GameObject minigamePrefab;

    //Game over prefabs
    private GameObject gameOverInstance;
    public GameObject gameOverPrefab;

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

    void Start()
    {
        instance = this;
        rainChance = rainChanceBase;
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
        }
        //Set to night
        else if (clockTimer >= (oneDayRevolution/2))
        {
            clockState = ClockState.NIGHT;
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

    //////////////////////////////////////////////
    //Check win con
    private void CheckWin()
    {
        if (animalsCollected >= 10 && gameOverInstance == null)
        {
            gameOverInstance = Instantiate(gameOverPrefab);
        }
    }

}
