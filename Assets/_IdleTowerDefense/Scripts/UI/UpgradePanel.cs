using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton upgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource buttonAudioSource;

    private void Start()
    {
        for (int i = 0; i < 25; i++)
        {
            UpgradeButton button = Instantiate(upgradeButtonPrefab, buttonContainer);
            button.ElementSound.audioObject = buttonAudioSource;
        }
    }
}