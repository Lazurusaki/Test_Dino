using System.Collections.Generic;
using _Project.Scripts.CommonServices.Localization;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Localization
{
    [CreateAssetMenu(fileName = "LocalizationAudioTable", menuName = "Configs/Localization/LocalizationAudioTable")]
    public class LocalizationAudioTableConfig : ScriptableObject
    {
        [SerializeField] private List<LocalizationAudioEntry> _entries;

        private Dictionary<LocalizationAudioKey, AssetReferenceT<AudioClip>> _map;

        public void Init()
        {
            if (_map != null)
                return;

            _map = new Dictionary<LocalizationAudioKey, AssetReferenceT<AudioClip>>(_entries.Count);

            foreach (var entry in _entries)
                _map[entry.Key] = entry.Value;
        }

        public AssetReferenceT<AudioClip> Get(LocalizationAudioKey key)
        {
            if (_map == null)
                Init();

            return _map.TryGetValue(key, out var clip)
                ? clip
                : null;
        }
    }
}