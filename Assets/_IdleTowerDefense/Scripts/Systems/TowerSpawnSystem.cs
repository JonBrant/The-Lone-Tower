using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerSpawnSystem : IEcsPreInitSystem, IEcsInitSystem
{
    private EcsWorld world;
    private SharedData sharedData;

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }

    public void Init(EcsSystems systems)
    {
        world = systems.GetWorld();

        // Create Entity, add components
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Tower> towerPool = world.GetPool<Tower>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();
        EcsPool<TowerTargetSelector> towerTargetingPool = world.GetPool<TowerTargetSelector>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        ref Tower tower = ref towerPool.Add(entity);
        ref TowerWeapon towerWeapon = ref towerWeaponPool.Add(entity);
        ref TowerTargetSelector towerTargetSelector = ref towerTargetingPool.Add(entity);
        ref Health towerHealth = ref healthPool.Add(entity);

        // Setup View
        TowerView towerView = GameObject.Instantiate(sharedData.Settings.TowerPrefab, Vector3.zero, Quaternion.identity);

        // Init components
        towerHealth.MaxHealth = towerView.StartingHealth;
        towerHealth.CurrentHealth = towerView.StartingHealth;
        towerHealth.OnDamaged += () => towerView.transform.DOPunchPosition(Random.insideUnitCircle / 10f, 0.1f, 3, 1, false)
            .OnComplete(() => towerView.transform.position = Vector3.zero);
        towerWeapon.AttackCooldown = sharedData.Settings.TowerStartingAttackCooldown;
        towerWeapon.AttackDamage = sharedData.Settings.TowerStartingAttackDamage;
        towerTargetSelector.TargetingRange = towerView.TargetingRange;
        towerTargetSelector.MaxTargets = towerView.MaxTargets;


        // Init View
        towerView.packedEntity = packedEntity;
        towerView.world = world;
        sharedData.TowerView = towerView;
    }
}