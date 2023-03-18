using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Additive Attack Damage Persistent Upgrade", menuName = "Idle Tower Defense/Persistent Upgrades/Additive Attack Damage")]
public class AdditiveAttackDamagePersistentUpgrade : PersistentUpgradeBase
{
    [Header("Upgrade Specific Values")]
    [SerializeField] private float damageIncrease = 1.01f;
    [SerializeField] private float upgradeCostExponent = 1.1f;

    private EcsFilter weaponFilter;


    public override void Init()
    {
        weaponFilter = GameManager.Instance.World.Filter<TowerWeapon>().End();
    }

    public override string GetDescription()
    {
        return string.Format(ShortDescription, damageIncrease, null);
    }


    public override float GetCost()
    {
        int upgradeCount = PersistentUpgradeManager.Instance.PersistentUpgradeCounts[Title];
        return Mathf.Pow(upgradeCostExponent, upgradeCount);
    }

    public override void Upgrade(int upgradeCount)
    {
        EcsPool<TowerWeapon> weaponPool = GameManager.Instance.World.GetPool<TowerWeapon>();

        foreach (int entity in weaponFilter)
        {
            ref TowerWeapon towerWeapon = ref weaponPool.Get(entity);
            towerWeapon.AttackDamageAdditions += damageIncrease * upgradeCount;
            towerWeapon.RecalculateAttackDamage();
        }
    }
}