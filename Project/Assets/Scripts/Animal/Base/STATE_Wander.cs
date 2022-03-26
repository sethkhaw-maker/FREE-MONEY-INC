using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STATE_Wander : SYS_FSMState
{
    public override bool IsInteractable => true;

    Vector3 endPos;
    bool wandering;
    float timer = 0f;

    public override void OnEnter() { }
    public override void OnExit()
    {
        wandering = false;
        progress = false;
        endPos = Vector3.zero;
        self.rb.velocity = Vector2.zero;
        timer = 0f;
    }

    public override void Running()
    {
        if (!wandering)
            GetWanderPosition();
        MoveTowardsWanderPosition();
        timer += Time.deltaTime;
        if (TimeHasPassed())
            EnterNextState();
    }

    void GetWanderPosition()
    {
        float x = Random.Range(-self.wanderRange, self.wanderRange);
        float y = Random.Range(-self.wanderRange, self.wanderRange);

        Vector3 wanderPoint = new Vector3(x, y) * (self.isTutorialAnimal ? 1f : 20f);

        if (self.isTutorialAnimal) endPos = self.wanderPoint.transform.position + wanderPoint;
        else endPos = self.transform.position + wanderPoint;

        wandering = true;
    }
    void MoveTowardsWanderPosition() => self.rb.velocity = SYS_AnimalTools.MoveTowards(endPos, self, self.wanderSpeed);
    bool TimeHasPassed() => timer >= (self.isTutorialAnimal ? 0.35f : 1.5f);
}
