using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public enum CurrencyTypes
{
    Gold,
    Scrap,
    Etc
}

public class GameManager : Singleton<GameManager>
{
    public EcsWorld World;
    public Dictionary<CurrencyTypes, float> Currency = new Dictionary<CurrencyTypes, float>();

    private void Awake()
    {
        // Init Currency dictionary
        List<CurrencyTypes> currencies = ((CurrencyTypes[])Enum.GetValues(typeof(CurrencyTypes))).ToList();
        foreach (CurrencyTypes currency in currencies)
        {
            Currency.Add(currency, 0f);
        }
        
        // Temporary
        Currency[CurrencyTypes.Gold] = 10;
    }
}