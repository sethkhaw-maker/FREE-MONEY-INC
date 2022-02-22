using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void SkipTutorial() // Load to Main Level when clicked.
    {
        Fade("FadeIn");
        StartCoroutine(DelayedLoad("Tutorial Prompt"));
    }

    public void NoSkip() // Load to Tutorial level when clicked.
    {
        Fade("FadeIn");
        StartCoroutine(DelayedLoad("Tutorial"));
    }

    #region Transition Functions
    public void Fade(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void Load(string levelName)
    {
        SceneManager.LoadScene(levelName);

    }

    IEnumerator DelayedLoad(string levelName)
    {
       yield return new WaitForSeconds(1.5f);
        Load(levelName);
    }

    #endregion
}
