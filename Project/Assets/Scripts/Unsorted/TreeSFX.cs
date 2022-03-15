using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSFX : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>()?.Play("Tree SFX");
        }
    }
}
