using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_InteractionPredator : SYS_FSMState
{
    public override bool IsInteractable => false;
    public int miniState = 0;

    float prepareTime = 1.5f;
    float feedingTime = 5f;
    float failChaseTime = 5f;
    float leniency = 0.5f;

    float speed, timer;

    bool hasShaken = false;

    public override void OnEnter() 
    {
        if (self.preyPredatorInteraction == 2)
        {
            speed = self.wanderSpeed * 1.15f;
            failChaseTime = SYS_AnimalTools.RollDice(3) + 1;
        }
        else speed = self.runSpeed;
    }
    public override void OnExit() 
    {
        self.preyPredatorInteraction = 0;
        progress = false;
        speed = 0;
        timer = 0;
        miniState = 0;
        hasShaken = false;
    }

    public override void Running()
    {
        timer += Time.deltaTime;
        switch (miniState)
        {
            case 0: FacePrey(); break;
            case 1: ShuffleBackwards(); break;
            case 2: Shake(); break;
            case 3: ChasePrey(); break;
            case 4: Feed(); break;
            case 10: FailChasePrey(); break;
            case 11: EnterNextState(); break;
        }
    }

    void Shake()
    {
        if (!hasShaken)
        {
            self.PlayShakeEmote();
            hasShaken = true;
            return;
        }
        if (hasShaken && self.animalEmote.isShaking)
        {
            ProgressMiniState();
            if (self.preyPredatorInteraction == 2) miniState = 10;
        }
    }
    void ShuffleBackwards()
    {
        if (timer > prepareTime) ProgressMiniState();
        self.flipAnimal.dontFlip = true;
        MoveBackwards();
    }

    void FailChasePrey()
    {
        if (timer > failChaseTime) ProgressMiniState();
        Vector3 targetPos = self.target.transform.position;
        targetPos.y -= 0.5f;    // to address layering
        self.rb.velocity = SYS_AnimalTools.MoveTowards(targetPos, self, speed);
    }

    void ChasePrey()
    {
        Vector3 targetPos = self.target.transform.position;
        targetPos.y -= 0.5f;    // to address layering
        self.rb.velocity = SYS_AnimalTools.MoveTowards(targetPos, self, speed);
        if (CaughtPrey())
        {
            self.targetAsAnimal.isDespawning = true;
            self.targetAsAnimal.preyPredatorInteraction = 0;
            self.ClearTarget();
            ProgressMiniState();
        }
    }

    void FacePrey()
    {
        if (timer > 1f) ProgressMiniState();
        self.rb.velocity = Vector2.zero;
        float dist = SYS_AnimalTools.MoveTowards(self.target.transform.position, self, 1f).x;
        if (dist > 0 && self.flipAnimal.facingRight != 1) self.flipAnimal.Flip();
        if (dist < 0 && self.flipAnimal.facingRight != -1) self.flipAnimal.Flip();
    }

    void Feed()
    {
        self.flipAnimal.dontFlip = true;
        if (timer < feedingTime) return;

        ProgressMiniState();
        miniState = 11;
        self.isHungry = false;
        self.flipAnimal.dontFlip = false;
        ObjectPool.instance.SpawnBones(self.transform);
    }
    void MoveBackwards() => self.rb.velocity = SYS_AnimalTools.MoveTowards(-self.target.transform.position, self, self.wanderSpeed * 0.25f);
    bool CaughtPrey() => CalcDist() < leniency ? true : false;
    float CalcDist() => Vector3.Distance(self.target.transform.position, self.transform.position);
    void ProgressMiniState()
    {
        timer = 0;
        miniState++;
        self.rb.velocity = Vector2.zero;
    }
}