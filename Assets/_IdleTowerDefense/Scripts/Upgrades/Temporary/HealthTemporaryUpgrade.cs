using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Upgrade", menuName = "Idle Tower Defense/Temporary Upgrades/Health")]
public class HealthTemporaryUpgrade : TemporaryUpgradeBase
{
    [Header("Upgrade Specific Values")]
    public float HealthPerUpgrade = 10;

    private EcsFilter healthFilter;

    public override void Init()
    {
        healthFilter = GameManager.Instance.World.Filter<Tower>().Inc<Health>().End();
    }

    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] + 1
            }
        };
    }

    public override void Upgrade()
    {
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());
        TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] += 1;

        // Handle Upgrade
        EcsPool<Health> healthPool = GameManager.Instance.World.GetPool<Health>();
        foreach (int entity in healthFilter)
        {
            ref Health towerHealth = ref healthPool.Get(entity);
            towerHealth.MaxHealthAdditions += HealthPerUpgrade;
            towerHealth.RecalculateMaxHealth();
        }
    }
}