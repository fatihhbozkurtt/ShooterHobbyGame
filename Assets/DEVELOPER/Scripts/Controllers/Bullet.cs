using System;
using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Controllers
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [Header("Debug")] [SerializeField] private int damage;
        private GameObject _prefabRef;
        private float _speed;
        private Vector3 _direction;
        private float _lifetime;
        LevelData _levelData;


        public void Initialize(int dmg, float speed)
        {
            _levelData = GameManager.instance.GetGeneralLevelData();
            _prefabRef = _levelData.GameplayData.bulletPrefab.gameObject;
            damage = dmg;
            _speed = speed;

            Transform target = transform.position.GetClosestEnemy(EnemySpawner.instance.GetSpawnedEnemies());
            _direction = target != null
                ? (target.position - transform.position).normalized
                : transform.forward;

            _lifetime = 0f;
        }

        private void Start()
        {
            GameManager.instance.LevelEndedEvent += OnLevelEnded;
        }

        private void OnLevelEnded()
        {
            GameManager.instance.LevelEndedEvent -= OnLevelEnded;
            Destroy(gameObject);
        }

        private void Update()
        {
            if (!gameObject.activeInHierarchy) return;

            transform.position += _direction * (_speed * Time.deltaTime);
            _lifetime += Time.deltaTime;

            if (_lifetime >= 5f)
            {
                ObjectPoolManager.instance.ReturnToPool(_prefabRef, gameObject);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<EnemyAI>(out var enemy))
            {
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                damageable.TakeDamage(damage);
                ObjectPoolManager.instance.ReturnToPool(_prefabRef, gameObject);
            }
        }


        public void OnSpawn()
        {
            _lifetime = 0f;
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            // Optional: reset velocity, trail, etc.
            gameObject.SetActive(false);
        }
    }
}