using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private Collider2D _collider2D;
    private Transform _transform;
    private BalloonsHandler _balloonsHandler;

    private float minSpeed;
    private float maxSpeed;

    private float speed;

    private bool isDestroyed = false;

    private void Start()
    {
        _balloonsHandler = FindObjectOfType<BalloonsHandler>();

        _transform = GetComponent<Transform>();
        _collider2D = GetComponent<Collider2D>();

        Initialize();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        MoveUp();

        if(!_balloonsHandler.UseMouse)
            TouchHandle();
        else
            MouseHandle();
    }

    private void Initialize()
    {
        _balloonsHandler.IncreaseBalloonsCount();
        minSpeed = _balloonsHandler.MinSpeed;
        maxSpeed = _balloonsHandler.MaxSpeed;
    }

    private void MoveUp()
    {
        Vector3 delta = Vector3.up * speed * Time.deltaTime;
        _transform.position += delta;
    }

    private void TouchHandle()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            CheckInputPosition(touchPosition);
        }
    }

    private void MouseHandle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            CheckInputPosition(touchPosition);
        }
    }

    private void CheckInputPosition(Vector2 position)
    {
        if (_collider2D == Physics2D.OverlapPoint(position))
        {
            _balloonsHandler.PlayObjectParticle(_transform.position);
            DestroyGameObject();
        }
    }

    private void DestroyGameObject()
    {
        if (!isDestroyed)
        {
            _balloonsHandler.DecreaseBalloonsCount();
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        DestroyGameObject();
    }
}
