using Leopotam.EcsLite;
using Nomnom.EcsLiteDebugger;
using UnityEngine;

class ECSWorld : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;

    public EcsWorld _world;
    EcsSystems _systems;

    void Awake()
    {
        SharedData sharedData = new SharedData();
        sharedData.InitDefaultValues(gameSettings);

        _world = new EcsWorld();
        _systems = new EcsSystems(_world, sharedData).Add(new TowerSpawnSystem())
            .Add(new TowerUpgradeLoadingSystem())
            .Add(new TowerTargetingSystem())
            .Add(new TowerFiringSystem())
            .Add(new EnemySpawnSystem())
            .Add(new EnemyDamageSystem())
            .Add(new HealthRegenerationSystem())
            .Add(new DestroySystem())
            .Add(new WorldDebugSystem("Main World"))
            .Add(new MovementSystem());

        _systems.Init();

        GameManager.Instance.World = _world;
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