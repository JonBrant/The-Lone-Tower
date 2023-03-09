using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemyDamageSystem : IEcsPreInitSystem, IEcsRunSystem
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
            .Inc<MeleeDamage>()
            .End();
        towerFilter = world.Filter<Tower>()
            .Inc<Health>()
            .End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<MeleeDamage> meleeDamagePool = world.GetPool<MeleeDamage>();
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
                
                ref MeleeDamage enemyMeleeDamage = ref meleeDamagePool.Get(enemy);
                enemyMeleeDamage.DamageCooldownRemaining -= Time.deltaTime;
                if (enemyMeleeDamage.DamageCooldownRemaining <= 0)
                {
                    enemyMeleeDamage.DamageCooldownRemaining = enemyMeleeDamage.DamageCooldown;
                    
                    towerHealth.CurrentHealth -= enemyMeleeDamage.Damage;
                    if (towerHealth.CurrentHealth < 0)
                    {
                        towerHealth.CurrentHealth = 0;
                    }
                    enemyMeleeDamage.OnDamageDealt?.Invoke(enemyMeleeDamage.Damage, sharedData.TowerView.transform);
                    towerHealth.OnDamaged?.Invoke();
                }
            }
        }
    }
}