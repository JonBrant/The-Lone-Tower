using System;
using Leopotam.EcsLite;
using UnityEngine;
using System.Linq;
public class ProjectileView : MonoBehaviour
{
    public float MovementSpeed = 5;

    public EcsPackedEntity packedEntity;
    public EcsWorld world;
    private EcsFilter destroyFilter;
    private void Start()
    {
         destroyFilter = world.Filter<Destroy>().End();
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
                // Check if entities are already marked for deletion
                // ToDo: There has to be better way to go about this
                bool enemyMarkedForDeletion = false;
                bool projectileMarkedForDeletion = false;
                foreach (int i in destroyFilter)
                {
                    if (unpackedEnemy == i)
                    {
                        enemyMarkedForDeletion = true;
                    }

                    if (unpackedProjectile == i)
                    {
                        projectileMarkedForDeletion = true;
                    }
                }
                
                // Mark entities for deletion
                EcsPool<Destroy> destroyPool = world.GetPool<Destroy>();
                if (!enemyMarkedForDeletion)
                {
                    destroyPool.Add(unpackedEnemy);
                }

                if (!projectileMarkedForDeletion)
                {
                    destroyPool.Add(unpackedProjectile);
                }

                // Destroy views
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}