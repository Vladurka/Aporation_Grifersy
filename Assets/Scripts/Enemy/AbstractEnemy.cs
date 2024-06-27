using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        protected Animator _animator;

        protected GameObject[] _points;
        protected GameObject _mainCharacter;

        protected NavMeshAgent _agent;

        public bool IsDetected = false;
        public bool IsDead = false;

        public abstract void Chill();
        public abstract void EnemyDetected();
    }
}
