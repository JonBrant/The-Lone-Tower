using Leopotam.EcsLite;
using UnityEngine;

public class FriendlyMovementSystem : IEcsPreInitSystem, IEcsRunSystem {
    private EcsWorld world;
    private EcsFilter friendlyFilter;
    private EcsFilter towerTargetSelectorFilter;
    public void PreInit(EcsSystems systems) {
        world = systems.GetWorld();
        friendlyFilter = world.Filter<Position>().Inc<Movement>().Inc<Friendly>().End();
        towerTargetSelectorFilter = GameManager.Instance.World.Filter<Tower>().Inc<TowerTargetSelector>().End();
    }

    public void Run(EcsSystems systems) {
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<TowerTargetSelector> targetSelectorPool = GameManager.Instance.World.GetPool<TowerTargetSelector>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<FriendlyVision> friendlyVisionPool = world.GetPool<FriendlyVision>();

        float towerRange = -1;
        foreach (int entity in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerWeapon = ref targetSelectorPool.Get(entity);
            towerRange = towerWeapon.TargetingRange;
        }
        Debug.Assert(towerRange != -1, "Failed to get Tower Range!");
        
        foreach (int entity in friendlyFilter) {
            ref Position position = ref positionPool.Get(entity);
            ref Movement movement = ref movementPool.Get(entity);
            ref FriendlyVision friendlyVision = ref friendlyVisionPool.Get(entity);

            // If has target, and not in range, move toward it. Else stop
            if (friendlyVision.CurrentTarget.Unpack(world, out int targetEntity)) {
                ref Position enemyPosition = ref positionPool.Get(targetEntity);

                if (Vector2.Distance(position, enemyPosition) > movement.StopRadius) {
                    movement.Stopped = false;
                    movement.Velocity = movement.Speed * ((Vector2)enemyPosition - (Vector2)position).normalized;
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

            // Prevent movement outside tower radius
            if (Vector2.SqrMagnitude(position) > towerRange*towerRange) {
                // Debug.Log($"Out of range! Distance: {Vector2.SqrMagnitude(position)}, Range: {towerRange}");
                movement.Velocity = -(Vector2)position;
            }

            var newPosition = position + Time.deltaTime * movement.Velocity;
            position = newPosition;
        }

    }
}