using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyMeleeDamage
{
    public float Damage;
    public float DamageCooldown;
    public float DamageCooldownRemaining;
    public Action<float, Transform> OnDamageDealt;
}