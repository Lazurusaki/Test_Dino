using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.MainMenu
{
    [CreateAssetMenu(fileName = "MainMenuConfig", menuName = "Configs/Game/MainMenuConfig")]
    public class MainMenuConfig:ScriptableObject
    {
        [field: SerializeField] public AssetReferenceT<AudioClip> MainMenuMusic { get; private set; }
        [field: SerializeField] public AssetReferenceT<AudioClip> LevelsSwipeSound { get; private set; }
        [field: SerializeField] public AssetReferenceSprite Background { get; private set; }
        
        [field: SerializeField] public AssetReferenceGameObject LevelButtonPrefab { get; private set; }

        [field: SerializeField] public AssetReferenceGameObject LevelsGroupPrefab { get; private set; }
    }
}