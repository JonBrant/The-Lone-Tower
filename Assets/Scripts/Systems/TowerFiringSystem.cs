using Leopotam.EcsLite;
using UnityEngine;

public class TowerFiringSystem : IEcsPreInitSystem, IEcsRunSystem
{
    private SharedData sharedData;
    private EcsWorld world;
    private EcsFilter towerTargetSelectorFilter;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        towerTargetSelectorFilter = world.Filter<Tower>()
            .Inc<TowerTargetSelector>()
            .Inc<TowerWeapon>()
            .End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<TowerTargetSelector> towerTargetSelectorPool = world.GetPool<TowerTargetSelector>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();

        foreach (int tower in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerTargetSelector = ref towerTargetSelectorPool.Get(tower);
            ref TowerWeapon towerWeapon = ref towerWeaponPool.Get(tower);
            towerWeapon.AttackCooldownRemaining -= Time.deltaTime;

            if (towerTargetSelector.CurrentTarget == -1)
            {
                continue;
            }

            if (towerWeapon.AttackCooldownRemaining >= 0)
            {
                continue;
            }

            towerWeapon.AttackCooldownRemaining = towerWeapon.AttackCooldown;

            // Spawn projectile
            int projectileEntity = world.NewEntity();
            EcsPackedEntity packedProjectileEntity = world.PackEntity(projectileEntity);
            EcsPool<Projectile> projectilePool = world.GetPool<Projectile>();
            EcsPool<Movement> movementPool = world.GetPool<Movement>();
            EcsPool<Position> positionPool = world.GetPool<Position>();
            ref Projectile projectile = ref projectilePool.Add(projectileEntity);
            ref Movement projectileMovement = ref movementPool.Add(projectileEntity);
            ref Position projectilePosition = ref positionPool.Add(projectileEntity);

            // Setup View
            ProjectileView projectileView = GameObject.Instantiate(sharedData.Settings.ProjectilePrefab, Vector3.zero, Quaternion.identity);

            // Init components
            projectilePosition = ((Vector2)positionPool.Get(towerTargetSelector.CurrentTarget)).normalized * 0.05f; 
            projectileMovement.Velocity = ((Vector2)positionPool.Get(towerTargetSelector.CurrentTarget)).normalized * projectileView.MovementSpeed;
            projectileMovement.StopRadius = 0;


            // Init View
            projectileView.packedEntity = packedProjectileEntity;
            projectileView.transform.position = (Vector2)projectilePosition;
            projectileView.world = world;
        }
    }
}