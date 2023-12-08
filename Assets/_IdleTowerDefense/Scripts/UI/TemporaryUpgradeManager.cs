using System;
using System.Collections.Generic;
using System.Linq;
using DuloGames.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TemporaryUpgradeManager : Singleton<TemporaryUpgradeManager> {
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TemporaryUpgradeButton temporaryUpgradeButtonPrefab;
    [FormerlySerializedAs("buttonContainer")]
    [SerializeField] private Transform upgradeButtonContainer;
    [SerializeField] private Transform friendlySpawnButtonContainer;
    [SerializeField] private AudioSource buttonAudioSource;
    [SerializeField] private float autoUpgradeInterval = 0.5f;
    [SerializeField] private List<Transform> tabContents = new List<Transform>();

    // String = Upgrade.Title, int = upgrade level
    public Dictionary<string, int> TemporaryUpgradeCounts = new Dictionary<string, int>();
    public bool MenuOpen => menuOpen;
    private bool menuOpen = false;
    private int currentTabIndex = 0;
    private bool autoUpgrading = false;
    private float autoUpgradeTimeRemaining = 0;

    private void Start() {
        gameSettings.UpgradeSettings.InitTemporaryUpgrades();

        SetCurrentTabIndex(currentTabIndex);

        InitTemporaryUpgradeButtons();
        InitFriendlySpawnButtons();
    }

    private void InitTemporaryUpgradeButtons() {
        for (int i = 0; i < gameSettings.UpgradeSettings.TemporaryUpgrades.Count; i++) {
            // Capture i to avoid closure issues
            int index = i;

            // Init dictionary element for the upgrade
            TemporaryUpgradeCounts.Add(gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title, 0);

            // Setup UpgradeButton values
            TemporaryUpgradeButton temporaryUpgradeButton = Instantiate(temporaryUpgradeButtonPrefab, upgradeButtonContainer);
            temporaryUpgradeButton.targetTemporaryUpgrade = gameSettings.UpgradeSettings.TemporaryUpgrades[i];
            temporaryUpgradeButton.ElementSound.audioObject = buttonAudioSource;
            temporaryUpgradeButton.titleObj.text = gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title.ToUpper();
            temporaryUpgradeButton.descriptionObj.text = gameSettings.UpgradeSettings.TemporaryUpgrades[i].ShortDescription;

            if (gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage != null) {
                temporaryUpgradeButton.backgroundImageObj.sprite = gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage;
            }

            // Setup tooltip
            List<UITooltipLineContent> LineList = new List<UITooltipLineContent>();
            LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title));

            foreach (string t in gameSettings.UpgradeSettings.TemporaryUpgrades[i].DescriptionLines) {
                LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Default, null, t));
            }

            temporaryUpgradeButton.Tooltip.contentLines = LineList.ToArray();

            // ScriptableObject handles upgrading itself

            temporaryUpgradeButton.Button.onClick.AddListener(
                () => {
                    if (gameSettings.UpgradeSettings.TemporaryUpgrades[index].CanUpgrade()) {
                        gameSettings.UpgradeSettings.TemporaryUpgrades[index].Upgrade();
                    }
                });
        }
    }

    private void InitFriendlySpawnButtons() {
        for (int i = 0; i < gameSettings.UpgradeSettings.FriendlySpawnables.Count; i++) {
            // Capture i to avoid closure issues
            int index = i;

            // Init dictionary element for the upgrade
            // TemporaryUpgradeCounts.Add(gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title, 0);

            // Setup UpgradeButton values
            TemporaryUpgradeButton friendlySpawnButton = Instantiate(temporaryUpgradeButtonPrefab, friendlySpawnButtonContainer);
            friendlySpawnButton.targetTemporaryUpgrade = gameSettings.UpgradeSettings.FriendlySpawnables[i];
            friendlySpawnButton.ElementSound.audioObject = buttonAudioSource;
            friendlySpawnButton.titleObj.text = gameSettings.UpgradeSettings.FriendlySpawnables[i].Title.ToUpper();
            friendlySpawnButton.descriptionObj.text = gameSettings.UpgradeSettings.FriendlySpawnables[i].ShortDescription;

            if (gameSettings.UpgradeSettings.FriendlySpawnables[i].BackgroundImage != null) {
                friendlySpawnButton.backgroundImageObj.sprite = gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage;
            }

            // Setup tooltip
            List<UITooltipLineContent> LineList = new List<UITooltipLineContent>();
            LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, gameSettings.UpgradeSettings.FriendlySpawnables[i].Title));

            foreach (string t in gameSettings.UpgradeSettings.FriendlySpawnables[i].DescriptionLines) {
                LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Default, null, t));
            }

            friendlySpawnButton.Tooltip.contentLines = LineList.ToArray();

            // ScriptableObject handles spawning itself

            friendlySpawnButton.Button.onClick.AddListener(
                () => {
                    if (gameSettings.UpgradeSettings.FriendlySpawnables[index].CanUpgrade()) {
                        gameSettings.UpgradeSettings.FriendlySpawnables[index].Upgrade();
                    }
                });
        }
    }

    private void Update() {
        autoUpgradeTimeRemaining -= Time.deltaTime;

        if (autoUpgrading && autoUpgradeTimeRemaining <= 0) {
            autoUpgradeTimeRemaining = autoUpgradeInterval;

            foreach (TemporaryUpgradeBase upgrade in gameSettings.UpgradeSettings.TemporaryUpgrades.Where(upgrade => upgrade.CanUpgrade())) {
                upgrade.Upgrade();
                Debug.Log($"{nameof(TemporaryUpgradeManager)}.{nameof(Update)}() - Upgrading {upgrade.Title}");
            }
        }
    }

    public void SetMenuOpen(bool value) {
        menuOpen = value;
    }

    public void SetCurrentTabIndex(int index) {
        currentTabIndex = index;
        for (int i = 0; i < tabContents.Count; i++) {
            tabContents[i].gameObject.SetActive(currentTabIndex == i);
        }
    }

    public void ToggleAutoUpgrade(bool inputValue) {
        autoUpgrading = inputValue;
        Debug.Log($"{nameof(TemporaryUpgradeManager)}.{nameof(ToggleAutoUpgrade)}() - AutoUpgrading: {autoUpgrading}");
    }
}