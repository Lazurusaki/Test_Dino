using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace _Project.Scripts.CommonServices.AssetsManagement
{
    public sealed class AddressablesAssetLoader
    {
        private readonly Dictionary<string, AsyncOperationHandle> _keyHandles = new();
        private readonly Dictionary<AssetReference, AsyncOperationHandle> _referenceHandles = new();

        // private readonly Dictionary<object, List<AsyncOperationHandle>> _ownerHandles = new();

        private readonly List<AsyncOperationHandle> _sceneContext = new();

        // ---------------- INTERNAL ----------------

        private static async UniTask AwaitHandle(AsyncOperationHandle handle, string key)
        {
            try
            {
                await handle.Task;
            }
            catch
            {
                Addressables.Release(handle);
                throw;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(handle);
                throw new System.Exception($"Failed to load addressable asset: {key}");
            }
        }

        /*
        private void RegisterOwnerHandle(object owner, AsyncOperationHandle handle)
        {
            if (!_ownerHandles.TryGetValue(owner, out var list))
            {
                list = new List<AsyncOperationHandle>();
                _ownerHandles[owner] = list;
            }

            if (!list.Contains(handle))
                list.Add(handle);
        }
        */

        // ---------------- KEY LOAD ----------------

        public async UniTask<T> LoadAsync<T>(string key) where T : Object
        {
            if (_keyHandles.TryGetValue(key, out var existingHandle))
            {
                if (existingHandle.IsDone == false)
                    await existingHandle.Task;

                return (T)existingHandle.Result;
            }

            var handle = Addressables.LoadAssetAsync<T>(key);
            _keyHandles[key] = handle;

            await AwaitHandle(handle, key);

            return handle.Result;
        }

        // ---------------- REFERENCE LOAD ----------------

        /*
        public async Task<T> LoadAsync<T>(AssetReferenceT<T> reference) where T : Object
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            // AssetReferenceT<T> уже знает тип → безопаснее
            if (_referenceHandles.TryGetValue(reference, out var existing) && existing.IsValid())
            {
                if (!existing.IsDone)
                    await existing.Task;

                if (existing.Status == AsyncOperationStatus.Failed)
                    throw new Exception($"Failed to load asset {reference.editorAsset?.name}");

                return (T)existing.Result;
            }

            var handle = reference.LoadAssetAsync<T>();
            _referenceHandles[reference] = handle;

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Failed)
            {
                _referenceHandles.Remove(reference);
                throw new Exception($"Failed to load {reference.RuntimeKey}: {handle.OperationException?.Message}");
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // Debug.Log только в редакторе / дебаг-билде
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"Loaded {typeof(T).Name} → count: {_referenceHandles.Count}");
#endif
                return handle.Result;
            }

            // На всякий случай
            _referenceHandles.Remove(reference);
            throw new Exception("Unexpected handle status");
        }
        */

        
        public async UniTask<T> LoadAsync<T>(AssetReference reference) where T : Object
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            if (_referenceHandles.TryGetValue(reference, out var existingHandle))
            {
                if (existingHandle.IsDone == false) await existingHandle.Task;
                return (T)existingHandle.Result;
            }

            var handle = reference.LoadAssetAsync<T>();
            _referenceHandles[reference] = handle;
            await AwaitHandle(handle, reference.RuntimeKey.ToString());

            Debug.Log($"LOADED {typeof(T).Name} - {reference.Asset.name} → count: {_referenceHandles.Count}");
            return handle.Result;
        }
        

        /*
        public async Task<T> LoadAsync<T>(AssetReference reference, object owner = null) where T : Object
        {
            if (reference == null)
                throw new System.ArgumentNullException(nameof(reference));

            if (_referenceHandles.TryGetValue(reference, out var existingHandle))
            {
                if (!existingHandle.IsDone)
                    await existingHandle.Task;

                if (owner != null)
                    RegisterOwnerHandle(owner, existingHandle);

                return (T)existingHandle.Result;
            }

            var handle = reference.LoadAssetAsync<T>();
            _referenceHandles[reference] = handle;

            if (owner != null)
                RegisterOwnerHandle(owner, handle);

            await AwaitHandle(handle, reference.RuntimeKey.ToString());

            Debug.Log($"Global handles: {_referenceHandles.Count}, Owner handles: {_ownerHandles.Count}");

            return handle.Result;
        }
        */

        // ---------------- LOAD BY LABEL ----------------

        public async UniTask<IList<T>> LoadByLabelAsync<T>(string label) where T : Object
        {
            var result = new List<T>();

            var handle = Addressables.LoadAssetsAsync<T>(label, asset => { result.Add(asset); });

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(handle);
                throw new System.Exception($"Failed to load assets with label: {label}");
            }

            // кэшировать handle, если нужно Release
            //_labelHandles[label] = handle;

            return result;
        }

        // ---------------- RELEASE ----------------

        public void Release(string key)
        {
            if (_keyHandles.TryGetValue(key, out var handle) == false)
                return;

            Addressables.Release(handle);
            _keyHandles.Remove(key);
        }

        public void Release(AssetReference reference)
        {
            if (reference == null)
                return;

            if (_referenceHandles.TryGetValue(reference, out var handle) == false)
                return;

            Addressables.Release(handle);
            _referenceHandles.Remove(reference);
        }

        public void ReleaseAllKeyHandles()
        {
            foreach (var handle in _keyHandles.Values)
                Addressables.Release(handle);

            _keyHandles.Clear();
        }

        public void ReleaseAllAssetReferenceHandles()
        {
            foreach (var handle in _referenceHandles.Values)
            {
                Debug.Log($"UNLOADED {handle.Result?.GetType()} - {handle.Result?.GetType().Name} : → count: {_referenceHandles.Count}");
                Addressables.Release(handle);
            }

            _referenceHandles.Clear();
            Debug.Log(_referenceHandles.Count);
        }

        public void ReleaseAll()
        {
            ReleaseAllKeyHandles();
            ReleaseAllAssetReferenceHandles();
        }

        /*
        // --------------------- RELEASE BY OWNER ---------------------

        public void ReleaseAllOwned(object owner)
        {
            if (_ownerHandles.TryGetValue(owner, out var handles) == false)
                return;

            foreach (var handle in handles)
                Addressables.Release(handle);

            _ownerHandles.Remove(owner);
        }
        */


        // ---------------- GET ----------------

        public T Get<T>(string key) where T : Object
        {
            if (_keyHandles.TryGetValue(key, out var handle) == false)
                return null;

            if (handle.IsValid() == false)
                return null;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                return null;

            return handle.Result as T;
        }

        public T Get<T>(AssetReference reference) where T : Object
        {
            if (reference == null)
                return null;

            if (_referenceHandles.TryGetValue(reference, out var handle) == false)
                return null;

            if (handle.IsValid() == false)
                return null;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                return null;

            return handle.Result as T;
        }
    }
}