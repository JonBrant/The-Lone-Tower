using System.Collections.Generic;
using Guirao.UltimateTextDamage;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemySpawnSystem : IEcsPreInitSystem, IEcsRunSystem {
    private SharedData sharedData;
    private double spawnTimeRemaining = 0;
    private EcsWorld world;

    private float enemySpawnDelay;
    private float enemyHealthMultiplier = 1;
    private int spawnCount = 1;

    public void PreInit(EcsSystems systems) {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        enemySpawnDelay = sharedData.Settings.InitialEnemySpawnDelay;
    }

    public void Run(EcsSystems systems) {
        // Return early if settings say not to spawn - for debugging
        if (!sharedData.Settings.EnemySpawnSettings.EnemySpawning) {
            return;
        }

        spawnTimeRemaining -= Time.deltaTime;
        if (!(spawnTimeRemaining <= 0))
            return;

        for (int i = 0; i < spawnCount; i++) {
            SpawnEnemy();
        }

        // Reduce delay to increase spawn speed, increase health multiplier
        enemySpawnDelay *= sharedData.Settings.EnemySpawnSettings.EnemySpawnDelayMultiplier;
        enemyHealthMultiplier *= sharedData.Settings.EnemySpawnSettings.EnemyHealthMultiplier;

        // Spawn multiple enemies if delay gets too low, because floating point errors occur quickly
        if (enemySpawnDelay <= sharedData.Settings.InitialEnemySpawnDelay / 2.0f) {
            spawnCount++;
            enemySpawnDelay = sharedData.Settings.InitialEnemySpawnDelay;
        }

        spawnTimeRemaining = enemySpawnDelay;
    }

    private void SpawnEnemy() {
        // Create Entity, add components
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Enemy> enemyPool = world.GetPool<Enemy>();
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<EnemyMeleeDamage> meleeDamagePool = world.GetPool<EnemyMeleeDamage>();
        EcsPool<CurrencyDrop> currencyDropPool = world.GetPool<CurrencyDrop>();

        ref Enemy enemy = ref enemyPool.Add(entity);
        ref Position position = ref positionPool.Add(entity);
        ref Movement movement = ref movementPool.Add(entity);
        ref Health health = ref healthPool.Add(entity);
        ref EnemyMeleeDamage enemyMeleeDamage = ref meleeDamagePool.Add(entity);
        ref CurrencyDrop currencyDrop = ref currencyDropPool.Add(entity);

        // Setup View
        EnemyView enemyView = GameObject.Instantiate(sharedData.Settings.EnemySpawnSettings.GetRandomEnemy());

        // Calculate a random starting position
        Vector2 randomPosition = Random.insideUnitCircle.normalized * sharedData.Settings.EnemySpawnRadius;

        // Init Components
        position = randomPosition;
        movement.Velocity = -randomPosition.normalized * enemyView.MovementSpeed;
        movement.StopRadius = enemyView.AttackRange;
        health.MaxHealth = enemyView.StartingHealth * enemyHealthMultiplier;
        health.CurrentHealth = enemyView.StartingHealth * enemyHealthMultiplier;
        health.OnKilled += () => GameManager.Instance.EnemiesKilled++;
        enemyMeleeDamage.Damage = enemyView.Damage;
        enemyMeleeDamage.DamageCooldown = enemyView.DamageCooldown;
        enemyMeleeDamage.OnDamageDealt += (damage, enemyTransform) => UltimateTextDamageManager.Instance.AddStack(damage, enemyTransform, "normal");
        currencyDrop.Drops = new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, 1.0f
            }, {
                CurrencyTypes.Scrap, 1.0f
            }
        };

        // Init View
        enemyView.transform.position = randomPosition;
        enemyView.PackedEntity = packedEntity;
        enemyView.World = world;
    }
}