using Leopotam.EcsLite;
using UnityEngine;

public class FriendlyMovementSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter friendlyFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        friendlyFilter = world.Filter<Position>()
            .Inc<Movement>()
            .Inc<Friendly>()
            .End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<FriendlyVision> friendlyVisionPool = world.GetPool<FriendlyVision>();

        foreach (int entity in friendlyFilter)
        {
            ref Position position = ref positionPool.Get(entity);
            ref Movement movement = ref movementPool.Get(entity);
            ref FriendlyVision friendlyVision = ref friendlyVisionPool.Get(entity);

            // If has target, and not in range, move toward it. Else stop
            if (friendlyVision.CurrentTarget.Unpack (world, out int targetEntity)) {
                ref Position targetPosition = ref positionPool.Get(targetEntity);

                if (Vector2.Distance(position, targetPosition) > movement.StopRadius) {
                    movement.Stopped = false;
                    movement.Velocity = movement.Speed*((Vector2)targetPosition - (Vector2)position).normalized;
                    friendlyVision.InAttackRange = false;
                    // Debug.Log($"Movement - Entity ({targetEntity}) out of range of target, moving! Speed: {movement.Speed}, Velocity: {movement.Velocity}");
                }
                else {
                    movement.Stopped = true;
                    movement.Velocity = Vector2.zero;
                    friendlyVision.InAttackRange = true;
                    // Debug.Log($"Movement - Entity in range of target, stopping!");
                }
            }
            else {
                // Debug.Log($"Movement - Entity has no target, stopping!");
                movement.Stopped = true;
                movement.Velocity = Vector2.zero;
                friendlyVision.InAttackRange = false;
            }

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