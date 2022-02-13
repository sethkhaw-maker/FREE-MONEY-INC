using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Emote 
{
    Animator anim;
    Animal self;

    public void Start(Animal _a)
    {
        self = _a;
        anim = self.animalAnimator;
    }

    // animations go here
    public void EmoteShowAction(EMOTE emote)
    {
        switch (emote)
        {
            case EMOTE.NORMAL:  break;
            case EMOTE.HUNGRY:  break;
            case EMOTE.SCARED:  break;
            case EMOTE.HAPPY:   break;
            case EMOTE.ANGRY:   break;
            default: break;
        }
    }

    // sprite showing goes here; not sure what input is yet.
    public void EmoteShowBubble(EMOTE emote)
    {
        switch (emote)
        {
            case EMOTE.NORMAL: break;
            case EMOTE.HUNGRY: break;
            case EMOTE.SCARED: break;
            case EMOTE.HAPPY: break;
            case EMOTE.ANGRY: break;
            default: break;
        }
    }
}
