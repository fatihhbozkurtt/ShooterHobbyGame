using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using EssentialManagers.Scripts.Managers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace DEVELOPER.Scripts.Controllers
{
    public class EnemyAI : MonoBehaviour, IDamageable, IPoolable
    {
        [Header("Debug")] [SerializeField] private int currentHealth;
        [SerializeField] private EnemyType enemyType;

        private NavMeshAgent agent;
        private Transform target;
        public float3 TargetPosition => target != null ? target.position : float3.zero;

        private GameObject _prefabReference;
        MeshRenderer _meshRenderer;
        Collider _collider;

        public void Initialize(Transform targetTransform, GameObject prefab, EnemyType type)
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            enemyType = type;

            var stats = DataExtensions.GetEnemyStatsByType(GameManager.instance.GetGeneralLevelData()
                , enemyType);

            gameObject.name = stats.Name;
            _meshRenderer.material = stats.Material;
            currentHealth = stats.MaxHealth;

            target = targetTransform;
            _prefabReference = prefab;

            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
                agent.updateRotation = false;
                agent.updateUpAxis = false;
            }

            JobsLogicManager.instance?.Register(this);

            _collider = GetComponent<Collider>();
            _collider.enabled = true;
            agent.enabled = true;

            if (GameManager.instance != null)
                GameManager.instance.LevelEndedEvent += OnLevelEnded;
        }

        private void OnLevelEnded()
        {
            agent.enabled = false;
            GameManager.instance.LevelEndedEvent -= OnLevelEnded;
            ObjectPoolManager.instance.ReturnToPool(_prefabReference, gameObject);
        }

        public void ApplyDirection(float3 direction)
        {
            if (!agent.enabled) return;

            if (agent != null && math.length(direction) > 0.1f)
            {
                Vector3 destination = transform.position + (Vector3)direction * 0.5f;
                agent.SetDestination(destination);
            }
        }

        #region IDamageable

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // TODO: Add particle/sound feedback here

            gameObject.SetActive(false);
            _collider.enabled = false;
            agent.enabled = false;

            CanvasManager.instance.RegisterKill();
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
            EnemySpawner.instance.RemoveEnemy(this);
            JobsLogicManager.instance?.Unregister(this);
        }

        #endregion
    }
}