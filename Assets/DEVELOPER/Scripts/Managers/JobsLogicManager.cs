using System.Collections.Generic;
using DEVELOPER.Scripts.Controllers;
using EssentialManagers.Scripts.Managers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace DEVELOPER.Scripts.Managers
{
    public class JobsLogicManager : MonoSingleton<JobsLogicManager>
    {
        private readonly List<EnemyAI> activeEnemies = new();

        public void Register(EnemyAI enemy)
        {
            if (!activeEnemies.Contains(enemy))
                activeEnemies.Add(enemy);
        }

        public void Unregister(EnemyAI enemy)
        {
            if (activeEnemies.Contains(enemy))
                activeEnemies.Remove(enemy);
        }

        private void Update()
        {
            if (!GameManager.instance.isLevelActive) return;
            if (activeEnemies.Count == 0) return;

            NativeArray<EnemyNavmeshData> inputData = new(activeEnemies.Count, Allocator.TempJob);
            NativeArray<float3> outputDirections = new(activeEnemies.Count, Allocator.TempJob);

            for (int i = 0; i < activeEnemies.Count; i++)
            {
                inputData[i] = new EnemyNavmeshData()
                {
                    index = i,
                    enemyPosition = activeEnemies[i].transform.position,
                    targetPosition = activeEnemies[i].TargetPosition
                };
            }

            var job = new EnemyDirectionJob
            {
                input = inputData,
                output = outputDirections
            };

            JobHandle handle = job.Schedule(activeEnemies.Count, 32);
            handle.Complete();

            for (int i = 0; i < activeEnemies.Count; i++)
            {
                activeEnemies[i].ApplyDirection(outputDirections[i]);
            }

            inputData.Dispose();
            outputDirections.Dispose();
        }

        [BurstCompile]
        private struct EnemyDirectionJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<EnemyNavmeshData> input;
            public NativeArray<float3> output;

            public void Execute(int index)
            {
                float3 dir = input[index].targetPosition - input[index].enemyPosition;
                output[index] = math.normalize(dir);
            }
        }
    }
}