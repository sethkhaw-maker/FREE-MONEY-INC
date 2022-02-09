using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPageLore : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void StartPageLoreAnim()
    {
        anim.SetTrigger("StartPageLoreAnim");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
