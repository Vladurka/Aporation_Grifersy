using System;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterStatesController : MonoBehaviour, IService, IStatesConntroller
{
    [SerializeField] private Material[] _materials;

    public string HelicopterState = "Destroyed";

    private MeshRenderer _renderer;

    private Dictionary<Type, IStates> _statesMap;

    private IStates _currentState;

    public void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
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
        _renderer.material = _materials[1];
    }

    public void SetFixedState()
    {
        var state = GetState<HelicopterFixedState>();
        SetState(state);
        HelicopterState = "Fixed";
        _renderer.material = _materials[0];
    }
}
