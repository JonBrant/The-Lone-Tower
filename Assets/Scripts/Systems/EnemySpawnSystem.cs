using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemySpawnSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private float spawnTimeRemaining = 0;
    private EcsWorld world;

    public void PreInit(EcsSystems systems)
    {
        // Will be called once during EcsSystems.Init() call and before IEcsInitSystem.Init().
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }

    public void Run(EcsSystems systems)
    {
        spawnTimeRemaining -= Time.deltaTime;
        if (spawnTimeRemaining <= 0)
        {
            SpawnEnemy();
            spawnTimeRemaining = sharedData.WaveSpawnDelay;
        }
    }

    private void SpawnEnemy()
    {
        // Setup Entity
        int entity = world.NewEntity();
        EcsPool<Enemy> enemyPool = world.GetPool<Enemy>();
        ref Enemy enemy = ref enemyPool.Add(entity);

        // Setup View
        GameObject enemyViewGameObject = GameObject.Instantiate(sharedData.Settings.EnemyPrefab);
        EnemyView enemyView = enemyViewGameObject.AddComponent<EnemyView>();
        enemyView.entity = entity;

        // Temp
        enemyView.transform.position = Random.insideUnitCircle.normalized * sharedData.Settings.SpawnRadius;
    }
}