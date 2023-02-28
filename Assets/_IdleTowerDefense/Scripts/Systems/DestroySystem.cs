using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class DestroySystem : IEcsPreInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter destroyFilter;

    public void PreInit(EcsSystems systems)
    {
        world = systems.GetWorld();
        destroyFilter = world.Filter<Destroy>()
            .End();
    }

    public void Run(EcsSystems systems)
    {
        foreach (int destroyedEntity in destroyFilter)
        {
            world.DelEntity(destroyedEntity);
        }
    }
}