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
        // Create Entity, add components
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Tower> towerPool = world.GetPool<Tower>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();
        EcsPool<TowerTargetSelector> towerTargetingPool = world.GetPool<TowerTargetSelector>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        towerPool.Add(entity);
        ref TowerWeapon towerWeapon = ref towerWeaponPool.Add(entity);
        ref TowerTargetSelector towerTargetSelector = ref towerTargetingPool.Add(entity);
        ref Health towerHealth = ref healthPool.Add(entity);

        // Setup View
        TowerView towerView = GameObject.Instantiate(sharedData.Settings.TowerView, Vector3.zero, Quaternion.identity);

        // Init components
        // Health
        towerHealth.BaseMaxHealth = towerView.BaseMaxHealth;
        towerHealth.MaxHealthMultiplier = 1;
        towerHealth.MaxHealthAdditions = 0;
        towerHealth.RecalculateMaxHealth();
        towerHealth.CurrentHealth = towerHealth.MaxHealth;

        // Health Regeneration
        towerHealth.BaseHealthRegeneration = towerView.BaseHealthRegeneration;
        towerHealth.HealthRegenerationMultiplier = 1;
        towerHealth.HealthRegenerationAdditions = 0;
        towerHealth.RecalculateHealthRegeneration();
        towerHealth.OnDamaged += () => towerView.transform.DOPunchPosition(Random.insideUnitCircle / 10f, 0.1f, 3, 1, false)
            .OnComplete(() => towerView.transform.position = Vector3.zero);
        towerHealth.OnKilled += () => GameManager.Instance.OnTowerKilled();

        // Attack Cooldown
        towerWeapon.BaseAttackCooldown = towerView.BaseAttackCooldown;
        towerWeapon.AttackCooldownMultiplier = 1;
        towerWeapon.RecalculateAttackCooldown();

        // Attack Damage
        towerWeapon.BaseAttackDamage = towerView.BaseAttackDamage;
        towerWeapon.AttackDamageMultiplier = 1;
        towerWeapon.AttackDamageAdditions = 0;
        towerWeapon.RecalculateAttackDamage();

        towerTargetSelector.TargetingRange = towerView.BaseTargetingRange;
        towerTargetSelector.MaxTargets = towerView.BaseMaxTargets;


        // Init View
        towerView.PackedEntity = packedEntity;
        sharedData.TowerView = towerView;
    }
}