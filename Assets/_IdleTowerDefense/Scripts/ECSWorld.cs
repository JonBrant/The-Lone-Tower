using Leopotam.EcsLite;
using Nomnom.EcsLiteDebugger;
using UnityEngine;

class ECSWorld : Singleton<ECSWorld>
{
    [SerializeField] private GameSettings gameSettings;

    public EcsWorld World;
    EcsSystems _systems;

    void Awake()
    {
        SharedData sharedData = new SharedData();
        sharedData.InitDefaultValues(gameSettings);

        World = new EcsWorld();
        _systems = new EcsSystems(World, sharedData).Add(new TowerSpawnSystem())
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
        World?.Destroy();
        _systems?.Destroy();
    }
}