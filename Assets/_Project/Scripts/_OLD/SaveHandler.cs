using UnityEngine;

public static class SaveHandler
{
    private const string LevelsBuyKey = "LevelsBuy";

    public static void SaveBuyState(bool state)
    {
        PlayerPrefs.SetInt(LevelsBuyKey, state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetBuyState()
    {
        return PlayerPrefs.GetInt(LevelsBuyKey, 0) == 1;
    }
    
    /*
    public static void SaveBuyState(bool state)
    {
        if (PlayerPrefs.HasKey(levelsBuyKey))
        {
            PlayerPrefs.SetString(levelsBuyKey, state.ToString());
        }
        else
        {
            PlayerPrefs.SetString(levelsBuyKey, state.ToString());
        }
    }

    public static string GetBuyState()
    {
        if (PlayerPrefs.HasKey(levelsBuyKey))
            return PlayerPrefs.GetString(levelsBuyKey);

        return "";
    }
    */
}
