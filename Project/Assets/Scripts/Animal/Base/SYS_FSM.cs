﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SYS_FSM : MonoBehaviour
{
    [HideInInspector] public SYS_FSMState currState;
    [HideInInspector] public List<SYS_FSMState> states = new List<SYS_FSMState>();
    [HideInInspector] public Animal self;
    [HideInInspector] public bool active = true;

    public void Init(Animal _s)
    {
        self = _s;
    }

    public void Update()
    {
        if (!active || self == null) return;
        //if (GameOver()) return;
        currState.Running();
        CheckForStateSwitch();
    }

    public void SwitchToState(eSTATE state)
    {
        //Debug.Log("state: " + state);
        SYS_FSMState newState = FindState(state);
        SwitchToState(newState);
    }

    protected void SwitchToState(SYS_FSMState newState)
    {
        //Debug.Log("changing states. " + newState);
        if (currState != null)
            currState.OnExit();
        currState = newState;
        currState.OnEnter();
    }

    protected SYS_FSMState FindState(eSTATE state)
    {
        switch (state)
        {
            case eSTATE.IDLE:           return states.Find(x => x is STATE_Idle);
            case eSTATE.WANDER:         return states.Find(x => x is STATE_Wander);
            case eSTATE.LOOKTOCHASE:    return states.Find(x => x is STATE_LookToChase);
            case eSTATE.LOOKTOFLEE:     return states.Find(x => x is STATE_LookToFlee);
            case eSTATE.CHASE:          return states.Find(x => x is STATE_Chase);
            case eSTATE.FLEE:           return states.Find(x => x is STATE_Flee);
            case eSTATE.FOLLOWNOAH:         return states.Find(x => x is STATE_FollowNoah);
            default: return null;
        }
    }

    //protected bool GameOver() => SYS_GeneralDashboard.instance.gameOver;
    protected virtual void SetFirstState(eSTATE state) => currState = FindState(state);

    public abstract void SetupStates();
    protected abstract void CheckForStateSwitch();
}
