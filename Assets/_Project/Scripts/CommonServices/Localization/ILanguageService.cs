using System;
using _Project.Scripts.Configs.Localization;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.CommonServices.Localization
{
    public interface ILanguageService
    {
        IReadOnlyReactiveProperty<Language> CurrentLanguage { get; }
        void SetLanguage(Language language);
        void SetLanguage(string language);
        string GetText(string key);
        AudioClip GetVoice(string key);
    }
}