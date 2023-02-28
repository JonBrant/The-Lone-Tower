using System;
using UnityEngine;

public struct Projectile
{
    public float Damage;
    public Action<float, Vector3> OnDamageDealt;
}
