using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.CommonServices.AssetsManagement
{
    public sealed class ResourcesAssetLoader 
    {
        private readonly Dictionary<string, Object> _cache = new();

        public Task<T> LoadAsync<T>(string key) where T : Object
        {
            if (_cache.TryGetValue(key, out var asset))
                return Task.FromResult((T)asset);

            var loaded = Resources.Load<T>(key);

            if (loaded == null)
                throw new System.Exception($"Resource not found: {key}");

            _cache[key] = loaded;
            return Task.FromResult(loaded);
        }

        public void Release(string key)
        {
            if (_cache.TryGetValue(key, out var asset) == false)
                return;

            Resources.UnloadAsset(asset);
            _cache.Remove(key);
        }

        public void ReleaseAll()
        {
            _cache.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}