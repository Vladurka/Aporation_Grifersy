using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace Game.Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] protected string _tag = "Player";
        [SerializeField] protected bool _isMission5 = false;
        [SerializeField] protected float _range = 20f;

        protected GameObject _mainCharacter;
        protected Animator _animator;

        protected GameObject[] _points;
        protected NavMeshAgent _agent;

        protected bool _isGone = false;
        [HideInInspector] public bool IsDetected = false;
        [HideInInspector] public bool IsDead = false;

        protected virtual void Chill() { }
        protected abstract void EnemyDetected();
        protected virtual IEnumerator FindPlayer()
        {
            if (_mainCharacter == null)
            {
                _mainCharacter = GameObject.FindGameObjectWithTag(_tag);
                yield return new WaitForSeconds(3f);
                StartCoroutine(FindPlayer());
            }

            if (_mainCharacter != null)
                yield break;
        }
    }
}
