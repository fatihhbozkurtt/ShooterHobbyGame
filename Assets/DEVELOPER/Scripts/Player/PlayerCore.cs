using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using UnityEngine;

namespace DEVELOPER.Scripts.Player
{
    public class PlayerCore : MonoBehaviour
    {

        [Header("Debug")] [SerializeField] private PlayerStatsSO stats;
        PlayerHealthHandler _playerHealthHandler;
 
        public void Initialize(PlayerStatsSO playerStatsSo)
        {
            _playerHealthHandler = GetComponent<PlayerHealthHandler>();
            stats =  playerStatsSo;

            _playerHealthHandler.SetMaxHealth(stats.maxHealth);
        }
        
        private void Start()
        {
            EnemySpawner.instance.SetTargetAndStartWaveLoop(transform);
        }

     
    }
}