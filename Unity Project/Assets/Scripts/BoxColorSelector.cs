using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxColorSelector : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Color32 testColor;
    [Space, SerializeField] private Image cursor;
    [SerializeField] private Color drag;
    private RawImage rawImage;
    private Vector2 cursorStartPosition;
    private Texture2D texture;

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        texture = new Texture2D((int) transform.parent.GetComponent<RectTransform>().rect.width,
            (int) transform.parent.parent.GetComponent<RectTransform>().rect.height);
        rawImage.texture = texture;

        ChangeTexture();

        DOVirtual.DelayedCall(0.25f, () => { cursorStartPosition = cursor.transform.position; });
    }

    private void OnValidate()
    {
        if (texture == null) return;
        ChangeTexture();
    }

    private void ChangeTexture()
    {
        for (int x = 0; x < texture.width; x++)
        {
            var percentTextureWidth = (float) x / texture.width;
            var inversePercentTextureWidth = (1 - percentTextureWidth);

            for (int y = 0; y < texture.height; y++)
            {
                byte r = 0, g = 0, b = 0;

                var percentTextureHeight = (float) y / texture.height;
                var heightWidth = percentTextureHeight * inversePercentTextureWidth;

                r = (byte) (heightWidth * Mathf.Lerp(testColor.r, 0, percentTextureWidth)); //testColor.r);
                g = (byte) (heightWidth * Mathf.Lerp(testColor.g, 0, percentTextureWidth)); //testColor.g);
                b = (byte) (heightWidth * Mathf.Lerp(testColor.b, 0, percentTextureWidth)); //testColor.b);

                var color = new Color32(r, g, b, 255);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    [SerializeField] private RectTransform parentRect;

    public void OnDrag(PointerEventData eventData)
    {
        var eventDataPosition = eventData.position + Vector2.up * 200;
        var parentSize = parentRect.rect.width;
        var newPoint = new Vector2(
            Mathf.Clamp(eventDataPosition.x, cursorStartPosition.x - parentSize / 2,
                cursorStartPosition.x + parentSize / 2),
            Mathf.Clamp(eventDataPosition.y, cursorStartPosition.y - parentSize / 2,
                cursorStartPosition.y + parentSize / 2));
        cursor.rectTransform.position = newPoint;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cursor.color = drag;
        cursor.GetComponent<Outline>().effectColor = drag;
        foreach (Transform children in cursor.transform)
        {
            children.GetComponent<Image>().color = drag;
        }

        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cursor.color = Color.white;
        cursor.GetComponent<Outline>().effectColor = Color.white;
        ;
        foreach (Transform children in cursor.transform)
        {
            children.GetComponent<Image>().color = Color.white;
            ;
        }
    }
}