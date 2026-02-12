using System.Collections.Generic;
using _Project.Scripts.Gameplay.Puzzles;
using YG;

namespace _Project.Scripts.CommonServices.SaveLoadManagement
{
    public class SaveLoadService
    {
        public bool CheckIsFirstRun()
        {
            if (YG2.saves.LevelsData == null || YG2.saves.LevelsData.Levels == null|| YG2.saves.LevelsData.Levels.Count == 0)
                return true;

            return false;
        }
        
        public void Save()
        {
            YG2.SaveProgress();
        }
        
        public void Save(LevelsData levelsData)
        {
            YG2.saves.LevelsData = levelsData;
            YG2.SaveProgress();
        }

        public void Load()
        {
            if (CheckIsFirstRun())
                ClearSaveData();
        }
        
        public void ClearSaveData()
        {
            Save(new LevelsData());
        }
    }
}

public class PlayerData
{
    
}

namespace YG
{
    public partial class SavesYG
    {
        public LevelsData LevelsData;
    }

    [System.Serializable]
    public class LevelData
    {
        public LevelID LevelID;
        public bool IsLocked;

        public LevelData(LevelID levelID, bool isLocked = false)
        {
            LevelID = levelID;
            IsLocked = isLocked;
        }
    }
    
    [System.Serializable]
    public class LevelsData
    {
        public List<LevelData> Levels = new();
    }
}







