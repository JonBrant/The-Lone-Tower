using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Idle Tower Defense/Game Settings")]
public class GameSettings : ScriptableObject
{
    public GameObject TowerPrefab;
    public GameObject EnemyPrefab;
    public GameObject ProjectilePrefab;
    public float SpawnRadius = 10;
    public float EnemySpawnDelay = 0.5f;
}
