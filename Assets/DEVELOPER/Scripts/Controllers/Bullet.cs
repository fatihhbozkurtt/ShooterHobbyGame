using DEVELOPER.Scripts.Data;
using DEVELOPER.Scripts.Managers;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float speed;

    public void Initialize(float damageAmount, float bulletSpeed)
    {
        damage = damageAmount;
        speed = bulletSpeed;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(Mathf.RoundToInt(damage));
        }

        ObjectPoolManager.instance.ReturnToPool(gameObject, gameObject);
    }
}