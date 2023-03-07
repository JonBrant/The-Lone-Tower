using System.Collections.Generic;
using DuloGames.UI;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private UpgradeSettings upgradeSettings;
    [SerializeField] private UpgradeButton upgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource buttonAudioSource;

    // String = Upgrade.Title, int = upgrade level
    public Dictionary<string, int> UpgradeCounts = new Dictionary<string, int>();


    private void Start()
    {
        upgradeSettings.Init();
        for (int i = 0; i < upgradeSettings.Upgrades.Count; i++)
        {
            // Init dictionary element for the upgrade
            UpgradeCounts.Add(upgradeSettings.Upgrades[i].Title, 0);

            // Setup UpgradeButton values
            UpgradeButton upgradeButton = Instantiate(upgradeButtonPrefab, buttonContainer);
            upgradeButton.ElementSound.audioObject = buttonAudioSource;
            upgradeButton.titleObj.text = upgradeSettings.Upgrades[i].Title.ToUpper();
            upgradeButton.descriptionObj.text = upgradeSettings.Upgrades[i].ShortDescription;
            if (upgradeSettings.Upgrades[i].BackgroundImage != null)
            {
                upgradeButton.backgroundImageObj.sprite = upgradeSettings.Upgrades[i].BackgroundImage;
            }

            // Setup tooltip
            List<UITooltipLineContent> LineList = new List<UITooltipLineContent>();
            LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Title, null, upgradeSettings.Upgrades[i].Title));
            for (int j = 0; j < upgradeSettings.Upgrades[i].DescriptionLines.Count; j++)
            {
                LineList.Add(new UITooltipLineContent(UITooltipLines.LineStyle.Default, null, upgradeSettings.Upgrades[i].DescriptionLines[j]));
            }

            upgradeButton.Tooltip.contentLines = LineList.ToArray();

            // ScriptableObject handles upgrading itself
            upgradeButton.Button.onClick.AddListener(upgradeSettings.Upgrades[i].Upgrade);
        }
    }
}