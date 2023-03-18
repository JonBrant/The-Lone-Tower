using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ToDo: Add tooltips?
public class PersistentUpgradeManager : Singleton<PersistentUpgradeManager>
{
    public Dictionary<string, int> PersistentUpgradeCounts = new Dictionary<string, int>();
    public float RemainingScrap = 0;

    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TextMeshProUGUI remainingScrapText;
    [SerializeField] private PersistentUpgradeButton persistentUpgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        // Init default empty dictionary
        Dictionary<string, int> defaultValues = new Dictionary<string, int>();
        foreach (var upgrade in gameSettings.UpgradeSettings.PersistentUpgrades)
        {
            defaultValues.Add(upgrade.Title, 0);
        }

        // Load saved upgrade counts and Scrap count
        RemainingScrap = ES3.Load(SaveKeys.Scrap, 0f);
        PersistentUpgradeCounts = ES3.Load(SaveKeys.PersistentUpgradeCounts, defaultValues);
        remainingScrapText.text = $"SCRAP {RemainingScrap:N0}";
        
        // Check for new upgrades to avoid exception
        foreach (var upgrade in gameSettings.UpgradeSettings.PersistentUpgrades)
        {
            if (!PersistentUpgradeCounts.ContainsKey(upgrade.Title))
            {
                PersistentUpgradeCounts.Add(upgrade.Title, 0);
                Debug.Log($"{nameof(PersistentUpgradeManager)}.{nameof(Start)}() - Adding new upgrade: {upgrade.Title}");
            }
        }
        
        InitButtons();
    }

    private void InitButtons()
    {
        foreach (var upgrade in gameSettings.UpgradeSettings.PersistentUpgrades)
        {
            PersistentUpgradeButton currentButton = Instantiate(persistentUpgradeButtonPrefab, buttonContainer);
            currentButton.TargetUpgrade = upgrade;
            currentButton.TitleText.text = upgrade.Title;
            //currentButton.DescriptionText.text = upgrade.ShortDescription;
            //upgrade.SetText(currentButton.DescriptionText);
            currentButton.DescriptionText.text = upgrade.GetDescription();
            currentButton.CostText.text = $"COST: {upgrade.GetCost():N1}";
            currentButton.UpgradeAmountText.text = $"LEVEL: {PersistentUpgradeCounts[upgrade.Title].ToString()}";
            currentButton.ElementSound.audioObject = audioSource;
            if (RemainingScrap < upgrade.GetCost())
            {
                currentButton.Button.interactable = false;
            }

            currentButton.Button.onClick.AddListener(
                () => {
                    if (RemainingScrap < upgrade.GetCost())
                    {
                        currentButton.Button.interactable = false;
                        return;
                    }

                    PersistentUpgradeCounts[upgrade.Title]++;
                    RemainingScrap -= upgrade.GetCost();
                    ES3.Save(SaveKeys.Scrap, RemainingScrap);
                    ES3.Save(SaveKeys.PersistentUpgradeCounts, PersistentUpgradeCounts);
                    currentButton.CostText.text = $"COST: {upgrade.GetCost():N1}";
                    currentButton.UpgradeAmountText.text = $"LEVEL: {PersistentUpgradeCounts[upgrade.Title].ToString()}";
                    remainingScrapText.text = $"SCRAP {RemainingScrap:N0}";
                }
            );
        }
    }

    public void ClearSaveData()
    {
        ES3.DeleteFile();
    }
}