using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Upgrade Settings", menuName = "Idle Tower Defense/Upgrade Settings")]
public class UpgradeSettings : ScriptableObject
{
    public List<UpgradeBase> TemporaryUpgrades = new List<UpgradeBase>();
    public List<UpgradeBase> PersistentUpgrades = new List<UpgradeBase>();

    public void Init()
    {
        foreach (UpgradeBase upgrade in TemporaryUpgrades)
        {
            upgrade.Init();
        }
    }
}