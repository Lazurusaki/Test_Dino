using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Puzzles;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField] private ParticleHandler _winParticleHandler;
    
    [SerializeField] private GameObject puzzleBackground;

    private int piecesCount;

    [SerializeField] private List<Vector2> positions = new List<Vector2>();

    [SerializeField] private float correctPositionAccuracy = 0.5f;
    [SerializeField] private float animationAccuracy = 0.01f;
    [SerializeField] private float smoothDragMultiplier = 10f;
    [SerializeField] private float minSmoothAnimationMultiplier = 2.5f;
    [SerializeField] private float maxSmoothAnimationMultiplier = 4f;
    [SerializeField] private float shadowAnimationDuration = 0.5f;
    [SerializeField] private float shakeTime = 0.05f;
    [SerializeField] private float shakeMultiplier = 5f;
    
    [field: SerializeField] public  Transform RealAnimal {get; private set;}

    public delegate void OnPlayerWin();

    public event OnPlayerWin onPlayerWin;

    public event Action Good;
    public event Action Bad;
    
    private Puzzle _puzzle;
    
    public void Initialize(Puzzle puzzle)
    {
        _puzzle = puzzle;
    }

    public bool UseMouse => YG2.envir.isDesktop;
    public float CorrectPositionAccuracy => correctPositionAccuracy;
    public float AnimationAccuracy => animationAccuracy;
    public float SmoothDragMultiplier =>smoothDragMultiplier;
    public float MinSmoothAnimationMultiplier => minSmoothAnimationMultiplier;
    public float MaxSmoothAnimationMultiplier => maxSmoothAnimationMultiplier;
    public float ShadowAnimationDuration => shadowAnimationDuration;
    public float ShakeTime => shakeTime;
    public float ShakeMultiplier => shakeMultiplier;

    public void IncreasePiecesCount(Vector2 position)
    {
        piecesCount++;
        positions.Add(position);
    }

    public void DecreasePiecesCount()
    {
        piecesCount--;

        if (piecesCount == 0)
        {
            _puzzle.ShowAnimal();
            _puzzle.HideAnimalBackground();
            onPlayerWin();
        }
    }

    public void ShowRealAnimal()
    {
        _puzzle.ShowRealAnimal();
    }

    public void PlayObjectParticle(Vector2 position)
    {
        _winParticleHandler.PlayParticle(position);
    }

    public void OnPlayerGood()
    {
        Good?.Invoke();
    }
    
    public void OnPlayerBad()
    {
        Bad?.Invoke();
    }

    public Vector2 GenerateInitialPosition()
    {
        int index = Random.Range(0, positions.Count - 1);
        Vector2 newPosition = positions[index];
        positions.Remove(newPosition);

        return newPosition;
    }
}