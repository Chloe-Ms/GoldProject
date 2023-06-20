using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMap : MonoBehaviour
{
    [SerializeField] Sprite _sprite;
    private int _width;
    private int _height;

    public Sprite Sprite
    {
        get { return _sprite; }
    }

    public void Init(int width, int height)
    {
        _width = width;
        _height = height;
    }
}
