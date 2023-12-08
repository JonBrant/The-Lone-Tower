using Leopotam.EcsLite;
using UnityEngine;

public class FriendlyVisionSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter friendlyFilter;
    private EcsFilter enemyFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        friendlyFilter = world.Filter<Position>()
            .Inc<Friendly>()
            .End();
        enemyFilter = world.Filter<Position>()
            .Inc<Enemy>()
            .End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<FriendlyVision> friendlyVisionPool = world.GetPool<FriendlyVision>();

        foreach (int entity in friendlyFilter)
        {
            ref FriendlyVision friendlyVision = ref friendlyVisionPool.Get(entity);
            ref Position position = ref positionPool.Get(entity);

            // If entity has a target, continue. Else try and find one
            if (friendlyVision.CurrentTarget.Unpack(world, out int unpackedEnemyEntity)) {
                // Debug.Log($"Vision - Entity already has target!");
                continue;
            }
            else {
                // Debug.Log($"Vision - Trying to find target...");
                foreach (int enemyEntity in enemyFilter) {
                    ref Position enemyPosition = ref positionPool.Get(enemyEntity);
                    if (Vector2.Distance(position, enemyPosition) < friendlyVision.VisionRadius) {
                        // Debug.Log($"Vision - Found target!");
                        friendlyVision.CurrentTarget = world.PackEntity(enemyEntity);
                    }
                }
            }
        }

    }
}