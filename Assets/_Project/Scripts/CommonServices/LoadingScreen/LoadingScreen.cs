using System.Threading.Tasks;
using _Project.Scripts.CommonUI;
using UnityEngine;

namespace _Project.Scripts.CommonServices.LoadingScreen
{
    [RequireComponent(typeof(CanvasGroupFader))]
    public class LoadingScreen : MonoBehaviour
    {
        private CanvasGroupFader _source;
        
        private void Awake()
        {
            _source = GetComponent<CanvasGroupFader>();
        }

        public async Task Show()
        {
            gameObject.SetActive(true);
            await _source.Show();
        }
        
        public async Task Hide()
        {
            await _source.Hide();
            gameObject.SetActive(false);
        }
    }
}