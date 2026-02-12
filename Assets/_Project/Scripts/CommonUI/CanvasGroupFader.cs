using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.CommonUI
{
    public class CanvasGroupFader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Range(0, 10)] private float _fadeInDuration = 0.0f;
        [SerializeField, Range(0, 10)] private float _fadeOutDuration = 0.0f;

        private Tween _tween;

        private void Awake()
        {
            if (_canvasGroup == null)
                throw new NullReferenceException("CanvasGroup source is not set");
        }

        public async Task Show()
        {
            if (_canvasGroup == null) return;

            KillTween();

            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            _tween = _canvasGroup
                .DOFade(1f, _fadeInDuration)
                .SetUpdate(true);

            await _tween.AsyncWaitForCompletion();
        }

        public async Task Hide()
        {
            if (_canvasGroup == null) return;

            KillTween();

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _tween = _canvasGroup
                .DOFade(0f, _fadeOutDuration)
                .SetUpdate(true);

            await _tween.AsyncWaitForCompletion();
        }

        private void KillTween()
        {
            if (_tween != null && _tween.IsActive())
                _tween.Kill();
        }
    }
}