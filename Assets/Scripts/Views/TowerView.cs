using System;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    public float StartingHealth = 50;
    public float AttackCooldown = 1;
    public float TargetingRange = 5;
    

    [SerializeField] private SpriteRenderer HealthBar;
    public EcsPackedEntity packedEntity;
    public EcsWorld world;

    private void Update()
    {
        // Update HealthBar
        if (packedEntity.Unpack(world, out int unpackedTower))
        {
            EcsPool<Health> healthPool = world.GetPool<Health>();
            ref Health towerHealth = ref healthPool.Get(unpackedTower);
            
            HealthBar.sharedMaterial.SetFloat("_Arc2", Mathf.Lerp(360, 0, towerHealth.CurrentHealth/towerHealth.MaxHealth));
        }
    }
}
