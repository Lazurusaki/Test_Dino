using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Localization
{
    [CreateAssetMenu(fileName = "LocalizationConfig", menuName = "Configs/Localization/LocalizationConfig")]
    public class LanguagesListConfig:ScriptableObject
    {
        [field: SerializeField] public List<LanguageConfig> Languages { get; private set; }
        
        public LanguageConfig GetLanguageConfig(Language language)
        {
            return Languages.Find(c => c.Language == language);
        }
    }
}