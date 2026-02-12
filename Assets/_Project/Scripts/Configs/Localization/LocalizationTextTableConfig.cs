using System.Collections.Generic;
using _Project.Scripts.CommonServices.Localization;
using UnityEngine;

namespace _Project.Scripts.Configs.Localization
{
    [CreateAssetMenu(fileName = "LocalizationTextTable", menuName = "Configs/Localization/LocalizationTextTable")]
    public class LocalizationTextTableConfig : ScriptableObject
    {
        [SerializeField] private List<LocalizationTextEntry> _entries;

        private Dictionary<LocalizationTextKey, string> _map;

        public void Init()
        {
            _map = new Dictionary<LocalizationTextKey, string>(_entries.Count);

            foreach (var entry in _entries)
                _map[entry.Key] = entry.Value;
        }

        public string Get(LocalizationTextKey key)
        {
            if (_map == null)
                Init();

            return _map.TryGetValue(key, out var value)
                ? value
                : $"Localization missing: {key}";
        }
    }
}

