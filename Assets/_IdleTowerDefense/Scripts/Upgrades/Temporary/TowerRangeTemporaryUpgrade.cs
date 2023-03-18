using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Range Upgrade", menuName = "Idle Tower Defense/Temporary Upgrades/Tower Range")]
public class TowerRangeTemporaryUpgrade : TemporaryUpgradeBase
{
    [Header("Upgrade Specific Values")]
    public float RangePerUpgrade = 0.5f;
    public float MaxRange = 6.0f;

    private EcsFilter towerTargetSelectorFilter;

    public override void Init()
    {
        towerTargetSelectorFilter = GameManager.Instance.World.Filter<Tower>().Inc<TowerTargetSelector>().End();
    }

    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, (TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] + 1) * 2
            }, {
                CurrencyTypes.Scrap, (TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] + 1) * 2
            }
        };
    }

    public override bool CanUpgrade()
    {
        bool maxRangeReached = false;
        EcsPool<TowerTargetSelector> targetSelectorPool = GameManager.Instance.World.GetPool<TowerTargetSelector>();
        foreach (int entity in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerWeapon = ref targetSelectorPool.Get(entity);
            if (towerWeapon.TargetingRange >= MaxRange)
            {
                maxRangeReached = true;
            }
        }

        return base.CanUpgrade() && !maxRangeReached;
    }

    public override void Upgrade()
    {
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());
        TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] += 1;

        // Handle upgrade
        EcsPool<TowerTargetSelector> targetSelectorPool = GameManager.Instance.World.GetPool<TowerTargetSelector>();
        foreach (int entity in towerTargetSelectorFilter)
        {
            ref TowerTargetSelector towerWeapon = ref targetSelectorPool.Get(entity);
            towerWeapon.TargetingRange += RangePerUpgrade;
        }
    }
}