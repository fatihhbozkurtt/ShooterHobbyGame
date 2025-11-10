using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.SO;
using EssentialManagers.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DEVELOPER.Scripts.Managers
{
    public class EnemySpawner : MonoSingleton<EnemySpawner>
    {
        [Header("Spawner Settings")] [SerializeField]
        private EnemySpawnConfigSO enemySpawnData;

        private GameplayDataSO _gameplayDataSo;
        private CancellationTokenSource waveCTS;
        private Transform _target;
        private List<EnemyAI> _spawnedEnemies = new();


        protected override void Awake()
        {
            base.Awake();
            _gameplayDataSo = DataExtensions.GetGameplayData();
            enemySpawnData = _gameplayDataSo.enemySpawnConfig;
        }

        public void SetTargetAndStartWaveLoop(Transform targetTransform)
        {
            _target = targetTransform;
            waveCTS = new CancellationTokenSource();
            StartWaveLoopAsync(waveCTS.Token).Forget();
        }

        public void StopSpawning()
        {
            waveCTS?.Cancel();
        }

        private async UniTaskVoid StartWaveLoopAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: token);

            while (!token.IsCancellationRequested)
            {
                await SpawnWaveAsync(token);
                await UniTask.Delay(TimeSpan.FromSeconds(enemySpawnData.waveInterval), cancellationToken: token);
            }
        }

        private async UniTask SpawnWaveAsync(CancellationToken token)
        {
            for (int i = 0; i < enemySpawnData.enemiesPerWave; i++)
            {
                if (token.IsCancellationRequested) return;

                Vector3 spawnPosition = GetValidSpawnPosition();
                if (spawnPosition == Vector3.zero) continue;

                GameObject enemy =
                    ObjectPoolManager.instance.GetFromPool(_gameplayDataSo.enemyPrefab.gameObject, transform);
                enemy.transform.position = spawnPosition;

                var ai = enemy.GetComponent<EnemyAI>();
                ai.Initialize(_target, _gameplayDataSo.enemyPrefab.gameObject);

                AddEnemy(ai);

                await UniTask.Delay(TimeSpan.FromSeconds(enemySpawnData.spawnInterval),
                    cancellationToken: token);
            }
        }

        private Vector3 GetValidSpawnPosition()
        {
            for (int i = 0; i < enemySpawnData.maxNavMeshSampleAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * enemySpawnData.spawnRadius;
                randomDirection.y = 0f;
                Vector3 candidatePosition = _target.position + randomDirection;

                if (Vector3.Distance(candidatePosition, _target.position) < enemySpawnData.minDistanceFromPlayer)
                    continue;

                if (NavMesh.SamplePosition(candidatePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    return hit.position;
            }

            return Vector3.zero;
        }

        public List<EnemyAI> GetSpawnedEnemies() => _spawnedEnemies;

        private void AddEnemy(EnemyAI ai)
        {
            if (ai != null && !_spawnedEnemies.Contains(ai))
                _spawnedEnemies.Add(ai);
        }

        public void RemoveEnemy(EnemyAI enemy) => _spawnedEnemies.Remove(enemy);
    }
}