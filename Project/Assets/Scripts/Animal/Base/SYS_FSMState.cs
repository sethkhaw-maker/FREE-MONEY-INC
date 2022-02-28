using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SYS_FSMState
{
    protected Animal self;
    public bool progress;
    public bool isInteractable = true;

    public void EnterNextState() => progress = true;
    public void Init(Animal _s) => self = _s;

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Running();
}
