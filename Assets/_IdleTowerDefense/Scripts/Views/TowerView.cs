using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using DuloGames.UI;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    public float StartingHealth = 50;
    public float StartingHealthRegeneration = 10;
    public float StartingAttackDamage = 1;
    public float StartingAttackCooldown = 1;
    public int StartingMaxTargets = 1;
    public float StartingTargetingRange = 2;
    
    [SerializeField] private SpriteRenderer HealthBar;
    [SerializeField] private LineRenderer RadiusRenderer;
    private float targetingRange = -1;

    public EcsPackedEntity PackedEntity;
    public EcsWorld World;

    

    private void Update()
    {
        // Update HealthBar
        if (!PackedEntity.Unpack(World, out int unpackedTower))
            return;

        EcsPool<Health> healthPool = World.GetPool<Health>();
        EcsPool<TowerTargetSelector> targetSelectorPool = World.GetPool<TowerTargetSelector>();
        ref Health towerHealth = ref healthPool.Get(unpackedTower);
        ref TowerTargetSelector towerTargetSelector = ref targetSelectorPool.Get(unpackedTower);

        // Update Health display
        HealthBar.sharedMaterial.SetFloat("_Arc2", Mathf.Lerp(360, 0, towerHealth.CurrentHealth / towerHealth.MaxHealth));
        HealthBar.color = Color.Lerp(Color.red, Color.green, towerHealth.CurrentHealth / towerHealth.MaxHealth);

        // Update targeting range if necessary
        if (!targetingRange.Equals(towerTargetSelector.TargetingRange))
        {
            targetingRange = towerTargetSelector.TargetingRange;
            UpdateTargetingRange(targetingRange);
        }
    }

    private void UpdateTargetingRange(float range)
    {
        int numSegments = 80;
        float deltaTheta = (2 * Mathf.PI) / numSegments;
        float theta = 0f;

        RadiusRenderer.positionCount = numSegments + 1;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = range * Mathf.Cos(theta);
            float y = range * Mathf.Sin(theta);

            RadiusRenderer.SetPosition(i, new Vector3(x, y, 0f));

            theta += deltaTheta;
        }
    }
}