using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerUpgradeLoadingSystem : IEcsPreInitSystem, IEcsInitSystem
{
    private EcsWorld world;
    private SharedData sharedData;
    private Dictionary<int, int> TestUpgrades = new Dictionary<int, int>();

    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
    }

    public void Init(EcsSystems systems)
    {
        TestUpgrades = ES3.Load("Test", TestUpgrades);
        foreach (KeyValuePair<int, int> kvp in TestUpgrades)
        {
            Debug.Log($"Key: {kvp.Key} => {kvp.Value}");
        }
    }
}