using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.CommonUI
{
    public class UnlockableButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private Transform _lock;
        
        public event Action<UnlockableButton> Clicked;
        
        public bool IsLocked => _lock.gameObject.activeSelf;
        

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            Clicked?.Invoke(this);
        }

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }
        
        public void SetLocked(bool locked)
        {
            _lock.gameObject.SetActive(locked);
            _icon.gameObject.SetActive(!locked);
        }
    }
}