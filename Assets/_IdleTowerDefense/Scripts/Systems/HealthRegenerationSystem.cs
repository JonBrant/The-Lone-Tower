using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class HealthRegenerationSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter healthFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        healthFilter = world.Filter<Health>().End();
    }


    public void Run(EcsSystems systems)
    {
        EcsPool<Health> healthPool = world.GetPool<Health>();
        
        foreach (int entity in healthFilter)
        {
            ref Health entityHealth = ref healthPool.Get(entity);
            entityHealth.CurrentHealth += entityHealth.CurrentHealthRegeneration * Time.deltaTime;
            if (entityHealth.CurrentHealth > entityHealth.MaxHealth)
            {
                entityHealth.CurrentHealth = entityHealth.MaxHealth;
            }
        }
    }
}