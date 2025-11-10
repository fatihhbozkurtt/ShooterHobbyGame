using System.Collections.Generic;
using DEVELOPER.Scripts.Controllers;
using UnityEngine;

namespace DEVELOPER.Scripts.Data
{
    public static class DataExtensions
    {
        public static GameplayDataSO GetGameplayData()
        {
            var dataSo = Resources.Load<GameplayDataSO>("GameplayDataSO_01");
            return dataSo;
        }

        public static Transform GetClosestEnemy(this Vector3 origin, List<EnemyAI> enemies)
        {
            Transform closest = null;
            float minDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                if (enemy == null || !enemy.gameObject.activeInHierarchy) continue;

                float distance = Vector3.Distance(origin, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy.transform;
                }
            }

            return closest;
        }
    }
}