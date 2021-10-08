using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonClickListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform children;
    private Tweener animation;

    private void Start()
    {
        children.localScale = Vector3.zero;
    }

    private void StopAnimation()
    {
        if (animation != null && animation.active) animation.Kill();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        children.position = Input.mousePosition;

        StopAnimation();
        animation = children.DOScale(1, (1 - children.localScale.y) / 4);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        children.anchoredPosition = Vector2.zero;

        StopAnimation();

        animation = children.DOScale(0, children.localScale.y / 4);
    }
}
