using UnityEngine;
using UnityEngine.UI;

public class Speedometr : MonoBehaviour
{
    [SerializeField] private Rigidbody _target;

    [SerializeField] private float _maxSpeed = 260f;

    [SerializeField] private float _minSpeedArrowAngle;
    [SerializeField] private float _maxSpeedArrowAngle;

    [Header("UI")]
    [SerializeField] private Text _speedLabel;
    [SerializeField] private RectTransform _arrow; 

    private float speed = 0.0f;
    private void Update()
    {
        if(_target != null)
            speed = _target.velocity.magnitude * 3.6f;

        if (_speedLabel != null)
            _speedLabel.text = ((int)speed) + " km/h";
        if (_arrow != null)
            _arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(_minSpeedArrowAngle, _maxSpeedArrowAngle, speed / _maxSpeed));
    }
}
