using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Speed Upgrade", menuName = "Idle Tower Defense/Temporary Upgrades/Attack Speed")]
public class AttackSpeedTemporaryUpgrade : TemporaryUpgradeBase
{
    private EcsFilter weaponFilter;

    public override void Init()
    {
        weaponFilter = GameManager.Instance.World.Filter<TowerWeapon>().End();
    }

    public override Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, TemporaryUpgradeMenu.Instance.TemporaryUpgradeCounts[Title]+1
            }
        };
    }

    public override void Upgrade()
    {
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());
        TemporaryUpgradeMenu.Instance.TemporaryUpgradeCounts[Title] += 1;

        // Handle upgrade
        EcsPool<TowerWeapon> weaponPool = GameManager.Instance.World.GetPool<TowerWeapon>();
        foreach (int entity in weaponFilter)
        {
            ref TowerWeapon towerWeapon = ref weaponPool.Get(entity);
            towerWeapon.AttackCooldown *= 0.9f;
        }
    }
}