using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC_Event : MonoBehaviour
{
    public GameObject randoNPCBox;
    public bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (randoNPCBox.activeInHierarchy)
            {
                //randoNPCBox.SetActive(false);
            }
            else
            {
                randoNPCBox.SetActive(true);
            }
        }
        else if (playerInRange == false)
        {
            randoNPCBox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D encounter)
    {
        if (encounter.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D encounter)
    {
        if (encounter.CompareTag ("Player"))
        {
            playerInRange = false;
        }
    }
}
