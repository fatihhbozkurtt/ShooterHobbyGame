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
        LevelData _levelData;


        private void Start()
        {
            _levelData = GameManager.instance.GetGeneralLevelData();
            _gameplayDataSo = _levelData.GameplayData;
            enemySpawnData = _gameplayDataSo.EnemySpawnConfig;

            TimerManager.instance.OnTimerEnd += StopSpawning;
            GameManager.instance.LevelEndedEvent += StopSpawning;
        }

        public void SetTarget(Transform targetTransform)
        {
            _target = targetTransform;
            waveCTS = new CancellationTokenSource();

            StartWaveLoopAsync(waveCTS.Token).Forget();
        }

        private void StopSpawning()
        {
            waveCTS?.Cancel();
            waveCTS?.Dispose();
            waveCTS = null;
        }

        private async UniTaskVoid StartWaveLoopAsync(CancellationToken token)
        {
            if (_target == null)
            {
                Debug.LogError("No target assigned");
                return;
            }

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: token);

                while (!token.IsCancellationRequested)
                {
                    await SpawnWaveAsync(token);
                    await UniTask.Delay(TimeSpan.FromSeconds(enemySpawnData.waveInterval), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Wave spawning cancelled.");
            }
        }


        private async UniTask SpawnWaveAsync(CancellationToken token)
        {
            try
            {
                for (int i = 0; i < enemySpawnData.enemiesPerWave; i++)
                {
                    token.ThrowIfCancellationRequested();

                    if (_target == null)
                    {
                        Debug.LogWarning("SpawnWaveAsync aborted mid-loop: target is null.");
                        return;
                    }

                    Vector3 spawnPosition = GetValidSpawnPosition();
                    spawnPosition.y = 1.6f;

                    if (spawnPosition == Vector3.zero) continue;

                    GameObject enemy =
                        ObjectPoolManager.instance.GetFromPool(_gameplayDataSo.enemyPrefab.gameObject, transform);
                    enemy.transform.position = spawnPosition;

                    var ai = enemy.GetComponent<EnemyAI>();
                    if (ai == null) continue;

                    EnemyType type = DataExtensions.GetEnemyTypeByTime();
                    ai.Initialize(_target, _gameplayDataSo.enemyPrefab.gameObject, type);
                    AddEnemy(ai);

                    await UniTask.Delay(TimeSpan.FromSeconds(enemySpawnData.spawnInterval), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("SpawnWaveAsync cancelled.");
            }
        }


        private Vector3 GetValidSpawnPosition()
        {
            for (int i = 0; i < enemySpawnData.maxNavMeshSampleAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * enemySpawnData.spawnRadius;
                randomDirection.y = 0f;

                if (_target == null) break;

                Vector3 candidatePosition = _target.position + randomDirection;

                if (Vector3.Distance(candidatePosition, _target.position) < enemySpawnData.minDistanceFromPlayer)
                    continue;

                if (NavMesh.SamplePosition(candidatePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    return hit.position;
            }

            return new Vector3(-10, 0, 5);
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