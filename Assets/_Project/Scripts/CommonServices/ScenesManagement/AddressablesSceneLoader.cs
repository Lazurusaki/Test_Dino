using System;
using System.Threading.Tasks;
using _Project.Scripts.CommonServices.AssetsManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Scripts.CommonServices.ScenesManagement
{
    public class AddressablesSceneLoader
    {
        private readonly AddressablesAssetLoader _assetLoader;

        private AsyncOperationHandle<SceneInstance>? _currentHandle;

        [Inject]
        public AddressablesSceneLoader(
            AddressablesAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }


        public async UniTask LoadSceneAsync(SceneID sceneID)
        {
            string key = SceneKeys.GetSceneKey(sceneID);
            
            _assetLoader.ReleaseAllAssetReferenceHandles();
            
            var newHandle = Addressables.LoadSceneAsync(key, LoadSceneMode.Single);
            await newHandle.Task;

            if (newHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (_currentHandle.HasValue && _currentHandle.Value.IsValid())
                {
                    Addressables.Release(_currentHandle.Value);
                }

                _currentHandle = newHandle;

                SceneManager.SetActiveScene(newHandle.Result.Scene);
            }
            else
            {
                Addressables.Release(newHandle);
                throw new Exception($"Failed to load scene {key}");
            }
        }

        /* ADDITIVE
        public async Task LoadSceneAsync(SceneID sceneID)
        {
            string key = SceneKeys.GetSceneKey(sceneID);

            //_assetLoader.ReleaseAllAssetReferenceHandles();

            AsyncOperationHandle<SceneInstance>? previousHandle = _currentHandle;

            _currentHandle = Addressables.LoadSceneAsync(key, LoadSceneMode.Additive);
            await _currentHandle.Value.Task;

            if (_currentHandle.Value.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load scene {key}");

            SceneManager.SetActiveScene(_currentHandle.Value.Result.Scene);
            Debug.Log($"Scene {key} loaded");

            if (previousHandle.HasValue && previousHandle.Value.IsValid())
            {
                await Addressables.UnloadSceneAsync(previousHandle.Value).Task;
                _assetLoader.ReleaseAllAssetReferenceHandles();
            }
        }

        //FIRST VERSION
        public async Task LoadSceneAsync(SceneID sceneID)
        {
            string key = SceneKeys.GetSceneKey(sceneID);

            if (_currentHandle.HasValue && _currentHandle.Value.IsValid())
            {
                _assetLoader.ReleaseAllAssetReferenceHandles();

                SceneManager.LoadScene("Empty", LoadSceneMode.Single);

                await Addressables.UnloadSceneAsync(_currentHandle.Value).Task;
                //ВОТ НА ЭТОМ МОМЕНТЕ ОШИБКА
            }


            _currentHandle = Addressables.LoadSceneAsync(key, LoadSceneMode.Single);

            await _currentHandle.Value.Task;

            if (_currentHandle.Value.Status == AsyncOperationStatus.Succeeded)
            {
                SceneManager.SetActiveScene(_currentHandle.Value.Result.Scene);
                Debug.Log($"Scene {key} loaded");
            }
            else
            {
                throw new Exception($"Failed to load scene {key}");
            }
        }
        */

        public async UniTask UnloadCurrentScene()
        {
            if (_currentHandle.HasValue && _currentHandle.Value.IsValid())
            {
                await Addressables.UnloadSceneAsync(_currentHandle.Value).Task;
                _currentHandle = null;
            }
        }
    }
}