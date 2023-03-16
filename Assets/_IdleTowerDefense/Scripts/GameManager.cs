using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Leopotam.EcsLite;
using Michsky.LSS;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrencyTypes
{
    Exp,
    Scrap
}

// ToDo: Max levels on upgrades, maybe unlock new ones
public class GameManager : Singleton<GameManager>
{
    public EcsWorld World;
    public Dictionary<CurrencyTypes, float> Currency = new Dictionary<CurrencyTypes, float>();
    public int EnemiesKilled = 0;
    public bool Paused = false;

    [SerializeField] private LoadingScreenManager LoadingScreenManager;
    [SerializeField] private ModalWindowManager RestartWindow;

    private void Awake()
    {
        // Init Currency dictionary
        List<CurrencyTypes> currencies = ((CurrencyTypes[])Enum.GetValues(typeof(CurrencyTypes))).ToList();
        foreach (CurrencyTypes currency in currencies)
        {
            Currency.Add(currency, 0f);
        }

        LoadGame();
    }

    public void SetGameSpeed(float newSpeed)
    {
        Time.timeScale = newSpeed;
    }

    public void OnTowerKilled()
    {
        Paused = true;

        // Cleanup all views
        var views = new List<Component>();
        views.AddRange(FindObjectsOfType<ProjectileView>());
        views.AddRange(FindObjectsOfType<EnemyView>());
        views.Add(FindObjectOfType<TowerView>());

        foreach (Component view in views)
        {
            Destroy(view.gameObject);
        }

        // Check for new high score and save if necessary
        bool isNewHighScore = false;
        int highScore = ES3.KeyExists(SaveKeys.EnemiesKilled) ? (int)ES3.Load(SaveKeys.EnemiesKilled) : 0;
        if (highScore < EnemiesKilled)
        {
            isNewHighScore = true;
            ES3.Save(SaveKeys.EnemiesKilled, EnemiesKilled);
        }

        RestartWindow.windowDescription.text = $"Enemies Killed: {EnemiesKilled} \n" +
                                               $"Highest: {highScore.ToString()} " +
                                               $"{(isNewHighScore ? "New High score!" : "")} \n" +
                                               $"Try again?";
        RestartWindow.ModalWindowIn();
    }

    public void ReloadGame()
    {
        Time.timeScale = 1;
        SaveGame();
        LoadingScreenManager.LoadScene(SceneManager.GetActiveScene().name);

        // Currency[CurrencyTypes.Exp] = 0;
        // EnemiesKilled = 0;

    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1;
        SaveGame();
        LoadingScreenManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SaveGame();
        Application.Quit();
    }

    public void SaveGame()
    {
        ES3.Save(SaveKeys.Scrap, Currency[CurrencyTypes.Scrap]);
    }

    public void LoadGame()
    {
        Currency[CurrencyTypes.Scrap] = ES3.Load(SaveKeys.Scrap, 0f);
    }
}