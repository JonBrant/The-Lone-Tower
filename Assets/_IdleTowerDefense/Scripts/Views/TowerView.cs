using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using DuloGames.UI;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerView : MonoBehaviour
{
    public float BaseMaxHealth = 50;
    public float BaseHealthRegeneration = 10;
    public float BaseAttackDamage = 1;
    public float BaseAttackCooldown = 1;
    public int BaseMaxTargets = 1;
    public float BaseTargetingRange = 2;

    [SerializeField] private SpriteRenderer HealthBar;
    [SerializeField] private LineRenderer RadiusRenderer;
    private float targetingRange = -1;

    public EcsPackedEntity PackedEntity;

    private void Update()
    {
        if (!PackedEntity.Unpack(GameManager.Instance.World, out int unpackedTower))
        {
            Debug.Log($"{nameof(TowerView)}.{nameof(Update)}() - Attempted to get an invalid packed entity!");
            return;
        }

        EcsPool<Health> healthPool = GameManager.Instance.World.GetPool<Health>();
        EcsPool<TowerTargetSelector> targetSelectorPool = GameManager.Instance.World.GetPool<TowerTargetSelector>();
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