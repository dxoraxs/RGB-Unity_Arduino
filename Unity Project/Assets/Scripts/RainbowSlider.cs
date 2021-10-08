using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowSlider : MonoBehaviour
{
    private RectTransform rectTransform;
    public RawImage rawImage;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
        rectTransform = rawImage.rectTransform;

        var newTexture = new Texture2D(1, (int)transform.parent.parent.parent.GetComponent<RectTransform>().rect.height);

        var stepY = newTexture.height / 3f;
        for (int y = 0; y < newTexture.height; y++)
        {
            newTexture.Color32(y, stepY);
        }

        newTexture.Apply();
        rawImage.texture = newTexture;
    }
}