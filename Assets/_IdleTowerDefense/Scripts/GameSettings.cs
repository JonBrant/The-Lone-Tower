using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Idle Tower Defense/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public EnemySpawnSettings EnemySpawnSettings;
    public UpgradeSettings UpgradeSettings;

    [Header("Prefabs")]
    public TowerView TowerView;
    public EnemyView EnemyView;
    public ProjectileView ProjectileView;

    [Header("Tower Starting Values")]
    public float TowerStartingAttackDamage = 1;
    public float TowerStartingAttackCooldown = 1;
    public int TowerStartingAttackTargets = 1;
    public float TowerStartingTargetingRange = 2;

    [Header("Misc")]
    public float EnemySpawnRadius = 10;
    public float InitialEnemySpawnDelay = 0.5f;
}