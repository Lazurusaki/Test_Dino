using _Project.Scripts.CommonServices.AdvertisementManagement;
using _Project.Scripts.CommonServices.AssetsManagement;
using _Project.Scripts.CommonServices.AudioManagement;
using _Project.Scripts.CommonServices.ConfigsManagement;
using _Project.Scripts.CommonServices.LoadingScreen;
using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.CommonServices.SaveLoadManagement;
using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.Meta.Purchases;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Scripts.EntryPoint
{
    public class ProjectInstaller : MonoInstaller
    {
        [FormerlySerializedAs("_audioPlayer")] [SerializeField] private AudioService _audioService;
        [SerializeField] private LoadingScreen _loadingScreen;
        
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindSceneSwitcher();
            BindAssetLoader();
            BindConfigsProviderService();
            BindLanguageService();
            BindAudioPlayer();
            BindLoadingScreen();
            BindAdService();
            BindLevelUnlocker();
            BindSaveLoadServiece();
        }
        
        private void BindSceneLoader()
            => Container.Bind<AddressablesSceneLoader>().AsSingle();

        private void BindSceneSwitcher()
            => Container.Bind<SceneSwitcher>().AsSingle();

        private void BindAssetLoader()
            => Container.Bind<AddressablesAssetLoader>().AsSingle();
        
        private void BindConfigsProviderService()
            => Container.Bind<ConfigsProviderService>().AsSingle();
        
        private void BindLanguageService()
            => Container.Bind<LanguageService>().AsSingle();
        
        private void BindAdService()
            => Container.Bind<AdService>().AsSingle();
        
        private void BindSaveLoadServiece()
            => Container.Bind<SaveLoadService>().AsSingle();
        
        private void BindAudioPlayer()
            => Container.Bind<AudioService>().FromInstance(_audioService).AsSingle();
        
        private void BindLoadingScreen()
            => Container.Bind<LoadingScreen>().FromInstance(_loadingScreen).AsSingle();
        
        private void BindLevelUnlocker()
            => Container.Bind<LevelUnlocker>().AsSingle();
    }
}