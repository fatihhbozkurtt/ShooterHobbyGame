using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Player
{
    public class PlayerInitializer : MonoSingleton<PlayerInitializer>
    {
        [Header("Debug")]
        [SerializeField] private GameplayDataSO gameplayData;

        private GameObject playerInstance;

        private void Start()
        {
            gameplayData = DataExtensions.GetGameplayData();
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            
            PlayerStatsSO stats = gameplayData.playerStats;

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

            // var renderer = player.GetComponentInChildren<Renderer>();
            // if (renderer != null && stats.playerMaterial != null)
            // {
            //     renderer.material = stats.playerMaterial;
            // }
            
            EnemySpawner.instance.SetTargetAndStartWaveLoop(playerCore.transform);
        }

        public Transform GetPlayerTransform()
        {
            return playerInstance?.transform;
        }
    }
}