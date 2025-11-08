using System;
using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.SO;
using UnityEngine;

namespace DEVELOPER.Scripts.Player
{
    public class PlayerHealthHandler : MonoBehaviour, IDamageable
    {
        [Header("Debug")] [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;


        public void SetMaxHealth(int health)
        {
            maxHealth = health;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // TODO: Trigger game over state
            Debug.Log("Player died.");
        }
    }
}