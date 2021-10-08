using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleRainbow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private static CircleRainbow instance;
    [SerializeField] private float minRadius;
    [SerializeField] private float minRadiusWhiteBlack;
    [SerializeField] private float maxRadius;
    [SerializeField] private float colorRadius;
    [SerializeField, Space] private Image cursor;
    [SerializeField] private Color drag;
    private RectTransform rectTransform;
    private RawImage rawImage;
    private Texture2D texture;

    private void OnValidate()
    {
        lastPosition = cursor.rectTransform.anchoredPosition + 
            (Vector2)(cursor.rectTransform.rotation * (Vector2.up * (1 - cursor.rectTransform.pivot.y) * cursor.rectTransform.rect.height)); /*+
            Vector2.one * (GetComponent< RawImage >().texture.width / 2);*/
        //colorRadius = (minRadius + maxRadius) / 2f;
        //cursor.rectTransform.anchoredPosition = Vector2.right * colorRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + (Vector3)lastPosition, 25);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minRadius);
        Gizmos.DrawWireSphere(transform.position, maxRadius);
        Gizmos.DrawWireSphere(transform.position, minRadiusWhiteBlack);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, colorRadius);
    }

    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();

        rawImage = GetComponent<RawImage>();
        texture = new Texture2D((int) rectTransform.rect.width, (int) rectTransform.rect.width);
        ChangeTexture();
    }

    private void Start()
    {
        GetColor();
    }

    private void ChangeTexture()
    {
        var stepY = 360f / 3f;
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                var vectorPoint = new Vector2(x - texture.width / 2, y - texture.height / 2);
                if (vectorPoint.magnitude > maxRadius || vectorPoint.magnitude < minRadiusWhiteBlack)
                {
                    texture.SetPixel(x, y, Color.clear);
                }
                else if (vectorPoint.magnitude < minRadius)// && vectorPoint.magnitude > minRadiusWhiteBlack)
                {
                    texture.SetPixel(x, y, vectorPoint.x < 0 ? Color.white : Color.black);
                }
                else
                {
                    var angle = Vector2.Angle(vectorPoint.normalized, Vector2.up);
                    angle = vectorPoint.x < 0 ? 360f - Mathf.Abs(angle) : angle;
                    texture.Color32(x, y, angle, stepY);
                }
            }
        }

        texture.Apply();
        rawImage.texture = texture;
    }

    public static float lastAngle;

    public static void OnStartDragSlide()
    {
        var transformPosition = (Vector2)instance.transform.position;
        lastAngle = Vector2.Angle(Vector2.up, (Vector2)instance.cursor.rectTransform.position - transformPosition);
        lastAngle = instance.cursor.rectTransform.position.x < 0 ? 360f - Mathf.Abs(lastAngle) : lastAngle;
    }
    
    public static void SetColor(Color newColor, TypeSlider type)
    {
       /* //var stepAngle = 360 / 3;
        var transformPosition = (Vector2)instance.transform.position;
        
        var oldAngle = Vector2.Angle(Vector2.up, (Vector2)instance.cursor.rectTransform.position - transformPosition);
        oldAngle = instance.cursor.rectTransform.position.x < 0 ? 360f - Mathf.Abs(oldAngle) : oldAngle;

        float findAngle;
        *//*int offsetBorderTexture = instance.texture.width / 2 - (int)instance.colorRadius;
        for (int x = offsetBorderTexture; x < instance.texture.width - offsetBorderTexture; x++)
        {
            for (int y = offsetBorderTexture; y < instance.texture.height - offsetBorderTexture; y++)
            {
                var vectorPoint = new Vector2(x - instance.texture.width / 2, y - instance.texture.height / 2);
                if (vectorPoint.magnitude - instance.maxRadius > 5f)
                {
                    continue;
                }
                else
                {
                    var angle = Vector2.Angle(vectorPoint.normalized, Vector2.up);
                    angle = vectorPoint.x < 0 ? 360f - Mathf.Abs(angle) : angle;
                    texture.Color32(x, y, angle, stepY);
                }
            }
        }*//*

        float newAngle = lastAngle;
        
         switch (type)
        {
            case TypeSlider.R:
                newAngle += AngleReform(oldAngle, value);
                break;
            case TypeSlider.G:
                oldAngle -= stepAngle;
                oldAngle = oldAngle < 0 ? 360f - Mathf.Abs(oldAngle) : oldAngle;
                newAngle += AngleReform(oldAngle, value);
                break;
            case TypeSlider.B:
                oldAngle -= 2 * stepAngle;
                oldAngle = oldAngle < 0 ? 360f - Mathf.Abs(oldAngle) : oldAngle;
                newAngle += AngleReform(oldAngle, value);
                break;
        }
        
        var newVector = Quaternion.Euler(newAngle * Vector3.back) * Vector3.up;
        
        Debug.Log($"oldAngle = {lastAngle} | new = {newAngle}");
        
        var newPoint = transformPosition + (Vector2)newVector.normalized * instance.colorRadius;
        instance.cursor.rectTransform.position = newPoint;

        instance.GetColor(type);*/
    }

    private static float AngleReform(float oldAngle, float value)
    {
        if (oldAngle > 180)
            return (value / 255f) * 120f / 2;
        return (value / -255f) * 120f / 2;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        var transformPosition = (Vector2)transform.position;
        var eventDataPosition = eventData.position + Vector2.up * 200 - transformPosition;

        Vector3 newPoint;
        if (eventDataPosition.magnitude < minRadius)
            cursor.rectTransform.sizeDelta = Vector2.up * ((minRadius + minRadiusWhiteBlack) / 2 + 75 ) + Vector2.right * cursor.rectTransform.rect.width;
        //newPoint = transformPosition + eventDataPosition.normalized * ((minRadius + minRadiusWhiteBlack) / 2);     
        else
            cursor.rectTransform.sizeDelta = Vector2.up * (colorRadius + 150) + Vector2.right * cursor.rectTransform.rect.width;
        //newPoint = transformPosition + eventDataPosition.normalized * colorRadius;

        float newAngle = Vector3.Angle(Vector3.up, eventDataPosition);
        newAngle = eventDataPosition.x > 0 ? 360f - Mathf.Abs(newAngle) : newAngle;

        cursor.rectTransform.rotation = Quaternion.Euler(Vector3.forward * newAngle);
        GetColor();
    }

    private Vector2 lastPosition;

    private void GetColor(TypeSlider type = TypeSlider.Default)
    {
        /*var x = (int) cursor.rectTransform.anchoredPosition.x + texture.width / 2;
        var y = (int) cursor.rectTransform.anchoredPosition.y + texture.height / 2;*/

        lastPosition = cursor.rectTransform.anchoredPosition +
            (Vector2)(cursor.rectTransform.rotation * (Vector2.up * (1 - cursor.rectTransform.pivot.y) * cursor.rectTransform.rect.height));
        /*lastPosition = cursor.rectTransform.anchoredPosition +
            (Vector2)(cursor.rectTransform.rotation * (Vector2.up * cursor.rectTransform.rect.height)) + Vector2.one * (texture.width/2);*/

        SelectorController.OnChangeColorBox(texture.GetPixel((int)lastPosition.x + texture.width / 2, (int)lastPosition.y + texture.height / 2), type);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //cursor.color = drag;
        //cursor.GetComponent<Outline>().effectColor = drag;
        foreach (Transform children in cursor.transform)
        {
            children.GetComponent<Image>().color = drag;
        }

        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //cursor.color = Color.white;
        //cursor.GetComponent<Outline>().effectColor = Color.white;
        ;
        foreach (Transform children in cursor.transform)
        {
            children.GetComponent<Image>().color = Color.white;
            ;
        }
    }
}

