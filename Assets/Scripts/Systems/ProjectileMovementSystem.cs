using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileMovementSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<Projectile> projectilePool = world.GetPool<Projectile>();

        EcsFilter projectileFilter = world.Filter<Projectile>()
            .Inc<Movement>()
            .Inc<Position>()
            .End();

        foreach (int entity in projectileFilter)
        {
            ref Projectile projectile = ref projectilePool.Get(entity);
            ref Position projectilePosition = ref positionPool.Get(entity);
            ref Movement projectileMovement = ref movementPool.Get(entity);

            var newPosition = projectilePosition + Time.deltaTime * projectileMovement.Velocity;
            projectilePosition = newPosition;
        }
    }
}