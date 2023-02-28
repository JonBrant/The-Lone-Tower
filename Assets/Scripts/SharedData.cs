using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData 
{
    public float WaveSpawnDelay = 0.5f;
    public GameSettings Settings { get; private set; }

    public void InitDefaultValues(GameSettings inputSettings)
    {
        // ToDo: Load default values from ScriptableObject or something
        Settings = inputSettings;
    }
}
