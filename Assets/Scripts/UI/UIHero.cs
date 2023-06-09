using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHero : MonoBehaviour
{
    [SerializeField] TrapList _trapsIcon;
    [SerializeField] Image _image;
    public void ChangeData(Effect effect, int sensibility)
    {
        _image.color = GetColorFromEffect(effect);
        //_image.sprite = getspritefromeffect(effect);
        //switch (sensibility)
        //{
        //    case 1:
        //        _image.color = color.green;
        //        break;
        //    case 0:
        //        _image.color = color.gray;
        //        break;
        //    case -2:
        //        _image.color = color.red;
        //        break;
        //}
    }

    private Color GetColorFromEffect(Effect effect)
    {
        int i = 0;
        Color color = Color.white;
        bool found = false;
        while (i < _trapsIcon.TrapData.Count && !found)
        {
            if (_trapsIcon.TrapData[i].Effect == effect)
            {
                color = _trapsIcon.TrapData[i].Color;
                found = true;
            }
            i++;
        }
        return color;
    }
}
