using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYS_Emote 
{
    Animal self;
    public GameObject thoughtBubble;
    public SpriteRenderer icon;
    float shakeTime = 1f;
    float shakeIntensity = 1.5f;
    public bool isShaking = false;

    public void Init(Animal _a)
    {
        self = _a;
        icon = thoughtBubble.GetComponentInChildren<SpriteRenderer>();
    }

    // sprite showing goes here; not sure what input is yet.
    public IEnumerator EmoteShowBubble(EMOTE emote, float displayDuration = 1.5f, bool isPartyInteraction = false)
    {
        if (isPartyInteraction) self.animalFSM.active = false;

        Sprite tryIcon = null;
        SYS_AnimalDB.emoteIcons.TryGetValue(emote, out tryIcon);
        if (tryIcon != null) icon.sprite = tryIcon;

        thoughtBubble.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        thoughtBubble.SetActive(false);

        if (isPartyInteraction) self.animalFSM.active = true;
    }

    public IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float timer = 0;
        bool shakeDir = false;
        Vector2 shakeForce = Vector2.zero;
        self.flipAnimal.dontFlip = true;

        while (timer < shakeTime)
        {
            if (!shakeDir) shakeForce = Vector2.left * shakeIntensity;
            if (shakeDir) shakeForce = Vector2.right * shakeIntensity;
            self.rb.velocity = shakeForce;
            shakeDir = !shakeDir;
            timer += Time.deltaTime;
            yield return null;
        }

        self.flipAnimal.dontFlip = false;
        isShaking = false;
    }
}
