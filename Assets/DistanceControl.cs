using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DistanceControl : MonoBehaviour
{
    [SerializeField] private float _distance = 10000f;
    [SerializeField] private float _timeToComeBack = 10f;
    private float _time;

    [SerializeField] private Text _text;
    [SerializeField] private GameObject _panel;

    [SerializeField] private Transform _target;

    private bool _isStarted = false;
    private bool _isStopped = true;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _panel.SetActive(false);

        _time = _timeToComeBack;
        _text.text = _time.ToString();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _target.position) >= _distance)
        {
            if (!_isStarted)
            {
                _panel.SetActive(true);
                StartCoroutine(ComeBack());
                _isStarted = true;
                _isStopped = false;
            }
        }

        if (Vector3.Distance(transform.position, _target.position) < _distance)
        {
            if (!_isStopped)
            {
                _panel.SetActive(false);
                StopAllCoroutines();
                _isStarted = false;
                _isStopped = true;
                _time = _timeToComeBack;
                _text.text = _time.ToString();
            }
        }         
    }

    private IEnumerator ComeBack()
    {
        yield return new WaitForSeconds(1f);

        _time--;
        _text.text = _time.ToString();

        if (_time <= 0)
        {
            _eventBus.Invoke(new SetDie());
            _panel.SetActive(false);
            yield break;
        }

        StartCoroutine(ComeBack());
    }
}
