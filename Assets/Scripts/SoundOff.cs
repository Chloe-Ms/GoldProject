using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOff : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Sprite _imageOn;
    [SerializeField] Sprite _imageOff;
    private bool _isClicked = false;

    public void Clicked()
    {
        _isClicked = !_isClicked;
        if (_isClicked){
            AudioManager.Instance.MuteAllSounds();
            _image.sprite = _imageOff;
        } else {
            AudioManager.Instance.DemuteAllSounds();
            _image.sprite = _imageOn;
        }
    }
}
