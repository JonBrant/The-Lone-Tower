using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Enemy Spawn Settings", menuName = "Idle Tower Defense/Enemy Spawn Settings")]
public class EnemySpawnSettings : ScriptableObject {
    public float EnemySpawnDelayMultiplier = 0.95f;
    public float EnemyHealthMultiplier = 1.01f;
    
    public bool EnemySpawning = true; // For debugging
    
    [Serializable]
    public struct EnemySpawnEntry
    {
        public EnemyView EnemyView;
        public float SpawnChance;
    }

    public List<EnemySpawnEntry> EnemySpawns = new List<EnemySpawnEntry>();

    public EnemyView GetRandomEnemy()
    {
        float rand = Random.value;
        for (int i = 0; i < EnemySpawns.Count; i++)
        {
            rand -= EnemySpawns[i].SpawnChance;
            if (rand <= 0)
            {
                return EnemySpawns[i].EnemyView;
            }
        }

        // We should only reach this point if a floating point issue occured, I think, so return the last entry
        return EnemySpawns[^1].EnemyView;
    }
    
    public void NormalizeEntries()
    {
        Debug.Log($"{nameof(EnemySpawnSettings)}.{nameof(NormalizeEntries)}()");
        float sum = 0;
        List<EnemySpawnEntry> newList = new List<EnemySpawnEntry>();
        for (int i = 0; i < EnemySpawns.Count; i++)
        {
            sum += EnemySpawns[i].SpawnChance;
        }

        foreach (EnemySpawnEntry t in EnemySpawns)
        {
            EnemySpawnEntry enemySpawnEntry = t;
            enemySpawnEntry.SpawnChance /= sum;
            newList.Add(enemySpawnEntry);
        }

        EnemySpawns = newList;
    }
}