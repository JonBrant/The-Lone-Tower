using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// ToDo: Add tooltips?
public class PersistentUpgradeManager : Singleton<PersistentUpgradeManager>
{
    public Dictionary<string, int> PersistentUpgradeCounts = new Dictionary<string, int>();
    public float RemainingScrap = 0;
    public GameSettings GameSettings;

    [SerializeField] private TextMeshProUGUI remainingScrapText;
    [SerializeField] private PersistentUpgradeButton persistentUpgradeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AudioSource audioSource;

    private List<PersistentUpgradeButton> buttons = new List<PersistentUpgradeButton>();

    private void Start()
    {
        InitSaveData();
        InitMenu();
    }

    private void InitSaveData()
    {
        // Init default empty dictionary
        Dictionary<string, int> defaultValues = new Dictionary<string, int>();
        foreach (var upgrade in GameSettings.UpgradeSettings.PersistentUpgrades)
        {
            defaultValues.Add(upgrade.Title, 0);
        }

        // Load saved upgrade counts and Scrap count
        RemainingScrap = ES3.Load(SaveKeys.Scrap, 0f);
        PersistentUpgradeCounts = ES3.Load(SaveKeys.PersistentUpgradeCounts, defaultValues);
    }

    private void InitMenu()
    {
        remainingScrapText.text = $"SCRAP {RemainingScrap:N2}";
        foreach (var upgrade in GameSettings.UpgradeSettings.PersistentUpgrades)
        {
            PersistentUpgradeButton currentButton = Instantiate(persistentUpgradeButtonPrefab, buttonContainer);
            buttons.Add(currentButton);
            currentButton.TargetUpgrade = upgrade;
            currentButton.TitleText.text = upgrade.Title;
            currentButton.DescriptionText.text = upgrade.GetDescription();
            currentButton.CostText.text = $"COST: {upgrade.GetCost():N1}";
            currentButton.UpgradeAmountText.text = $"LEVEL: {PersistentUpgradeCounts[upgrade.Title].ToString()}";
            currentButton.ElementSound.audioObject = audioSource;
            currentButton.Button.onClick.AddListener(
                () => {
                    RemainingScrap -= upgrade.GetCost();
                    PersistentUpgradeCounts[upgrade.Title]++;
                    ES3.Save(SaveKeys.Scrap, RemainingScrap);
                    ES3.Save(SaveKeys.PersistentUpgradeCounts, PersistentUpgradeCounts);
                    currentButton.CostText.text = $"COST: {upgrade.GetCost():N1}";
                    currentButton.UpgradeAmountText.text = $"LEVEL: {PersistentUpgradeCounts[upgrade.Title].ToString()}";
                    remainingScrapText.text = $"SCRAP {RemainingScrap:N1}";
                    UpdateButtonInteractability();
                }
            );
        }

        UpdateButtonInteractability();
    }

    private void UpdateButtonInteractability()
    {

        foreach (PersistentUpgradeButton button in buttons)
        {
            button.Button.interactable = button.TargetUpgrade.CanUpgrade();
        }
    }

    public void ClearSaveData()
    {
        ES3.DeleteFile();
        InitSaveData();
        // Clear menu and re-initialize it
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            Destroy(buttons[i].gameObject);
        }

        InitMenu();
    }
}