using System;
using System.Threading.Tasks;
using _Project.Scripts.CommonServices.AssetsManagement;
using _Project.Scripts.Configs.Game;
using _Project.Scripts.Configs.Gameplay;
using _Project.Scripts.Configs.Gameplay.Levels;
using _Project.Scripts.Configs.Localization;
using _Project.Scripts.Configs.MainMenu;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CommonServices.ConfigsManagement
{
    public sealed class ConfigsProviderService 
    {
        private readonly AddressablesAssetLoader _assetLoader;
        private GameConfig _gameConfig;

        public MainMenuConfig MainMenuConfig => _gameConfig.MainMenuConfig;
        public GameplayConfig GameplayConfig => _gameConfig.GameplayConfig;
        public LanguagesListConfig LanguagesConfig => _gameConfig.LanguagesListConfig;
        public LevelsListConfig LevelsConfig => _gameConfig.LevelsListConfig;

        [Inject]
        public ConfigsProviderService(AddressablesAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async Task LoadAll()
        {
            _gameConfig = await _assetLoader.LoadAsync<GameConfig>(ConfigKeys.GameConfig);

            CheckConfig(MainMenuConfig);
            CheckConfig(GameplayConfig);
            CheckConfig(LanguagesConfig);
            CheckConfig(LevelsConfig);
        }
        
        private void CheckConfig<T>(T config) where T : ScriptableObject
        {
            if (config == null)
                throw new NullReferenceException($"Config of type {typeof(T)} is not assigned in GameConfig");
        }

        /*
        private bool TryGetConfig<T>(string key, out T config) where T : ScriptableObject
        {
            config = _assetLoader.Get<T>(key);
            return config != null;
        }

        private async Task LoadConfigsByLabel<T>(string label) where T : ScriptableObject
        {
            var handle = Addressables.LoadAssetsAsync<T>(label, null);
            var configs = await handle.Task;

            foreach (var config in configs)
            {
                _configs[config.name] = config;
            }
        }

        public T Get<T>(string key) where T : ScriptableObject
        {
            if (_configs.TryGetValue(key, out var config) == false)
                throw new KeyNotFoundException($"Config not found: {key}");

            return (T)_configs[key];
        }
        */
    }
}