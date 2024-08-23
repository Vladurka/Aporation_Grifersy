using System.Collections;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
public class AirDefence : MonoBehaviour, IVehicleShoot
{
    [SerializeField] private float _range = 2000f;

    [SerializeField] private GameObject _missile;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnPosition;

    [SerializeField] private float _minStart = 20f;
    [SerializeField] private float _maxStart = 30f;

    [SerializeField] private float _minInterval = 15f;
    [SerializeField] private float _maxInterval = 26f;

    [SerializeField] private bool _autoStart = true;

    private EventBus _eventBus;

    private float _time;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<StartAirdefence>(StartAirdefence, 1);
        _eventBus.Subscribe<StopAirdefence>(StopAirdefence, 1);

        _time = Random.Range(_minStart, _maxStart);

        if(_autoStart)
            StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_time);

        if (Vector3.Distance(transform.position, _target.position) <= _range)
        {
            GameObject missileObject = Instantiate(_missile, _spawnPosition.position, _spawnPosition.rotation);
            Missile missile = missileObject.GetComponent<Missile>();
            missile.Target = _target;
        }

        _time = Random.Range(_minInterval, _maxInterval);
        StartCoroutine(Shoot());
    }

    private void StartAirdefence(StartAirdefence start)
    {
        StartCoroutine(Shoot());
    }

    private void StopAirdefence(StopAirdefence stop)
    {
        StopAllCoroutines();
    }

    public void Stop()
    {
        StopAllCoroutines();
        this.enabled = false;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<StartAirdefence>(StartAirdefence);
        _eventBus.Unsubscribe<StopAirdefence>(StopAirdefence);
    }
}
