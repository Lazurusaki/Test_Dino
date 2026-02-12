using System;
using System.Linq;
using System.Threading.Tasks;
using _Project.Scripts.CommonServices.AssetsManagement;
using _Project.Scripts.CommonServices.AudioManagement;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.CommonServices.LoadingScreen;
using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.Gameplay.Controller;
using _Project.Scripts.Gameplay.Puzzles;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay
{
    public class GameplayBootstrap : ISceneBootstrap
    {
            private readonly AudioService _audioService;
            private readonly AddressablesAssetLoader _assetLoader;
            private readonly ConfigsProviderService _configsProvider;
            private readonly LanguageService _languageService;
            private readonly GameplayController _gameplayController;

            [Inject]
            public GameplayBootstrap(
                AudioService audioService,
                AddressablesAssetLoader assetLoader,
                ConfigsProviderService configsProvider,
                LoadingScreen loadingScreen,
                LanguageService languageService,
                GameplayController gameplayController)
            {
                _audioService = audioService;
                _assetLoader = assetLoader;
                _configsProvider = configsProvider;
                _languageService = languageService;
                _gameplayController = gameplayController;
            }

            public async UniTask Run(SceneParams sceneParams)
            {
                
                try
                {
                    var levelIndex = ((GameplaySceneParams)sceneParams).LevelIndex;

                    var gameplayConfig = _configsProvider.GameplayConfig;
                    var levelConfig = _configsProvider.LevelsConfig.Levels[levelIndex].LevelConfig;

                    // SCENE
                    var startSoundTask = _assetLoader.LoadAsync<AudioClip>(gameplayConfig.StartGameSound);
                    var winSoundTask = _assetLoader.LoadAsync<AudioClip>(gameplayConfig.WinSound);
                    var looseSoundTask = _assetLoader.LoadAsync<AudioClip>(gameplayConfig.LooseSound);
                    var cheersSoundTask = _assetLoader.LoadAsync<AudioClip>(gameplayConfig.CheersSound);
                    var balloonHitSoundTask = _assetLoader.LoadAsync<AudioClip>(gameplayConfig.BalloonHitSound);
                    var musicTask = LoadMusic();

                    // LEVEL
                    var levelNameAudioTask = _assetLoader.LoadAsync<AudioClip>(
                        _languageService.GetVoice(levelConfig.LevelAudioKeyKey));
                    var puzzleTask = _assetLoader.LoadAsync<GameObject>(levelConfig.PuzzlePrefab);
                    var voiceTask = _assetLoader.LoadAsync<AudioClip>(levelConfig.Voice);

                    var (
                        startSound,
                        winSound,
                        looseSound,
                        cheersSound,
                        balloonHitSound,
                        musicClip,
                        levelNameAudio,
                        puzzlePrefab,
                        voiceClip
                        ) = await UniTask.WhenAll(
                        startSoundTask,
                        winSoundTask,
                        looseSoundTask,
                        cheersSoundTask,
                        balloonHitSoundTask,
                        musicTask,
                        levelNameAudioTask,
                        puzzleTask,
                        voiceTask);


                    var levelNameText = _languageService.GetText(levelConfig.LevelNameKey);
                    var textColor = levelConfig.TextColor;

                    var puzzleInstance = UnityEngine.Object
                        .Instantiate(puzzlePrefab)
                        .GetComponent<Puzzle>();

                    var audioData = new GameplayAudioData(
                        startSound,
                        winSound,
                        looseSound,
                        cheersSound,
                        balloonHitSound);

                    var localizationData = new GameplayLocalizationData(
                        levelNameText,
                        levelNameAudio);

                    var levelData = new GameplayLevelData(
                        puzzleInstance,
                        voiceClip,
                        textColor);
                    
                    _audioService.AddMusic(musicClip);
                    _gameplayController.Initialize( levelIndex, audioData, levelData, localizationData);

                    
                    _gameplayController.Run();
                    
                }
                catch (Exception exception)
                {
                    Debug.LogError($"MainMenu scene loading error: {exception}");
                }
            }

            private async UniTask<AudioClip[]> LoadMusic()
            {
                var tasks = Enumerable.Select(_configsProvider.GameplayConfig.GameplayMusicList, music => _assetLoader.LoadAsync<AudioClip>(music))
                    .ToArray();

                var music = await UniTask.WhenAll(tasks);
                return music;
            }
    }

    public class GameplayAudioData
    {
        public readonly AudioClip StartGameSound;
        public readonly AudioClip WinSound;
        public readonly AudioClip LooseSound;
        public readonly AudioClip CheersSound;
        public readonly AudioClip BalloonHitSound;

        
        public GameplayAudioData(AudioClip startGameSound, AudioClip winSound, AudioClip looseSound, AudioClip cheersSound, AudioClip balloonHitSound)
        {
            StartGameSound = startGameSound;
            WinSound = winSound;
            LooseSound = looseSound;
            CheersSound = cheersSound;
            BalloonHitSound = balloonHitSound;
        }
    }
    
    public class GameplayLevelData
    {
        public Puzzle Puzzle;
        public AudioClip Voice;
        public Color TextColor;

        public GameplayLevelData(Puzzle puzzle, AudioClip voice, Color textColor)
        {
            Puzzle = puzzle;
            Voice = voice;
            TextColor = textColor;
        }
    }
    
    public class GameplayLocalizationData
    {
        public readonly string DinoNameText;
        public readonly AudioClip DinoNameAudio;

        public GameplayLocalizationData(string dinoNameText, AudioClip dinoNameAudio)
        {
            DinoNameText = dinoNameText;
            DinoNameAudio = dinoNameAudio;
        }
    }
}