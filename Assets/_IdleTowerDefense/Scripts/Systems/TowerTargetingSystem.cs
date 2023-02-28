using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerTargetingSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter enemyFilter;
    private EcsFilter towerTargetSelectorFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        enemyFilter = world.Filter<Enemy>()
            .Inc<Position>()
            .End();
        towerTargetSelectorFilter = world.Filter<Tower>()
            .Inc<TowerTargetSelector>()
            .Inc<TowerWeapon>()
            .End();
    }


    public void Run(EcsSystems systems)
    {
        EcsPool<TowerTargetSelector> towerTargetSelectorPool = world.GetPool<TowerTargetSelector>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();

        foreach (int towerEntity in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerTargetSelector = ref towerTargetSelectorPool.Get(towerEntity);
            ref TowerWeapon towerWeapon = ref towerWeaponPool.Get(towerEntity);
            List<int> sortedEntities = null;

            // Make sure tower is ready to fire before acquiring targets to avoid unnecessary calculation
            if (towerWeapon.AttackCooldownRemaining >= 0)
            {
                towerTargetSelector.CurrentTargets = null;
                return;
            }
            
            // Get a list of enemy entities, sorted by distance and checked against firing range
            sortedEntities = GetSortedTargets(ref towerTargetSelector);

            // Set CurrentTargets to sorted list entries, up to MaxTargets
            towerTargetSelector.CurrentTargets = new List<int>();
            for (int i = 0; i < towerTargetSelector.MaxTargets && i < sortedEntities.Count; i++)
            {
                towerTargetSelector.CurrentTargets.Add(sortedEntities[i]);
            }
        }
    }

    private List<int> GetSortedTargets(ref TowerTargetSelector towerTargetSelector)
    {
        EcsPool<Position> enemyPositionPool = world.GetPool<Position>();
        List<int> sortedEntities = new List<int>();

        // Add all enemies in range to list to be sorted
        foreach (int enemy in enemyFilter)
        {
            ref Position enemyPosition = ref enemyPositionPool.Get(enemy);
            float enemyDistance = ((Vector2)enemyPosition).magnitude;
            if (enemyDistance <= towerTargetSelector.TargetingRange)
            {
                sortedEntities.Add(enemy);
            }
        }

        // Sort enemies in range by distance
        sortedEntities.Sort(
            delegate(int a, int b) {
                ref Position aPosition = ref enemyPositionPool.Get(a);
                ref Position bPosition = ref enemyPositionPool.Get(b);
                return (((Vector2)aPosition).magnitude < ((Vector2)bPosition).magnitude) ? -1 : 1;
            }
        );

        return sortedEntities;
    }
}