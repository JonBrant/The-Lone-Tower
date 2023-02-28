using Leopotam.EcsLite;
using UnityEngine;

public class TowerSpawnSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
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
        GameObject towerViewGameObject = GameObject.Instantiate(sharedData.Settings.TowerPrefab, Vector3.zero, Quaternion.identity);
        TowerView towerView = towerViewGameObject.AddComponent<TowerView>();
        
        // Init components
        towerWeapon.AttackCooldown = 1; // ToDo: Pull from game settings or something
        towerTargetSelector.TargetingRange = 5;
        
        
        // Init View
        towerView.packedEntity = packedEntity;
        
    }

    public void Run(EcsSystems systems)
    {

    }

    
}