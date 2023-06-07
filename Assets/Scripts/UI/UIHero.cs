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
        _image.sprite = GetSpriteFromEffect(effect);
        switch (sensibility)
        {
            case 1:
                _image.color = Color.green;
                break;
            case 0:
                _image.color = Color.gray;
                break;
            case -2:
                _image.color = Color.red;
                break;
        }
    }

    private Sprite GetSpriteFromEffect(Effect effect)
    {
        Debug.Log(effect);
        int i = 0;
        Sprite sprite = null;
        while (i < _trapsIcon.TrapData.Count && sprite == null)
        {
            if (_trapsIcon.TrapData[i].Effect == effect)
            {
                sprite = _trapsIcon.TrapData[i].MiniSprite;
            }
            i++;
        }
        return sprite;
    }
}
