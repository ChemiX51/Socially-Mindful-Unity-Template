using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using System;
using System.IO;
using UnityEngine.UIElements;

public class PixelArtWindow : EditorWindow
{
    public int x = 16;
    public int y = 16;
    public Texture2D texture;

    PixelGrid pixels;

    bool editExistingTexture = false;
    bool zoomIn = false;
    bool zoomOut = false;
    double zoomMultiplier = 1;
    Color color = Color.black;

    [MenuItem("Window/Pixel Art Maker")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(PixelArtWindow));
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        editExistingTexture = EditorGUILayout.Toggle("Edit Existing Texture", editExistingTexture);

        if (!editExistingTexture)
        {
            EditorGUILayout.LabelField("Image Size", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            x = Mathf.Clamp(EditorGUILayout.IntField("X:", x), 0, 256);
            y = Mathf.Clamp(EditorGUILayout.IntField("Y:", y), 0, 256);
            texture = new Texture2D(x, y);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            texture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), false);
            x = texture.width;
            y = texture.height;
        }

        EditorGUILayout.BeginHorizontal();

        Rect sideButtons = EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        color = EditorGUILayout.ColorField(color, GUILayout.Width(50), GUILayout.Height(50));

        Texture2D zoomInIcon = Resources.Load<Texture2D>("zoomIn");
        zoomIn = GUILayout.Button(zoomInIcon, GUILayout.Width(50), GUILayout.Height(50));

        Texture2D zoomOutIcon = Resources.Load<Texture2D>("zoomOut");
        zoomOut = GUILayout.Button(zoomOutIcon, GUILayout.Width(50), GUILayout.Height(50));

        if (zoomIn)
        {
            zoomMultiplier += 0.25;
        } else if (zoomOut && zoomMultiplier > 0.25)
        {
            zoomMultiplier -= 0.25;
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();

        Rect pixelEditor = EditorGUILayout.BeginVertical();

        if (pixels == null)
        {
            pixels = new PixelGrid(x, y, position, pixelEditor, 1);
        }

        pixels.Draw();
        pixels.PixelUpdate(color);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}

class Pixel {
    public float x;
    public float y;
    public int size;
    public Color color;

    public Pixel()
    {
        x = 16;
        y = 16;
        size = 16;
        color = Color.black;
    }

    public void ColorFill(Color color)
    {
        Rect rect = new Rect(x, y, size, size);

        if (rect.Contains(Event.current.mousePosition))
        {
            this.color = color;
            //EditorGUI.DrawRect(rect, color);
        }
    }

    public void Draw() {
        EditorGUI.DrawRect(new Rect(x, y, size, size), color);
    }
}

class PixelGrid {
    public Pixel[,] pixels;
    public int x;
    public int y;

    public PixelGrid(int x, int y, Rect position, Rect pixelEditor, double zoomMultiplier)
    {
        this.x = x;
        this.y = y;

        pixels = new Pixel[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                pixels[i, j] = new Pixel();

                Color pixelColor = (i + j) % 2 == 0 ? Color.white : new Color(0.8f, 0.8f, 0.8f, 1f);
                pixels[i, j].color = pixelColor;

                int pixelSize = (int)((0.75f * position.width / x < 0.75f * position.height / y ? 0.75f * position.width / x : 0.75f * position.height / y) * zoomMultiplier);
                pixels[i, j].size = pixelSize;

                float xPos = ((position.width - (x * pixelSize)) / 2f) + (pixelSize * i);
                pixels[i, j].x = xPos;

                float yPos = position.height - pixelEditor.y > (y * pixelSize) + 16 ? ((position.height - (y * pixelSize)) / 2f) + (pixelSize * j) : (pixelEditor.y + 16) + (pixelSize * j);
                pixels[i, j].y = yPos;
            }
        }
    }

    public void Draw()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                pixels[i, j].Draw();
            }
        }
    }

    public void PixelUpdate(Color pixelColor)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                pixels[i, j].ColorFill(pixelColor);
            }
        }
    }
}
