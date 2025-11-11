using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace DEVELOPER.Scripts.Controllers
{
    public class LevelHolder : MonoBehaviour
    {
        private void Start()
        {
            GameManager.instance.LevelEndedEvent += OnLevelEnded;
        }

        private void OnLevelEnded()
        {
            GameManager.instance.LevelEndedEvent -= OnLevelEnded;
            Destroy(gameObject);
        }
    }
}