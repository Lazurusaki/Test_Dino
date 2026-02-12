using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private Transform _rightPlaceTransform;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowSpriteRenderer;

    private Collider2D _collider2D;

    private PuzzleHandler _puzzleHandler;
    private SoundHandler _soundHandler;
    private ShadowAnimationHandler _animationHandler;

    private Vector2 initialPosition;

    private float correctPositionAccuracy;
    private float animationAccuracy;
    private float smoothDragMultiplier;
    private float minSmoothAnimationMultiplier;
    private float maxSmoothAnimationMultiplier;
    private float shadowAnimationDuration;

    private bool inInitialPlace;
    private bool inRightPlace;
    private bool isTouched;
    private bool isShaking;

    private bool firstPress;

    public event Action Good;
    public event Action Bad;

    private void Awake()
    {
        _puzzleHandler = FindObjectOfType<PuzzleHandler>();
        _transform = GetComponent<Transform>();

        initialPosition = _transform.position;
        _puzzleHandler.IncreasePiecesCount(initialPosition);
    }

    private void OnEnable()
    {
        _puzzleHandler.onPlayerWin += Disactivate;
    }

    private void Start()
    {
        _soundHandler = FindObjectOfType<SoundHandler>();

        _animationHandler = GetComponent<ShadowAnimationHandler>();

        _collider2D = GetComponent<Collider2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];

        Initialize();

        initialPosition = _puzzleHandler.GenerateInitialPosition();
        StartCoroutine(VisitStartPosition(_rightPlaceTransform.position, false));
    }

    private void Update()
    {
        if (inInitialPlace)
        {
            if (!_puzzleHandler.UseMouse)
            {
                if (Input.touchCount > 0 && !inRightPlace)
                {
                    HandlePressing();
                }
            }
            else
            {
                if (!inRightPlace)
                    MouseMove();
            }
        }
    }

    private void OnDisable()
    {
        _puzzleHandler.onPlayerWin -= Disactivate;
    }

    private void Initialize()
    {
        correctPositionAccuracy = _puzzleHandler.CorrectPositionAccuracy;
        animationAccuracy = _puzzleHandler.AnimationAccuracy;
        smoothDragMultiplier = _puzzleHandler.SmoothDragMultiplier;
        minSmoothAnimationMultiplier = _puzzleHandler.MinSmoothAnimationMultiplier;
        maxSmoothAnimationMultiplier = _puzzleHandler.MaxSmoothAnimationMultiplier;
        shadowAnimationDuration = _puzzleHandler.ShadowAnimationDuration;
    }

    private Vector2 distance;

    private void OnMouseDown()
    {
        distance = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(transform.position).z)) - transform.position;
    }

    private void HandlePressing()
    {
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);


        Vector2 distance_to_screen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 pos = new Vector3(pos_move.x - distance.x, pos_move.y - distance.y);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (_collider2D == Physics2D.OverlapPoint(touchPosition))
                {
                    isTouched = true;
                    ChangeSortingOrder(2);
                    _animationHandler.StartAnimation(shadowAnimationDuration, 1, false, Vector2.zero);
                }

                break;
            case TouchPhase.Moved:
                if (isTouched)
                {
                    MovePiece(pos, smoothDragMultiplier);
                }

                break;
            case TouchPhase.Stationary:
                if (isTouched)
                {
                    MovePiece(pos, smoothDragMultiplier);
                }

                break;
            case TouchPhase.Ended:
                if (!inRightPlace && isTouched)
                {
                    inInitialPlace = false;
                    StartCoroutine(VisitStartPosition(_transform.position, true));
                }

                isTouched = false;
                break;
            default:
                break;
        }

        StickToRightPlace(_rightPlaceTransform.position, PositionsTypes.Initial, correctPositionAccuracy);
    }

    private void MovePiece(Vector2 targetPosition, float smoothMultiplier)
    {
        Vector2 motionPosition = Vector2.Lerp(_transform.position, targetPosition, smoothMultiplier * Time.deltaTime);
        _transform.position = motionPosition;
    }

    private void StickToRightPlace(Vector2 targetPlace, PositionsTypes type, float accuracy)
    {
        if (Mathf.Abs(_transform.position.x - targetPlace.x) <= accuracy &&
            Mathf.Abs(_transform.position.y - targetPlace.y) <= accuracy)
        {
            switch (type)
            {
                case PositionsTypes.Start:
                    inInitialPlace = true;
                    break;
                case PositionsTypes.Initial:

                    _animationHandler.outline.gameObject.SetActive(false);

                    ChangeSortingOrder(-2);

                    _transform.position = targetPlace + new Vector2(-0.1f, 0.1f);
                    _animationHandler.StartAnimation(shadowAnimationDuration, -1, true, new Vector2(0.1f, -0.1f));

                    _puzzleHandler.DecreasePiecesCount();
                    _puzzleHandler.PlayObjectParticle(_transform.position);

                    _puzzleHandler.OnPlayerGood();
                    //_soundHandler.PlayWinClip();
                    inRightPlace = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void MouseMove() 
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TouchPhase touch = TouchPhase.Canceled;

        Vector2 distance_to_screen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 pos = new Vector3(pos_move.x - distance.x, pos_move.y - distance.y);


        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPress = true;
                touch = TouchPhase.Began;
            }

            if (Input.GetMouseButton(0) && !firstPress)
            {
                touch = TouchPhase.Moved;
            }

            if (Input.GetMouseButtonUp(0))
            {
                touch = TouchPhase.Ended;
            }
        }

        switch (touch)
        {
            case TouchPhase.Began:
                if (_collider2D == Physics2D.OverlapPoint(mousePosition))
                {
                    ChangeSortingOrder(2);
                    _animationHandler.StartAnimation(shadowAnimationDuration, 1, false, Vector2.zero);

                    isTouched = true;
                    firstPress = false;
                }

                break;
            case TouchPhase.Moved:
                if (isTouched)
                {
                    MovePiece(pos, smoothDragMultiplier);
                }

                break;
            case TouchPhase.Ended:
                if (!inRightPlace && isTouched)
                {
                    inInitialPlace = false;
                    StartCoroutine(VisitStartPosition(_transform.position, true));
                }

                isTouched = false;
                break;
            default:
                break;
        }

        StickToRightPlace(_rightPlaceTransform.position, PositionsTypes.Initial, correctPositionAccuracy);
    }

    private IEnumerator VisitStartPosition(Vector2 targetPosition, bool useDragSmooth)
    {
        float smooth;
        if (useDragSmooth)
        {
            smooth = smoothDragMultiplier;
        }
        else
        {
            smooth = Random.Range(minSmoothAnimationMultiplier, maxSmoothAnimationMultiplier);
        }

        _transform.position = targetPosition;
        yield return new WaitForSeconds(0.2f);

        while (!inInitialPlace)
        {
            smooth += Time.deltaTime;
            MovePiece(initialPosition, smooth);
            StickToRightPlace(initialPosition, PositionsTypes.Start, animationAccuracy);

            yield return null;
        }

        if (useDragSmooth)
        {
            _puzzleHandler.OnPlayerBad();
            //_soundHandler.PlayFailClip();
            StartCoroutine(ChangeShakeState());
            StartCoroutine(ShakeObject());
        }
    }

    private void Disactivate() => gameObject.SetActive(false);

    private void ChangeSortingOrder(int count)
    {
        _spriteRenderer.sortingOrder += count;
        _shadowSpriteRenderer.sortingOrder += count;

        _animationHandler.outline.GetComponent<SpriteRenderer>().sortingOrder += count;
    }

    private void DownPuzzlePiece()
    {
        _transform.position = initialPosition + new Vector2(-0.1f, 0.1f);
        _animationHandler.StartAnimation(shadowAnimationDuration, -1, true, new Vector2(0.1f, -0.1f));

        ChangeSortingOrder(-2);
    }

    private IEnumerator ChangeShakeState()
    {
        if (!isShaking)
            isShaking = true;

        yield return new WaitForSeconds(_puzzleHandler.ShakeTime);

        DownPuzzlePiece();
        isShaking = false;
    }

    private IEnumerator ShakeObject()
    {
        int i = 1;
        while (isShaking)
        {
            Vector3 newPosition =
                new Vector3(Mathf.Sin(Time.time * 1f) * 1f, _transform.position.y, _transform.position.z);
            i *= -1;
            newPosition.x = initialPosition.x + i * newPosition.x / 7;
            _transform.position = newPosition;
            yield return null;
        }
    }
}

enum PositionsTypes
{
    Start,
    Initial
}