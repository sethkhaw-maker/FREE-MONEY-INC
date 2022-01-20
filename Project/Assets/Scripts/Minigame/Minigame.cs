using UnityEngine;

//The real time scrolling minigame controller

public class Minigame : MonoBehaviour
{
    //Variables
    private float minigameWidth;     //Technical stuff, allows free sizing of Minigame width
    public float hitboxPercentage;  //Percentage of the hitbox will cover on minigame screen

    public RectTransform hitboxDisplay;

    void Start()
    {
        GenerateHitbox();
    }

    //Generate a hitbox for the minigame
    public void GenerateHitbox()
    {
        //Get the latest sized minigame width
        minigameWidth = GetComponent<RectTransform>().rect.width;

        //Set a percentage amount for hitbox
        hitboxPercentage = Random.Range(10f, 40f);

        //Convert value to localised width length
        float hitBoxSize = hitboxPercentage / 100 * minigameWidth;
        
        //Set the hitbox width
        hitboxDisplay.sizeDelta = new Vector2(hitBoxSize, 0);

        //Get highest possible offset value for hitbox
        float hitboxOffset = (minigameWidth - hitBoxSize) / 2;

        //Randomize hitbox offset within minigame screen
        hitboxDisplay.anchoredPosition = new Vector2(Random.Range(-hitboxOffset, hitboxOffset), 0);
    }
}
