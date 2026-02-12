using System.Threading.Tasks;

namespace _Project.Scripts.CommonServices.ScenesManagement
{
    public interface ISceneLoader
    {
        Task LoadSceneAsync(SceneID sceneID);
    }
}