using System.Collections.Generic;
using DuloGames.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TemporaryUpgradeMenu : Singleton<TemporaryUpgradeMenu>
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private UpgradeButton upgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource buttonAudioSource;

    // String = Upgrade.Title, int = upgrade level
    public Dictionary<string, int> UpgradeCounts = new Dictionary<string, int>();
    private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
    public bool MenuOpen => menuOpen;
    private bool menuOpen = false;
    
    private void Start()
    {
        gameSettings.UpgradeSettings.Init();
        for (int i = 0; i < gameSettings.UpgradeSettings.TemporaryUpgrades.Count; i++)
        {
            // Capture i to avoid closure issues
            int index = i;

            // Init dictionary element for the upgrade
            UpgradeCounts.Add(gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title, 0);

            // Setup UpgradeButton values
            UpgradeButton upgradeButton = Instantiate(upgradeButtonPrefab, buttonContainer);
            upgradeButtons.Add(upgradeButton);
            upgradeButton.TargetUpgrade = gameSettings.UpgradeSettings.TemporaryUpgrades[i];
            upgradeButton.ElementSound.audioObject = buttonAudioSource;
            upgradeButton.titleObj.text = gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title.ToUpper();
            upgradeButton.descriptionObj.text = gameSettings.UpgradeSettings.TemporaryUpgrades[i].ShortDescription;
            if (gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage != null)
            {
                upgradeButton.backgroundImageObj.sprite = gameSettings.UpgradeSettings.TemporaryUpgrades[i].BackgroundImage;
            }

            // Setup tooltip
            List<UITooltipLineContent> LineList = new List<UITooltipLineContent>();
            LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, gameSettings.UpgradeSettings.TemporaryUpgrades[i].Title));
            foreach (string t in gameSettings.UpgradeSettings.TemporaryUpgrades[i].DescriptionLines)
            {
                LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Default, null, t));
            }

            upgradeButton.Tooltip.contentLines = LineList.ToArray();

            // ScriptableObject handles upgrading itself

            upgradeButton.Button.onClick.AddListener(
                () => {
                    if (gameSettings.UpgradeSettings.TemporaryUpgrades[index].CanUpgrade())
                    {
                        gameSettings.UpgradeSettings.TemporaryUpgrades[index].Upgrade();
                    }
                }
            );
        }
    }

    public void SetMenuOpen(bool value)
    {
        menuOpen = value;
    }
}