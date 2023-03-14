using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Regeneration Upgrade", menuName = "Idle Tower Defense/Upgrades/Health Regeneration")]
public class HealthRegenerationUpgrade : UpgradeBase
{
    [Header("Upgrade Specific Values")]
    public float HealthRegenerationPerUpgrade = 0.5f;
    
    private EcsFilter healthFilter;

    public override void Init()
    {
        healthFilter = GameManager.Instance.World.Filter<Tower>().Inc<Health>().End();
    }
    
    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, UpgradeManager.Instance.UpgradeCounts[Title] + 1
            }
        };
    }
    
    public override void Upgrade()
    {
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());
        UpgradeManager.Instance.UpgradeCounts[Title] += 1;

        // Handle Upgrade
        EcsPool<Health> healthPool = GameManager.Instance.World.GetPool<Health>();
        foreach (int entity in healthFilter)
        {
            ref Health towerHealth = ref healthPool.Get(entity);
            towerHealth.HealthRegeneration += HealthRegenerationPerUpgrade;
        }
    }
}
