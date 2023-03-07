using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private StatDisplayElement StatDisplayElementPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private float updateInterval = 0.5f;

    private float timeSinceLastUpdate = 0;
    private StatDisplayElement HealthDisplay;
    private StatDisplayElement HealthRegenDisplay;
    private StatDisplayElement AttackDamageDisplay;
    private StatDisplayElement AttackCooldownDisplay;
    private StatDisplayElement MultishotDisplay;

    private EcsWorld world;
    private EcsFilter towerFilter;

    private void Start()
    {
        HealthDisplay = Instantiate(StatDisplayElementPrefab, container);
        HealthRegenDisplay = Instantiate(StatDisplayElementPrefab, container);
        AttackDamageDisplay = Instantiate(StatDisplayElementPrefab, container);
        AttackCooldownDisplay = Instantiate(StatDisplayElementPrefab, container);
        MultishotDisplay = Instantiate(StatDisplayElementPrefab, container);

        HealthDisplay.LabelText.text = "Health";
        HealthRegenDisplay.LabelText.text = "Health Regeneration";
        AttackDamageDisplay.LabelText.text = "Attack Damage";
        AttackCooldownDisplay.LabelText.text = "Attack Cooldown";
        MultishotDisplay.LabelText.text = "Multishot Count";

        world = GameManager.Instance.World;
        towerFilter = world.Filter<Tower>().End();
    }

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate < updateInterval) return;

        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<TowerWeapon> towerWeaponPool = world.GetPool<TowerWeapon>();
        EcsPool<TowerTargetSelector> towerTargetingPool = world.GetPool<TowerTargetSelector>();

        foreach (int entity in towerFilter)
        {
            // Health
            ref Health towerHealth = ref healthPool.Get(entity);
            ref TowerWeapon towerWeapon = ref towerWeaponPool.Get(entity);
            ref TowerTargetSelector targetSelector = ref towerTargetingPool.Get(entity);
            
            HealthDisplay.ValueText.text = $"{towerHealth.CurrentHealth:N0} / {towerHealth.MaxHealth:N0}";
            HealthRegenDisplay.ValueText.text = $"{towerHealth.HealthRegeneration}/s";
            
            // Attack Damage
            AttackDamageDisplay.ValueText.text = towerWeapon.AttackDamage.ToString();
            AttackCooldownDisplay.ValueText.text = $"{1/towerWeapon.AttackCooldown:N2}/s";
            MultishotDisplay.ValueText.text = targetSelector.MaxTargets.ToString();
        }
    }
}