using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShowHidePanelAnimation : MonoBehaviour
{
    [SerializeField] private Image backgroundShadow;
    [SerializeField] private RectTransform backgroundMain;
    //[SerializeField] private Image image;
        
    private bool backgroundCompleteFade;
    private bool startHide;
    private Sequence panelAnimation;
    private Vector2 startPosition;

    public void EnablePanel(bool value)
    {
        backgroundCompleteFade = value;
        gameObject.SetActive(true);
    }
    
    private void Awake()
    {
        startPosition = backgroundMain.anchoredPosition;
        panelAnimation = DOTween.Sequence();
    }

    private void OnEnable()
    {
        ShowBackground();
    }

    public void ShowBackground()
    {
        Debug.Log("Show panel");
        //gameObject.SetActive(true);
        backgroundMain.gameObject.SetActive(true);
        backgroundShadow.raycastTarget = true;

        panelAnimation.Complete();
        if (!backgroundCompleteFade)
        {
            var endValue = backgroundShadow.color.a;
            backgroundShadow.color = Color.clear;
            panelAnimation.Append(backgroundShadow.DOFade(endValue, .5f));
        }

        ShowMainPanel();
    }

    public void ShowMainPanel()
    {
        panelAnimation.Complete();
        panelAnimation = DOTween.Sequence();

        panelAnimation.Append(backgroundMain.DOAnchorPos(startPosition, .5f));
        backgroundMain.anchoredPosition += Vector2.down * Screen.height;
    }

    public void Hide()
    {
        if (startHide) return;
        backgroundShadow.raycastTarget = false;
        panelAnimation.Kill();
        startHide = true;
        //if (afterDisableBackground)
        
            var endValue = backgroundShadow.color;
            panelAnimation.Append(backgroundShadow.DOFade(0, .5f).OnComplete(() => backgroundShadow.color = endValue));
        
        
        var endPosition = startPosition - Vector2.up * Screen.height;

        panelAnimation.Append(backgroundMain.DOAnchorPos(endPosition, .5f).OnComplete(() =>
        {
            backgroundMain.anchoredPosition = startPosition;
            gameObject.SetActive(false);
            startHide = false;
        }));
    }
}