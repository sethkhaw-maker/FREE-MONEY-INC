using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesDisplay : MonoBehaviour
{
    public Image[] imageDisplay;
    public Text[] text;
    public Animator fadeCanvas;
    public GameObject cloudCanvas;

    private void OnEnable() => SetQuota();

    public void SetQuota()
    {
        GameplayManager.gameState = GameplayManager.GameState.MINIGAME;

        imageDisplay[0].sprite = GameplayHUD.instance.objectiveOnePortrait.sprite;
        imageDisplay[1].sprite = GameplayHUD.instance.objectiveTwoPortrait.sprite;
        imageDisplay[2].sprite = GameplayHUD.instance.objectiveThreePortrait.sprite;

        text[0].text = "x" + GameplayManager.instance.animalsToCollect_Required[0].ToString();
        text[1].text = "x" + GameplayManager.instance.animalsToCollect_Required[1].ToString();
        text[2].text = "x" + GameplayManager.instance.animalsToCollect_Required[2].ToString();
    }

    public void ButtonClick()
    {
        GameplayManager.gameState = GameplayManager.GameState.PLAYING;
        fadeCanvas.SetInteger("fadeState", 1);
        Invoke("ShowClouds", 0.5f);
        gameObject.SetActive(false);
    }
    public void ShowClouds() => cloudCanvas.SetActive(true);
}
