using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Puzzles
{
    public class Puzzle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _animal;
        [SerializeField] private SpriteRenderer _realAnimal;
        [SerializeField] private SpriteRenderer _animalBackground;

        [SerializeField] private Transform _targetPlacesContainer;
        [SerializeField] private Transform _piecesContainer;

        private Tween _tween;
        
        

        private void Awake()
        {
            //_realAnimal.color = new Color(1f, 1f, 1f, 0f);
            
            _animal.gameObject.SetActive(false);
            _realAnimal.gameObject.SetActive(false);
        }
        
        public void HideAnimalBackground()
        {
            _animalBackground.gameObject.SetActive(false);
        }
        
        public void ShowAnimal()
        {
            _animal.gameObject.SetActive(true);
            _animal.GetComponent<CompletePuzzleHandler>().Run();
        }

        public void ShowRealAnimal()
        {
            _realAnimal.gameObject.SetActive(true);
            _realAnimal.DOFade( 1f, 0.5f);
        }
    }
}