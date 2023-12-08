using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FriendlyMeleeDamage
{
    public float Damage;
    public float DamageCooldown;
    public float DamageCooldownRemaining;
    public Action<float, Transform> OnDamageDealt;
}