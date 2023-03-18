using System;
using System.Collections.Generic;
using System.Linq;
using DuloGames.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TemporaryUpgradeManager : Singleton<TemporaryUpgradeManager>
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TemporaryUpgradeButton temporaryUpgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource buttonAudioSource;
    [SerializeField] private float autoUpgradeInterval = 0.5f;

    // String = Upgrade.Title, int = upgrade level
    public Dictionary<string, int> TemporaryUpgradeCounts = new Dictionary<string, int>();
    public bool MenuOpen => menuOpen;
    private bool menuOpen = false;
    private bool autoUpgrading = false;
    private float autoUpgradeTimeRemaining = 0;

    private void Start()
    {
        gameSettings.UpgradeSettings.InitTemporaryUpgrades();
        for (int i = 0; i < gameSettings.UpgradeSettings.TemporaryUpgrades.Count; i++)
        {
            // Capture i to avoid closure issues
            int index = i;

            // Init dictionary element for the upgrade
            TemporaryUpgradeCounts.Add(gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title, 0);

            // Setup UpgradeButton values
            TemporaryUpgradeButton temporaryUpgradeButton = Instantiate(temporaryUpgradeButtonPrefab, buttonContainer);
            temporaryUpgradeButton.targetTemporaryUpgrade = gameSettings.UpgradeSettings.TemporaryUpgrades[i];
            temporaryUpgradeButton.ElementSound.audioObject = buttonAudioSource;
            temporaryUpgradeButton.titleObj.text = gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title.ToUpper();
            temporaryUpgradeButton.descriptionObj.text = gameSettings.UpgradeSettings.TemporaryUpgrades[i].ShortDescription;
            if (gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage != null)
            {
                temporaryUpgradeButton.backgroundImageObj.sprite = gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage;
            }

            // Setup tooltip
            List<UITooltipLineContent> LineList = new List<UITooltipLineContent>();
            LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title));
            foreach (string t in gameSettings.UpgradeSettings.TemporaryUpgrades[i].DescriptionLines)
            {
                LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Default, null, t));
            }

            temporaryUpgradeButton.Tooltip.contentLines = LineList.ToArray();

            // ScriptableObject handles upgrading itself

            temporaryUpgradeButton.Button.onClick.AddListener(
                () => {
                    if (gameSettings.UpgradeSettings.TemporaryUpgrades[index].CanUpgrade())
                    {
                        gameSettings.UpgradeSettings.TemporaryUpgrades[index].Upgrade();
                    }
                }
            );
        }
    }

    private void Update()
    {
        autoUpgradeTimeRemaining -= Time.deltaTime;
        if (autoUpgrading && autoUpgradeTimeRemaining <= 0)
        {
            autoUpgradeTimeRemaining = autoUpgradeInterval;
            foreach (TemporaryUpgradeBase upgrade in gameSettings.UpgradeSettings.TemporaryUpgrades.Where(upgrade => upgrade.CanUpgrade()))
            {
                upgrade.Upgrade();
                Debug.Log($"{nameof(TemporaryUpgradeManager)}.{nameof(Update)}() - Upgrading {upgrade.Title}");
            }
        }
    }

    public void SetMenuOpen(bool value)
    {
        menuOpen = value;
    }

    public void ToggleAutoUpgrade(bool inputValue)
    {
        autoUpgrading = inputValue;
        Debug.Log($"{nameof(TemporaryUpgradeManager)}.{nameof(ToggleAutoUpgrade)}() - AutoUpgrading: {autoUpgrading}");
    }
}