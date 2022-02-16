using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script runs the world logic for day/night cycle and weather system
public class GameplayManager : MonoBehaviour
{
    //Variables
    public static GameplayManager instance;

    public enum GameState
    {
        PLAYING,
        MINIGAME,
        PAUSED
    }
    public static GameState gameState;

    //Canvas prefabs
    private GameObject minigameInstance;
    public GameObject minigamePrefab;

    private GameObject gameOverInstance;
    public GameObject gameOverPrefab;

    //UI stuff
    public GameObject clockhand;
    public float clockSpeed = 5;

    public Text animalsCollectedText;
    public int animalsCollected = 0;

    //Game timers
    private float dayTimer;


    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (gameState == GameState.PLAYING)
        {
            UpdateGameTime();
            UpdateAnimalsCollected();
        }

        CheckWin();
    }

    //Update UI for in game timer
    public void UpdateGameTime()
    {
        dayTimer += Time.deltaTime * clockSpeed;
        clockhand.transform.rotation = Quaternion.Euler(0, 0, -dayTimer);
    }

    //Update UI for animals collected
    public void UpdateAnimalsCollected()
    {
        animalsCollectedText.text = "Animals collected: " + animalsCollected;
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
