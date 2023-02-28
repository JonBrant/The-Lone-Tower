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
        ref Tower tower = ref towerPool.Add(entity);
        ref TowerWeapon towerWeapon = ref towerWeaponPool.Add(entity);
        ref TowerTargetSelector towerTargetSelector = ref towerTargetingPool.Add(entity);
        
        // Setup View
        TowerView towerView = GameObject.Instantiate(sharedData.Settings.TowerPrefab, Vector3.zero, Quaternion.identity);
        
        // Init components
        towerWeapon.AttackCooldown = towerView.AttackCooldown;
        towerTargetSelector.TargetingRange = towerView.TargetingRange;
        
        
        // Init View
        towerView.packedEntity = packedEntity;
    }
}