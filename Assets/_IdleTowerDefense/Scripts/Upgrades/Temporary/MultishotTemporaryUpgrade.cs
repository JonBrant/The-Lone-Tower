using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Multishot Upgrade", menuName = "Idle Tower Defense/Temporary Upgrades/Multishot")]
public class MultishotTemporaryUpgrade : TemporaryUpgradeBase
{
    [Header("Upgrade Specific Values")]
    
    private EcsFilter towerTargetSelectorFilter;

    public override void Init()
    {
        towerTargetSelectorFilter = GameManager.Instance.World.Filter<Tower>()
            .Inc<TowerTargetSelector>()
            .End();
    }
    
    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, (TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] + 1) * 5
            }, {
                CurrencyTypes.Scrap, (TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] + 1) * 5
            }
        };
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
            towerWeapon.MaxTargets += 1;
        }
    }
}
