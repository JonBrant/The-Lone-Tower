using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TemporaryFriendlySpawnBase : TemporaryUpgradeBase {
    [Header("Friendly Base Values")]
    public FriendlyView FriendlyViewPrefab;
    public float ExpCost = 0;
    public float ScrapCost = 0;

    public override Dictionary<CurrencyTypes, float> GetCost() {
        return new Dictionary<CurrencyTypes, float> {
            {
                CurrencyTypes.Exp, ExpCost
            }, {
                CurrencyTypes.Scrap, ScrapCost
            }
        };
    }

    public override void Upgrade() {
        Spawn();
    }

    public virtual void Spawn()
    {
    }
}