using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_InteractionPrey : SYS_FSMState
{
    bool willNoticePredator, willShake, willFlee;
    public override bool IsInteractable => false;

    float shakeIntensity = 0.5f;
    float shakeInterval = 0.1f;
    bool shakeDir = false;
    float shakeTimer = 1f;
    float flipTimer = 1f;

    int miniState = 0;
    float timer, intervalTimer;
    Vector2 shakeForce;

    public override void OnEnter() 
    {
        DecideBehaviours();
    }
    public override void OnExit() 
    { 
        progress = false;
        intervalTimer = 0;
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
        willShake = willNoticePredator ? true : SYS_AnimalTools.RollDice(2) <= 1 ? true : false;
        willFlee = ShouldFlee();
    
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
        if (timer > shakeTimer) ProgressMiniState();
        if (!willShake) return;

        self.flipAnimal.dontFlip = true;

        intervalTimer += Time.deltaTime;
        if (intervalTimer < shakeInterval) return;
        if (!shakeDir) shakeForce = Vector2.left * shakeIntensity;
        if (shakeDir) shakeForce = Vector2.right * shakeIntensity;

        self.rb.velocity = shakeForce;
        shakeDir = !shakeDir;

        intervalTimer = 0;
    }

    void ProgressMiniState()
    {
        timer = 0;
        self.rb.velocity = Vector2.zero;
        miniState++;
        self.flipAnimal.dontFlip = false;
    }
}
