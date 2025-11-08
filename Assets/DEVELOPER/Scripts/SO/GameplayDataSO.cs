using DEVELOPER.Scripts.Controllers;
using DEVELOPER.Scripts.Player;
using DEVELOPER.Scripts.SO;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayDataSO", menuName = "Game/Gameplay Data")]
public class GameplayDataSO : ScriptableObject
{
    [Header("Level Settings")]
    public int levelIndex;
    public string levelName;
    public float levelDuration;

    [Header("Core Prefabs")]
    public PlayerCore playerPrefab;
    public EnemyAI enemyPrefab;
    public Bullet bulletPrefab;

    [Header("Weapon Configuration")]
    public WeaponDataSo defaultWeapon;
    public WeaponDataSo[] availableWeapons;

    [Header("Scriptable References")]
   public EnemySpawnConfigSO enemySpawnConfig;
    public PlayerStatsSO playerStats;
   // public UIThemeSO uiTheme;
   // public AudioConfigSO audioConfig;

    [Header("Environment Settings")]
    public Material skyboxMaterial;
    public GameObject[] environmentProps;

    [Header("Misc")]
    public int maxEnemyCount;
    public bool enablePowerups;
    public float globalGravity = -9.81f;
}