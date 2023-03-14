using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Damage Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Attack Damage")]
public class AttackDamagePersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private float damageExponent = 1.01f;
    [SerializeField] private float upgradeExponent = 1.1f;

    private EcsFilter weaponFilter;

    public override void Init()
    {
        weaponFilter = GameManager.Instance.World.Filter<TowerWeapon>().End();
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