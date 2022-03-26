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
    public Image weatherImage;              //Change this weather Image
    public Sprite clearWeatherImage;
    public Sprite rainyWeatherImage;

    public Text dayIntroText;               //The disclaimer for the day during fade

    public Text objectiveOneText;             //Update objective 1 text
    public Text objectiveTwoText;             //Update objective 2 text
    public Text objectiveThreeText;           //Update objective 3 text

    public Image objectiveOnePortrait;
    public Image objectiveTwoPortrait;
    public Image objectiveThreePortrait;

    [Header("Animal Portraits")]
    public Sprite[] animalPortraits;

    //Ref
    private GameplayManager gameplayManager;
    private TUT_GameManager tutorialGameManager;

    void Start()
    {
        gameplayManager = FindObjectOfType<GameplayManager>();
        tutorialGameManager = FindObjectOfType<TUT_GameManager>();
        SetAnimalPortraits();
        dayIntroText.text = "DAY " + (tutorialGameManager != null ? 0 : (GameplayManager.dayCount + 1));
    }

    void Update()
    {
        if (gameplayManager != null)
        {
            UpdateClockhand();
            UpdateAnimalsCollected();
            UpdateWeatherImage();
        }
        if (tutorialGameManager != null)
        {
            UpdateAnimalsCollected();
        }
    }

    void SetAnimalPortraits()
    {
        string[] animalsToCollect = new string[3];

        if (gameplayManager != null) animalsToCollect = gameplayManager.animalsToCollect;
        if (tutorialGameManager != null) animalsToCollect = tutorialGameManager.animalsToCollect;

        if (animalsToCollect != null) ChangePortraits(animalsToCollect);
    }

    void ChangePortraits(string[] animalsToCollect)
    {
        switch (animalsToCollect[0])
        {
            case "Zebra":
                objectiveOnePortrait.sprite = animalPortraits[0];
                break;
            case "Giraffe":
                objectiveOnePortrait.sprite = animalPortraits[1];
                break;
            case "Buffalo":
                objectiveOnePortrait.sprite = animalPortraits[2];
                break;
            default:
                break;
        }

        switch (animalsToCollect[1])
        {
            case "Lion":
                objectiveTwoPortrait.sprite = animalPortraits[3];
                break;
            case "Tiger":
                objectiveTwoPortrait.sprite = animalPortraits[4];
                break;
            case "Hyena":
                objectiveTwoPortrait.sprite = animalPortraits[5];
                break;
            default:
                break;
        }

        switch (animalsToCollect[2])
        {
            case "Elephant":
                objectiveThreePortrait.sprite = animalPortraits[6];
                break;
            case "Rhino":
                objectiveThreePortrait.sprite = animalPortraits[7];
                break;
            default:
                break;
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
        if (gameplayManager != null)
        {
            objectiveOneText.text = ": " + gameplayManager.preysCollected + "/" + gameplayManager.preysRequired;
            objectiveTwoText.text = ": " + gameplayManager.predatorsCollected + "/" + gameplayManager.predatorsRequired;
            objectiveThreeText.text = ": " + gameplayManager.mediatorsCollected + "/" + gameplayManager.mediatorsRequired;
        }
        else if (tutorialGameManager != null)
        {
            objectiveOneText.text   = ": " + tutorialGameManager.animalsToCollect_Current[0] + "/" + tutorialGameManager.animalsToCollect_Required[0];
            objectiveTwoText.text   = ": " + tutorialGameManager.animalsToCollect_Current[1] + "/" + tutorialGameManager.animalsToCollect_Required[1];
            objectiveThreeText.text = ": " + tutorialGameManager.animalsToCollect_Current[2] + "/" + tutorialGameManager.animalsToCollect_Required[2];
        }
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
