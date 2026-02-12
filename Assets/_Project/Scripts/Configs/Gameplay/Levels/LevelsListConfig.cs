using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Puzzles;
using UnityEngine;

namespace _Project.Scripts.Configs.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/Levels/LevelsConfig")]
    public class LevelsListConfig : ScriptableObject
    {
        [field: SerializeField] public List<LevelStatusConfig> Levels { get; private set; }


        [Serializable]
        public class LevelStatusConfig
        {
            [field: SerializeField] public LevelConfig LevelConfig { get; private set; }
            [field: SerializeField] public bool IsLocked { get; private set; }
        }
    }
}