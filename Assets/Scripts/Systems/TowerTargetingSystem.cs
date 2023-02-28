using System;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerTargetingSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private EcsWorld world;
    private EcsFilter enemyFilter;
    private EcsFilter towerTargetSelectorFilter;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        enemyFilter = world.Filter<Enemy>()
            .Inc<Position>()
            .End();
        towerTargetSelectorFilter = world.Filter<Tower>()
            .Inc<TowerTargetSelector>()
            .End();
    }


    public void Run(EcsSystems systems)
    {
        EcsPool<Position> enemyPositionPool = world.GetPool<Position>();
        EcsPool<TowerTargetSelector> towerTargetSelectorPool = world.GetPool<TowerTargetSelector>();

        foreach (int tower in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerTargetSelector = ref towerTargetSelectorPool.Get(tower);
            towerTargetSelector.CurrentTarget = -1;
            float shortestDistance = Single.PositiveInfinity;

            foreach (int enemy in enemyFilter)
            {
                ref Position enemyPosition = ref enemyPositionPool.Get(enemy);
                float enemyDistance = ((Vector2)enemyPosition).magnitude;
                if (enemyDistance <= towerTargetSelector.TargetingRange && enemyDistance < shortestDistance)
                {
                    shortestDistance = enemyDistance;
                    towerTargetSelector.CurrentTarget = enemy;
                }
            }
        }
    }
}