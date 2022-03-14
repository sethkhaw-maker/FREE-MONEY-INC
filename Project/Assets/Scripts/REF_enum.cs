using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSTATE
{
    IDLE,
    WANDER,
    LOOKTOCHASE,
    LOOKTOFLEE,
    CHASE,
    FLEE,
    FOLLOWNOAH,
    FOLLOWLEADER
}

public enum SPAWNTIME
{
    BOTH,
    DAY,
    NIGHT
}

public enum SPAWNWEATHER
{
    BOTH,
    CLEAR,
    RAINY
}

public enum EMOTE
{
    NORMAL,
    HUNGRY,
    SCARED,
    HAPPY,
    ANGRY,
    CONFUSED
}

public enum ANIMALTYPE
{
    PREY,
    PREDATOR,
    MEDIATOR
}
