using System.Collections.Generic;
using UnityEngine;

public static class GUIStyleDTO
{
    private static Dictionary<TypeGUIStyle, GUIStyle> styles = new Dictionary<TypeGUIStyle, GUIStyle>();

    public static GUIStyle StyleWithType(TypeGUIStyle type)
    {
        if (!styles.TryGetValue(type, out var value))
        {
            switch (type)
            {
                case TypeGUIStyle.BoxHeader:
                    styles.Add(type, GetHeaderStyle());
                    break;
                case TypeGUIStyle.Button:
                    styles.Add(type, GetButtonStyle());
                    break;
            }
        }

        return styles[type];
    }
    
    private static GUIStyle GetButtonStyle()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 30;

        Font myFont = (Font) Resources.Load("Fonts/comic", typeof(Font));
        myButtonStyle.font = myFont;

        myButtonStyle.normal.textColor = Color.white;
        myButtonStyle.hover.textColor = Color.gray;
        return myButtonStyle;
    }

    private static GUIStyle GetHeaderStyle()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.box);
        myButtonStyle.fontSize = 20;

        Font myFont = (Font) Resources.Load("Fonts/comic", typeof(Font));
        myButtonStyle.font = myFont;

        myButtonStyle.normal.textColor = Color.white;
        myButtonStyle.hover.textColor = Color.gray;
        return myButtonStyle;
    }
}

public enum TypeGUIStyle { BoxHeader, Button }