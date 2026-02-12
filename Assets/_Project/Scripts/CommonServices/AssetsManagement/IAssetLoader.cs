using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.CommonServices.AssetsManagement
{
    public interface IAssetLoader
    {
        Task<T> LoadAsync<T>(string key) where T : Object;
        void Release(string path);
        void ReleaseAll();
    }
}