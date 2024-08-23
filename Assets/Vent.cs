using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Vent : MonoBehaviour, ITargetHealth
{
    public float Health { get; set; } = 100f;
    public bool IsArmored { get; set; } = false;

    [SerializeField] private Material _material;

    private MeshRenderer[] _meshRenderer;

    private EventBus _eventBus;

    private bool _isDead = false;
   

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    public void Destroy()
    {
        if (!_isDead)
        {
            _eventBus.Invoke(new EndSignal());

            foreach (MeshRenderer render in _meshRenderer)
                render.material = _material;

            _isDead = true;
        }
    }

    public void GetDamage(float damage)
    {
       Health -= damage;

       if(Health <= 0)
            Destroy();
    }
}
