using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<ItemColor> SavedColor;

    public SaveData()
    {
        SavedColor = new List<ItemColor>();
    }
}

[Serializable]
public class ItemColor
{
    private static int count = 0;
    public int id;
    public string name;
    public Color32 color;

    public ItemColor()
    {
        count++;
        id = count;
        name = "New Color";
        color = Color.white;
    }

    public ItemColor(string _name, Color _color)
    {
        count++;
        id = count;
        name = _name;
        color = _color;
    }
}