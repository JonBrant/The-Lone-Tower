using Leopotam.EcsLite;
using UnityEngine;

public class EnemySpawnSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private float spawnTimeRemaining = 0;
    private EcsWorld world;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }

    public void Run(EcsSystems systems)
    {
        spawnTimeRemaining -= Time.deltaTime;
        if (spawnTimeRemaining <= 0)
        {
            SpawnEnemy();
            spawnTimeRemaining = sharedData.Settings.EnemySpawnDelay;
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

        ref Enemy enemy = ref enemyPool.Add(entity);
        ref Position position = ref positionPool.Add(entity);
        ref Movement movement = ref movementPool.Add(entity);
        ref Health health = ref healthPool.Add(entity);
        ref MeleeDamage meleeDamage = ref meleeDamagePool.Add(entity);

        // Setup View
        EnemyView enemyView = GameObject.Instantiate(sharedData.Settings.EnemyPrefab);

        // Calculate a random starting position
        Vector2 randomPosition = Random.insideUnitCircle.normalized * sharedData.Settings.SpawnRadius;

        // Init Components
        position = randomPosition;
        movement.Velocity = -randomPosition.normalized*enemyView.MovementSpeed;
        movement.StopRadius = 1;
        health.MaxHealth = enemyView.StartingHealth;
        health.CurrentHealth = enemyView.StartingHealth;
        meleeDamage.Damage = enemyView.Damage;
        meleeDamage.DamageCooldown = enemyView.DamageCooldown;
        
        // Init View
        enemyView.transform.position = randomPosition;
        enemyView.packedEntity = packedEntity;
        enemyView.world = world;
    }
}