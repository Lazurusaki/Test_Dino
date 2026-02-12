using System.Collections.Generic;
using _Project.Scripts.CommonServices.ScenesManagement;

namespace _Project.Scripts.CommonServices.AssetsManagement
{
    public static class SceneKeys
    {
        private static readonly Dictionary<SceneID, string> _sceneKeys = new()
        {
            { SceneID.MainMenu, "MainMenu" },
            { SceneID.Gameplay, "Gameplay" },
        };
        

        public static string GetSceneKey(SceneID scene) => _sceneKeys[scene];
    }
}