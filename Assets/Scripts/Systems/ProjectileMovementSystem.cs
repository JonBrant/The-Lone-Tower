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


        //Debug.Log($"{nameof(ProjectileMovementSystem)}.{nameof(Run)}() - Count: {projectileFilter.GetEntitiesCount()}");

        foreach (int entity in projectileFilter)
        {

            
            ref Projectile projectile = ref projectilePool.Get(entity);
            ref Position projectilePosition = ref positionPool.Get(entity);
            ref Movement projectileMovement = ref movementPool.Get(entity);
            ref Position enemyPosition = ref positionPool.Get(projectile.TargetEntity);

            if (Vector2.Distance(projectilePosition, enemyPosition) > 0)
            {
                Debug.Log($"{nameof(ProjectileMovementSystem)}.{nameof(Run)}() - Moving!");
                var direction = new Vector2(enemyPosition.x, enemyPosition.y);
                var newPosition = projectilePosition + direction * Time.deltaTime * projectileMovement.Speed;
                projectilePosition.x = newPosition.x;
                projectilePosition.y = newPosition.y;
            }
            else
            {
                Debug.Log($"{nameof(ProjectileMovementSystem)}.{nameof(Run)}() - Not moving!");
            }
        }
    }
}