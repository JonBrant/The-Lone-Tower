using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class UpgradeBase : ScriptableObject
{
    public string Title = "Default Title";
    public string ShortDescription = "Short Description"; // Used for button
    public List<string> DescriptionLines = new List<string>(); // Used for tooltip
    public Sprite BackgroundImage = null;

    public virtual void Init()
    {
        
    }

    public virtual Dictionary<CurrencyTypes, float> GetCost()
    {
        return new Dictionary<CurrencyTypes, float>();
    }

    public virtual bool CanUpgrade() => true;

    public virtual void Upgrade()
    {
    }
}