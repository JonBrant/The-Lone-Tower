using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public struct FriendlyVision {
    public float VisionRadius;
    public float AttackRange;
    public bool InAttackRange;
    public EcsPackedEntity CurrentTarget;
}
