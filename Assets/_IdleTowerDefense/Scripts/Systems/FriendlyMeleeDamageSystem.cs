using System.Collections;
using System.Collections.Generic;
using Guirao.UltimateTextDamage;
using Leopotam.EcsLite;
using UnityEngine;

public class FriendlyMeleeDamageSystem : IEcsPreInitSystem, IEcsRunSystem {
    private EcsWorld world;
    private EcsFilter enemyFilter;
    private EcsFilter friendlyMeleeFilter;
    private SharedData sharedData;

    public void PreInit(EcsSystems systems) {
        sharedData = systems.GetShared<SharedData>();
        world = systems.GetWorld();
        enemyFilter = world.Filter<Enemy>().Inc<Movement>().Inc<EnemyMeleeDamage>().End();
        friendlyMeleeFilter = world.Filter<Friendly>().Inc<Health>().End();
    }

    public void Run(EcsSystems systems) {
        EcsPool<Position> positionPool = world.GetPool<Position>();
        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<FriendlyMeleeDamage> friendlyMeleeDamagePool = world.GetPool<FriendlyMeleeDamage>();
        EcsPool<FriendlyVision> friendlyVisionPool = world.GetPool<FriendlyVision>();
        EcsPool<Destroy> destroyPool = world.GetPool<Destroy>();

        foreach (int friendlyEntity in friendlyMeleeFilter) {
            ref FriendlyVision vision = ref friendlyVisionPool.Get(friendlyEntity);
            ref FriendlyMeleeDamage meleeDamage = ref friendlyMeleeDamagePool.Get(friendlyEntity);

            meleeDamage.DamageCooldownRemaining -= Time.deltaTime;

            if (!vision.InAttackRange) {
                continue;
            }

            if (vision.CurrentTarget.Unpack(world, out int targetEntity)) {
                if (meleeDamage.DamageCooldownRemaining <= 0) {
                    Debug.Log($"Entity {friendlyEntity} attacking!");

                    ref Health targetHealth = ref healthPool.Get(targetEntity);
                    ref Position targetPosition = ref positionPool.Get(targetEntity);

                    meleeDamage.DamageCooldownRemaining = meleeDamage.DamageCooldown;
                    targetHealth.CurrentHealth -= meleeDamage.Damage;
                    UltimateTextDamageManager.Instance.Add(meleeDamage.Damage.ToString("N0"), (Vector2)targetPosition);

                    if (targetHealth.CurrentHealth <= 0 && !destroyPool.Has(targetEntity)) {
                        targetHealth.OnKilled?.Invoke();
                        destroyPool.Add(targetEntity);
                    }
                }
            }
            else {
                Debug.LogError($"Failed to unpack entity!");
            }
        }
//projectile.OnDamageDealt += (damage, enemyTransform) => UltimateTextDamageManager.Instance.AddStack(damage, enemyTransform, "normal");
        /*
        foreach (int tower in friendlyMeleeFilter) {
            ref Health towerHealth = ref healthPool.Get(tower);

            foreach (int enemy in enemyFilter) {
                ref Movement enemyMovement = ref movementPool.Get(enemy);

                if (!enemyMovement.Stopped) {
                    continue;
                }

                ref FriendlyMeleeDamage enemyEnemyMeleeDamage = ref friendlyMeleeDamagePool.Get(enemy);
                enemyEnemyMeleeDamage.DamageCooldownRemaining -= Time.deltaTime;

                if (enemyEnemyMeleeDamage.DamageCooldownRemaining <= 0) {
                    enemyEnemyMeleeDamage.DamageCooldownRemaining = enemyEnemyMeleeDamage.DamageCooldown;

                    towerHealth.CurrentHealth -= enemyEnemyMeleeDamage.Damage;

                    if (towerHealth.CurrentHealth <= 0) {
                        towerHealth.CurrentHealth = 0;
                        towerHealth.OnKilled?.Invoke();
                    }

                    enemyEnemyMeleeDamage.OnDamageDealt?.Invoke(enemyEnemyMeleeDamage.Damage, sharedData.TowerView.transform);
                    towerHealth.OnDamaged?.Invoke();
                }
            }
        }
        */
    }
}