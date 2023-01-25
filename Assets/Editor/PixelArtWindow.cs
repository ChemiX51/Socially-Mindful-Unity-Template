using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PixelArtWindow : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    [MenuItem("Window/Pixel Art Window")]

    public static void ShowWindow()
    {
        GetWindow(typeof(PixelArtWindow));
    }

    void OnGUI()
    {

    }
}
