using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Emote 
{
    Animal self;
    public void Start(Animal _s) => self = _s;

    public void EmoteAct(EMOTE emote)
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

    public void EmoteShow(EMOTE emote)
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
    
    void Normal() { }
    void Happy() { }
    void Hungry() { }
    void Scared() { }
    void Angry() { }
}
