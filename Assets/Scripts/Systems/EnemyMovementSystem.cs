using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemyMovementSystem : IEcsPreInitSystem, IEcsRunSystem
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

        EcsFilter enemyFilter = world.Filter<Enemy>()
            .Inc<Position>()
            .Inc<Movement>()
            .End();

        foreach (int entity in enemyFilter)
        {
            ref Position position = ref positionPool.Get(entity);
            ref Movement movement = ref movementPool.Get(entity);

            if (((Vector2)position).magnitude > movement.StopRadius)
            {
                var newPosition = position + Time.deltaTime * movement.Velocity;
                position = newPosition;
            }
        }

    }
}