using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using DG.Tweening;
using EssentialManagers.Scripts.Managers;
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
            stats = playerStatsSo;

            _playerHealthHandler.SetMaxHealth(stats.maxHealth);
        }

        private void Start()
        {
            EnemySpawner.instance.SetTarget(transform);
            GameManager.instance.LevelEndedEvent += OnLevelEnded;
        }

        private void OnLevelEnded()
        {
            GameManager.instance.LevelEndedEvent -= OnLevelEnded;
            transform.DOScale(Vector3.zero, .5f).OnComplete(() => Destroy(gameObject));
        }
    }
}