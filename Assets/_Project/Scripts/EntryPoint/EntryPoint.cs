using System;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.CommonServices.SaveLoadManagement;
using _Project.Scripts.CommonServices.ScenesManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
using Zenject;

namespace _Project.Scripts.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        private SceneSwitcher _sceneSwitcher;
        private ConfigsProviderService _configsProviderService;
        private LanguageService _languageService;
        private SaveLoadService _saveLoadService;

        [Inject]
        public void Construct(
            LanguageService languageService,
            ConfigsProviderService configsProviderService,
            SceneSwitcher sceneSwitcher,
            SaveLoadService saveLoadService)
        {
            _languageService = languageService;
            _configsProviderService = configsProviderService;
            _sceneSwitcher = sceneSwitcher;
            _saveLoadService = saveLoadService;
        }

        private async void Start()
        {
            try
            {
                await Run();
            }
            catch (Exception exception)
            {
                Debug.LogError($"{exception.GetType().Name} - {exception.Message}");
            }
        }


        private async UniTask Run()
        {
            _saveLoadService.Load();
            //_saveLoadService.ClearSaveData();
            await _configsProviderService.LoadAll();
            _languageService.Initialize();
            _languageService.SetLanguageByCountry(YG2.lang);
            await _sceneSwitcher.SwitchTo(SceneID.MainMenu);
        }
    }
}