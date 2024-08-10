using Game.Enemy;
using System.Collections.Generic;
using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class EnemyListController : IService
{
    private static List<GameObject> _enemies = new List<GameObject>();

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<AddObj>(AddObject, 1);
        _eventBus.Subscribe<CheckList>(CheckDistance, 1);
        _eventBus.Subscribe<RemoveObj>(RemoveObject, 1);

        _enemies.Clear();
    }

    private void AddObject(AddObj obj)
    {
        _enemies.Add(obj.Enemy);
    }

    private void RemoveObject(RemoveObj obj)
    {
        if (_enemies.Count > 0)
            _enemies.Remove(obj.Enemy);

            if (_enemies.Count <= 0)
            _eventBus.Invoke(new EndSignal());
    }

    private void CheckDistance(CheckList check)
    {
        if (_enemies.Count > 0)
        {
            foreach (GameObject obj in _enemies)
            {
                if (Vector3.Distance(check.Position, obj.transform.position) <= check.Range)
                {
                    if (obj.TryGetComponent(out AbstractEnemy enemy))
                        enemy.IsDetected = true;
                }
            }
        }
    }
}
