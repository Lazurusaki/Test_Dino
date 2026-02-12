using System;
using _Project.Scripts.CommonServices.AdvertisementManagement;
using _Project.Scripts.CommonServices.AudioManagement;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.CommonServices.SaveLoadManagement;
using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.MainMenu.UI;
using _Project.Scripts.Meta.Purchases;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Scripts.MainMenu.Controller
{
    public class MainMenuController : IDisposable
    {
        private ConfigsProviderService _configsProvider;
        private SceneSwitcher _sceneSwitcher;
        private LanguageService _languageService;
        private MainMenuUIRoot _mainMenuUIRoot;
        private AudioService _audioService;
        private AdService _adService;
        private LevelUnlocker _levelUnlocker;
        private SaveLoadService _saveLoadService;

        private AudioClip _swipeSound;
        private MainMenuLocalizationData _localizationData;
        
        private bool _isInitialized;

        [Inject]
        public void Construct(
            ConfigsProviderService configsProviderService,
            SceneSwitcher sceneSwitcher,
            LanguageService languageService,
            MainMenuUIRoot mainMenuUIRoot,
            AudioService audioService,
            AdService adService,
            LevelUnlocker levelUnlocker,
            SaveLoadService saveLoadService)
        {
            _configsProvider =  configsProviderService;
            _sceneSwitcher = sceneSwitcher;
            _languageService = languageService;
            _mainMenuUIRoot = mainMenuUIRoot;
            _audioService = audioService;
            _adService = adService;
            _levelUnlocker = levelUnlocker;
            _saveLoadService =  saveLoadService;
        }

        public void Initialize(
            MainMenuAudioData mainMenuAudioData,
            MainMenuUIData mainMenuUIData,
            MainMenuLocalizationData localizationData)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already Initialized");
            
            _swipeSound = mainMenuAudioData.SwipeSound;
            _audioService.SetMusic(mainMenuAudioData.Music);

            _mainMenuUIRoot.SetBackground(mainMenuUIData.Background);

            _localizationData = localizationData;

            UpdateUILanguage();

            _isInitialized = true;
        }

        public void Run()
        {
            if (_isInitialized == false)
                throw new InvalidOperationException("Run without initialization");

            _mainMenuUIRoot.GetComponent<MainMenuUIAnimation>().Run();
            _audioService.PlayMusic();

            _mainMenuUIRoot.LanguageClicked += OnLanguageClicked;
            _mainMenuUIRoot.LevelSelected += OnLevelSelected;
            _mainMenuUIRoot.SwipeClicked += OnSwiped;
        }

        private void UpdateUILanguage()
        {
            _mainMenuUIRoot.SetLanguage(
                _localizationData.Titles[_languageService.CurrentLanguage.Value],
                _localizationData.LanguageIcons[_languageService.CurrentLanguage.Value]
            );
        }

        private void OnLanguageClicked()
        {
            _languageService.NextLanguage();
            UpdateUILanguage();
        }

        private void OnSwiped()
        {
            _audioService.PlaySound(_swipeSound);
        }

        private void OnLevelSelected(int index)
        {
            _audioService.StopMusic();
            _ = ChangeLevel(index);
        }

        private async UniTask ChangeLevel(int index)
        {
            var levelConfig = _configsProvider.LevelsConfig.Levels[index];
            var levelButton = _mainMenuUIRoot.LevelSelector.Levels[index];
            
            if (levelConfig.IsLocked)
            {
                levelButton.SetLocked(false);
                await _adService.ShowFullscreenAsync();
                _levelUnlocker.UnlockLevel(levelConfig.LevelConfig.ID);
                _saveLoadService.Save();
            }

            _sceneSwitcher.SwitchTo(SceneID.Gameplay, new GameplaySceneParams(index));
        }

        public void Dispose()
        {
            _mainMenuUIRoot.LanguageClicked -= OnLanguageClicked;
            _mainMenuUIRoot.LevelSelected -= OnLevelSelected;
            _mainMenuUIRoot.SwipeClicked -= OnSwiped;
        }
    }
}