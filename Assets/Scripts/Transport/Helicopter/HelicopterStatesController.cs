using System;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterStatesController : MonoBehaviour, IService, IStatesConntroller
{
    public string HelicopterState = "Destroyed";

    private Dictionary<Type, IStates> _statesMap;

    private IStates _currentState;

    public void Init()
    {
        InitStates();
        SetStartState();
    }

    private void InitStates()
    {
        _statesMap = new Dictionary<Type, IStates>();
        _statesMap[typeof(HelicopterFixedState)] = new HelicopterFixedState();
        _statesMap[typeof(HelicopterDestroyedState)] = new HelicopterDestroyedState();
    }

    private void SetState(IStates state)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = state;
        _currentState.Enter();
    }

    private void SetStartState()
    {
        if (HelicopterState == "Destroyed")
            SetDestroyedState();

        if (HelicopterState == "Fixed")
            SetFixedState();
    }

    private T GetState<T>() where T : IStates
    {
        var type = typeof(T);
        return (T)_statesMap[type];
    }

    public void SetDestroyedState()
    {
        var state = GetState<HelicopterDestroyedState>();
        SetState(state);
        HelicopterState = "Destroyed";
    }

    public void SetFixedState()
    {
        var state = GetState<HelicopterFixedState>();
        SetState(state);
        HelicopterState = "Fixed";
    }
}
