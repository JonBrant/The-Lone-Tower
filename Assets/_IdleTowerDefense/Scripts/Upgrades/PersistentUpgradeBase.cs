using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentUpgradeBase : ScriptableObject
{
    [Header("Base Values")]
    public string Title = "Default Title";
    public string ShortDescription = "Short Description"; // Used for button
    public List<string> DescriptionLines = new List<string>();
    
    public virtual void Init()
    {
        
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