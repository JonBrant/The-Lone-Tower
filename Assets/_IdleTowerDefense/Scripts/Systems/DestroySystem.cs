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
                
                string dropText = currencyDrop.Drops.ToCommaString();
                /*
                List<CurrencyTypes> keys = currencyDrop.Drops.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    GameManager.Instance.Currency[keys[i]] += currencyDrop.Drops[keys[i]];
                    dropText += $"{currencyDrop.Drops[keys[i]]} {keys[i]}";
                    if (i+1 < keys.Count)
                    {
                        dropText += ", ";
                    }
                }
                */

                if (dropText!= "")
                {
                    // UltimateTextDamageManager.Instance.Add(dropText, (Vector2)enemyPosition, "loot");
                    UltimateTextDamageManager.Instance.Add(dropText, (Vector2)enemyPosition, "loot");
                }
            }

            world.DelEntity(destroyedEntity);
        }
    }
}