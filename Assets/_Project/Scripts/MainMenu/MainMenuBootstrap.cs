using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.CommonServices.AssetsManagement;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.Configs.Localization;
using _Project.Scripts.MainMenu.Controller;
using _Project.Scripts.MainMenu.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
using Zenject;

namespace _Project.Scripts.MainMenu
{
    public class MainMenuBootstrap : ISceneBootstrap
    {
        private readonly AddressablesAssetLoader _assetLoader;
        private readonly ConfigsProviderService _configsProvider;
        private readonly LanguageService _languageService;
        private readonly LevelsMenuBuilder _levelsMenuBuilder;
        private readonly MainMenuController _mainMenuController;
        private readonly MainMenuUIRoot _mainMenuUIRoot;

        [Inject]
        public MainMenuBootstrap(
            AddressablesAssetLoader assetLoader,
            ConfigsProviderService configsProvider,
            LanguageService languageService,
            LevelsMenuBuilder mainMenuBuilder,
            MainMenuController mainMenuController,
            MainMenuUIRoot mainMenuUIRoot)
        {
            _assetLoader = assetLoader;
            _configsProvider = configsProvider;
            _languageService = languageService;
            _levelsMenuBuilder = mainMenuBuilder;
            _mainMenuController = mainMenuController;
            _mainMenuUIRoot = mainMenuUIRoot;
        }

        public async UniTask Run(SceneParams sceneParams)
        {
            try
            {
                var config = _configsProvider.MainMenuConfig;

                var (
                    background,
                    music,
                    swipe,
                    levelButton,
                    levelsGroup,
                    flags,
                    levelIcons
                    ) = await UniTask.WhenAll(
                    _assetLoader.LoadAsync<Sprite>(config.Background),
                    _assetLoader.LoadAsync<AudioClip>(config.MainMenuMusic),
                    _assetLoader.LoadAsync<AudioClip>(config.LevelsSwipeSound),
                    _assetLoader.LoadAsync<GameObject>(config.LevelButtonPrefab),
                    _assetLoader.LoadAsync<GameObject>(config.LevelsGroupPrefab),
                    LoadLanguageFlagsAsync(),
                    LoadLevelIconsAsync());

                var titlesInfo = _languageService.GetTexts(LocalizationTextKey.GameTitleText);

                var audioData = new MainMenuAudioData(music, swipe);
                var uiData = new MainMenuUIData(background);
                var localizationData = new MainMenuLocalizationData(titlesInfo, flags);

                //UPDATE SAVE DATA FROM LEVELSTATUSCONFIG
                UpdateSaveDataFromLevelsConfig();

                _levelsMenuBuilder.Build(
                    _configsProvider.LevelsConfig,
                    levelButton,
                    levelsGroup,
                    _mainMenuUIRoot.LevelSelector,
                    levelIcons);

                _mainMenuController.Initialize(audioData, uiData, localizationData);
                _mainMenuController.Run();
            }
            catch (Exception exception)
            {
                Debug.LogError($"MainMenu scene loading error: {exception}");
            }
        }

        private async UniTask<Dictionary<Language, Sprite>> LoadLanguageFlagsAsync()
        {
            var flagRefs = _languageService.GetFlags();

            var results = await UniTask.WhenAll(
                flagRefs.Select(async pair =>
                    new KeyValuePair<Language, Sprite>(
                        pair.Key,
                        await _assetLoader.LoadAsync<Sprite>(pair.Value)))
            );

            return results.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private async UniTask<Sprite[]> LoadLevelIconsAsync()
        {
            var tasks = _configsProvider.LevelsConfig.Levels
                .Select(statusConfig => _assetLoader.LoadAsync<Sprite>(statusConfig.LevelConfig.Icon))
                .ToArray();

            var sprites = await UniTask.WhenAll(tasks);
            return sprites;
        }

        private void UpdateSaveDataFromLevelsConfig()
        {
            var saveData = YG2.saves.LevelsData.Levels;
            var levelsStatusConfigs = _configsProvider.LevelsConfig.Levels;

            foreach (var config in levelsStatusConfigs)
            {
                var levelSaveData = saveData.FirstOrDefault(levelData => levelData.LevelID == config.LevelConfig.ID);

                if (levelSaveData == null)
                    saveData.Add(new LevelData(config.LevelConfig.ID, config.IsLocked));
            }
        }
    }

    public class MainMenuLocalizationData
    {
        public readonly IReadOnlyDictionary<Language, string> Titles;
        public readonly IReadOnlyDictionary<Language, Sprite> LanguageIcons;

        public MainMenuLocalizationData(Dictionary<Language, string> titles,
            Dictionary<Language, Sprite> languageIcons)
        {
            Titles = titles;
            LanguageIcons = languageIcons;
        }
    }


    public class MainMenuUIData
    {
        public readonly Sprite Background;

        public MainMenuUIData(Sprite background)
        {
            Background = background;
        }
    }

    public class MainMenuAudioData
    {
        public readonly AudioClip Music;
        public readonly AudioClip SwipeSound;

        public MainMenuAudioData(AudioClip music, AudioClip swipeSound)
        {
            Music = music;
            SwipeSound = swipeSound;
        }
    }
}