using System;
using System.Threading.Tasks;
using _Project.Scripts.CommonServices.AdvertisementManagement;
using _Project.Scripts.CommonServices.AudioManagement;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.CommonServices.SaveLoadManagement;
using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.Configs.Gameplay.Levels;
using _Project.Scripts.Gameplay.UI;
using _Project.Scripts.MainMenu.UI;
using _Project.Scripts.Meta.Purchases;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Controller
{
    public class GameplayController : IDisposable
    {
        private PuzzleHandler _puzzleHandler;
        private BalloonsHandler _balloonsHandler;
        private SceneSwitcher _sceneSwitcher;
        private GameplayUIRoot _gameplayUIRoot;
        private AudioService _audioService;
        private AdService _adService;
        private LevelUnlocker _levelUnlocker;
        private SaveLoadService _saveLoadService;
        
        private GameplayUIAnimation _gameplayUIAnimation;
        
        private int _currentLevelIndex;

        private AudioClip _voiceSound;
        private AudioClip _levelNameAudio;
        private AudioClip _balloonSound;
        private AudioClip _winSound;
        private AudioClip _looseSound;
        private AudioClip _clapsSound;
        private AudioClip _startSound;

        private LevelsListConfig _levelsConfig;
        
        
        private bool _isInitialized;
        
        [Inject]
        public void Construct(
            ConfigsProviderService configsProviderService,
            PuzzleHandler puzzleHandler,
            BalloonsHandler balloonsHandler,
            SceneSwitcher sceneSwitcher,
            GameplayUIRoot gameplayUIRoot,
            AudioService audioService,
            AdService adService,
            LevelUnlocker levelUnlocker,
            SaveLoadService saveLoadService)
        {
            _levelsConfig = configsProviderService.LevelsConfig;
            _puzzleHandler = puzzleHandler;
            _balloonsHandler = balloonsHandler;
            _sceneSwitcher = sceneSwitcher;
            _gameplayUIRoot = gameplayUIRoot;
            _audioService = audioService;
            _adService = adService;
            _levelUnlocker = levelUnlocker;
            _saveLoadService = saveLoadService;
        }

        public void Initialize(int currentLevelIndex,
            GameplayAudioData audioData,
            GameplayLevelData levelData,
            GameplayLocalizationData localizationData)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already Initialized");

            _currentLevelIndex = currentLevelIndex;

            _voiceSound = levelData.Voice;
            _levelNameAudio = localizationData.DinoNameAudio;
            _winSound = audioData.WinSound;
            _looseSound = audioData.LooseSound;
            _clapsSound = audioData.CheersSound;
            _startSound = audioData.StartGameSound;
            _balloonSound = audioData.BalloonHitSound;

            _gameplayUIAnimation = _gameplayUIRoot.GetComponent<GameplayUIAnimation>();
            _gameplayUIRoot.SetLevelName(localizationData.DinoNameText, levelData.TextColor);
            _gameplayUIRoot.SetLevelNameVisible(false);
            _gameplayUIRoot.SetNextLevelVisible(false);
            _gameplayUIRoot.SetPreviousLevelVisible(false);
            _gameplayUIRoot.SetRestartLevelButonVisible(false);

            _puzzleHandler.Initialize(levelData.Puzzle);
            _isInitialized = true;
        }

        public void Run()
        {
            if (_isInitialized == false)
                throw new InvalidOperationException("Run without initialization");

            _audioService.PlayRandomMusic();
            _audioService.PlaySound(_startSound);
            _gameplayUIAnimation.RunStartGameAnimation();

            _gameplayUIRoot.NextLevel += OnNextLevel;
            _gameplayUIRoot.PreviousLevel += OnPreviousLevel;
            _gameplayUIRoot.Home += OnHome;
            _gameplayUIRoot.ChangeMusic += OnChangeMusic;
            _gameplayUIRoot.RestartLevel += OnRestartLevel;
            _puzzleHandler.onPlayerWin += OnPlayerWin;
            _puzzleHandler.Good += OnPlayerGood;
            _puzzleHandler.Bad += OnPlayerBad;
            _balloonsHandler.BalloonHit += OnBalloonHit;
        }
        
        private void OnPlayerBad()
        {
            _audioService.PlaySound(_looseSound);
        }

        private void OnPlayerGood()
        {
            _audioService.PlaySound(_winSound);
        }

        private void OnPreviousLevel()
        {
            if (_currentLevelIndex == 0)
                return;

            _ =  SwitchLevel(--_currentLevelIndex);
        }

        private void OnNextLevel()
        {
            if (_currentLevelIndex == _levelsConfig.Levels.Count - 1)
                return;

            _ = SwitchLevel(++_currentLevelIndex);
        }
        

        private void OnRestartLevel()
        {
            _ = SwitchLevel(_currentLevelIndex);
        }

        private void OnChangeMusic()
        {
            _audioService.PlayNextMusic();
        }

        private void OnHome()
        {
            _audioService.Clear();
            _sceneSwitcher.SwitchTo(SceneID.MainMenu);
        }

        private void OnBalloonHit()
        {
            _audioService.PlaySound(_balloonSound);
        }
        
        private async UniTask SwitchLevel(int index)
        {
            var levelConfig = _levelsConfig.Levels[index];
            
            if (levelConfig.IsLocked)
            {
                await _adService.ShowFullscreenAsync();
                _levelUnlocker.UnlockLevel(levelConfig.LevelConfig.ID);
                _saveLoadService.Save();
            }

            _sceneSwitcher.SwitchTo(SceneID.Gameplay, new GameplaySceneParams(index));
        }
        
        private void OnPlayerWin()
        {
            _ = PlayerWin();
        }

        private async UniTask PlayerWin()
        {
            _audioService.StopMusic();
            await UniTask.WaitForSeconds(1);
            _audioService.PlaySound(_levelNameAudio);
            _gameplayUIRoot.SetLevelNameVisible(true);
            _gameplayUIAnimation.RunLevelNameAnimation();
            await UniTask.WaitForSeconds(1);
            _audioService.PlaySound(_voiceSound);
            await UniTask.WaitForSeconds(3);
            _audioService.PlaySound(_clapsSound);
            _balloonsHandler.Run();
            await UniTask.WaitForSeconds(8);
            _puzzleHandler.ShowRealAnimal();
            await UniTask.WaitForSeconds(3);
            
            if (_currentLevelIndex > 0)
                _gameplayUIRoot.SetPreviousLevelVisible(true);
            
            if (_currentLevelIndex < _levelsConfig.Levels.Count - 1)
                _gameplayUIRoot.SetNextLevelVisible(true);
            
            _gameplayUIRoot.SetRestartLevelButonVisible(true);
            _gameplayUIAnimation.RunEndGameAnimation();
        }

        public void Dispose()
        {
            _gameplayUIRoot.NextLevel -= OnNextLevel;
            _gameplayUIRoot.PreviousLevel -= OnPreviousLevel;
            _gameplayUIRoot.Home -= OnHome;
            _gameplayUIRoot.ChangeMusic -= OnChangeMusic;
            _gameplayUIRoot.RestartLevel -= OnRestartLevel;
            _puzzleHandler.onPlayerWin -= OnPlayerWin;
            _puzzleHandler.Good -= OnPlayerGood;
            _puzzleHandler.Bad -= OnPlayerBad;
            _balloonsHandler.BalloonHit -= OnBalloonHit;
        }
    }
}