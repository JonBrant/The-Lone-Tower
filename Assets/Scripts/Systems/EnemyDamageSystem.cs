using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemyDamageSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter enemyFilter;
    private EcsFilter towerFilter;

    public void PreInit(EcsSystems systems)
    {
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
        EcsPool<Health> towerPool = world.GetPool<Health>();
        EcsPool<MeleeDamage> meleeDamagePool = world.GetPool<MeleeDamage>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();

        foreach (int tower in towerFilter)
        {
            ref Health towerHealth = ref towerPool.Get(tower);
            
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
                    // ToDo: Add health system instead of checking here (do the same for ProjectileView)
                    towerHealth.CurrentHealth -= enemyMeleeDamage.Damage;
                    towerHealth.OnDamaged?.Invoke();
                }
            }
        }
    }
}