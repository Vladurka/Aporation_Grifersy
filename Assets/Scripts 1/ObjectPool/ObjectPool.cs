using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T: MonoBehaviour
{
    public T Prefab { get; }
    public bool AutoExpand { get; set; }
    public Transform Container;
    private List<T> _pool;

    public ObjectPool(T prefab, int amount, Transform container)
    {
        Prefab = prefab;
        Container = container;
        CreatePool(amount);
    }

    private void CreatePool(int amount)
    {
        _pool = new List<T>();
        for (int i = 0; i < amount; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject (bool isDefault = false)
    {
        var createdObj = Object.Instantiate(Prefab, Container);
        createdObj.gameObject.SetActive(isDefault);
        _pool.Add(createdObj);
        return createdObj;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in _pool)
        {
            if(!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if(HasFreeElement(out var element))
            return element;

        if(AutoExpand)
            return CreateObject(true);

        throw new Exception("Pool is empty");
    }
}
