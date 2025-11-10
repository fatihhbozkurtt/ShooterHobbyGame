using UnityEngine;

namespace DEVELOPER.Scripts.SO
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Data So")]
    public class WeaponDataSo : ScriptableObject
    {
        public string weaponName;
        public GameObject bulletPrefab;
        public int damage = 1;
        public float fireRate = 0.2f;
        public float reloadTime = 2f;
        public float bulletSpeed = 20f;
        public int magazineSize = 10;
    }
}