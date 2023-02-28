using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerTargetingSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private EcsWorld world;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }


    public void Run(EcsSystems systems)
    {
        EcsFilter enemyFilter = world.Filter<Enemy>()
            .Inc<Position>()
            .End();

        EcsFilter towerTargetSelectorFilter = world.Filter<Tower>()
            .Inc<TowerTargetSelector>()
            .End();

        EcsPool<Position> enemyPositionPool = world.GetPool<Position>();
        EcsPool<TowerTargetSelector> towerTargetSelectorPool = world.GetPool<TowerTargetSelector>();
        
        foreach (int tower in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerTargetSelector = ref towerTargetSelectorPool.Get(tower);
            towerTargetSelector.CurrentTarget = -1;
            int closestEnemy = -1;
            float shortestDistance = Single.PositiveInfinity;
            
            foreach (int enemy in enemyFilter)
            {
                ref Position enemyPosition = ref enemyPositionPool.Get(enemy);
                float enemyDistance = ((Vector2)enemyPosition).magnitude;
                if (enemyDistance <= towerTargetSelector.TargetingRange && enemyDistance < shortestDistance)
                {
                    shortestDistance = enemyDistance;
                    closestEnemy = enemy;
                    towerTargetSelector.CurrentTarget = enemy;
                    Debug.Log($"{nameof(TowerTargetingSystem)}.{nameof(Run)}() - New target acquired: {closestEnemy}");
                }
            }
        }
        
        
    }
}