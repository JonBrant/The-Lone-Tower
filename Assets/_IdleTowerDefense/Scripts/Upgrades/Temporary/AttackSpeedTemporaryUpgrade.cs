using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Speed Upgrade", menuName = "Idle Tower Defense/Temporary Upgrades/Attack Speed")]
public class AttackSpeedTemporaryUpgrade : TemporaryUpgradeBase
{
    public float AttackSpeedMultiplier = 0.1f;
    public float MinimumAttackCooldown = 0.25f;
    private EcsFilter weaponFilter;

    public override void Init()
    {
        weaponFilter = GameManager.Instance.World.Filter<TowerWeapon>().End();
    }

    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] + 1
            }
        };
    }

    public override bool CanUpgrade()
    {
        bool limitReached = false;
        EcsPool<TowerWeapon> weaponPool = GameManager.Instance.World.GetPool<TowerWeapon>();
        foreach (int entity in weaponFilter)
        {
            ref TowerWeapon towerWeapon = ref weaponPool.Get(entity);
            if (towerWeapon.AttackCooldownMultiplier - AttackSpeedMultiplier <= MinimumAttackCooldown)
            {
                limitReached = true;
            }
        }

        return base.CanUpgrade() && !limitReached;
    }

    public override void Upgrade()
    {
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());
        TemporaryUpgradeManager.Instance.TemporaryUpgradeCounts[Title] += 1;

        // Handle upgrade
        EcsPool<TowerWeapon> weaponPool = GameManager.Instance.World.GetPool<TowerWeapon>();
        foreach (int entity in weaponFilter)
        {
            ref TowerWeapon towerWeapon = ref weaponPool.Get(entity);
            towerWeapon.AttackCooldownMultiplier -= AttackSpeedMultiplier;
            towerWeapon.RecalculateAttackCooldown();
        }
    }
}