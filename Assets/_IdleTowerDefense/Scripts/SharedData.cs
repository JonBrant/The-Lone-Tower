using System.Collections.Generic;

public class SharedData 
{
    public GameSettings Settings { get; private set; }
    public TowerView TowerView;
    public List<int> EntitiesInTowerRange;

    public void InitDefaultValues(GameSettings inputSettings)
    {
        Settings = inputSettings;
    }
}
