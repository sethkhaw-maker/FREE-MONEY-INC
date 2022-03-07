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

    //Gameplay Variables
    private float rate = 0;
    private bool isGaming = true;       //true = minigame slider is moving

    [Range(0.25f,1.75f)]
    public float speedModifier = 1;     //Speed of the slider based on weather

    public int timesToAttract = 2;
    public Text attractCountText;   //Temporary display for attract count

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
        }

        //Temporary count display
        attractCountText.text = timesToAttract.ToString();
    }

    //Slider which moves left and right
    private void UpdateSlider()
    {
        sliderValue = Mathf.PingPong(rate * speedModifier, 1);
        hitSlider.value = sliderValue;
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
            Debug.LogWarning("Player instance does not exist!");
        }

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

        float hitboxLeft = (hitboxOffset + finalOffset) / minigameWidth;

        float hitboxRight = (hitboxOffset + finalOffset + hitBoxSize) / minigameWidth;

        //Success
        if (sliderValue > hitboxLeft && sliderValue < hitboxRight)
        {
            //GenerateHitbox();

            //Decrease number of times to attract
            timesToAttract--;
            rate = 0;

            //Animal has been attracted
            if (timesToAttract <= 0)
            {
                //Return back to gameplay
                if (GameplayManager.instance != null)
                {
                    isGaming = false;
                    GameplayManager.instance.EndMinigame(true);
                }
                //Play minigame success sfx
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

            //Play minigame failed sfx
            FindObjectOfType<AudioManager>()?.Play("Minigame Fail");
        }
    }
}
