using System.Collections.Generic;
using DEVELOPER.Scripts.Data;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Managers
{
    /// <summary>
    /// A generic object pooling system for reusing GameObjects efficiently.
    /// Supports multiple prefab types and avoids unnecessary instantiation.
    /// A modular and extensible object pooling manager supporting
    /// lifecycle hooks, prewarming, and scene organization.
    /// </summary>
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        [Header("Pool Settings")] [SerializeField]
        private int defaultPrewarmCount = 5;

        [SerializeField] private int defaultMaxPoolSize = 150;

        [Header("Prewarm Prefabs")] [SerializeField]
        private List<GameObject> prewarmPrefabs;

        [Header("Custom Max Sizes")] [SerializeField]
        private List<PoolLimitEntry> customLimits;

        private Dictionary<GameObject, Queue<GameObject>> poolDict = new();
        private Dictionary<GameObject, int> poolSizes = new();
        private Dictionary<GameObject, int> maxSizeDict = new();


        protected override void Awake()
        {
            base.Awake();
            foreach (var entry in customLimits)
            {
                if (entry.prefab != null)
                    maxSizeDict[entry.prefab] = entry.maxSize;
            }

            PrewarmAll();
        }

        /// <summary>
        /// Prewarms all configured prefabs at startup.
        /// </summary>
        private void PrewarmAll()
        {
            foreach (var prefab in prewarmPrefabs)
            {
                Prewarm(prefab, defaultPrewarmCount, transform);
            }
        }

        /// <summary>
        /// Prewarms the pool by instantiating a number of inactive objects upfront.
        /// </summary>
        private void Prewarm(GameObject prefab, int count, Transform parent)
        {
            if (!poolDict.ContainsKey(prefab))
                poolDict[prefab] = new Queue<GameObject>();

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab, parent);
                obj.SetActive(false);
                poolDict[prefab].Enqueue(obj);
            }

            poolSizes[prefab] = count;
        }


        /// <summary>
        /// Retrieves an object from the pool or instantiates a new one if the pool is empty.
        /// </summary>
        public GameObject GetFromPool(GameObject prefab, Transform parent)
        {
            if (!poolDict.ContainsKey(prefab))
            {
                poolDict[prefab] = new Queue<GameObject>();
                Prewarm(prefab, defaultPrewarmCount, parent);
            }

            GameObject obj;
            if (poolDict[prefab].Count > 0)
            {
                obj = poolDict[prefab].Dequeue();
            }
            else
            {
                poolSizes.TryAdd(prefab, 0);
                int maxSize = maxSizeDict.ContainsKey(prefab) ? maxSizeDict[prefab] : defaultMaxPoolSize;

                if (poolSizes[prefab] >= maxSize)
                {
                    Debug.LogWarning($"Pool for {prefab.name} exceeded max size ({maxSize}).");
                    return null;
                }

                obj = Instantiate(prefab, parent);
                poolSizes[prefab]++;
            }

            obj.SetActive(true);

            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnSpawn();
            }

            return obj;
        }


        /// <summary>
        /// Returns an object to the pool and optionally triggers its despawn logic.
        /// </summary>
        public void ReturnToPool(GameObject prefab, GameObject obj)
        {
            if (!poolDict.ContainsKey(prefab))
                poolDict[prefab] = new Queue<GameObject>();

            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnDespawn();
            }

            obj.SetActive(false);
            obj.transform.SetParent(transform);
            poolDict[prefab].Enqueue(obj);
        }
    }
}