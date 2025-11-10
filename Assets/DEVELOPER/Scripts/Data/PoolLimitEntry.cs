using UnityEngine;

namespace DEVELOPER.Scripts.Data
{
    [System.Serializable]
    public class PoolLimitEntry
    {
        public GameObject prefab;
        public int maxSize = 150;
    }
}