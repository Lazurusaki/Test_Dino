using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.MainMenu.Controller;
using _Project.Scripts.MainMenu.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuUIRoot _mainMenuUIRoot;

        public override void InstallBindings()
        {
            BindMainMenuBootstrap();
            BindMainMenuUIRoot();
            BindLevelsuMenuBuilder();
            BindMainMenuController();
            
        }
        
        private void BindMainMenuBootstrap()
            => Container.Bind<ISceneBootstrap>().To<MainMenuBootstrap>().AsSingle().NonLazy();
        
        private void BindMainMenuUIRoot()
            => Container.Bind<MainMenuUIRoot>().FromInstance(_mainMenuUIRoot).AsSingle().NonLazy();

        private void BindLevelsuMenuBuilder()
            => Container.Bind<LevelsMenuBuilder>().AsSingle();
        
        
        private void BindMainMenuController()
            => Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle();
    }
}