using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagLanguage : MonoBehaviour
{
    [SerializeField] Image _languageImage;
    [SerializeField] List<Sprite> _sprites;
    Language _language = Language.FR;
    void Start()
    {
        PlayerPrefs.SetInt("language", (int)_language);
        _languageImage.sprite = _sprites[(int)_language];
    }

    public void ChangeLanguage()
    {
        _language = (Language)(((int)_language + 1) % 2);
        PlayerPrefs.SetInt("language", (int)_language);
        _languageImage.sprite = _sprites[(int)_language];
    }
}
