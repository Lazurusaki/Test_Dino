using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.CommonUI;
using _Project.Scripts.Configs.Gameplay.Levels;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Project.Scripts.MainMenu.UI
{
    public class LevelsMenuBuilder
    {
        private const int RowSize = 3;
        
        public void Build(LevelsListConfig config, GameObject levelPrefab, GameObject groupPrefab, LevelSelector levelSelector, Sprite[] icons)
        {
            var levelsSaveData = YG2.saves.LevelsData;
            
            List<UnlockableButton> _levels = new();
            Transform content = levelSelector.Scroll.Content;
            
            Transform currentGroup = null;
            int indexInRow = 0;
            
            for (int i = 0; i < config.Levels.Count; i++)
            {
                if (currentGroup == null || indexInRow >= RowSize)
                {
                    currentGroup = CreateGroup(groupPrefab);
                    currentGroup.SetParent(content, false);
                    indexInRow = 0;
                }


                var levelData = levelsSaveData.Levels.First(levelData => levelData.LevelID == config.Levels[i].LevelConfig.ID);
                var isLocked = levelData.IsLocked;
                
                var levelButton = CreateLevel(icons[i],levelPrefab, isLocked);
                levelButton.transform.SetParent(currentGroup, false);
                
                _levels.Add(levelButton);
                
                indexInRow++;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(content as RectTransform);
            Canvas.ForceUpdateCanvases();
            
            levelSelector.Initialize(_levels);
        }

        private UnlockableButton CreateLevel(Sprite icon, GameObject levelPrefab, bool isLocked = false)
        {
            var button = Object.Instantiate(levelPrefab).GetComponent<UnlockableButton>();

            button.SetIcon(icon);
            button.SetLocked(isLocked);

            return button;
        }

        private Transform CreateGroup(GameObject levelPrefab)
        {
            return Object.Instantiate(levelPrefab).transform;
        }
    }
}