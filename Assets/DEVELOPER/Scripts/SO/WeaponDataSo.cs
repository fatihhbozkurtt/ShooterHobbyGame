using UnityEngine;

namespace DEVELOPER.Scripts.SO
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Data So")]
    public class WeaponDataSo : ScriptableObject
    {
        public string weaponName;
        public GameObject bulletPrefab;
        public float damage = 1f;
        public float fireRate = 0.2f;
        public float reloadTime = 1.5f;
        public float bulletSpeed = 20f;
        public int magazineSize = 10;
    }
}