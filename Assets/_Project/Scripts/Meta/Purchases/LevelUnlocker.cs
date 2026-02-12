using System.Linq;
using _Project.Scripts.Gameplay.Puzzles;
using YG;

namespace _Project.Scripts.Meta.Purchases
{
    public class LevelUnlocker
    {
        public void UnlockLevel(LevelID id)
        {
            var level = YG2.saves.LevelsData.Levels.FirstOrDefault(level => level.LevelID == id);

            if (level != null)
            {
                level.IsLocked = false;
            }
        }
    }
}