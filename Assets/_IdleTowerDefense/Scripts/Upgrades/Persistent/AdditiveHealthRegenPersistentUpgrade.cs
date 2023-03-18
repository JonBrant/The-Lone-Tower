using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Additive Health Regeneration Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Additive Health Regeneration")]
public class AdditiveHealthRegenPersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private float healthRegenerationIncrease = 1.0f;
    [SerializeField] private float baseCost = 1;
    [SerializeField] private float upgradeCostExponent = 1.1f;

    private EcsFilter towerHealthFilter;

    public override void Init()
    {
        towerHealthFilter = GameManager.Instance.World.Filter<Health>().Inc<Tower>().End();
    }

    public override string GetDescription()
    {
        return string.Format(ShortDescription, healthRegenerationIncrease, null);
    }

    public override float GetCost()
    {
        int upgradeCount = PersistentUpgradeManager.Instance.PersistentUpgradeCounts[Title];
        return baseCost * Mathf.Pow(upgradeCostExponent, upgradeCount);
    }

    public override void Upgrade(int upgradeCount)
    {
        EcsPool<Health> healthPool = GameManager.Instance.World.GetPool<Health>();

        foreach (int entity in towerHealthFilter)
        {
            ref Health towerHealth = ref healthPool.Get(entity);
            towerHealth.HealthRegenerationAdditions += healthRegenerationIncrease * upgradeCount;
            towerHealth.RecalculateHealthRegeneration();
        }
    }
}