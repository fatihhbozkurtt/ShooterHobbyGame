using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using UnityEngine;

namespace DEVELOPER.Scripts.Controllers
{
    public class Weapon : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform firePoint;

        [Header("Debug")] [SerializeField] private WeaponDataSo weaponDataSo;
        private float lastFireTime;
        private int currentAmmo;

        public void Initialize(WeaponDataSo data)
        {
            weaponDataSo = data;
            currentAmmo = weaponDataSo.magazineSize;
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time >= lastFireTime + weaponDataSo.fireRate)
            {
                Fire();
            }
        }

        private void Fire()
        {
            if (currentAmmo <= 0)
            {
                // TODO: Trigger reload animation
                return;
            }

            GameObject bullet = ObjectPoolManager.instance.GetFromPool(weaponDataSo.bulletPrefab, transform);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(weaponDataSo.damage, weaponDataSo.bulletSpeed);

            currentAmmo--;
            lastFireTime = Time.time;
        }


      
    }
}