using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Idle Tower Defense/Game Settings")]
public class GameSettings : ScriptableObject
{
    public TowerView TowerPrefab;
    public EnemyView EnemyPrefab;
    public ProjectileView ProjectilePrefab;
    public float SpawnRadius = 10;
    public float EnemySpawnDelay = 0.5f;
}
