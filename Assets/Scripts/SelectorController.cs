using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorController : Singleton<SelectorController>
{
    [SerializeField] private Color32 mutableColor;
    [SerializeField] private Image previewImage;
    [SerializeField] private InputField input;
    [Space, SerializeField] private BluetoothController bluetoothController; 

    private Action<ItemColor> onColorSelected;

    public static void EnablePanel(ItemColor item, Action<ItemColor> callback)
    {
        Instance.gameObject.SetActive(true);

        Instance.mutableColor = item.color;
        Instance.input.text = item.name;
        Instance.onColorSelected = callback;

        foreach (var slider in Instance.GetComponentsInChildren<SliderValueChanger>())
            slider.SetNewValue(item.color);
    }

    public void OnClickComplite() => onColorSelected?.Invoke(new ItemColor(input.text, mutableColor));

    public static void OnChangeColorBox(Color color, TypeSlider type = TypeSlider.Default)
    {
        Instance.previewImage.color = Instance.mutableColor = color;
    }

    public static void OnSliderPointerUp(TypeSlider type)
    {
        int sendColor = 0;
        switch (type)
        {
            case TypeSlider.R:
                sendColor = Instance.mutableColor.r;
                break;
            case TypeSlider.G:
                sendColor = Instance.mutableColor.g;
                break;
            case TypeSlider.B:
                sendColor = Instance.mutableColor.b;
                break;
        }
        Instance.bluetoothController.SendData(type + MainController.GetColorString(sendColor));
    }

    public static void OnChangeColorSlider(byte value, TypeSlider type)
    {
        switch (type)
        {
            case TypeSlider.R:
                Instance.mutableColor = new Color32(value, Instance.mutableColor.g, Instance.mutableColor.b, 255);
                break;
            case TypeSlider.G:
                Instance.mutableColor = new Color32(Instance.mutableColor.r, value, Instance.mutableColor.b, 255);
                break;
            case TypeSlider.B:
                Instance.mutableColor = new Color32(Instance.mutableColor.r, Instance.mutableColor.g, value, 255);
                break;
        }
        Instance.previewImage.color = Instance.mutableColor;
        
        //CircleRainbow.SetColor(Instance.mutableColor, type);
    }
}
