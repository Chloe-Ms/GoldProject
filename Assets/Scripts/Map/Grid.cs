using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] List<Sprite> _listSprite = new List<Sprite>();
    private int _width;
    private int _height;

    public void Init(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public Sprite GetSprite(int x, int y)
    {
        int index = 0;

        if (_width == 1) {
            if (y == 0)
                index = (int)direction.down;
            else if (y == _height - 1)
                index = (int)direction.up;
            else
                index = (int)direction.center;
        } else {
            if (x == 0) {
                if (y == 0)
                    index = (int)direction.leftDown;
                else if (y == _height - 1)
                    index = (int)direction.leftUp;
                else
                    index = (int)direction.left;
            } else if (x == _width - 1) {
                if (y == 0)
                    index = (int)direction.rightDown;
                else if (y == _height - 1)
                    index = (int)direction.rightUp;
                else
                    index = (int)direction.right;
            } else {
                if (y == 0)
                    index = (int)direction.down;
                else if (y == _height - 1)
                    index = (int)direction.up;
                else
                    index = (int)direction.center;
            }
        }
        return _listSprite[index];
    }
}

public enum direction
{
    leftUp = 0,
    up = 1,
    rightUp = 2,
    left = 3,
    center = 4,
    right = 5,
    leftDown = 6,
    down = 7,
    rightDown = 8
}
