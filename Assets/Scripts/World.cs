using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Nomnom.EcsLiteDebugger;
using UnityEngine;

class World : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;

    EcsWorld _world;
    EcsSystems _systems;

    void Start()
    {
        SharedData sharedData = new SharedData();
        sharedData.InitDefaultValues(gameSettings);

        // create ecs environment.
        _world = new EcsWorld();
        _systems = new EcsSystems(_world, sharedData)
            .Add(new TowerSpawnSystem())
            .Add(new TowerTargetingSystem())
            .Add(new TowerFiringSystem())
            .Add(new ProjectileMovementSystem())
            .Add(new EnemySpawnSystem())
            .Add(new WorldDebugSystem("Main World"))
            .Add(new EnemyMovementSystem());
        _systems.Init();
    }

    void Update()
    {
        // process all dependent systems.
        _systems?.Run();
    }

    void OnDestroy()
    {
        // destroy systems logical group.
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        // destroy world.
        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}