using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] protected bool _isMission5 = false;
        [SerializeField] protected float _range = 20f;
        [SerializeField] protected GameObject _mainCharacter;

        protected Animator _animator;

        protected GameObject[] _points;
        protected NavMeshAgent _agent;

        /*[HideInInspector] */public bool IsDetected = false;
        [HideInInspector] public bool IsDead = false;

        public abstract void EnemyDetected();
    }
}
