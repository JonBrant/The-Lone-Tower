using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ToDo: Add tooltip
public class PersistentUpgradeManager : Singleton<PersistentUpgradeManager>
{
    public Dictionary<string, int> PersistentUpgradeCounts = new Dictionary<string, int>();
    public float RemainingScrap = 0;

    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TextMeshProUGUI remainingScrapText;
    [SerializeField] private PersistentUpgradeButton persistentUpgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Dictionary<string, int> defaultValues = new Dictionary<string, int>();
        
        foreach (var upgrade in gameSettings.UpgradeSettings.PersistentUpgrades)
        {
            defaultValues.Add(upgrade.Title, 0);
        }
        

        RemainingScrap = ES3.Load(SaveKeys.Scrap, 0f);
        remainingScrapText.text = $"SCRAP {RemainingScrap:N0}";
        PersistentUpgradeCounts = ES3.Load(SaveKeys.PersistentUpgradeCounts, defaultValues);
        foreach (KeyValuePair<string, int> kvp in PersistentUpgradeCounts)
        {
            Debug.Log($"{nameof(PersistentUpgradeManager)}.{nameof(Awake)}() - {kvp.Key} - {kvp.Value}");
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
            currentButton.DescriptionText.text = upgrade.ShortDescription;
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