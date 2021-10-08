using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainController : Singleton<MainController>
{
    [SerializeField] private RawImage currentColorPreview;
    [SerializeField] private Text currentNameColor;
    [SerializeField] private BluetoothController bluetoothController;
    private List<ItemColor> savedColors = new List<ItemColor>();

    private void Awake()
    {
        Vibration.Init();
        if (!SaveDataManager.IsFirstGameStarted) OpenSelectorColor();
        savedColors = SaveDataManager.GetAllSavedColors;
        ItemSpawner.SpawnListItems(savedColors);
    }

    public static void SetCurrentColor(ItemColor item)
    {
        Instance.currentNameColor.text = item.name;
        NewMethod(item);
        Texture2D newTexture = new Texture2D(1, 1);
        newTexture.SetPixel(0, 0, item.color);
        newTexture.Apply();
        Instance.currentColorPreview.texture = newTexture;
    }

    private static void NewMethod(ItemColor item)
    {
        Instance.bluetoothController.SendData(Instance.CreateMessageToArduino(item.color));
    }

    private string CreateMessageToArduino(Color color)
    {
        StringBuilder str = new StringBuilder();
        str.Append('C');
        str.Append(GetColorString((int)(color.r * 255f)));
        str.Append(GetColorString((int)(color.g * 255f)));
        str.Append(GetColorString((int)(color.b * 255f)));
        return str.ToString();
    }

    public static string GetColorString(int component)
    {
        if (component < 10) return $"00{component}";
        if (component < 100) return $"0{component}";
        return component.ToString();
    }

    public static void Remove(ItemColor item)
    {
        Instance.savedColors.Remove(item);
        Instance.OnListChanged();
    }

    public static void OpenSelectorColor(ItemColor itemColor)
    {
        var indexChanges = Instance.savedColors.IndexOf(itemColor);
        NewMethod(itemColor);
        SelectorController.EnablePanel(itemColor, (item) => 
        {
            Instance.savedColors[indexChanges] = item;
            Instance.OnListChanged();
        });
    }

    public void OpenSelectorColor()
    {
        var newItemColor = new ItemColor();
        NewMethod(newItemColor);
        SelectorController.EnablePanel(newItemColor, (item) => 
        {
            savedColors.Add(item);
            OnListChanged();
        });
    }

    private void OnListChanged()
    {
        ItemSpawner.SpawnListItems(savedColors);
        SaveDataManager.SaveData(Instance.savedColors);
    }
}