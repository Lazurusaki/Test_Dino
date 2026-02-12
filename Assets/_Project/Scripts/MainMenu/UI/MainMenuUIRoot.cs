using System;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.MainMenu.UI
{
    public class MainMenuUIRoot : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Button _languageButton;
        [SerializeField] private LevelSelector _levelSelector;
        
        public LevelSelector LevelSelector => _levelSelector;
        
        //[SerializeField] private CanvasGroup _levelSelector;
        
        public event Action LanguageClicked;
        public event Action<int> LevelSelected;
        public event Action SwipeClicked;
        
        private void OnEnable()
        {
            _languageButton.onClick.AddListener(OnLanguageClicked);
            _levelSelector.Scroll.OnSwiped += OnSwipeClicked;
            _levelSelector.LevelSelected += OnLevelSelected;
        }

        private void OnDisable()
        {
            _languageButton.onClick.RemoveListener(OnLanguageClicked);
            _levelSelector.Scroll.OnSwiped -= OnSwipeClicked;
            _levelSelector.LevelSelected -= OnLevelSelected;
        }
        
        public void SetBackground(Sprite background)
        {
            _background.sprite = background;
        }
        
        public void SetLanguage(string title, Sprite flag)
        {
            _languageButton.image.sprite = flag;
            _titleText.text = title;
        }

        public void UnlockLevel(int index)
        {
            _levelSelector.Levels[index].SetLocked(false);
        }

        private void OnLanguageClicked()
        {
            LanguageClicked?.Invoke();
        }

        private void OnSwipeClicked()
        {
            SwipeClicked?.Invoke();
        }
        
        private void OnLevelSelected(int index)
        {
            LevelSelected?.Invoke(index);
        }
        
    }
}