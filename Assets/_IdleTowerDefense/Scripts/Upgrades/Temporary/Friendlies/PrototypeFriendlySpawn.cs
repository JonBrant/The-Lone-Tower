using System.Collections.Generic;
using Guirao.UltimateTextDamage;
using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(fileName = "Prototype Friendly Spawn", menuName = "Idle Tower Defense/Temporary Upgrades/Friendlies/Prototype Friendly")]
public class PrototypeFriendlySpawn : TemporaryFriendlySpawnBase
{
    [Header("Friendly Specific Values")]
    private EcsFilter friendlyFilter;
    public override void Init()
    {
        friendlyFilter = GameManager.Instance.World.Filter<Friendly>().End();
    }

    public override void Spawn()
    {
        Debug.Log($"Spawning Prototype Unit");
        
        EcsWorld world = GameManager.Instance.World;
        
        // Handle cost
        GameManager.Instance.Currency.SubtractValues(GetCost());

        // Create Entity, add components
        
        int entity = world.NewEntity();
        EcsPackedEntity packedEntity = world.PackEntity(entity);
        EcsPool<Friendly> friendlyPool = world.GetPool<Friendly>();
        EcsPool<FriendlyVision> friendlyVisionPool = world.GetPool<FriendlyVision>();
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<FriendlyMeleeDamage> friendlyMeleeDamagePool = world.GetPool<FriendlyMeleeDamage>();
        
        ref Friendly friendly = ref friendlyPool.Add(entity);
        ref FriendlyVision friendlyVision = ref friendlyVisionPool.Add(entity);
        ref Position position = ref positionPool.Add(entity);
        ref Movement movement = ref movementPool.Add(entity);
        ref Health health = ref healthPool.Add(entity);
        ref FriendlyMeleeDamage friendlyMeleeDamage = ref friendlyMeleeDamagePool.Add(entity);
        
        // Setup View
        FriendlyView friendlyView = Instantiate(FriendlyViewPrefab);
        
        // Calculate a random starting position
        // ToDo: Remove magic number
        Vector2 randomPosition = Random.insideUnitCircle.normalized * 3;

        // Init Components
        position = randomPosition;
        //movement.Velocity = -randomPosition.normalized * friendlyView.MovementSpeed;
        movement.StopRadius = friendlyView.AttackRange;
        movement.Speed = friendlyView.MovementSpeed;
        health.MaxHealth = friendlyView.StartingHealth;
        health.CurrentHealth = friendlyView.StartingHealth;
        // health.OnKilled += () => GameManager.Instance.EnemiesKilled++;
        friendlyMeleeDamage.Damage = friendlyView.Damage;
        friendlyMeleeDamage.DamageCooldown = friendlyView.DamageCooldown;
        friendlyMeleeDamage.OnDamageDealt += (damage, enemyTransform) => UltimateTextDamageManager.Instance.AddStack(damage, enemyTransform, "normal");
        friendlyVision.VisionRadius = friendlyView.VisionRadius;
        friendlyVision.AttackRange = friendlyView.AttackRange;

        
        
        // Init View
        friendlyView.transform.position = randomPosition;
        friendlyView.PackedEntity = packedEntity;
        friendlyView.World = world;
    }
}