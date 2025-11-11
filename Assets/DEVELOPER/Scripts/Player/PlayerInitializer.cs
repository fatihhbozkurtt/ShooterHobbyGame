using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Player
{
    public class PlayerInitializer : MonoSingleton<PlayerInitializer>
    {
        [Header("Debug")] [SerializeField] private GameplayDataSO gameplayData;

        private GameObject playerInstance;
        LevelData _levelData;

        private void Start()
        {
            GameManager.instance.LevelInstantiatedEvent += OnLevelInstantiated;
            _levelData = GameManager.instance.GetGeneralLevelData();
            gameplayData = _levelData.GameplayData;
            SpawnPlayer();
        }

        private void OnLevelInstantiated(LevelData data)
        {
            _levelData = data;
            gameplayData = _levelData.GameplayData;
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            PlayerStatsSO stats = gameplayData.PlayerStats;

            playerInstance = Instantiate(gameplayData.playerPrefab.gameObject, transform.position, Quaternion.identity);
            playerInstance.name = "Player";

            ApplyStats(playerInstance, stats);
        }

        private void ApplyStats(GameObject player, PlayerStatsSO stats)
        {
            var playerCore = player.GetComponent<PlayerCore>();
            playerCore.Initialize(stats);

            var weapon = player.GetComponentInChildren<Weapon>();
            weapon.Initialize(stats.startingWeapon);


            EnemySpawner.instance.SetTarget(playerCore.transform);
        }

        public Transform GetPlayerTransform()
        {
            return playerInstance?.transform;
        }
    }
}