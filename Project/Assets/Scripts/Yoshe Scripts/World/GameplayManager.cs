using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private GameObject minigameInstance;
    public GameObject minigamePrefab;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void InitMinigame()
    {
        gameState = GameState.MINIGAME;

        if (minigameInstance == null)
        {
            minigameInstance = Instantiate(minigamePrefab);
        }
        print("minigame has started");

        //targetAnimal.RegisterAnimalToParty();
    }

    public void EndMinigame(bool win)
    {
        StartCoroutine(DelayEndMinigame(win));
    }

    private IEnumerator DelayEndMinigame(bool win)
    {
        //Add a delay for atas
        yield return new WaitForSeconds(1f);

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
}
