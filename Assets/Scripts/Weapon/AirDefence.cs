using UnityEngine;
public class AirDefence : MonoBehaviour
{
    [SerializeField] private GameObject _missile;
    [SerializeField] private float _range = 50f;

    private GameObject _target;
    private bool _isLaunched  = false;
    private void Start()
    {
        _missile.SetActive(false);
        _target = GameObject.FindGameObjectWithTag("Helicopter");
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) <= _range && _isLaunched == false)
        {
            _missile.SetActive(true);
            _isLaunched = true;
        }
    }

}
