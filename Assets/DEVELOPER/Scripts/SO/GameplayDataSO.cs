using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Player;
using DEVELOPER.Scripts.SO;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayDataSO", menuName = "Game/Gameplay Data")]
public class GameplayDataSO : ScriptableObject
{
    [Header("Core Prefabs")] public PlayerCore playerPrefab;
    public EnemyAI enemyPrefab;
    public Bullet bulletPrefab;


    [Header("Scriptable References")] public EnemySpawnConfigSO EnemySpawnConfig;
    public PlayerStatsSO PlayerStats;
    public WeaponDataSo WeaponData;
    public EnemyStatsSO EnemyStats;
}