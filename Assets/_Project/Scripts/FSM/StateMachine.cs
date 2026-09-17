using System;
using System.Collections.Generic;

public class StateMachine
{
    private Dictionary<Enum, State> states = new Dictionary<Enum, State>();

    private State currentState;

    public State CurrentState => currentState;

    public void RegisterState(Enum stateType, State state)
    {
        if (!states.ContainsKey(stateType))
        {
            states.Add(stateType, state);
        }
    }

    public void ChangeState(Enum stateType)
    {
        if (!states.ContainsKey(stateType))
            return;

        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = states[stateType];

        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}