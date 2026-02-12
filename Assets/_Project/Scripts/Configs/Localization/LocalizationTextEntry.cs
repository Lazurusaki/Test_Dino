using System;
using _Project.Scripts.CommonServices.Localization;
using UnityEngine;

namespace _Project.Scripts.Configs.Localization
{
    [Serializable]
    public class LocalizationTextEntry
    {
        [field: SerializeField] public LocalizationTextKey Key { get; private set; }
        [field: SerializeField] public String Value { get; private set; }
    }
}