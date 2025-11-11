using Cysharp.Threading.Tasks;
using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Controllers
{
    public class Weapon : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform firePoint;

        [Header("Debug")] [SerializeField] private WeaponDataSo weaponDataSo;

        private int currentAmmo;
        private bool isFiring;
        private bool isReloading;

        private InputManager _inputManager;

        public void Initialize(WeaponDataSo data)
        {
            _inputManager = InputManager.instance;
            weaponDataSo = data;
            currentAmmo = weaponDataSo.magazineSize;
        }

        private void Update()
        {
            if (_inputManager.IsFiring && !isFiring && !isReloading)
            {
                isFiring = true;
                FireLoopAsync().Forget();
            }

            if (!_inputManager.IsFiring && isFiring)
            {
                isFiring = false;
            }
        }

        private async UniTaskVoid FireLoopAsync()
        {
            while (isFiring)
            {
                if (currentAmmo > 0)
                {
                    Fire();
                    await UniTask.Delay((int)(weaponDataSo.fireRate * 1000));
                }
                else
                {
                    await ReloadAsync();
                    isFiring = false;
                }

                await UniTask.Yield();
            }
        }

        private void Fire()
        {
            GameObject bullet = ObjectPoolManager.instance.GetFromPool(weaponDataSo.bulletPrefab, null);

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.Initialize(weaponDataSo.damage, weaponDataSo.bulletSpeed);

                currentAmmo--;
            }
        }

        private async UniTask ReloadAsync()
        {
            isReloading = true;

            // TODO: Trigger reload animation or sound
            await UniTask.Delay((int)(weaponDataSo.reloadTime * 1000));

            currentAmmo = weaponDataSo.magazineSize;
            isReloading = false;
        }
    }
}