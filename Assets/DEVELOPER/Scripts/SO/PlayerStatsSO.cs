namespace DEVELOPER.Scripts.SO
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Game/Player Stats")]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Health Settings")]
        public int maxHealth = 10;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 720f;

        [Header("Combat Settings")]
        public WeaponDataSo startingWeapon;
        public float damageMultiplier = 1f;
        
        [Header("Visuals")]
        public GameObject playerModel;
        public Material playerMaterial;
    }


}