using UnityEngine;
using UnityEngine.UI;

//The real time scrolling minigame controller

public class Minigame : MonoBehaviour
{
    //UI Variables
    public float minigameWidth;     //Technical stuff, allows free sizing of Minigame width
    [SerializeField] private float hitboxPercentage;  //Percentage of the hitbox will cover on minigame screen
    public float hitBoxSize;
    public float hitboxOffset;

    public float finalOffset;

    public RectTransform hitboxDisplay;
    private Slider hitSlider;       //The moving slider
    public float sliderValue;

    //Gameplay Variables
    private bool isGaming;
    public Text scoreText;
    int score;
    void Start()
    {
        hitSlider = GetComponentInChildren<Slider>();
        GenerateHitbox();
    }

    private void Update()
    {
        UpdateSlider();
    }

    //Generate a hitbox for the minigame
    public void GenerateHitbox()
    {
        //Get the latest sized minigame width
        minigameWidth = GetComponent<RectTransform>().rect.width;

        //Set a percentage amount for hitbox
        hitboxPercentage = Random.Range(10f, 40f);

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

    //Update the slider to move left and right
    private void UpdateSlider()
    {
        sliderValue = Mathf.PingPong(Time.time, 1);
        hitSlider.value = sliderValue;
    }


    //The button to stop the minigame
    public void StopMinigame()
    {
        isGaming = false;

        float hitboxLeft = (hitboxOffset + finalOffset) / minigameWidth;

        float hitboxRight = (hitboxOffset + finalOffset + hitBoxSize) / minigameWidth;
        if (sliderValue > hitboxLeft && sliderValue < hitboxRight)
        {
            print("WIN!");
            GenerateHitbox();
            scoreText.text = score++.ToString();
        }
    }

}
