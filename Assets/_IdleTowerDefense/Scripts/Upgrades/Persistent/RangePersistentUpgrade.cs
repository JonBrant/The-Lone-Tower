using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Range Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Range")]
public class RangePersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private float rangeIncrease = 0.1f;
    [SerializeField] private float maxRange = 6.0f;
    [SerializeField] private float baseCost = 500;
    [SerializeField] private float upgradeCostExponent = 1.1f;

    private EcsFilter towerTargetSelectorFilter;

    public override void Init()
    {
        towerTargetSelectorFilter = GameManager.Instance.World.Filter<Tower>().Inc<TowerTargetSelector>().End();
    }

    public override string GetDescription()
    {
        return string.Format(ShortDescription, rangeIncrease, maxRange);
    }

    public override bool CanUpgrade()
    {
        float baseTowerRange = PersistentUpgradeManager.Instance.GameSettings.TowerView.BaseTargetingRange;
        float potentialRangeIncrease = rangeIncrease * PersistentUpgradeManager.Instance.PersistentUpgradeCounts[Title];
        return base.CanUpgrade() && baseTowerRange + potentialRangeIncrease < maxRange;
    }

    public override float GetCost()
    {
        int upgradeCount = PersistentUpgradeManager.Instance.PersistentUpgradeCounts[Title];
        return baseCost * Mathf.Pow(upgradeCostExponent, upgradeCount);
    }

    public override void Upgrade(int upgradeCount)
    {
        EcsPool<TowerTargetSelector> targetSelectorPool = GameManager.Instance.World.GetPool<TowerTargetSelector>();
        foreach (int entity in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerTargetSelector = ref targetSelectorPool.Get(entity);
            towerTargetSelector.TargetingRange += rangeIncrease * upgradeCount;
        }
    }
}