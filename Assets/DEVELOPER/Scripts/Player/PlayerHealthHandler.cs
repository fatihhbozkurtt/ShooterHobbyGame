using System;
using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Data;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Player
{
    public class PlayerHealthHandler : MonoBehaviour, IDamageable
    {
        [Header("Configuration")] [SerializeField]
        private float invincibilityDuration = 3f;

        [Header("Debug")] [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;

        [SerializeField] private bool isInvincible;
        private const int ContactDamage = 1;

        public event Action OnDamaged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void SetMaxHealth(int health)
        {
            maxHealth = health;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (isInvincible) return;


            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0);

            OnDamaged?.Invoke();
            TriggerInvincibility();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void TriggerInvincibility()
        {
            isInvincible = true;
        }

        public void ResetInvincibility()
        {
            isInvincible = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyAI enemy))
            {
                if (!enemy.gameObject.activeInHierarchy) return;
                TakeDamage(ContactDamage);
            }
        }

        public void Die()
        {
            Debug.Log("Player died.");
            GameManager.instance.EndGame(false);
        }

        public int GetCurrentHealth() => currentHealth;
        public int GetMaxHealth() => maxHealth;
    }
}