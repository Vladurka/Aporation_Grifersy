using UnityEngine;

public abstract class AbstractTransport : MonoBehaviour
{
    [SerializeField] protected float _forwardSpeed;
    [SerializeField] protected Transform _spawnCharacter;
    [SerializeField] protected GameObject _mainCharacter;
    [SerializeField] protected Camera _camera;

    public abstract void Init();
    public abstract void Enter();
    public abstract void Exit();
    public abstract void TransportReset();
    protected abstract void Move();
}
