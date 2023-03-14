using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerUpgradeLoadingSystem : IEcsPreInitSystem, IEcsInitSystem
{
    private EcsWorld world;
    private SharedData sharedData;
    private Dictionary<string, int> persistentUpgradeCounts = new Dictionary<string, int>();
    
    public void PreInit(EcsSystems systems)
    {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        
        // Init default empty dictionary
        Dictionary<string, int> defaultValues = new Dictionary<string, int>();
        foreach (var upgrade in sharedData.Settings.UpgradeSettings.PersistentUpgrades)
        {
            defaultValues.Add(upgrade.Title, 0);
        }
        
        // Load saved upgrade counts
        persistentUpgradeCounts = ES3.Load(SaveKeys.PersistentUpgradeCounts, defaultValues);
    }

    public void Init(EcsSystems systems)
    {
        
        foreach (PersistentUpgradeBase upgrade in sharedData.Settings.UpgradeSettings.PersistentUpgrades)
        {
            upgrade.Init();
            if (persistentUpgradeCounts[upgrade.Title] == 0)
                continue;

            Debug.Log($"{nameof(TowerUpgradeLoadingSystem)}.{nameof(Init)}() - Upgrading {upgrade.Title} {persistentUpgradeCounts[upgrade.Title]} times!");
            upgrade.Upgrade(persistentUpgradeCounts[upgrade.Title]);
        }
    }
}