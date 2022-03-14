using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Emote 
{
    Animal self;
    public GameObject thoughtBubble;
    public SpriteRenderer icon;

    public void Init(Animal _a)
    {
        self = _a;
        icon = thoughtBubble.GetComponentInChildren<SpriteRenderer>();
    }

    // animations go here
    public void EmoteShowAction(EMOTE emote)
    {

    }

    // sprite showing goes here; not sure what input is yet.
    public IEnumerator EmoteShowBubble(EMOTE emote, float displayDuration = 1.5f)
    {
        Sprite tryIcon = null;
        SYS_AnimalDB.emoteIcons.TryGetValue(emote, out tryIcon);
        if (tryIcon != null) icon.sprite = tryIcon;

        thoughtBubble.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        thoughtBubble.SetActive(false);
    }
}
