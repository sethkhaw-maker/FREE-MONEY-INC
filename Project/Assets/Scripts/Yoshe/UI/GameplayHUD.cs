using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script runs 1 way dependent on GameplayManager
public class GameplayHUD : MonoBehaviour
{
    //Variables
    //UI stuff
    public GameObject clockhandImage;       //Rotate this clockhand Image
    public Text animalsCollectedText;       //Update this animals collected Text
    public Image weatherImage;              //Change this weather Image
    public Sprite clearWeatherImage;
    public Sprite rainyWeatherImage;

    //Ref
    private GameplayManager gameplayManager;

    void Start()
    {
        gameplayManager = FindObjectOfType<GameplayManager>();
    }

    void Update()
    {
        if (gameplayManager != null)
        {
            UpdateClockhand();
            UpdateAnimalsCollected();
            UpdateWeatherImage();
        }
    }

    void UpdateClockhand()
    {
        //Rotate the clockhand UI
        clockhandImage.transform.rotation = Quaternion.Euler(0, 0, -gameplayManager.clockTimer / gameplayManager.oneDayRevolution * 360);
    }

    //Update UI for animals collected
    public void UpdateAnimalsCollected()
    {
        animalsCollectedText.text = "Animals collected: " + gameplayManager.animalsCollected;
    }

    public void UpdateWeatherImage()
    {
        switch (gameplayManager.weatherState)
        {
            case GameplayManager.WeatherState.CLEAR:
                weatherImage.sprite = clearWeatherImage;
                break;
            case GameplayManager.WeatherState.RAINY:
                weatherImage.sprite = rainyWeatherImage;
                break;
            default:
                break;
        }
    }
}
