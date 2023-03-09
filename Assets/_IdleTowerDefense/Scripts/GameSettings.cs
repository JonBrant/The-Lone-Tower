using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Idle Tower Defense/Game Settings")]
public class GameSettings : ScriptableObject
{
    public EnemySpawnSettings EnemySpawnSettings;
    public TowerView TowerPrefab;
    public EnemyView EnemyPrefab;
    public ProjectileView ProjectilePrefab;
    [FormerlySerializedAs("SpawnRadius")] public float EnemySpawnRadius = 10;
    public float InitialEnemySpawnDelay = 0.5f;
    

    [Header("Tower Starting Values")]
    public float TowerStartingAttackDamage = 1;
    public float TowerStartingAttackCooldown = 1;
    public int TowerStartingAttackTargets = 1;
    public float TowerStartingTargetingRange = 2;
    
}
