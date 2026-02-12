using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAnimationHandler : MonoBehaviour
{
    private Transform _transform;
    [HideInInspector]
    public Transform _shadowTransform;
    
    
    [NonSerialized]
    public Transform shadow, outline;
    
    
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _shadowTransform = GetComponentsInChildren<Transform>()[1];
        
        if (transform.childCount > 1)
        {
            outline = transform.GetChild(1);
            outline.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        
        shadow = Instantiate(_shadowTransform,transform);
        shadow.transform.SetSiblingIndex(transform.childCount);
        
        shadow.transform.localPosition = new Vector2(_shadowTransform.localPosition.x,_shadowTransform.localPosition.y - 0.25f);
    }

    public void StartAnimation(float duration, int direction, bool isBack, Vector2 shadowMoveVector)
    {
        StartCoroutine(AnimateGrab(duration, direction, isBack, shadowMoveVector));
    }

    private IEnumerator AnimateGrab(float duration, int direction, bool isBack, Vector2 shadowMoveVector)
    {
        if(outline != null)
            outline.localScale = Vector3.one;

        float time = 0;

        Vector2 startPiecePosition = _transform.position;
        Vector2 targetPiecePosition = new Vector2(startPiecePosition.x - 0.1f * direction, startPiecePosition.y + 0.1f * direction);

        while (time < duration)
        {
            _transform.position = Vector2.Lerp(startPiecePosition, targetPiecePosition, time / duration);
            _shadowTransform.position = Vector2.Lerp(_shadowTransform.position, startPiecePosition + shadowMoveVector, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _transform.position = targetPiecePosition;
        if(!isBack)
        {
            _shadowTransform.position = startPiecePosition;
            if(outline != null)
                outline.localScale = Vector3.one * 1.1f;

        }

    }
    
}
