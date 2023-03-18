using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Health
{
    // Health
    public float BaseMaxHealth;
    public float MaxHealth;
    public float CurrentHealth;
    public float MaxHealthMultiplier;
    public float MaxHealthAdditions;

    // HealthRegeneration
    public float BaseHealthRegeneration;
    public float HealthRegenerationMultiplier;
    public float HealthRegenerationAdditions;
    public float CurrentHealthRegeneration;

    public Action OnDamaged;
    public Action OnKilled;

    public void RecalculateMaxHealth()
    {
        MaxHealth = BaseMaxHealth * MaxHealthMultiplier + MaxHealthAdditions;
    }

    public void RecalculateHealthRegeneration()
    {
        CurrentHealthRegeneration = BaseHealthRegeneration * HealthRegenerationMultiplier + HealthRegenerationAdditions;
    }
}