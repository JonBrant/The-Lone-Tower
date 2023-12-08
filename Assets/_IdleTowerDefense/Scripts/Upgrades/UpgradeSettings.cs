using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Upgrade Settings", menuName = "Idle Tower Defense/Upgrade Settings")]
public class UpgradeSettings : ScriptableObject
{
    public List<TemporaryUpgradeBase> TemporaryUpgrades = new List<TemporaryUpgradeBase>();
    public List<TemporaryFriendlySpawnBase> FriendlySpawnables = new List<TemporaryFriendlySpawnBase>();
    public List<PersistentUpgradeBase> PersistentUpgrades = new List<PersistentUpgradeBase>();

    public void InitTemporaryUpgrades()
    {
        foreach (TemporaryUpgradeBase upgrade in TemporaryUpgrades)
        {
            upgrade.Init();
        }
    }
}