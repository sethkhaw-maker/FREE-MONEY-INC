using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkIndicator : MonoBehaviour
{
    public Transform noah;
    public Transform ark;
    private SpriteRenderer rend;
    public SpriteRenderer arkRend;

    public float padding = 10;

    public Transform arkCenter;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Get ark's position from world space to screen pos
        Vector3 screenPos = Camera.main.WorldToScreenPoint(ark.position);

        DisplayArkIndicator(screenPos);
        MoveArkIndicator(screenPos);

        arkCenter.up = Vector3.up;
    }

    public void DisplayArkIndicator(Vector3 screenPos)
    {
        //Check if ark is visible within screen
        if (screenPos.x < Screen.width && screenPos.x > 0
            && screenPos.y < Screen.height && screenPos.y > 0)
        {
            rend.enabled = false;
            arkRend.enabled = false;
        }
        else
        {
            rend.enabled = true;
            arkRend.enabled = true;
        }

    }

    public void MoveArkIndicator(Vector3 screenPos)
    {
        //Restrict indicator within x of screen
        if (screenPos.x > Screen.width) screenPos.x = Screen.width - padding;
        else if (screenPos.x < 0) screenPos.x = 0 + padding;

        //Restrict indicator within y of screen
        if (screenPos.y > Screen.height) screenPos.y = Screen.height - padding;
        else if (screenPos.y < 0) screenPos.y = 0 + padding;

        //Convert screen pos of restricted ark pos back to world space
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        //Assign position to indicator
        transform.position = worldPos;

        //Rotate indicator to face center of screen
        transform.right = (ark.position - transform.position).normalized;
    }

}
