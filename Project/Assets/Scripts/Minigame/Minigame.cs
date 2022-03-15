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

    public RectTransform hitboxDisplay; //Size of the hitbox area
    private Slider hitSlider;           //The slider which moves back and forth
    private float sliderValue;
    public Image[] attractBoxDisplay;   //Display boxes for attract counter
    public Sprite emptyBox;
    public Sprite tickBox;
    public Sprite crossBox;

    //Gameplay Variables
    private float rate = 0;
    private bool isGaming = true;       //true = minigame slider is moving

    [Range(0.25f, 1.75f)]
    public float sliderSpeed = 1;     //Speed of the slider based on weather
    public float sliderSpeedModifier = 1f;

    public int maxAttractCount = 2;
    private int attractCounter = 0;

    public enum DifficultyLevel
    {
        EASY,
        NORMAL,
        HARD
    }
    public DifficultyLevel difficultyLevel;

    public Sprite[] soundWaves; //Sound wave sprites from longest to shortest

    void Start()
    {
        //Assign the slider from child variable
        hitSlider = GetComponentInChildren<Slider>();

        //Set the minigame difficulty
        SetDifficulty();

        //Set the slider speed
        SetSliderSpeed();

        //Generate the starting hitbox
        GenerateHitbox();
    }

    private void Update()
    {
        rate += Time.deltaTime;

        if (isGaming)
        {
            //Update the slider to move in real time
            UpdateSlider();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HitMinigameButton();
            }
        }

        //Display attract counter
        UpdateAttractCounter();
    }

    //Slider which moves left and right
    private void UpdateSlider()
    {
        sliderValue = Mathf.PingPong(rate * sliderSpeed, 1);
        hitSlider.value = sliderValue;
    }

    //Set the slider speed based on weather
    public void SetSliderSpeed()
    {
        if (GameplayManager.instance == null) return;
        switch (GameplayManager.instance.weatherState)
        {
            case GameplayManager.WeatherState.CLEAR:
                sliderSpeed = 0.75f * sliderSpeedModifier;
                break;
            case GameplayManager.WeatherState.RAINY:
                sliderSpeed = 1.25f * sliderSpeedModifier;
                break;
            default:
                break;
        }
    }

    //Set the difficulty of the minigame
    public void SetDifficulty()
    {
        int animalDifficulty = 0;
        if (PlayerController.instance != null)
        {
            animalDifficulty = PlayerController.instance.targetAnimal.difficultyLevel;
        }
        else
        {
            Debug.LogWarning("Player instance does not exist, Speed set to 1.");
            sliderSpeedModifier = 1f;
            maxAttractCount = 3;
            return;
        }

        switch (animalDifficulty)
        {
            case 0:
                difficultyLevel = DifficultyLevel.EASY;
                sliderSpeedModifier = 1.1f;
                maxAttractCount = 1;
                break;
            case 1:
                difficultyLevel = DifficultyLevel.NORMAL;
                sliderSpeedModifier = 1.15f;
                maxAttractCount = 2;
                break;
            case 2:
                difficultyLevel = DifficultyLevel.HARD;
                sliderSpeedModifier = 1.2f;
                maxAttractCount = 3;
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

        float hitboxLeft = (hitboxOffset + finalOffset) / minigameWidth;

        float hitboxRight = (hitboxOffset + finalOffset + hitBoxSize) / minigameWidth;

        //Success
        if (sliderValue > hitboxLeft && sliderValue < hitboxRight)
        {
            //GenerateHitbox();

            //Decrease number of times to attract
            attractCounter++;
            rate = 0;

            //Animal has been attracted
            if (attractCounter == maxAttractCount)
            {
                isGaming = false;

                //Return back to gameplay
                if (GameplayManager.instance != null)
                {
                    GameplayManager.instance.EndMinigame(true);
                }
                else
                {
                    Debug.LogWarning("No GameplayManager detected.");
                }
                //Play minigame success sfx
                MakeNoise(true);
                FindObjectOfType<AudioManager>()?.Play("Minigame Success");
            }
            //Animal is still being attracted
            else
            {
                //Regenerate the hitbox area
                GenerateHitbox();

                //Play passing sfx
                FindObjectOfType<AudioManager>()?.Play("Minigame Passing");
            }
        }
        //Failed to attract
        else
        {
            //Return back to gameplay
            if (GameplayManager.instance != null)
            {
                isGaming = false;
                GameplayManager.instance.EndMinigame(false);
            }
            else
            {
                Debug.LogWarning("No GameplayManager detected.");
            }

            //Play minigame failed sfx
            MakeNoise(false);
            FindObjectOfType<AudioManager>()?.Play("Minigame Fail");

        }
    }

    private void UpdateAttractCounter()
    {
        for (int i = 0; i < attractBoxDisplay.Length; i++)
        {
            if (i < maxAttractCount)
            {
                if (i < attractCounter)
                {
                    attractBoxDisplay[i].sprite = tickBox;
                }
                else
                {
                    attractBoxDisplay[i].sprite = emptyBox;
                }
            }
            else
            {
                attractBoxDisplay[i].sprite = crossBox;
            }
        }
    }

    private void MakeNoise(bool success)
    {
        if (success)
        {
            switch (PlayerController.instance.targetAnimal.animalType)
            {
                case ANIMALTYPE.PREY:
                    FindObjectOfType<AudioManager>()?.Play("Prey Noise");
                    break;
                case ANIMALTYPE.PREDATOR:
                    FindObjectOfType<AudioManager>()?.Play("Predator Roar");
                    break;
                case ANIMALTYPE.MEDIATOR:
                    FindObjectOfType<AudioManager>()?.Play("Mediator Noise");
                    break;
            }
        }
        else
        {
            switch (PlayerController.instance.targetAnimal.animalType)
            {
                case ANIMALTYPE.PREY:
                    FindObjectOfType<AudioManager>()?.Play("Predator Roar");
                    break;
                case ANIMALTYPE.PREDATOR:
                    FindObjectOfType<AudioManager>()?.Play("Prey Noise");
                    break;
                case ANIMALTYPE.MEDIATOR:
                    FindObjectOfType<AudioManager>()?.Play("Predator Roar");
                    break;
            }
        }


    }
}
