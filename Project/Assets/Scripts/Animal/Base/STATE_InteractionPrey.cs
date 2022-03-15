using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_InteractionPrey : SYS_FSMState
{
    bool willNoticePredator, willShake, willFlee;
    public override bool IsInteractable => false;

    float flipTimer = 1f;

    int miniState = 0;
    float timer;
    bool hasShaken = false;
    Vector2 shakeForce;

    public override void OnEnter() 
    {
        DecideBehaviours();
    }
    public override void OnExit() 
    { 
        progress = false;
        hasShaken = false;
        timer = 0;
        miniState = 0;
    }

    public override void Running()
    {
        timer += Time.deltaTime;
        switch (miniState)
        {
            case 0: NoticePredator(); break;
            case 1: Shake(); break;
            case 2: Flee(); break;
            case 3: EnterNextState(); break;
        }
    }

    void DecideBehaviours()
    {
        willNoticePredator = SYS_AnimalTools.RollDice(3) <= 2 ? true : false;
        willShake = WillShake();
        willFlee = ShouldFlee();

        bool WillShake()
        {
            if (willNoticePredator) return true;
            else return SYS_AnimalTools.RollDice(2) <= 1 ? true : false;
        }
        bool ShouldFlee()
        {
            if (willNoticePredator || willShake) return true;
            if (self.preyPredatorInteraction == 2) return true; 
            if (SYS_AnimalTools.RollDice(2) <= 1) return true;
            return false;
        }
    }

    void NoticePredator()
    {
        if (timer > flipTimer) ProgressMiniState();
        if (!willNoticePredator) return;
        self.rb.velocity = Vector2.zero;
        float dist = SYS_AnimalTools.MoveTowards(self.target.transform.position, self, 1f).x;
        if (dist > 0 && self.flipAnimal.facingRight != 1) self.flipAnimal.Flip();
        if (dist < 0 && self.flipAnimal.facingRight != -1) self.flipAnimal.Flip();
    }

    void Flee()
    {
        if (willFlee) self.shouldFlee = true;
        ProgressMiniState();
    }

    void Shake()
    {
        if (!willShake) return;
        if (!hasShaken)
        {
            self.PlayShakeEmote();
            hasShaken = true;
            return;
        }

        if (hasShaken && !self.animalEmote.isShaking)
        {
            ProgressMiniState();
            if (self.preyPredatorInteraction == 2) miniState = 10;
        }
    }

    void ProgressMiniState()
    {
        timer = 0;
        self.rb.velocity = Vector2.zero;
        miniState++;
        self.flipAnimal.dontFlip = false;
    }
}
