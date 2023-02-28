using Leopotam.EcsLite;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public float MovementSpeed = 5;
    
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
}