using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    public float StartingHealth = 50;
    public float AttackCooldown = 1;
    public float TargetingRange = 5;


    [SerializeField] private SpriteRenderer HealthBar;
    [SerializeField] private LineRenderer RadiusRenderer;

    public EcsPackedEntity packedEntity;
    public EcsWorld world;

    private void Update()
    {
        // Update HealthBar
        if (packedEntity.Unpack(world, out int unpackedTower))
        {
            EcsPool<Health> healthPool = world.GetPool<Health>();
            ref Health towerHealth = ref healthPool.Get(unpackedTower);

            HealthBar.sharedMaterial.SetFloat("_Arc2", Mathf.Lerp(360, 0, towerHealth.CurrentHealth / towerHealth.MaxHealth));
            HealthBar.color = Color.Lerp(Color.red, Color.green, towerHealth.CurrentHealth / towerHealth.MaxHealth);
        }
    }

    private void OnValidate()
    {
        UpdateTargetingRange(TargetingRange);
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