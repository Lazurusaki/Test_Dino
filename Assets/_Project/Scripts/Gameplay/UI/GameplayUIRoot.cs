using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.UI
{
    public class GameplayUIRoot : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _previousLevelButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartLevelButton;
        [SerializeField] private Button _changeMusicButton;
        [SerializeField] private TMP_Text _dinoNameText;
    
        
        public event Action NextLevel;
        public event Action PreviousLevel;
        public event Action Home;
        public event Action RestartLevel;
        public event Action ChangeMusic;
        
        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            _previousLevelButton.onClick.AddListener(OnPreviousLevelClicked);
            _homeButton.onClick.AddListener(OnHomeClicked);
            _restartLevelButton.onClick.AddListener(OnRestartLevelClicked);
            _changeMusicButton.onClick.AddListener(OnChangeMusicClicked);
        }
        
        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelClicked);
            _previousLevelButton.onClick.RemoveListener(OnPreviousLevelClicked);
            _homeButton.onClick.RemoveListener(OnHomeClicked);
            _restartLevelButton.onClick.RemoveListener(OnRestartLevelClicked);
            _changeMusicButton.onClick.RemoveListener(OnChangeMusicClicked);
        }
        
        private void OnChangeMusicClicked()
        {
            ChangeMusic?.Invoke();
        }

        private void OnRestartLevelClicked()
        {
            RestartLevel?.Invoke();
        }

        private void OnHomeClicked()
        {
            Home?.Invoke();
        }

        private void OnPreviousLevelClicked()
        {
            PreviousLevel?.Invoke();
        }

        private void OnNextLevelClicked()
        {
            NextLevel?.Invoke();
        }
        
        public void SetRestartLevelButonVisible(bool isVisible)
        {
            _restartLevelButton.gameObject.SetActive(isVisible);
        }
        
        public void SetPreviousLevelVisible(bool isVisible)
        {
            _previousLevelButton.gameObject.SetActive(isVisible);
        }
        
        public void SetNextLevelVisible(bool isVisible)
        {
            _nextLevelButton.gameObject.SetActive(isVisible);
        }
        public void SetLevelNameVisible(bool isVisible)
        {
            _dinoNameText.gameObject.SetActive(isVisible);
        }

        public void SetLevelName(string levelName, Color color = default)
        {
            _dinoNameText.text = levelName;
            _dinoNameText.color = color;
            //_dinoNameText.color = color;
        }
    }
}
