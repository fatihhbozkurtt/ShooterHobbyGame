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
        
    }
}