using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.MainMenu.UI
{
    public class MainMenuUIAnimation : MonoBehaviour
    {
        private const float ScrollerStartScale = 0.8f;
        private const float TitleStartOffset = 300f;
        private const float ArrowsStartOffset = 300f;
        
        [SerializeField] private CanvasGroup _title;
        [SerializeField] private CanvasGroup _language;
        [SerializeField] private CanvasGroup _scroller;
        [SerializeField] private CanvasGroup _leftArrow;
        [SerializeField] private CanvasGroup _rightArrow;
        
        [SerializeField] private float _titleFadeTime = 0.4f;
        [SerializeField] private float _languageFadeTime = 0.4f;
        [SerializeField] private float _scrollerFadeTime = 0.4f;
        [SerializeField] private float _arrowsFadeTime = 0.6f;
        
        private RectTransform _titleRect;
        private RectTransform _languageRect;
        private RectTransform _leftRect;
        private RectTransform _rightRect;
        private RectTransform _scrollerRect;

        private Vector2 _titleTargetPos;
        private Vector2 _leftTargetPos;
        private Vector2 _rightTargetPos;
        
        private Tween _animationTween;
        
        private void Awake()
        {
            _titleRect = _title.GetComponent<RectTransform>();
            _languageRect = _language.GetComponent<RectTransform>();
            _leftRect = _leftArrow.GetComponent<RectTransform>();
            _rightRect = _rightArrow.GetComponent<RectTransform>();
            _scrollerRect = _scroller.GetComponent<RectTransform>();
            
            _titleTargetPos = _titleRect.anchoredPosition;
            _leftTargetPos = _leftRect.anchoredPosition;
            _rightTargetPos = _rightRect.anchoredPosition;
        }

        public void Run()
        {
            _animationTween?.Kill();
            
            _title.alpha = 0;
            _language.alpha = 0;
            _scroller.alpha = 0;
            _leftArrow.alpha = 0;
            _rightArrow.alpha = 0;
            
            _titleRect.localScale = Vector3.zero;
            _scrollerRect.localScale = Vector3.one * ScrollerStartScale;

            _titleRect.anchoredPosition = _titleTargetPos + Vector2.up * TitleStartOffset;
            _leftRect.anchoredPosition = _leftTargetPos + Vector2.left * ArrowsStartOffset;
            _rightRect.anchoredPosition = _rightTargetPos + Vector2.right * ArrowsStartOffset;

            var sequence = DOTween.Sequence();
            
            //Title
            sequence.Join(_title.DOFade(1, _titleFadeTime));
            sequence.Join(_titleRect.DOAnchorPos(_titleTargetPos, _titleFadeTime).SetEase(Ease.OutCubic));
            sequence.Join(
                _titleRect
                    .DOScale(Vector3.one, _titleFadeTime*2)
                    .SetEase(Ease.OutElastic)
            );
            
            //Language
            sequence.Join(_language.DOFade(1, _languageFadeTime));
            //sequence.Join(_languageRect.DOScale(1f, _languageFadeTime).SetEase(Ease.OutBack));
            
            //scroller 
            sequence.Join(_scroller.DOFade(1f, _scrollerFadeTime));
            sequence.Join(_scrollerRect.DOScale(1f, _scrollerFadeTime).SetEase(Ease.OutBack));

            //left
            sequence.Join(_leftArrow.DOFade(1f, _arrowsFadeTime));
            sequence.Join(_leftRect.DOAnchorPos(_leftTargetPos, _arrowsFadeTime).SetEase(Ease.OutCubic));

            //right
            sequence.Join(_rightArrow.DOFade(1f, _arrowsFadeTime));
            sequence.Join(_rightRect.DOAnchorPos(_rightTargetPos, _arrowsFadeTime).SetEase(Ease.OutCubic));
            
            _animationTween = sequence;
        }
    }
}