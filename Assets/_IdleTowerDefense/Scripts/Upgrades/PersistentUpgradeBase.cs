using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersistentUpgradeBase : ScriptableObject
{
    [Header("Base Values")]
    public string Title = "Default Title";
    public string ShortDescription = "Short Description"; // Used for button
    public List<string> DescriptionLines = new List<string>();
    [Tooltip("Priority used by TowerUpgradeLoadingSystem, lower means it will be applied before.")]
    public int UpgradePriority = 0;

    public virtual void Init()
    {

    }

    public virtual string GetDescription()
    {
        return $"Default \"null\" description. Override {nameof(PersistentUpgradeBase)}.{nameof(GetDescription)}";
    }

    public virtual float GetCost()
    {
        return 0f;
    }

    public virtual bool CanUpgrade()
    {
        return PersistentUpgradeManager.Instance.RemainingScrap < GetCost();
    }

    public virtual void Upgrade(int upgradeCount)
    {

    }
}