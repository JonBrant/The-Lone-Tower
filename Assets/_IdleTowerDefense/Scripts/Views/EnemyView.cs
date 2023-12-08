using System;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemyView : MonoBehaviour {
    public float MovementSpeed = 5;
    public float StartingHealth = 10;
    public float Damage = 1;
    public float DamageCooldown = 1;
    public float AttackRange = 3;

    [SerializeField] private GameObject model;
    [SerializeField] private SpriteRenderer healthBar;

    public EcsPackedEntity PackedEntity;
    public EcsWorld World;

    private Vector3 originalHealthBarScale;

    private void Awake() {
        // Required because we scale it with health
        originalHealthBarScale = healthBar.transform.localScale;
    }

    private void Start() {
        // Rotate model to face the origin
        model.transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position);
    }

    private void Update() {

        if (PackedEntity.Unpack(World, out int unpackedEntity)) {
            EcsPool<Position> positionPool = World.GetPool<Position>();
            EcsPool<Health> healthPool = World.GetPool<Health>();
            ref Position position = ref positionPool.Get(unpackedEntity);
            ref Health health = ref healthPool.Get(unpackedEntity);

            // Update position
            transform.position = new Vector3(position.x, position.y, 0);

            // Update HealthBar
            healthBar.transform.localScale = new Vector3(health.CurrentHealth / health.MaxHealth, originalHealthBarScale.y, 1);

            if (health.CurrentHealth <= 0 || health.CurrentHealth.Equals(health.MaxHealth)) {
                healthBar.gameObject.SetActive(false);
            }
            else {
                healthBar.gameObject.SetActive(true);
            }
        }
        else {
            Debug.Log($"Entity failed to unpack, destroying!");
            Destroy(gameObject);
        }
    }
}