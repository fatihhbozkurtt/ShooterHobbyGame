using System;
using System.Collections.Generic;
using DEVELOPER.Scripts.Data;
using UnityEngine;

namespace DEVELOPER.Scripts.SO
{
    
    [CreateAssetMenu(fileName = "EnemyStatsSo", menuName = "Game/Enemy Stats")]
    public class EnemyStatsSO : ScriptableObject
    {
        public List<EnemyStarterInfo> EnemyStarterInfoList;
    }
}
[Serializable]
public class EnemyStarterInfo
{
    public EnemyType Type;
    public string Name;
    public int MaxHealth;
    public Material Material;
}