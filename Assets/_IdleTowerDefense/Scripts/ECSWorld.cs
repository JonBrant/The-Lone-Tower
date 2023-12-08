using Leopotam.EcsLite;
using Nomnom.EcsLiteDebugger;
using UnityEngine;
using UnityEngine.Serialization;

public class ECSWorld : MonoBehaviour {
    [FormerlySerializedAs("gameSettings")]
    public GameSettings GameSettings;

    public EcsWorld _world;
    EcsSystems _systems;

    void Awake() {
        SharedData sharedData = new SharedData();
        sharedData.InitDefaultValues(GameSettings);
        GameManager.Instance.ECSWorldInitFriendlyViews(GameSettings);

        _world = new EcsWorld();
        GameManager.Instance.World = _world;

        _systems = new EcsSystems(_world, sharedData).Add(new TowerSpawnSystem())
            .Add(new TowerUpgradeLoadingSystem())
            .Add(new TowerTargetingSystem())
            .Add(new TowerFiringSystem())
            .Add(new EnemySpawnSystem())
            .Add(new EnemyMeleeDamageSystem())
            .Add(new HealthRegenerationSystem())
            .Add(new DestroySystem())
            .Add(new WorldDebugSystem("Main World"))
            .Add(new EnemyMovementSystem())
            .Add(new FriendlyMovementSystem())
            .Add(new FriendlyMeleeDamageSystem())
            .Add(new FriendlyVisionSystem());

        _systems.Init();
    }

    void Update() {
        if (!GameManager.Instance.Paused) {
            _systems?.Run();
        }
    }

    void OnDestroy() {
        _world?.Destroy();
        _systems?.Destroy();
    }
}