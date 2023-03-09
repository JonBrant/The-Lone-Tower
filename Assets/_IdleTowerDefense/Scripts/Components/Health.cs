using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Health
{
    public float CurrentHealth;
    public float MaxHealth;
    public float HealthRegeneration;
    public Action OnDamaged;
    public Action OnKilled;
}
