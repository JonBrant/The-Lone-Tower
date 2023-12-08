using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemyMeleeDamageSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter enemyFilter;
    private EcsFilter towerFilter;
    private SharedData sharedData;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        enemyFilter = world.Filter<Enemy>()
            .Inc<Movement>()
            .Inc<EnemyMeleeDamage>()
            .End();
        towerFilter = world.Filter<Tower>()
            .Inc<Health>()
            .End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<EnemyMeleeDamage> meleeDamagePool = world.GetPool<EnemyMeleeDamage>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        
        foreach (int tower in towerFilter)
        {
            ref Health towerHealth = ref healthPool.Get(tower);
            
            foreach (int enemy in enemyFilter)
            {
                ref Movement enemyMovement = ref movementPool.Get(enemy);
                if (!enemyMovement.Stopped)
                {
                    continue;
                }
                
                ref EnemyMeleeDamage enemyEnemyMeleeDamage = ref meleeDamagePool.Get(enemy);
                enemyEnemyMeleeDamage.DamageCooldownRemaining -= Time.deltaTime;
                if (enemyEnemyMeleeDamage.DamageCooldownRemaining <= 0)
                {
                    enemyEnemyMeleeDamage.DamageCooldownRemaining = enemyEnemyMeleeDamage.DamageCooldown;
                    
                    towerHealth.CurrentHealth -= enemyEnemyMeleeDamage.Damage;
                    if (towerHealth.CurrentHealth <= 0)
                    {
                        towerHealth.CurrentHealth = 0;
                        towerHealth.OnKilled?.Invoke();
                    }
                    enemyEnemyMeleeDamage.OnDamageDealt?.Invoke(enemyEnemyMeleeDamage.Damage, sharedData.TowerView.transform);
                    towerHealth.OnDamaged?.Invoke();
                }
            }
        }
    }
}