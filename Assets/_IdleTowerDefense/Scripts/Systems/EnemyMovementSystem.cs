using Leopotam.EcsLite;
using UnityEngine;

public class EnemyMovementSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter enemyFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        enemyFilter = world.Filter<Position>()
            .Inc<Movement>()
            .Exc<Friendly>() // FriendlyMovementSystem handles these
            .End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();


        foreach (int entity in enemyFilter)
        {
            ref Position position = ref positionPool.Get(entity);
            ref Movement movement = ref movementPool.Get(entity);

            if (((Vector2)position).magnitude > movement.StopRadius)
            {
                movement.Stopped = false;
                var newPosition = position + Time.deltaTime * movement.Velocity;
                position = newPosition;
            }
            else
            {
                movement.Stopped = true;
            }
        }

    }
}