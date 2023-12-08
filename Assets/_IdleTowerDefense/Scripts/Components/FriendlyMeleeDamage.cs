using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FriendlyMeleeDamage
{
    //public delegate void 
    public float Damage;
    public float DamageCooldown;
    public float DamageCooldownRemaining;
    public Action<float, Vector2> OnDamageDealt;
}