using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.Gameplay.Puzzles;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Levels/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public LevelID ID { get; private set; }
        [field: SerializeField] public LocalizationTextKey LevelNameKey { get; private set; }
        [field: SerializeField] public LocalizationAudioKey LevelAudioKeyKey { get; private set; }
        [field: SerializeField] public AssetReferenceSprite Icon { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject PuzzlePrefab { get; private set; }
        
        [field: SerializeField] public Color TextColor { get; private set; }
        
        [field: SerializeField] public AssetReferenceT<AudioClip> Voice { get; private set; }
    }
}