using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesKilledDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private float updateInterval = 0.5f;
    private float timeSinceLastUpdate = 0;

    private void Update()
    {
        timeSinceLastUpdate -= Time.deltaTime;
        if (timeSinceLastUpdate > 0) return;

        timeSinceLastUpdate = updateInterval;
        textObject.text = $"Enemies Killed: {GameManager.Instance.EnemiesKilled}";
    }
}