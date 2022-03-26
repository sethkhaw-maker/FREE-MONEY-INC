using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MESSAGING_string(string stringName);
public delegate void MESSAGING_void();

public class TUT_GameManager : MonoBehaviour
{
    public GameObject minigamePrefab;
    public Animator fadeCanvas;
    public Animator cloudCanvas;

    [HideInInspector] public enum GameState { PLAYING, MINIGAME, SCOPING, PAUSED, TUTORIAL }
    [HideInInspector] public static GameState gameState;
    [HideInInspector] public string[] animalsToCollect = new string[3] { "Zebra", "Tiger", "Elephant" };
    [HideInInspector] public int[] animalsToCollect_Required = new int[3] { 1, 1, 1 };
    [HideInInspector] public int[] animalsToCollect_Current = new int[3] { 0,0,0 };
    [HideInInspector] public GameObject minigameInstance;

    public static TUT_GameManager instance;

    private void Awake() => ResetTutorial();
    private void OnEnable() => Subscription(true);
    private void OnDisable() => Subscription(false);
    private void Update() => CheckWin();

    private IEnumerator DelayEndMinigame(bool win)
    {
        yield return new WaitForSeconds(0.5f);                   //Add a delay for atas
        if (minigameInstance != null) Destroy(minigameInstance); //Destroy minigame instance
        gameState = GameState.PLAYING;                           //Resume game 

        if (win)    //Add animal to party if minigame won
        {
            PlayerController.instance.targetAnimal.RegisterAnimalToParty();
            if (PlayerController.instance.PartyHasInteractionAfterRecruitment())
            {
                ANIMALTYPE targetAnimalType = PlayerController.instance.targetAnimal.animalType;
                PlayerController.instance.targetAnimal.PlayPartyInteractionAfterRecruit(true);
                PlayerController.instance.PlayRecruitmentInteraction(targetAnimalType == ANIMALTYPE.PREY ? ANIMALTYPE.PREDATOR : ANIMALTYPE.PREY);
            }
            TUT_TutorialStateManager.instance.ProgressTutorialState();
        }
        else        //Otherwise just let it go
        {
            PlayerController.instance.targetAnimal.MinigameFailed();
        }

        
        PlayerController.instance.targetAnimal = null;          //Reset target animal from player
    }
    private void CheckWin()
    {
        if (CollectedRequiredAnimals() && TUT_TutorialStateManager.instance.tutorialState == 11)
            TUT_TutorialStateManager.instance.ProgressTutorialState();
    }
    public void EndDay()
    {
        cloudCanvas.SetBool("endDay", true);
        Invoke("FadeDay", 2.583f);
    }
    private void FadeDay()
    {
        fadeCanvas.SetInteger("fadeState", 1);
        Invoke("DelayReload", 1.5f);
    }
    private void DelayReload()
    {
        FindObjectOfType<SceneLoader>().LoadScene(3);
    }
    void Subscription(bool state)
    {
        if (state)
        {
            Animal.UpdateAnimalCount += UpdateAnimalCount;
        }
        else
        {
            Animal.UpdateAnimalCount -= UpdateAnimalCount;
        }
    }

    public void SendAnimalsIntoArk(GameObject ark)              //Send all into the ark (can probably do it to the 1st in party and recursive this)
    {
        for (int i = 0; i < PlayerController.instance.party.Count; i++)
        {
            PlayerController.instance.party[i].target = ark;
            PlayerController.instance.party[i].followOffset = 0.1f;
        }
    }
    bool CollectedRequiredAnimals()
    {
        for (int i = 0; i < animalsToCollect_Current.Length; i++)
            if (animalsToCollect_Required[i] > animalsToCollect_Current[i]) return false;
        return true;
    }
    void UpdateAnimalCount(string name)
    {
        int i = -1;
        switch (name)
        {
            case "Zebra": case "Giraffe": case "Buffalo":   i = 0; break;
            case "Tiger": case "Lion": case "Hyena":        i = 1; break;
            case "Rhino": case "Elephant":                  i = 2; break;
        }
        if (i == -1) return;
        if (name == animalsToCollect[i]) animalsToCollect_Current[i]++; 
    }
    void ResetTutorial() { Animal.ResetStaticObjs(); instance = this; }
    public void InitMinigame() { gameState = GameState.MINIGAME; if (minigameInstance == null) minigameInstance = Instantiate(minigamePrefab); }
    public void EndMinigame(bool win) => StartCoroutine(DelayEndMinigame(win));
}
