using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace DEVELOPER.Scripts.Controllers
{
    public class EnemyAI : MonoBehaviour, IDamageable, IPoolable
    {
        [SerializeField] private int health = 3;

        private NavMeshAgent agent;
        private Transform target;
        public float3 TargetPosition => target != null ? target.position : float3.zero;

        private GameObject _prefabReference;

        public void Initialize(Transform targetTransform, GameObject prefab)
        {
            target = targetTransform;
            _prefabReference = prefab;

            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
                agent.updateRotation = false;
                agent.updateUpAxis = false;
            }

            JobsLogicManager.instance?.Register(this);
        }


        public void ApplyDirection(float3 direction)
        {
            if (agent != null && math.length(direction) > 0.1f)
            {
                Vector3 destination = transform.position + (Vector3)direction * 0.5f;
                agent.SetDestination(destination);
            }
        }


        #region IDamageable

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // TODO: Add particle/sound feedback here
            ObjectPoolManager.instance.ReturnToPool(_prefabReference, gameObject);
        }

        #endregion

        #region IPoolable

        public void OnSpawn()
        {
            gameObject.SetActive(true);
            JobsLogicManager.instance?.Register(this);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
            JobsLogicManager.instance?.Unregister(this);
        }

        #endregion
    }
}