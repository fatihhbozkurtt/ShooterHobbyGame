using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Controllers
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        private GameObject _prefabRef;
        private int _damage;
        private float _speed;
        private Vector3 _direction;
        private float _lifetime;


        public void Initialize(int damage, float speed)
        {
            _prefabRef = DataExtensions.GetGameplayData().bulletPrefab.gameObject;
            _damage = damage;
            _speed = speed; 

            Transform target = transform.position.GetClosestEnemy(EnemySpawner.instance.GetSpawnedEnemies());
            _direction = target != null
                ? (target.position - transform.position).normalized
                : transform.forward;

            _lifetime = 0f;
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
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
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