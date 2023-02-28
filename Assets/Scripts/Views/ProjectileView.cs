using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    public EcsPackedEntity packedEntity;
    public EcsWorld world;
    
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
            Debug.Log($"{nameof(ProjectileView)}.{nameof(OnTriggerEnter2D)}() - Collision!");
        }
    }
}
