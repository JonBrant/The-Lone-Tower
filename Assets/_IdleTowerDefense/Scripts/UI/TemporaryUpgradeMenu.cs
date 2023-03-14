using System.Collections.Generic;
using DuloGames.UI;
using Unity.VisualScripting;
using UnityEngine;

public class TemporaryUpgradeMenu : Singleton<TemporaryUpgradeMenu>
{
    [SerializeField] private UpgradeSettings upgradeSettings;
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
        upgradeSettings.Init();
        for (int i = 0; i < upgradeSettings.TemporaryUpgrades.Count; i++)
        {
            // Capture i to avoid closure issues
            int index = i;

            // Init dictionary element for the upgrade
            UpgradeCounts.Add(upgradeSettings.TemporaryUpgrades[i].Title, 0);

            // Setup UpgradeButton values
            UpgradeButton upgradeButton = Instantiate(upgradeButtonPrefab, buttonContainer);
            upgradeButtons.Add(upgradeButton);
            upgradeButton.TargetUpgrade = upgradeSettings.TemporaryUpgrades[i];
            upgradeButton.ElementSound.audioObject = buttonAudioSource;
            upgradeButton.titleObj.text = upgradeSettings.TemporaryUpgrades[i].Title.ToUpper();
            upgradeButton.descriptionObj.text = upgradeSettings.TemporaryUpgrades[i].ShortDescription;
            if (upgradeSettings.TemporaryUpgrades[i].BackgroundImage != null)
            {
                upgradeButton.backgroundImageObj.sprite = upgradeSettings.TemporaryUpgrades[i].BackgroundImage;
            }

            // Setup tooltip
            List<UITooltipLineContent> LineList = new List<UITooltipLineContent>();
            LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, upgradeSettings.TemporaryUpgrades[i].Title));
            foreach (string t in upgradeSettings.TemporaryUpgrades[i].DescriptionLines)
            {
                LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Default, null, t));
            }

            upgradeButton.Tooltip.contentLines = LineList.ToArray();

            // ScriptableObject handles upgrading itself

            upgradeButton.Button.onClick.AddListener(
                () => {
                    if (upgradeSettings.TemporaryUpgrades[index].CanUpgrade())
                    {
                        upgradeSettings.TemporaryUpgrades[index].Upgrade();
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