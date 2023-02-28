using System;
using Leopotam.EcsLite;
using UnityEngine;
using System.Linq;
using Guirao.UltimateTextDamage;

public class ProjectileView : MonoBehaviour
{
    public float MovementSpeed = 5;
    public float Damage = 1;
    public float MaxLifetime = 10;

    public EcsPackedEntity packedEntity;
    public EcsWorld world;
    private EcsFilter destroyFilter;

    private void Start()
    {
        destroyFilter = world.Filter<Destroy>()
            .End();
        Destroy(gameObject, MaxLifetime);
    }

    private void Update()
    {
        if (packedEntity.Unpack(world, out int unpackedEntity))
        {
            EcsPool<Position> positionPool = world.GetPool<Position>();
            ref Position position = ref positionPool.Get(unpackedEntity);
            transform.position = new Vector3(position.x, position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyView enemyView))
        {
            if (enemyView.packedEntity.Unpack(world, out int unpackedEnemy) && packedEntity.Unpack(world, out int unpackedProjectile))
            {
                // ToDo: Separate destruction to allow for GameObject.Destroy(lifeTime) - Use HealthSystem, added before Destroy system
                EcsPool<Destroy> destroyPool = world.GetPool<Destroy>();
                EcsPool<Health> healthPool = world.GetPool<Health>();
                EcsPool<Projectile> projectilePool = world.GetPool<Projectile>();
                ref Health enemyHealth = ref healthPool.Get(unpackedEnemy);
                ref Projectile projectile = ref projectilePool.Get(unpackedProjectile);

                enemyHealth.CurrentHealth -= projectile.Damage;
                enemyHealth.OnDamaged?.Invoke();
                projectile.OnDamageDealt?.Invoke(projectile.Damage, other.ClosestPoint(transform.position));
                UltimateTextDamageManager.Instance.AddStack(projectile.Damage, other.transform, "normal");
                
                // Check enemy health and mark for deletion if necessary
                if (enemyHealth.CurrentHealth <= 0 && !destroyPool.Has(unpackedEnemy))
                {
                    destroyPool.Add(unpackedEnemy);
                    Destroy(other.gameObject);
                }

                // Mark projectile for deletion if not already
                if (!destroyPool.Has(unpackedProjectile))
                {
                    destroyPool.Add(unpackedProjectile);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (!packedEntity.Unpack(world, out int unpackedProjectile))
            return;

        EcsPool<Destroy> destroyPool = world.GetPool<Destroy>();


        if (!destroyPool.Has(unpackedProjectile))
        {
            destroyPool.Add(unpackedProjectile);
        }
    }
}