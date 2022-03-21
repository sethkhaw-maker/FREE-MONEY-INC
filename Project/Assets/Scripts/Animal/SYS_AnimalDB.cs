using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_AnimalDB : MonoBehaviour
{
    [Header("Emote Icons")]
    public Sprite hungryMeat;
    public Sprite hungryWheat, scared, happy, angry, confused;

    [Header("Bones")]
    public List<GameObject> bones;

    [HideInInspector] public static Dictionary<EMOTE, Sprite> emoteIcons = new Dictionary<EMOTE, Sprite>();
    [HideInInspector] public static List<GameObject> bonesList = new List<GameObject>();

    private void OnEnable()
    {
        EmoteIcons();
        Bones();
    }

    void Bones()
    {
        if (bonesList.Count != 0) return;
        bonesList.AddRange(bones);
    }

    void EmoteIcons()
    {
        if (emoteIcons.Count != 0) return;
        emoteIcons.Add(EMOTE.HUNGRY_PREY, hungryWheat);
        emoteIcons.Add(EMOTE.HUNGRY_PREDATOR, hungryMeat);
        emoteIcons.Add(EMOTE.SCARED, scared);
        emoteIcons.Add(EMOTE.HAPPY, happy);
        emoteIcons.Add(EMOTE.ANGRY, angry);
        emoteIcons.Add(EMOTE.CONFUSED, confused);
    }
}
