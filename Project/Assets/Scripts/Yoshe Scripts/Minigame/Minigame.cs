using UnityEngine;
using UnityEngine.UI;

//The real time scrolling minigame controller

public class Minigame : MonoBehaviour
{
    //UI Variables
    private float minigameWidth;         //Technical stuff, allows free sizing of Minigame width
    private float hitboxPercentage;     //Percentage of the hitbox will cover on minigame screen
    private float hitBoxSize;
    private float hitboxOffset;

    private float finalOffset;

    public RectTransform hitboxDisplay; //The red hitbox area
    private Slider hitSlider;           //The slider which moves back and forth
    private float sliderValue;

    //Gameplay Variables
    private bool isGaming = true;

    [Range(0.25f,1.75f)]
    public float speedModifier = 1;

    //[Range(0f, 100f)]
    //public int minHitboxPercentage = 20;      //Minimum size of hitbox in percentage

    //[Range(0f, 100f)]
    //public int maxHitboxPercentage = 100;     //Maximum size of hitbox in percentage

    public enum DifficultyLevel
    {
        EASY,
        NORMAL,
        HARD
    }
    public DifficultyLevel difficultyLevel;

    public Sprite[] soundWaves;

    void Start()
    {
        //Assign the slider from child variable
        hitSlider = GetComponentInChildren<Slider>();

        //Set the minigame difficulty
        SetDifficulty();

        //Generate the starting hitbox
        GenerateHitbox();
    }

    private void Update()
    {
        if (isGaming)
        {
            //Update the slider to move in real time
            UpdateSlider();
        }
    }

    //Slider which moves left and right
    private void UpdateSlider()
    {
        sliderValue = Mathf.PingPong(Time.time * speedModifier, 1);
        hitSlider.value = sliderValue;
    }

    //Set the difficulty of the minigame
    public void SetDifficulty()
    {
        int animalDifficulty = PlayerController.instance.targetAnimal.difficultyLevel;

        switch (animalDifficulty)
        {
            case 0:
                difficultyLevel = DifficultyLevel.EASY;
                break;
            case 1:
                difficultyLevel = DifficultyLevel.NORMAL;
                break;
            case 2:
                difficultyLevel = DifficultyLevel.HARD;
                break;
            default:
                break;
        }
    }

    //Generate a hitbox for the minigame
    public void GenerateHitbox()
    {
        //Get the latest sized minigame width
        minigameWidth = GetComponent<RectTransform>().rect.width;

        //Set a percentage amount for hitbox
        //hitboxPercentage = Random.Range(minHitboxPercentage, maxHitboxPercentage); //deprecated, use presets instead
        switch (difficultyLevel)
        {
            case DifficultyLevel.EASY:
                hitboxDisplay.GetComponent<Image>().sprite = soundWaves[0];
                hitboxPercentage = 26;
                break;
            case DifficultyLevel.NORMAL:
                hitboxDisplay.GetComponent<Image>().sprite = soundWaves[1];
                hitboxPercentage = 19;
                break;
            case DifficultyLevel.HARD:
                hitboxDisplay.GetComponent<Image>().sprite = soundWaves[2];
                hitboxPercentage = 8;
                break;
            default:
                break;
        }

        //Convert value to localised width length
        hitBoxSize = hitboxPercentage / 100 * minigameWidth;
        
        //Set the hitbox width
        hitboxDisplay.sizeDelta = new Vector2(hitBoxSize, hitboxDisplay.sizeDelta.y);

        //Get highest possible offset value for hitbox
        hitboxOffset = (minigameWidth - hitBoxSize) / 2;

        //Randomize hitbox offset within minigame screen
        finalOffset = Random.Range(-hitboxOffset, hitboxOffset);
        
        hitboxDisplay.anchoredPosition = new Vector2(finalOffset, hitboxDisplay.anchoredPosition.y);

        isGaming = true;
    }


    //The minigame hit button
    public void HitMinigameButton()
    {
        if (isGaming == false) return;
        isGaming = false;

        float hitboxLeft = (hitboxOffset + finalOffset) / minigameWidth;

        float hitboxRight = (hitboxOffset + finalOffset + hitBoxSize) / minigameWidth;
        if (sliderValue > hitboxLeft && sliderValue < hitboxRight)
        {
            //GenerateHitbox();

            //Return back to gameplay
            if (GameplayManager.instance != null)
                GameplayManager.instance.EndMinigame(true);
        }
        else
        {
            //Return back to gameplay
            if (GameplayManager.instance != null)
                GameplayManager.instance.EndMinigame(false);
        }
    }
}
