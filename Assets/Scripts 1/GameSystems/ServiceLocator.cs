using System.Collections.Generic;
using System;
using UnityEngine;
public class ServiceLocator
{
    private ServiceLocator()
    {

    }

    private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

    public static ServiceLocator Current { get; private set; }

    public static void Initialize()
    {
        Current = new ServiceLocator();
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)_services[key];
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            Debug.LogError(
                $"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
            return;
        }

        _services.Add(key, service);
    }

    public void Unregister<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError(
                $"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
            return;
        }

        _services.Remove(key);
    }
}
