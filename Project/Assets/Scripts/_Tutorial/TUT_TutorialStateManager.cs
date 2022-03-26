using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_TutorialStateManager : MonoBehaviour
{
    [Header("Dialogue Text")]
    public List<GameObject> dialogueText = new List<GameObject>();

    [Header("Obj References")]
    public Animator cloudCanvas;

    [Header("READONLY")]
    public int tutorialState = 0;

    [HideInInspector] public static TUT_TutorialStateManager instance;
    [HideInInspector] public static bool tutorialRunning = false;
    [HideInInspector] public static bool[] tutorialDisplayed = new bool[20];

    private void Awake() => ResetStaticObjs();
    private void Start() => instance = this;

    private void Update()
    {
        CheckForFlagsInUpdate();
    }

    void CheckForFlagsInUpdate()
    {
        if (tutorialRunning && tutorialState == 0 && !tutorialDisplayed[0]) { ReadAnimator(); }
    }

    void ProgressTutorial()
    {
        StartCoroutine(TutorialComponents());

        IEnumerator TutorialComponents()
        {
            StartTutorial();
            yield return null;

            switch (tutorialState)
            {
                case 0: ShowTutorialText(); break;
                case 2: ShowTutorialText(); break;
                case 3: SwitchArrow(); break;
                case 4: SwitchArrow(); yield return new WaitForSeconds(1f); ShowTutorialText(); break;
                case 5: SwitchArrow(); break;
                case 6: SwitchArrow(); yield return new WaitForSeconds(1f); ShowTutorialText(); break;
                case 7: SwitchArrow(); break;
                case 8: SwitchArrow(); yield return new WaitForSeconds(1f); ShowTutorialText(); break;
                case 9: SwitchArrow(); break;
                case 10: SwitchArrow(); yield return new WaitForSeconds(1f); ShowTutorialText(); break;
                case 12: yield return new WaitForSeconds(1f); ShowTutorialText(); break;
                case 13: TUT_GameManager.instance.EndDay(); break;
            }
        }
    }

    public void ShowTutorialText()
    {
        switch (tutorialState)
        {
            case 0: dialogueText[0].SetActive(true); break;
            case 2: dialogueText[1].SetActive(true); break;
            case 4: dialogueText[2].SetActive(true); break;
            case 6: dialogueText[3].SetActive(true); break;
            case 8: dialogueText[4].SetActive(true); break;
            case 10: dialogueText[5].SetActive(true); break;
            case 12: dialogueText[6].SetActive(true); break;
        }
    }

    public void SwitchArrow()
    {
        switch (tutorialState)
        {
            case 3: GameObject.Find("Zebra").GetComponent<Animal>().tutorialArrow.SetActive(true); break;
            case 4: GameObject.Find("Zebra").GetComponent<Animal>().tutorialArrow.SetActive(false); break;
            case 5: GameObject.Find("Tiger").GetComponent<Animal>().tutorialArrow.SetActive(true); break;
            case 6: GameObject.Find("Tiger").GetComponent<Animal>().tutorialArrow.SetActive(false); break;
            case 7: GameObject.Find("Elephant").GetComponent<Animal>().tutorialArrow.SetActive(true); break;
            case 8: GameObject.Find("Elephant").GetComponent<Animal>().tutorialArrow.SetActive(false); break;
            case 9: GameObject.Find("Tiger").GetComponent<Animal>().tutorialArrow.SetActive(true); break;
            case 10: GameObject.Find("Tiger").GetComponent<Animal>().tutorialArrow.SetActive(false); break;
        }
    }

    public void ProgressTutorialState() 
    {
        StopPlayerFromMoving();
        SetTutorialFlag(); 
        tutorialState++; 
        ProgressTutorial();
        Debug.Log("tutorialState: " + tutorialState);
    }

    void ResetStaticObjs()
    {
        tutorialRunning = true;
        tutorialDisplayed = new bool[20];
    }
    public void SetTutorialFlag() => tutorialDisplayed[tutorialState] = true;
    public static void StartTutorial() => tutorialRunning = true;
    public static void EndTutorial() { tutorialRunning = false; }
    public void StopPlayerFromMoving() { PlayerController.instance.targetMove = PlayerController.instance.gameObject.transform.position; PlayerController.instance.rb.velocity = Vector2.zero; }
    void ReadAnimator() { if (cloudCanvas.GetCurrentAnimatorStateInfo(0).normalizedTime > cloudCanvas.GetCurrentAnimatorClipInfo(0).Length) ProgressTutorial(); }
}
