using System;
using System.Collections.Generic;
using UnityEngine;

namespace EssentialManagers.Scripts.SO
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        public List<LevelData> levelData;
    }
}

[Serializable]
public class LevelData
{
    public GameObject LevelPrefab;
    public int EarningMoney;
    public int Time;
    public LevelDifficulty Difficulty;

    public enum LevelDifficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }
}