using System.Collections.Generic;
using Guirao.UltimateTextDamage;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemySpawnSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private float spawnTimeRemaining = 0;
    private EcsWorld world;
    private float EnemySpawnDelay;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        EnemySpawnDelay = sharedData.Settings.InitialEnemySpawnDelay;
    }

    public void Run(EcsSystems systems)
    {
        spawnTimeRemaining -= Time.deltaTime;
        if (spawnTimeRemaining <= 0)
        {
            SpawnEnemy();
            
            
            // ToDo: Update this 
            EnemySpawnDelay *= sharedData.Settings.EnemySpawnMultiplier;
            spawnTimeRemaining = EnemySpawnDelay;
        }
    }

    private void SpawnEnemy()
    {
        // Create Entity, add components
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Enemy> enemyPool = world.GetPool<Enemy>();
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<MeleeDamage> meleeDamagePool = world.GetPool<MeleeDamage>();
        EcsPool<CurrencyDrop> currencyDropPool = world.GetPool<CurrencyDrop>();

        ref Enemy enemy = ref enemyPool.Add(entity);
        ref Position position = ref positionPool.Add(entity);
        ref Movement movement = ref movementPool.Add(entity);
        ref Health health = ref healthPool.Add(entity);
        ref MeleeDamage meleeDamage = ref meleeDamagePool.Add(entity);
        ref CurrencyDrop currencyDrop = ref currencyDropPool.Add(entity);

        // Setup View
        EnemyView enemyView = GameObject.Instantiate(sharedData.Settings.EnemyPrefab);

        // Calculate a random starting position
        Vector2 randomPosition = Random.insideUnitCircle.normalized * sharedData.Settings.EnemySpawnRadius;

        // Init Components
        position = randomPosition;
        movement.Velocity = -randomPosition.normalized * enemyView.MovementSpeed;
        movement.StopRadius = 1;
        health.MaxHealth = enemyView.StartingHealth;
        health.CurrentHealth = enemyView.StartingHealth;
        meleeDamage.Damage = enemyView.Damage;
        meleeDamage.DamageCooldown = enemyView.DamageCooldown;
        meleeDamage.OnDamageDealt += (damage, enemyTransform) => UltimateTextDamageManager.Instance.AddStack(damage, enemyTransform, "normal");
        currencyDrop.Drops = new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Gold, 1.0f
            }, {
                CurrencyTypes.Scrap, 1.0f
            }
        };

        // Init View
        enemyView.transform.position = randomPosition;
        enemyView.packedEntity = packedEntity;
        enemyView.world = world;
    }
}