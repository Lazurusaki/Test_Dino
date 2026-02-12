using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Localization
{
    [CreateAssetMenu(fileName = "LanguageConfig", menuName = "Configs/Localization/LanguageConfig")]
    public class LanguageConfig : ScriptableObject
    {
        [field: SerializeField] public Language Language { get; private set; }

        [field: SerializeField] public AssetReferenceSprite Icon { get; private set; }

        [field: SerializeField] public LocalizationTextTableConfig TextTable { get; private set; }

        [field: SerializeField] public LocalizationAudioTableConfig AudioTable { get; private set; }
    }
}