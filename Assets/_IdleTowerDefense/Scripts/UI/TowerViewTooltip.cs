using System;
using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerViewTooltip : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.5f;
    private TowerView TowerView;
    private EcsWorld world;
    private float timeSinceLastUpdate = 0;

    private void Start()
    {
        TowerView = GetComponent<TowerView>();
        world = TowerView.World;
    }

    private void OnMouseOver()
    {
        timeSinceLastUpdate -= Time.deltaTime;
        if (timeSinceLastUpdate > 0)
            return;
        if (!TowerView.PackedEntity.Unpack(world, out int unpackedTower))
            return;

        timeSinceLastUpdate = updateInterval;
        
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();
        EcsPool<TowerTargetSelector> towerTargetingPool = world.GetPool<TowerTargetSelector>();
        
        ref Health towerHealth = ref healthPool.Get(unpackedTower);
        ref TowerWeapon towerWeapon = ref towerWeaponPool.Get(unpackedTower);
        ref TowerTargetSelector targetSelector = ref towerTargetingPool.Get(unpackedTower);
        
        UITooltip.InstantiateIfNecessary(GameObject.Find("UI"));
        UITooltip.Instance.CleanupLines();
        UITooltip.AddTitle("Tower Stats");


        UITooltip.AddLineColumn($"<b>Health</b>: {towerHealth.CurrentHealth}/{towerHealth.MaxHealth}");
        UITooltip.AddLineColumn($"<b>Health Regeneration</b>: {towerHealth.HealthRegeneration}");
        UITooltip.AddLineColumn($"<b>Damage</b>: {towerWeapon.AttackDamage}");
        UITooltip.AddLineColumn($"<b>Cooldown</b>: {towerWeapon.AttackCooldown}");
        UITooltip.AddLineColumn($"<b>Range</b>: {targetSelector.TargetingRange}");
        UITooltip.AddLineColumn($"<b>Targets</b>: {targetSelector.MaxTargets}");
        UITooltip.Show();
    }
    
    private void OnMouseExit()
    {
        UITooltip.Hide();
    }
}
