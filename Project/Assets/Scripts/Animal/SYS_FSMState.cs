using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SYS_FSMState
{
    protected Animal self;
    public bool progress;

    public void EnterNextState() => progress = true;

    public abstract void Init(Animal _s);
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Running();
}
