using TMPro;
using UnityEngine;

public class TextUILanguage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] TextLanguage _traduction;

    void Start()
    {
        _text.text = _traduction.GetStringInLanguage(GameManager.Instance.LanguageChosen);
    }
}

