using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EasyMobile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainBodySelectorColor : MonoBehaviour
{
    private static MainBodySelectorColor instance;

    [SerializeField] private CanvasScaler canvas;
    [SerializeField] private Color colorSelected;
    private float testYPos;
    private RectTransform rectTransform;
    private TouchScreenKeyboard mobileKeys;
    private Vector2 defaultPosition;

    public static void OnClickSlider(Image line, Slider slider, Text text, float yPosition)
    {
#if UNITY_EDITOR
        instance.testYPos = yPosition;
        return;
#endif
        instance.StartCoroutine(instance.ListenerKeyboardHeightChange(yPosition));
        instance.StartCoroutine(instance.WaitingKeyboardClose(line, slider, text));

        instance.mobileKeys = TouchScreenKeyboard.Open(text.text, TouchScreenKeyboardType.NumberPad,
            true, false, false, false, "0", 3);
        TouchScreenKeyboard.hideInput = true;
    }

    private IEnumerator WaitingKeyboardClose(Image line, Slider slider, Text text)
    {
        line.color = colorSelected;
        text.color = colorSelected;

        yield return new WaitForSeconds(1f);
        while (mobileKeys.active)
        {
            var newValue = mobileKeys.text.Length == 0 ? 0 : Mathf.Clamp(int.Parse(mobileKeys.text), 0, 255);
            slider.value = newValue;
            mobileKeys.text = text.text = newValue.ToString();
            yield return null;
        }

        line.color = Color.white;
        text.color = Color.white;

        instance.StartCoroutine(instance.ListenerKeyboardHeightChange());
    }

    private IEnumerator ListenerKeyboardHeightChange(float offset = -1)
    {
        var time = 0f;
        var allTime = time = 0.25f;
        var startPosition = rectTransform.position;
        yield return new WaitForSeconds(0.35f);
        var newHeight = MobileUtilities.GetKeyboardHeight(TouchScreenKeyboard.hideInput);
        /*while (time > 0)
        {
            time -= Time.deltaTime;*/
            if (offset < 0)
            {
                rectTransform.DOMoveY(defaultPosition.y, allTime);
                //rectTransform.position = Vector2.Lerp(startPosition, defaultPosition, allTime - time);
            }
            else
            {
                rectTransform.DOMoveY(defaultPosition.y + newHeight - offset, allTime);
                /*rectTransform.position = Vector2.Lerp(startPosition,
                    defaultPosition + Vector2.up * (newHeight /* + Mathf.Abs(offset)#1#), allTime - time);*/
            }

            /*
            yield return null;
        }*/

        //rectTransform.anchoredPosition = defaultPosition + Vector2.up * (newHeight - Mathf.Abs(offset));
    }

    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();

        defaultPosition = rectTransform.position;
        DOVirtual.DelayedCall(0.5f, () =>
        {
            defaultPosition = rectTransform.position;
        });
    }

/*#if UNITY_EDITOR
    private void Update()
    {
        rectTransform.anchoredPosition = defaultPosition + Vector2.up * (test * 10 - testYPos);
    }
#endif*/
}