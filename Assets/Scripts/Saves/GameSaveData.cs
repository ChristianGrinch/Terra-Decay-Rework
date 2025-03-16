using UnityEngine;
using MessagePack;

public class GameSaveData
{
    // Player Data (Keys 0-49)
    
    // Game Data (Keys 50-99)
    
    
    public static GameSaveData FetchSaveData()
    {
        return new GameSaveData()
        {

        };
    }

    public static GameSaveData CreateDefaultData()
    {
        return new GameSaveData
        {

        };
    }
    
}
