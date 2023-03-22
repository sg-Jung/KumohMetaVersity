using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private State currentState;
    private Dictionary<string, State> states = new Dictionary<string, State>();

    public void AddState(string name, State state)
    {
        states[name] = state;
    }

    /// <summary>
    /// 상태 리스트 : Freeze, Walk, Sprint, Jump, Crouch, Air, Slide
    /// </summary>
    public void SetState(string name)
    {
        if (states.ContainsKey(name))
        {
            if(currentState != null)
            {
                currentState.Exit();
            }

            currentState = states[name];
            currentState.Enter();
        }
    }

    public void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
    }
}
