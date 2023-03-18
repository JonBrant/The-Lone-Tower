using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Additive Health Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Additive Health")]
public class AdditiveHealthPersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private float healthIncrease = 1.0f;
    [SerializeField] private float upgradeCostExponent = 1.1f;

    private EcsFilter towerHealthFilter;

    public override void Init()
    {
        towerHealthFilter = GameManager.Instance.World.Filter<Tower>().Inc<Health>().End();
    }

    public override string GetDescription()
    {
        return string.Format(ShortDescription, healthIncrease, null);
    }

    public override float GetCost()
    {
        int upgradeCount = PersistentUpgradeManager.Instance.PersistentUpgradeCounts[Title];
        return Mathf.Pow(upgradeCostExponent, upgradeCount);
    }

    public override void Upgrade(int upgradeCount)
    {
        EcsPool<Health> healthPool = GameManager.Instance.World.GetPool<Health>();
        foreach (int entity in towerHealthFilter)
        {
            ref Health towerHealth = ref healthPool.Get(entity);
            towerHealth.MaxHealthAdditions += healthIncrease * upgradeCount;
            towerHealth.RecalculateMaxHealth();
        }
    }
}