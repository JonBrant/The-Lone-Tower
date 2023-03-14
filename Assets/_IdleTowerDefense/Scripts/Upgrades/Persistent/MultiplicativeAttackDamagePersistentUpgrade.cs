using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Multiplicative Attack Damage Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Multiplicative Attack Damage")]
public class MultiplicativeAttackDamagePersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private float damageExponent = 1.01f;
    [SerializeField] private float upgradeExponent = 1.1f;

    private EcsFilter weaponFilter;


    public override void Init()
    {
        weaponFilter = GameManager.Instance.World.Filter<TowerWeapon>().End();
    }

    public override string GetDescription()
    {
        return string.Format(ShortDescription, damageExponent, null);
    }


    public override float GetCost()
    {
        int upgradeCount = PersistentUpgradeManager.Instance.PersistentUpgradeCounts[Title];
        return Mathf.Pow(upgradeExponent, upgradeCount);
    }

    public override void Upgrade(int upgradeCount)
    {
        EcsPool<TowerWeapon> weaponPool = GameManager.Instance.World.GetPool<TowerWeapon>();

        foreach (int entity in weaponFilter)
        {
            ref TowerWeapon towerWeapon = ref weaponPool.Get(entity);
            towerWeapon.AttackDamage = towerWeapon.AttackDamage * Mathf.Pow(damageExponent, upgradeCount);
        }
    }
}