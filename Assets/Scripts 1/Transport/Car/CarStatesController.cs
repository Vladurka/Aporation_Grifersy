using System;
using System.Collections.Generic;
using UnityEngine;

public class CarStatesController : MonoBehaviour, IService,  IStatesConntroller
{
    public string CarState = "Destroyed";

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
        _statesMap[typeof(CarFixedState)] = new CarFixedState();
        _statesMap[typeof(CarDestroyedState)] = new CarDestroyedState();
    }

    private void SetState(IStates state)
    {
        if(_currentState != null)
           _currentState.Exit();

        _currentState = state;
        _currentState.Enter();
    }

    private void SetStartState()
    {
        if (CarState == "Destroyed")
            SetDestroyedState();

        if (CarState == "Fixed")
            SetFixedState();
    }

    private T GetState<T>() where T : IStates
    {
        var type = typeof(T);
        return (T) _statesMap[type];
    }

    public void SetDestroyedState()
    {
        var state = GetState<CarDestroyedState>();
        SetState(state);
        CarState = "Destroyed";
    }

    public void SetFixedState()
    {
        var state = GetState<CarFixedState>();
        SetState(state);
        CarState = "Fixed";
    }
}
