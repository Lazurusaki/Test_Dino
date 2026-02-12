using _Project.Scripts.CommonServices.ScenesManagement;
using _Project.Scripts.Gameplay.Controller;
using _Project.Scripts.Gameplay.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUIRoot _gameplayUiRoot;
        [SerializeField] private PuzzleHandler _puzzleHandler;
        [SerializeField] private BalloonsHandler _balloonsHandler;
        public override void InstallBindings()
        {
            BindGameplayBootstrap();
            BindGameplayController();
            BindGameplayUIRoot();
            BindPuzzleHandler();
            BindBalloonsHandler();
        }
        
        private void BindGameplayBootstrap()
            => Container.Bind<ISceneBootstrap>().To<GameplayBootstrap>().AsSingle().NonLazy();
        
        private void BindGameplayUIRoot()
            => Container.Bind<GameplayUIRoot>().FromInstance(_gameplayUiRoot).AsSingle().NonLazy();
        
        private void BindPuzzleHandler()
            => Container.Bind<PuzzleHandler>().FromInstance(_puzzleHandler).AsSingle().NonLazy();
        
        private void BindBalloonsHandler()
            => Container.Bind<BalloonsHandler>().FromInstance(_balloonsHandler).AsSingle().NonLazy();

        private void BindGameplayController()
            => Container.BindInterfacesAndSelfTo<GameplayController>().AsSingle();
    }
}