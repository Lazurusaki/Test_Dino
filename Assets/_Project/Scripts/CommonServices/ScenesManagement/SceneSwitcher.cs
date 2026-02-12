using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CommonServices.ScenesManagement
{
    public class SceneSwitcher
    {
        private readonly AddressablesSceneLoader _sceneLoader;
        private readonly LoadingScreen.LoadingScreen _loadingScreen;

        [Inject]
        public SceneSwitcher(
            AddressablesSceneLoader sceneLoader,
            LoadingScreen.LoadingScreen loadingScreen)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public async UniTask SwitchTo(SceneID sceneID, SceneParams sceneParams = null)
        {
            try
            {
                await _loadingScreen.Show();
                
                float timeStart = Time.time;
                
                await _sceneLoader.LoadSceneAsync(sceneID);

                var sceneBootstrap = GameObject.FindFirstObjectByType<SceneContext>()?.Container?.Resolve<ISceneBootstrap>();

                if (sceneBootstrap != null)
                    await sceneBootstrap.Run(sceneParams as GameplaySceneParams);
                
                Debug.Log($"Scene {sceneID} loaded, time: {Time.time - timeStart}");

                _loadingScreen.Hide();
            }
            catch (Exception exception)
            {
                Debug.LogError($"Scene loading error: {exception}");
                _ = _loadingScreen.Hide();
            }
        }
    }
    
    public abstract class SceneParams
    {
    }

    public class GameplaySceneParams : SceneParams
    {
        public readonly int LevelIndex;

        public GameplaySceneParams(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}