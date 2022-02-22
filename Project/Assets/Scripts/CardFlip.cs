using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    public bool Flip = false;
    //public Sprite CardFront;
    //public Sprite CardBack;
    //public Image LevelCard;
    public Animator FlipCheckAnim;

    public void BoolFlip()
    {
        Flip = true;
    }

    public void NormalCheck()
    {
        if (Flip == true)
        {
            FlipCheckAnim.Play("FlipToNormal");
        }
    }

    public void FlipToNormal()
    {
        Flip = false;
    }

    void Update()
    {
        //Debug.Log(Flip);
    }
    //    public void FlipCheck()
    //    {
    //        if (LevelCard.sprite == CardBack)
    //        {
    //            FlipCheckAnim.Play("FlipCheck");
    //        }
    //    }


    //    public void FlipCard()
    //    {
    //        if (LevelCard.sprite == CardFront)
    //        {
    //            LevelCard.sprite = CardBack;
    //        }
    //        else LevelCard.sprite = CardFront;


    //    }
}
