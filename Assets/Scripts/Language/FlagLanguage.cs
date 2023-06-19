using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlagLanguage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _languageText;
    Language _language = Language.FR;
    void Start()
    {
        _languageText.text = "EN";
        PlayerPrefs.SetInt("language", (int)_language);
    }

    public void ChangeLanguage()
    {
        _language = (Language)(((int)_language + 1) % 2);
        PlayerPrefs.SetInt("language", (int)_language);
        if ((int)_language == 0)
        {
            _languageText.text = "EN";
        } else
        {
            _languageText.text = "FR";
        }
    }
}
