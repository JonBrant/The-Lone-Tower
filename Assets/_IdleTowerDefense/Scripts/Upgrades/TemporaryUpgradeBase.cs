using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TemporaryUpgradeBase : ScriptableObject
{
    [Header("Base Values")]
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

    public virtual bool CanUpgrade()
    {
        return GameManager.Instance.Currency.HasAtLeast(GetCost());
    }

    public virtual void Upgrade()
    {
    }
}