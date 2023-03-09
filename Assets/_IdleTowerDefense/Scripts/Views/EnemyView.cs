using System;
using Leopotam.EcsLite;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public float MovementSpeed = 5;
    public float StartingHealth = 10;
    public float Damage = 1;
    public float DamageCooldown = 1;
    public float AttackRange = 3;

    [SerializeField] private GameObject model;
    [SerializeField] private SpriteRenderer healthBar;
    
    public EcsPackedEntity packedEntity;
    public EcsWorld world;

    private Vector3 originalHealthBarScale;

    private void Awake()
    {
        originalHealthBarScale = healthBar.transform.localScale;
        
    }

    private void Start()
    {
        // Rotate model to face the origin
        model.transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position);
    }

    private void Update()
    {
        
        if (packedEntity.Unpack(world, out int unpackedEntity))
        {
            EcsPool<Position> positionPool = world.GetPool<Position>();
            EcsPool<Health> healthPool = world.GetPool<Health>();
            ref Position position = ref positionPool.Get(unpackedEntity);
            ref Health health = ref healthPool.Get(unpackedEntity);
            
            // Update position
            transform.position = new Vector3(position.x, position.y, 0);
            
            // Update HealthBar
            healthBar.transform.localScale = new Vector3(health.CurrentHealth / health.MaxHealth, originalHealthBarScale.y, 1);
            
            if (health.CurrentHealth <=0 || health.CurrentHealth.Equals(health.MaxHealth))
            {
                healthBar.gameObject.SetActive(false);
            }
            else
            {
                healthBar.gameObject.SetActive(true);
            }
            
        }
    }
}