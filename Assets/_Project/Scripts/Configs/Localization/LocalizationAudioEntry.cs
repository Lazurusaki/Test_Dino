using System;
using _Project.Scripts.CommonServices.Localization;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs.Localization
{
    [Serializable]
    public class LocalizationAudioEntry
    {
        [field: SerializeField] public LocalizationAudioKey Key { get; private set; }
        [field: SerializeField] public AssetReferenceT<AudioClip> Value { get; private set; }
    }
}