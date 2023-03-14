using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrencyTypes
{
    Gold,
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

    [SerializeField] private ModalWindowManager RestartWindow;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // Init Currency dictionary
        List<CurrencyTypes> currencies = ((CurrencyTypes[])Enum.GetValues(typeof(CurrencyTypes))).ToList();
        foreach (CurrencyTypes currency in currencies)
        {
            Currency.Add(currency, 0f);
        }
    }

    public void SetGameSpeed(float newSpeed)
    {
        Time.timeScale = newSpeed;
    }

    public void OnTowerKilled()
    {
        Debug.Log($"{nameof(GameManager)}.{nameof(OnTowerKilled)}() - Message");

        Time.timeScale = 0;

        // Destroy tower, enemies and projetiles
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


        bool newHighScore = false;
        if (EnemiesKilled > PlayerPrefs.GetInt(PlayerPrefValues.HighestEnemiesKilled.ToString()))
        {
            PlayerPrefs.SetInt(PlayerPrefValues.HighestEnemiesKilled.ToString(), EnemiesKilled);
            newHighScore = true;
        }

        RestartWindow.windowDescription.text = $"Enemies Killed: {EnemiesKilled} " +
                                               $"\n Highest: {PlayerPrefs.GetInt(PlayerPrefValues.HighestEnemiesKilled.ToString())} " +
                                               $"{(newHighScore ? "New High score!" : "")}" +
                                               $"\n Try again?";
        RestartWindow.ModalWindowIn();

    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}