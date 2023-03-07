using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Damage Upgrade", menuName = "Idle Tower Defense/Upgrades/Attack Damage")]
public class AttackDamageUpgrade : UpgradeBase
{
    public override bool CanUpgrade()
    {
        return GameManager.Instance.Currency[CurrencyTypes.Gold] > 1;
    }

    public override void Upgrade()
    {
        Debug.Log($"{nameof(AttackDamageUpgrade)}.{nameof(Upgrade)}() - Upgrading!");
    }
}
