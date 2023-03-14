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

// ToDo: Save and display high score when losing. Score being EnemiesKilled. Display EnemiesKilled in Restart dialog box
// ToDo: More decimal places on attack speed
// ToDo: Max levels on upgrades, maybe unlock new ones

public class GameManager : Singleton<GameManager>
{
    
    public EcsWorld World;
    public Dictionary<CurrencyTypes, float> Currency = new Dictionary<CurrencyTypes, float>();
    public int EnemiesKilled = 0;
    
    [SerializeField] private LoadingScreenManager LoadingScreenManager;

    private ModalWindowManager RestartWindow;

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
        Time.timeScale = 0;

        // Destroy tower, enemies and projectiles
        var enemies = FindObjectsOfType<EnemyView>();
        var projectiles = FindObjectsOfType<ProjectileView>();
        var tower = FindObjectOfType<TowerView>();
        foreach (EnemyView enemyView in enemies)
        {
            Destroy(enemyView.gameObject);
        }

        foreach (ProjectileView projectileView in projectiles)
        {
            Destroy(projectileView.gameObject);
        }

        Destroy(tower.gameObject);

        // Check for new high score and save if necessary
        bool isNewHighScore = false;
        int highScore = ES3.KeyExists(SaveKeys.EnemiesKilled) ? (int)ES3.Load(SaveKeys.EnemiesKilled) : 0;
        if (highScore < EnemiesKilled)
        {
            isNewHighScore = true;
            ES3.Save(SaveKeys.EnemiesKilled, EnemiesKilled);
        }

        // Reference gets lost because of GameManager's DDOL
        if (RestartWindow == null)
        {
            RestartWindow = FindObjectOfType<ModalWindowManager>(true);
        }

        RestartWindow.windowDescription.text = $"Enemies Killed: {EnemiesKilled} \n" +
                                               $"Highest: {highScore.ToString()} " +
                                               $"{(isNewHighScore ? "New High score!" : "")} \n" +
                                               $"Try again?";
        RestartWindow.ModalWindowIn();
    }

    public void ReloadGame()
    {
        SaveGame();
        LoadingScreenManager.LoadScene(SceneManager.GetActiveScene().name);

        Currency[CurrencyTypes.Exp] = 0;
        EnemiesKilled = 0;
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
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