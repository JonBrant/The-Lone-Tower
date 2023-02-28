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

        _world = new EcsWorld();
        _systems = new EcsSystems(_world, sharedData).Add(new TowerSpawnSystem())
            .Add(new TowerTargetingSystem())
            .Add(new TowerFiringSystem())
            .Add(new EnemySpawnSystem())
            .Add(new EnemyDamageSystem())
            .Add(new DestroySystem())
            .Add(new WorldDebugSystem("Main World"))
            .Add(new MovementSystem());

        _systems.Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        _world?.Destroy();
        _systems?.Destroy();
    }
}