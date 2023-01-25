using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public SpriteRenderer bodyPart;
    public List<Sprite> sprites = new List<Sprite>();

    private int currentSprite = 0;

    public void Next()
    {
        if (currentSprite < sprites.Count - 1)
        {
            currentSprite++;
        } else
        {
            currentSprite = 0;
        }
    }

    public void Prev()
    {
        if (currentSprite > 0)
        {
            currentSprite--;
        }
        else
        {
            currentSprite = sprites.Count - 1;
        }
    }

    public void Update()
    {
        bodyPart.sprite = sprites[currentSprite];
    }
}
