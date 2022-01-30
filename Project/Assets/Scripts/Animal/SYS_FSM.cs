using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SYS_FSM : MonoBehaviour
{
    [HideInInspector] public SYS_FSMState currState;
    [HideInInspector] public List<SYS_FSMState> states = new List<SYS_FSMState>();
    [HideInInspector] public Animal self;

    public void Start(Animal _self)
    {
        self = _self;
    }

    public void Update()
    {
        //if (GameOver()) return;
        currState.Running();
        CheckForStateSwitch();
    }

    protected void SwitchToState(eSTATE state)
    {
        Debug.Log("state: " + state);
        SYS_FSMState newState = FindState(state);
        SwitchToState(newState);
    }

    protected void SwitchToState(SYS_FSMState newState)
    {
        currState.OnExit();
        currState = newState;
        currState.OnEnter();
    }

    protected SYS_FSMState FindState(eSTATE state)
    {
        switch (state)
        {
            case eSTATE.IDLE:           return states.Find(x => x is STATE_Wander);
            case eSTATE.WANDER:         return states.Find(x => x is STATE_Wander);
            case eSTATE.LOOKTOCHASE:    return states.Find(x => x is STATE_Wander);
            case eSTATE.LOOKTOFLEE:     return states.Find(x => x is STATE_Wander);
            case eSTATE.CHASE:          return states.Find(x => x is STATE_Wander);
            case eSTATE.FLEE:           return states.Find(x => x is STATE_Wander);
            case eSTATE.FOLLOW:         return states.Find(x => x is STATE_Wander);
            default: return null;
        }
    }

    //protected bool GameOver() => SYS_GeneralDashboard.instance.gameOver;
    protected virtual void SetFirstState(eSTATE state) => currState = FindState(state);

    protected abstract void SetupStates();
    protected abstract void CheckForStateSwitch();
}

