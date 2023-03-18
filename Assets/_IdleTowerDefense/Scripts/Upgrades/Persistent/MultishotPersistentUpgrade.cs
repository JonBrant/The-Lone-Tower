using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Multishot Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Multishot")]
public class MultishotPersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private int multishotIncrease = 1;
    [SerializeField] private float baseCost = 500;
    [SerializeField] private float upgradeCostExponent = 2f;

    private EcsFilter towerTargetSelectorFilter;

    public override void Init()
    {
        towerTargetSelectorFilter = GameManager.Instance.World.Filter<Tower>().Inc<TowerTargetSelector>().End();
    }

    public override string GetDescription()
    {
        return string.Format(ShortDescription, multishotIncrease, null);
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
            towerTargetSelector.MaxTargets += multishotIncrease;
        }
    }
}