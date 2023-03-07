using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guirao.UltimateTextDamage;
using Leopotam.EcsLite;
using UnityEngine;

public class DestroySystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter destroyFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        destroyFilter = world.Filter<Destroy>().End();
    }

    public void Run(EcsSystems systems)
    {
        EcsPool<CurrencyDrop> currencyDropPool = world.GetPool<CurrencyDrop>();
        EcsPool<Position> positionPool = world.GetPool<Position>();

        foreach (int destroyedEntity in destroyFilter)
        {
            if (currencyDropPool.Has(destroyedEntity))
            {
                ref CurrencyDrop currencyDrop = ref currencyDropPool.Get(destroyedEntity);
                ref Position enemyPosition = ref positionPool.Get(destroyedEntity);
                
                // Add currency
                GameManager.Instance.Currency.AddValues(currencyDrop.Drops);
                
                // Handle combat text
                string dropText = currencyDrop.Drops.ToCommaString();
                if (dropText!= "")
                {
                    UltimateTextDamageManager.Instance.Add(dropText, (Vector2)enemyPosition, "loot");
                }
            }

            world.DelEntity(destroyedEntity);
        }
    }
}