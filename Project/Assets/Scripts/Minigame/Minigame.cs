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
    private bool isGaming;
    public Text scoreText;
    int score;

    [Range(0.25f,1.75f)]
    public float speedModifier = 1;

    [Range(0f, 100f)]
    public int minHitboxPercentage = 20;      //Minimum size of hitbox in percentage

    [Range(0f, 100f)]
    public int maxHitboxPercentage = 100;     //Maximum size of hitbox in percentage

    void Start()
    {
        //Assign the slider from child variable
        hitSlider = GetComponentInChildren<Slider>();

        //Generate the starting hitbox
        GenerateHitbox();
    }

    private void Update()
    {
        //Update the slider to move in real time
        UpdateSlider();
    }

    //Slider which moves left and right
    private void UpdateSlider()
    {
        sliderValue = Mathf.PingPong(Time.time * speedModifier, 1);
        hitSlider.value = sliderValue;
    }

    //Generate a hitbox for the minigame
    public void GenerateHitbox()
    {
        //Get the latest sized minigame width
        minigameWidth = GetComponent<RectTransform>().rect.width;

        //Set a percentage amount for hitbox
        hitboxPercentage = Random.Range(minHitboxPercentage, maxHitboxPercentage);

        //hitboxPercentage = 10f; //TEMPTESTING

        //Convert value to localised width length
        hitBoxSize = hitboxPercentage / 100 * minigameWidth;
        
        //Set the hitbox width
        hitboxDisplay.sizeDelta = new Vector2(hitBoxSize, 0);

        //Get highest possible offset value for hitbox
        hitboxOffset = (minigameWidth - hitBoxSize) / 2;

        //Randomize hitbox offset within minigame screen
        finalOffset = Random.Range(-hitboxOffset, hitboxOffset);
        
        hitboxDisplay.anchoredPosition = new Vector2(finalOffset, 0);
    }


    //The minigame hit button
    public void HitMinigameButton()
    {
        isGaming = false;

        float hitboxLeft = (hitboxOffset + finalOffset) / minigameWidth;

        float hitboxRight = (hitboxOffset + finalOffset + hitBoxSize) / minigameWidth;
        if (sliderValue > hitboxLeft && sliderValue < hitboxRight)
        {
            GenerateHitbox();
            scoreText.text = (++score).ToString();
        }
    }

}
