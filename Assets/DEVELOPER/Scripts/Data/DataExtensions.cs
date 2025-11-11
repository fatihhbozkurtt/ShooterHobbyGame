using System.Collections.Generic;
using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Managers;
using DEVELOPER.Scripts.SO;
using EssentialManagers.Scripts.SO;
using UnityEngine;

namespace DEVELOPER.Scripts.Data
{
    public static class DataExtensions
    {
        public static LevelDataSO GetGeneralLevelData()
        {
            var dataSo = Resources.Load<LevelDataSO>("LevelData");
            return dataSo;
        }
        
        public static EnemyStarterInfo GetEnemyStatsByType(LevelData levelData,EnemyType type)
        {
            var dataSo = levelData.GameplayData.EnemyStats;

            foreach (var info in dataSo.EnemyStarterInfoList)
            {
                if (info.Type == type)
                {
                    return info;
                }
            }

            return null;
        }
        public static EnemyType GetEnemyTypeByTime()
        {
            float totalTime = TimerManager.instance.GetTotalGameTime();
            float remainingTime = TimerManager.instance.GetRemainingTime();
            float elapsed = totalTime - remainingTime;

            if (elapsed < totalTime / 3f) return EnemyType.Easy;
            if (elapsed < (2f * totalTime / 3f)) return EnemyType.Medium;
            return EnemyType.Hard;
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