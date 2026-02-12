using _Project.Scripts.CommonUI;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Gameplay.UI
{
    public class GameplayUIAnimation : MonoBehaviour
    {
        private const float FadeTime = 0.5f;
        private const float MoveOffset = 100;
        
        [SerializeField] private CanvasGroup _nextLevelButton;
        [SerializeField] private CanvasGroup _previousLevelButton;
        [SerializeField] private CanvasGroup _homeButton;
        [SerializeField] private CanvasGroup _restartLevelButton;
        [SerializeField] private CanvasGroup _changeMusicButton;
        [SerializeField] private CanvasGroup _dinoNameText;

        private Vector2 _nextLevelButtonTarget;
        private Vector2 _previousLevelButtonTarget;
        private Vector2 _dinoNameTextTarget;

        private RectTransform _homeRect;
        private RectTransform _musicRect;
        private RectTransform _nextLevelRect;
        private RectTransform _previousLevelRect;
        private RectTransform _restartRect;
        private RectTransform _dinoNameTextRect;
        
        private Tween _animation;

        private void Awake()
        {
            _nextLevelButton.alpha = 0;
            _previousLevelButton.alpha = 0;
            _homeButton.alpha = 0;
            _restartLevelButton.alpha = 0;
            _changeMusicButton.alpha = 0;
            _dinoNameText.alpha = 0;
            
            _nextLevelRect = _nextLevelButton.GetComponent<RectTransform>();
            _previousLevelRect = _previousLevelButton.GetComponent<RectTransform>();
            _dinoNameTextRect = _dinoNameText.GetComponent<RectTransform>();
            _restartRect = _restartLevelButton.GetComponent<RectTransform>();
            _homeRect = _homeButton.GetComponent<RectTransform>();
            _musicRect = _changeMusicButton.gameObject.GetComponent<RectTransform>();
            
            
            //START SETUP
            _homeRect.localScale = Vector3.zero;
            _musicRect.localScale = Vector3.zero;
            _restartRect.localScale = Vector3.zero;
            _dinoNameTextRect.localScale = Vector3.zero;
            
            _nextLevelButtonTarget = _nextLevelRect.anchoredPosition;
            _previousLevelButtonTarget = _previousLevelRect.anchoredPosition;
            _dinoNameTextTarget =  _dinoNameTextRect.anchoredPosition;
        }
        
        public void RunStartGameAnimation()
        {
            _animation?.Kill();

            _nextLevelRect.anchoredPosition = _nextLevelButtonTarget + Vector2.right *  MoveOffset;
            _previousLevelRect.anchoredPosition = _previousLevelButtonTarget + Vector2.left *  MoveOffset;
            
            var sequence = DOTween.Sequence();
            
            //Home button
            sequence.Join(_homeButton.DOFade(1, FadeTime));
            sequence.Join(_homeRect.DOScale(1f, FadeTime).SetEase(Ease.OutBack));
            
            //Music button
            sequence.Join(_changeMusicButton.DOFade(1, FadeTime));
            sequence.Join(_musicRect.DOScale(1f, FadeTime).SetEase(Ease.OutBack));

            _animation = sequence;
        }

        public void RunLevelNameAnimation()
        {
            _animation?.Kill();
            
            _dinoNameTextRect.anchoredPosition = _dinoNameTextTarget + Vector2.down *  MoveOffset;
            
            var sequence = DOTween.Sequence();
            
            //TEXT
            sequence.Join(_dinoNameText.DOFade(1, FadeTime));
            sequence.Join(_dinoNameTextRect.DOScale(1f, FadeTime).SetEase(Ease.OutBack));
            sequence.Join(_dinoNameTextRect.DOAnchorPos(_dinoNameTextTarget, FadeTime).SetEase(Ease.OutCubic));
            _animation = sequence;
        }

        public void RunEndGameAnimation()
        {
            _animation?.Kill();
            var sequence = DOTween.Sequence();
            
            //left
            sequence.Join(_previousLevelButton.DOFade(1f, FadeTime));
            sequence.Join(_previousLevelRect.DOAnchorPos(_previousLevelButtonTarget, FadeTime).SetEase(Ease.OutCubic));

            //right
            sequence.Join(_nextLevelButton.DOFade(1f, FadeTime));
            sequence.Join(_nextLevelRect.DOAnchorPos(_nextLevelButtonTarget, FadeTime).SetEase(Ease.OutCubic));
            
            //Home button
            sequence.Append(_restartLevelButton.DOFade(1, FadeTime));
            sequence.Join(_restartRect.DOScale(1f, FadeTime).SetEase(Ease.OutBack));
        }
    }
}