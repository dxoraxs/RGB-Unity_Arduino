using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using EasyMobile;
using DG.Tweening;

public class ButtonSwipe : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private float swipeDistance;
    [SerializeField] private float rotateAngle;
    [SerializeField] private Color colorSwipe;
    [SerializeField] private Color imageSwipe;
    [SerializeField] private UnityEvent onSwipeButton;
    [SerializeField] private Image imageButton;
    [SerializeField] private bool isRemoveButton;
    private Color defaultColor;
    private Color defaultImageColor;
    private RectTransform rectTransform;
    private float startXDrag;
    private Image background;
    private const int width = 1340;

    private void Awake()
    {
        background = GetComponent<Image>();
        defaultColor = background.color;
        defaultImageColor = imageButton.color;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startXDrag = eventData.position.x;
        GetComponentInParent<ScrollRect>().enabled = false;
        isSwipeButton = false;
    }

    private bool isSwipeButton;
    private bool isVibrate;

    public void OnDrag(PointerEventData eventData)
    {
        float newLeft = Mathf.Clamp(-(startXDrag - eventData.position.x), float.MinValue, 0);

        float newValue = Mathf.Abs(newLeft) / (width * swipeDistance);
        isSwipeButton = newValue >= 1;

        if (!isVibrate && isSwipeButton)
        {
            isVibrate = true;
            Vibration.VibratePop();
            GetComponentInParent<ItemSavedColor>().transform.rotation = Quaternion.Euler(Vector3.forward * rotateAngle);
        }
        else if (!isSwipeButton)
        {
            isVibrate = false;
            GetComponentInParent<ItemSavedColor>().transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        background.color = Color.Lerp(defaultColor, colorSwipe, newValue);
        imageButton.color = Color.Lerp(defaultImageColor, imageSwipe, newValue);
        rectTransform.SetLeft(newLeft);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponentInParent<ItemSavedColor>().transform.rotation = Quaternion.Euler(Vector3.zero);
        GetComponentInParent<ScrollRect>().enabled = true;

        if (isSwipeButton)
        {
            if (isRemoveButton)
            {
                var popup = NativeUI.ShowTwoButtonAlert("Remove", "Do you want to remove this item?", "Yes", "No");
                popup.OnComplete += OnSwipeAndAcceptPopup;
            }
            else
            {
                onSwipeButton?.Invoke();
            }
        }

        StartCoroutine(ReturnDefaultStateButton());
    }

    private IEnumerator ReturnDefaultStateButton()
    {
        var startOffsetLeft = rectTransform.offsetMin.x;
        var timeANimation = Mathf.Abs(startOffsetLeft / 100f) * 0.05f;
        var elapsed = 0f;

        while (elapsed < timeANimation)
        {
            float value = elapsed / timeANimation;
            rectTransform.SetLeft(Mathf.Lerp(startOffsetLeft, 0, value));
            background.color = Color.Lerp(colorSwipe, defaultColor, value);
            imageButton.color = Color.Lerp(imageSwipe, defaultImageColor, value);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.SetLeft(0);
        background.color = defaultColor;
        imageButton.color = defaultImageColor;
    }

    private void OnSwipeAndAcceptPopup(int index)
    {
        if (index == 0)
            onSwipeButton?.Invoke();
    }
}
