using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private float _checkDistance;
    [SerializeField] private Transform _rayPoint;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private Transform _obstacleModel;

    private RaycastHit _hit;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetObstacle>(SetObstacle, 1);
    }

    private void Update()
    {
        if (Physics.Raycast(_rayPoint.position, _rayPoint.forward, out _hit, _checkDistance))
        {
            Vector3 position = new Vector3(_hit.point.x, _hit.point.y + _obstacle.transform.localScale.y / 2, _hit.point.z);

            if (Input.GetMouseButtonDown(0))
                Build(position);

            //PreviewModel(position);
        }
        //else
        //{
        //    if (_obstacleModel.gameObject.activeSelf)
        //        _obstacleModel.gameObject.SetActive(false);
        //}
    }

    private void Build(Vector3 position)
    {
        if (Physics.Raycast(_rayPoint.position, _rayPoint.forward, out _hit, _checkDistance))
        {
            if (_hit.collider.CompareTag("Ground"))
                Instantiate(_obstacle, position, Quaternion.identity); 
        }
    }

    private void PreviewModel(Vector3 position)
    {
        if(!_obstacleModel.gameObject.activeSelf)
            _obstacleModel.gameObject.SetActive(true);

         _obstacleModel.position = position;
    }

    private void SetObstacle(SetObstacle obstacle)
    {
        _obstacle = obstacle.Obstacle;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetObstacle>(SetObstacle);
    }

}  
