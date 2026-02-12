using System;
using System.Collections.Generic;
using _Project.Scripts.CommonUI;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

namespace _Project.Scripts.MainMenu.UI
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private SimpleScrollSnap _scroll;
        
        public SimpleScrollSnap Scroll => _scroll;
        public List<UnlockableButton> Levels { get; private set; }= new();
        
        public event Action<int> LevelSelected;

        private void Awake()
        {
            _scroll.enabled = false;
        }

        public void Initialize(List<UnlockableButton> levels)
        {
            Levels = levels;
            _scroll.enabled = true;
            
            OnEnable();
        }
        
        private void OnEnable()
        {
            foreach (var level in Levels)
                level.Clicked += OnLevelSelected;
        }
        
        private void OnDisable()
        {
            foreach (var level in Levels)
                level.Clicked -= OnLevelSelected;
        }
        
        private void OnLevelSelected(UnlockableButton button)
        {
            Debug.Log("selected level: " + Levels.IndexOf(button));
            LevelSelected?.Invoke(Levels.IndexOf(button));
        }
    }
}