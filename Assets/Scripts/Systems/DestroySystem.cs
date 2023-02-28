using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class DestroySystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
    }

    public void Run(EcsSystems systems)
    {
        EcsFilter destroyFilter = world.Filter<Destroy>()
            .End();

        foreach (int destroyedEntity in destroyFilter)
        {
            world.DelEntity(destroyedEntity);
        }
    }
}
