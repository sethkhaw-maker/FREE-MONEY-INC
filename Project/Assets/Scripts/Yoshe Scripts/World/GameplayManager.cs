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

    }

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
        //if (PlayerController.party[0] == null) //Is not able to check for null
        //{
        //    print("You do not have any animals!");
        //    return;
        //}
        //PlayerController.party[0].target = ark;
        for (int i = 0; i < PlayerController.party.Count; i++)
        {
            PlayerController.party[i].target = ark;
            PlayerController.party[i].followOffset = 0.1f;
        }
    }
}
