using System;
using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;

public class EnemyViewTooltip : MonoBehaviour {
    [SerializeField] private float updateInterval = 0.5f;
    private EnemyView enemyView;
    private EcsWorld world;
    private float timeSinceLastUpdate = 0;
    private bool mouseOver = false;

    private void Start() {
        enemyView = GetComponent<EnemyView>();
        world = enemyView.World;
    }

    private void OnMouseOver() {
        mouseOver = true;
        timeSinceLastUpdate -= Time.deltaTime;

        if (enemyView == null) {
            Debug.LogError($"{nameof(enemyView)} == null!");
            return;
        }

        if (timeSinceLastUpdate > 0)
            return;
        if (!enemyView.PackedEntity.Unpack(world, out int unpackedEnemy))
            return;

        if (MenuScreen.Instance.IsOn)
            return;

        timeSinceLastUpdate = updateInterval;

        EcsPool<Health> healthPool = world.GetPool<Health>();
        EcsPool<Movement> movementPool = world.GetPool<Movement>();
        EcsPool<EnemyMeleeDamage> damagePool = world.GetPool<EnemyMeleeDamage>();
        EcsPool<CurrencyDrop> currencyDropPool = world.GetPool<CurrencyDrop>();


        UITooltip.InstantiateIfNecessary(GameObject.Find("UI"));
        UITooltip.Instance.CleanupLines();
        UITooltip.AddTitle($"{enemyView.gameObject.name.Replace("(Clone)", "")}");

        UITooltip.AddLineColumn($"<b>Health</b>: {healthPool.Get(unpackedEnemy).CurrentHealth:N1}/{healthPool.Get(unpackedEnemy).MaxHealth:N1}");
        UITooltip.AddLineColumn($"<b>Speed</b>: {movementPool.Get(unpackedEnemy).Velocity.magnitude:N1}");
        UITooltip.AddLineColumn($"<b>Damage</b>: {damagePool.Get(unpackedEnemy).Damage:N1}");
        UITooltip.AddLineColumn($"<b>Attack Speed</b>: {damagePool.Get(unpackedEnemy).DamageCooldown:N1}");
        UITooltip.AddLineColumn($"<b>Range</b>: {movementPool.Get(unpackedEnemy).StopRadius:N1}");
        UITooltip.AddLineColumn($"<b>Drops</b>: {currencyDropPool.Get(unpackedEnemy).Drops.ToCommaString()}");


        UITooltip.Show();
    }


    private void OnMouseExit() {
        mouseOver = false;
        UITooltip.Hide();
    }

    private void OnDestroy() {
        if (mouseOver) {
            UITooltip.Hide();
        }
    }
}