using System;
using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton upgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource buttonAudioSource;

    private void Start()
    {
        for (int i = 0; i < 25; i++)
        {
            UpgradeButton upgradeButton = Instantiate(upgradeButtonPrefab, buttonContainer);
            upgradeButton.ElementSound.audioObject = buttonAudioSource;
            upgradeButton.Tooltip.contentLines = new[] {
                new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, "Test Title"),
                new UITooltipLineContent(
                    UITooltipLines.LineStyle.Default,
                    null,
                    "<b>Lorem ipsum dolor sit amet</b>, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                ),
                new UITooltipLineContent(
                    UITooltipLines.LineStyle.Default,
                    null,
                    "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
                ),
                new UITooltipLineContent(
                    UITooltipLines.LineStyle.Description,
                    null,
                    "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam"
                ),
                new UITooltipLineContent(UITooltipLines.LineStyle.Description, null, "Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit.")
            };
            int i1 = i;
            upgradeButton.Button.onClick.AddListener(() => Debug.Log($"Upgrade button {i1} pressed!"));
        }
    }
}