public static class TextureRainbow
{
    public static void Color32(this Texture2D newTexture, int y, float stepY)
    {
        byte r = 0, g = 0, b = 0;
        if (y < stepY) r = (byte) ((1 - y / stepY) * 255f);
        else if (y >= newTexture.height - stepY) r = (byte) ((y % (2 * stepY)) / stepY * 255);

        if (y < stepY) g = (byte) (y / stepY * 255f);
        else if (y >= stepY && y < stepY * 2) g = (byte) ((1 - ((y - stepY) / stepY)) * 255f);

        if (y >= stepY && y < stepY * 2) b = (byte) ((y - stepY) / stepY * 255f);
        else if (y >= 2 * stepY) b = (byte) ((1 - (y - 2 * stepY) / stepY) * 255f);

        var color = new Color32(r, g, b, 255);

        for (int x = 0; x < newTexture.width; x++)
        {
            newTexture.SetPixel(x, y, color);
        }
    }

    public static void Color32(this Texture2D newTexture, int x, int y, float angleValue, float stepAngle)
    {
        byte r = 0, g = 0, b = 0;
        if (angleValue < stepAngle) r = (byte) ((1 - angleValue / stepAngle) * 255f);
        else if (angleValue >= 2 * stepAngle)
            r = (byte) ((angleValue % (2 * stepAngle)) / stepAngle * 255);

        if (angleValue < stepAngle) g = (byte) (angleValue / stepAngle * 255f);
        else if (angleValue >= stepAngle && angleValue < stepAngle * 2)
            g = (byte) ((1 - ((angleValue - stepAngle) / stepAngle)) * 255f);

        if (angleValue >= stepAngle && angleValue < stepAngle * 2)
            b = (byte) ((angleValue - stepAngle) / stepAngle * 255f);
        else if (angleValue >= 2 * stepAngle) b = (byte) ((1 - (angleValue - 2 * stepAngle) / stepAngle) * 255f);

        var color = new Color32(r, g, b, 255);

        newTexture.SetPixel(x, y, color);
    }
}