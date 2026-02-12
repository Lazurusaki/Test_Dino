using _Project.Scripts.Configs.Gameplay;
using _Project.Scripts.Configs.Gameplay.Levels;
using _Project.Scripts.Configs.Localization;
using _Project.Scripts.Configs.MainMenu;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public LanguagesListConfig LanguagesListConfig { get; private set; }
        [field: SerializeField] public LevelsListConfig LevelsListConfig { get; private set; }
        [field: SerializeField] public MainMenuConfig MainMenuConfig { get; private set;}
        [field: SerializeField] public GameplayConfig GameplayConfig { get; private set; }
    }
}