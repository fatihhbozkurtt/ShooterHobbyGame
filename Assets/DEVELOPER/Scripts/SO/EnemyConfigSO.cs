namespace DEVELOPER.Scripts.SO
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "EnemySpawnConfigSO", menuName = "Game/Enemy Spawn Config")]
    public class EnemySpawnConfigSO : ScriptableObject
    {
        [Header("Wave Settings")]
        public int enemiesPerWave = 5;
        public float spawnInterval = 1f;
        public float waveInterval = 10f;

        [Header("Spawn Area Settings")]
        public float spawnRadius = 15f;
        public float minDistanceFromPlayer = 5f;
        public int maxNavMeshSampleAttempts = 10;

        [Header("Difficulty Scaling")]
        public bool enableScaling = true;
        public int maxEnemyCount = 50;
        public AnimationCurve difficultyCurve; // e.g. waveIndex → enemyCount multiplier

        [Header("Spawn Prefabs")]
        public GameObject[] enemyVariants;
    }

}