using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Settings", menuName = "Idle Tower Defense/Upgrade Settings")]
public class UpgradeSettings : ScriptableObject
{
    public List<UpgradeBase> Upgrades = new List<UpgradeBase>();

    public void Init()
    {
        foreach (UpgradeBase upgrade in Upgrades)
        {
            upgrade.Init();
        }
    }
}