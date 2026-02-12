using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.Configs.Localization;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CommonServices.Localization
{
    public class LanguageService
    {
        private readonly ConfigsProviderService _configsProviderService;

        private readonly ReactiveProperty<Language> _currentLanguage = new();

        private LanguagesListConfig _languagesListConfig;
        private LanguageConfig _currentLanguageConfig;

        public IReadOnlyReactiveProperty<Language> CurrentLanguage => _currentLanguage;


        [Inject]
        public LanguageService(ConfigsProviderService configsProviderService)
        {
            _configsProviderService = configsProviderService;
        }

        public void Initialize()
        {
            _languagesListConfig = _configsProviderService.LanguagesConfig;
            
            if (_languagesListConfig.Languages == null || _languagesListConfig.Languages.Count == 0)
                throw new InvalidOperationException("Languages list is empty");

            SetLanguage(_languagesListConfig.Languages.First().Language);
        }

        public void SetLanguage(Language language)
        {
            var config = _languagesListConfig.GetLanguageConfig(language);

            if (config == null)
            {
                Debug.LogWarning($"Language {language} not found in configs");
                return;
            }

            _currentLanguage.Value = language;
            _currentLanguageConfig = config;
        }

        public void SetLanguageByCountry(string language)
        {
            SetLanguage(CountryLanguages.GetLanguageByCountry(language));
        }


        public void NextLanguage()
        {
            if (_languagesListConfig?.Languages == null || _languagesListConfig.Languages.Count == 0)
                return;

            int currentIndex = _languagesListConfig.Languages
                .FindIndex(c => c.Language == _currentLanguage.Value);

            int nextIndex = (currentIndex + 1) % _languagesListConfig.Languages.Count;

            SetLanguage(_languagesListConfig.Languages[nextIndex].Language);
        }

        // -------------------- GET CURRENT LANGUAGE INFO -------------------
        
        public AssetReferenceSprite GetFlag()
        {
            return _currentLanguageConfig.Icon;
        }
        
        public string GetText(LocalizationTextKey key)
        {
            return _currentLanguageConfig.TextTable.Get(key);
        }

        public AssetReferenceT<AudioClip> GetVoice(LocalizationAudioKey key)
        {
            return _currentLanguageConfig.AudioTable.Get(key);
        }
        
        // -------------------- GET INFO BY LANGUAGE -------------------
        
        public AssetReferenceSprite GetFlag(Language language)
        {
            return GetLanguageConfig(language).Icon;
        }
        
        public string GetText(LocalizationTextKey key, Language language)
        {
            return GetLanguageConfig(language).TextTable.Get(key);
        }
        
        public AssetReferenceT<AudioClip> GetVoice(LocalizationAudioKey key, Language language)
        {
            return GetLanguageConfig(language).AudioTable.Get(key);
        }
        
        // -------------- GET ALL KEY INFO IN ALL LANGUAGES

        public Dictionary<Language, AssetReferenceSprite> GetFlags()
        {
            Dictionary<Language, AssetReferenceSprite > info = new();
            
            foreach (var languageConfig in _languagesListConfig.Languages)
            {
                info.Add(languageConfig.Language,languageConfig.Icon);
            }
            
            return info;
        }
        
        public Dictionary<Language, string> GetTexts(LocalizationTextKey key)
        {
            Dictionary<Language, string > info = new();
            
            foreach (var languageConfig in _languagesListConfig.Languages)
            {
                info.Add(languageConfig.Language, languageConfig.TextTable.Get(key) );
            }
            
            return info;
        }
        
        public Dictionary<Language, AssetReferenceT<AudioClip>> GetAudios(LocalizationAudioKey key)
        {
            Dictionary<Language, AssetReferenceT<AudioClip> > info = new();
            
            foreach (var languageConfig in _languagesListConfig.Languages)
            {
                info.Add(languageConfig.Language, languageConfig.AudioTable.Get(key));
            }
            
            return info;
        }
        
        
        
        private LanguageConfig GetLanguageConfig(Language language)
        {
            var languageConfig = _languagesListConfig.Languages
                .First(c => c.Language == language);
            
            if (languageConfig == null)
                throw new InvalidOperationException($"Language {language} not found");
            
            return languageConfig;
        }
    }
}