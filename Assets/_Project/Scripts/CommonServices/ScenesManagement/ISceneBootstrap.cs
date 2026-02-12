using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CommonServices.ScenesManagement
{
    public interface ISceneBootstrap
    {
        UniTask Run(SceneParams sceneParams);
    }
}